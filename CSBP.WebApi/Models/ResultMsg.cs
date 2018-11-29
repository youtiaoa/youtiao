using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSBP.WebApi.Models
{
    public class ResultMsg
    {
        public ResultInfo ResultInfo { get; set; }
    }
    public class ResultInfo
    {
        public string ID { get; set; }
        public string Status { get; set; }

        public string Remark { get; set; }
    }
}