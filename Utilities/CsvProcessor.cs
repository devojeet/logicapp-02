//using CsvHelper;
//using CsvHelper.Configuration;
//using sample.Models;
//using System;
//using System.Collections.Generic;
//using System.Formats.Asn1;
////using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;
//using System.Xml.Linq;

//namespace sample.Utilities
//{
//    public class CsvProcessor
//    {
//        private readonly Dictionary<(string, string), (string, string)> mappings;

//        public CsvProcessor(string mappingsPath)
//        {
//            // Load and deserialize mappings.json
//            var mappingJson = File.ReadAllText(mappingsPath);
//            var mappingData = JsonSerializer.Deserialize<MappingRoot>(mappingJson);
//            var mappings = mappingData.Mappings.ToDictionary(
//                m => (m.TermOfPayment ?? "", m.MethodOfPayment ?? ""),
//                m => (m.MappedTerm, m.MappedMethod)
//            );
//        }

//        //public CsvProcessor()
//        //{
//        //}

//        public List<CsvRecord> ReadCsv(Stream inputStream)
//        {
//            var config = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
//            {
//                HasHeaderRecord = true
//            };

//            using var reader = new StreamReader(inputStream);
//            using var csv = new CsvReader(reader, config);

//            return csv.GetRecords<CsvRecord>().ToList();
//        }

//        public string ConvertToXml(List<CsvRecord> records, string mappingsPath)
//        {
//            // Apply mappings to records
//            foreach (var record in records)
//            {
//                var key = (record.TermOfPayment ?? "", record.MethodOfPayment ?? "");
//                if (mappings.TryGetValue(key, out var mappedValues))
//                {
//                    record.MappedTerm = mappedValues.Item1;
//                    record.MappedMethod = mappedValues.Item2;
//                }
//            }

//            // Generate XML
//            var xmlDocument = new XDocument(new XElement("Records"));
//            foreach (var record in records)
//            {
//                var recordElement = new XElement("Record");
//                foreach (var property in typeof(CsvRecord).GetProperties())
//                {
//                    var propertyName = property.Name;
//                    var value = property.GetValue(record)?.ToString() ?? string.Empty;

//                    recordElement.Add(new XElement(propertyName, value));
//                }

//                xmlDocument.Root?.Add(recordElement);
//            }

//            return xmlDocument.ToString();
//        }

//        public class MappingRoot
//        {
//            public List<Mapping> Mappings { get; set; }
//        }

//        public class Mapping
//        {
//            public string TermOfPayment { get; set; }
//            public string MethodOfPayment { get; set; }
//            public string MappedTerm { get; set; }
//            public string MappedMethod { get; set; }
//        }
//    }
//}




using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using sample.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;

namespace sample.Utilities
{
    public class CsvProcessor
    {
        private readonly Dictionary<(string, string), (string, string)> _mappings;

        public CsvProcessor(string mappingsPath)
        {

            var mappingJson = File.ReadAllText(mappingsPath);
            var mappingData = System.Text.Json.JsonSerializer.Deserialize<MappingRoot>(mappingJson);

            _mappings = mappingData.Mappings.ToDictionary(
                m => (m.TermOfPayment ?? string.Empty, m.MethodOfPayment ?? string.Empty),
                m => (m.MappedTerm ?? string.Empty, m.MappedMethod ?? string.Empty)
            );
        }

        public List<CsvRecord> ReadCsv(Stream inputStream)
        {
            var config = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null
            };

            using var reader = new StreamReader(inputStream);
            using var csv = new CsvReader(reader, config);

            return csv.GetRecords<CsvRecord>().ToList();
        }

        public string ConvertToXml(List<CsvRecord> records, string mappingsPath)
        {
            foreach (var record in records)
            {
                var key = (record.TermOfPayment ?? string.Empty, record.MethodOfPayment ?? string.Empty);
                if (_mappings.TryGetValue(key, out var mappedValues))
                {
                    record.MappedTerm = mappedValues.Item1;
                    record.MappedMethod = mappedValues.Item2;
                }
            }

            var xmlDocument = new XDocument(new XElement("Records"));
            foreach (var record in records)
            {
                var recordElement = new XElement("Record");
                foreach (var property in typeof(CsvRecord).GetProperties())
                {
                    var propertyName = property.Name;
                    var value = property.GetValue(record)?.ToString() ?? string.Empty;

                    recordElement.Add(new XElement(propertyName, value));
                }

                xmlDocument.Root?.Add(recordElement);
            }

            return xmlDocument.ToString();
        }

        public string ConvertXmlToJson(string xmlData)
        {
            var xmlDocument = XDocument.Parse(xmlData);
            var json = JsonConvert.SerializeXNode(xmlDocument, Newtonsoft.Json.Formatting.Indented);
            return json;
        }


        public class MappingRoot
        {
            public List<Mapping> Mappings { get; set; }
        }

        public class Mapping
        {
            public string TermOfPayment { get; set; }
            public string MethodOfPayment { get; set; }
            public string MappedTerm { get; set; }
            public string MappedMethod { get; set; }
        }
    }
}
