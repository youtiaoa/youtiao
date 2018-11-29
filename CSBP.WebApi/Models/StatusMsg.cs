using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSBP.WebApi.Models
{
    public class StatusMsg
    {
        public StatusInfo StatusInfo { get; set; }
    }
    public class StatusInfo
    {
        public string ID { get; set; }

        public string Status { get; set; }

        public string Remark { get; set; }

        public DeclformItem[] DeclformItem { get; set; }

        public EciqDeclformItem[] EciqDeclformItem { get; set; }
    }

    public class DeclformItem
    {
        public string CopNo { get; set; }

        public string WaybillID { get; set; }

        public string CustID { get; set; }

        public string Status { get; set; }

        public string Remark { get; set; }
    }

    public class EciqDeclformItem
    {
        public string CopNo { get; set; }

        public string WaybillID { get; set; }

        public string ChecklistNo { get; set; }

        public string Status { get; set; }

        public string Remark { get; set; }
    }
}