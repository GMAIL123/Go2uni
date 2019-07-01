using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Go2uniApi.Models
{
    public class ResultInfo<T>
    {
        public int ErrorCode { get; set; }
        public string Description { get; set; }
        public string TokenId { get; set; }
        public bool Status { get; set; }
        public long LastModifiedId { get; set; }
        public T Info { get; set; }
    }
}