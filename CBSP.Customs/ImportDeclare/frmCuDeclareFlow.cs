using CBSP.Business;
using GZFramework.UI.Dev.LibForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CBSP.Customs.ImportDeclare
{
    public partial class frmCuDeclareFlow : frmBaseDialog
    {
        public frmCuDeclareFlow()
        {
            InitializeComponent();

            Library.DataBinderTools.Bound.BoundCommonDictDataID(lueStatus, Business.CustomerEnum.EnumCommonDicData.海关总状态, false, false);
            Library.DataBinderTools.Bound.BoundCommonDictDataID(lueStatusSouce, Business.CustomerEnum.EnumCommonDicData.状态来源, false, false);
        }

        public void ShowForm(string key)
        {
            frmCuDeclareFlow statusform = new frmCuDeclareFlow();
            //取table
            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic.Add("GUID", key);

            bllImDeclareFlow bll = new bllImDeclareFlow();

            DataTable dt = bll.Search(dic);

            statusform.gcMainData.DataSource = dt;
            statusform.gvMainData.BestFitColumns();
          
            statusform.ShowDialog();
        }
    }
}
