using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;

namespace sample
{
    public class MyTableEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        public string Data_Value { get; set; }
        public string Period { get; set; }
        public string Subject { get; set; }
        public string TermOfPayment { get; set; }
        public string MethodOfPayment { get; set; }
        public string MappedTerm { get; set; }
        public string MappedMethod { get; set; }
        public ETag ETag { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        DateTimeOffset? ITableEntity.Timestamp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
