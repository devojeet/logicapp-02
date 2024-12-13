//using System.IO;
//using System.Reflection.Metadata;
//using System.Threading.Tasks;
//using Microsoft.Azure.Functions.Worker;
//using Microsoft.Extensions.Logging;
//using sample.Utilities;


//namespace sample
//{
//    public class BlobTriggerFunction
//    {
//        private readonly ILogger<BlobTriggerFunction> _logger;
//        private  readonly CsvProcessor _csvProcessor;

//        public BlobTriggerFunction(ILogger<BlobTriggerFunction> logger, CsvProcessor csvProcessor)
//        {
//            _logger = logger;
//            _csvProcessor = csvProcessor; // Instantiate the utility class
//        }

//        [Function("BlobTriggerFunction")]
//        public async Task RunAsync(
//            [BlobTrigger("archive-acumant-02/{name}", Connection = "AzureWebJobsStorage")] Stream inputBlob,
//            string name)
//        {
//            _logger.LogInformation($"Processing blob: {name}");

//            try
//            {

//                var records = _csvProcessor.ReadCsv(inputBlob);
//                _logger.LogInformation($"Parsed {records.Count} records from the CSV file.");

//                var mappingsPath = Path.Combine(AppContext.BaseDirectory, "mappings.json");

//                var xmlData = _csvProcessor.ConvertToXml(records, mappingsPath);

//                Console.WriteLine("Transformed XML:");
//                Console.WriteLine(xmlData);


//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Error processing blob: {ex.Message}");
//            }
//        }
//    }
//}




using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using sample.Utilities;

namespace sample
{
    public class BlobTriggerFunction
    {
        private readonly ILogger<BlobTriggerFunction> _logger;
        private readonly CsvProcessor _csvProcessor;
        private readonly HttpClient _httpClient;

        public BlobTriggerFunction(ILogger<BlobTriggerFunction> logger, CsvProcessor csvProcessor, HttpClient httpClient)
        {
            _logger = logger;
            _csvProcessor = csvProcessor;
            _httpClient = httpClient; 
        }

        [Function("BlobTriggerFunction")]
        public async Task Run(
            [BlobTrigger("archive-acumant-02/{name}", Connection = "AzureWebJobsStorage")] Stream inputBlob,
            string name)
        {
            _logger.LogInformation($"Processing blob: {name}");

            try
            {
                var records = _csvProcessor.ReadCsv(inputBlob);

                _logger.LogInformation($"Parsed {records.Count} records from the CSV file.");

                var xmlData = _csvProcessor.ConvertToXml(records, "C:\\Users\\Inno\\OneDrive - Acumant Technologies Private Limited\\Desktop\\sample2\\mappings.json");

                var jsonData = _csvProcessor.ConvertXmlToJson(xmlData);
                //

                //
                _logger.LogInformation("Converted XML to JSON.");

                var logicAppUrl = "https://prod-26.centralindia.logic.azure.com:443/workflows/3d9da41a6e2d4b9a8a3ea6562d98eac5/triggers/When_a_HTTP_request_is_received/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2FWhen_a_HTTP_request_is_received%2Frun&sv=1.0&sig=jBFYhkimFCvFX2FF8a7D7bsMC6y0ymcMO2loYg3QP8o"; // Replace with the actual Logic App endpoint
                //_logger.LogInformation($"Logic App URL: {logicAppUrl}");

                var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(logicAppUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("JSON data successfully uploaded to Logic App.");
                }
                else
                {
                    _logger.LogError($"Failed to upload JSON data. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing blob: {ex.Message}", ex);
            }
        }
    }
}
