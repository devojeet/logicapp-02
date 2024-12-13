using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sample.Models
{
    public class CsvRecord
    {
        //public int PartitionKey { get; set; }
        //public int RowKey { get; set; }
        public string Data_Value { get; set; }
        public string Period { get; set; }
        public string Subject { get; set; }
        public string TermOfPayment { get; set; }
        public string MethodOfPayment { get; set; }
        public string MappedTerm { get; set; }
        public string MappedMethod { get; set; }
    }
}
