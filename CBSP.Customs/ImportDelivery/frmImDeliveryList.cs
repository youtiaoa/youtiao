using CBSP.Business;
using CBSP.Models.Import;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using GZFramework.UI.Dev.Common;
using GZFramework.UI.Dev.LibForm;
using System;
using System.Data;

namespace CBSP.Customs.ImportDelivery
{
    public partial class frmImDeliveryList : frmBaseDialog
    {
        bllImDelivery bll = new bllImDelivery();
        DataRow dtlist;

        public frmImDeliveryList()
        {
            InitializeComponent();

        }

        public static DataRow ShowForm(string count, bool isflag, DataRow row, string GUID)
        {
            frmImDeliveryList form = new frmImDeliveryList();
            if (row != null)
            {
                string fieldName = "";
                int length = "txt".Length;
                //绑定数据
                foreach (LayoutControlItem ctl in form.LCGroup_Edit.Items)
                {
                    //判断不为空  
                    if (!object.Equals(null, ctl))
                    {
                        //控件类型  
                        if (ctl.Control is BaseEdit)
                        {
                            BaseEdit be = ctl.Control as BaseEdit;
                            if (be.Name.Substring(0, length) == "txt")
                            {
                                fieldName = be.Name.Substring(length, be.Name.Length - length);
                                be.EditValue = row[fieldName];
                                be.Properties.ReadOnly = !isflag;
                            }
                        }
                    }
                }

                form.btn_ok.Enabled = isflag;
                form.btn_cancel.Enabled = isflag;
            }
            else
            {
                form.txtGUID.Text = GUID;
                form.txtG_NUM.Text = count;
            }

            form.ShowDialog();
            return form.dtlist;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (ValidateBeforSave())
            {
                dtlist = bll.GetTable().Rows.Add();
                dtlist[CU_I_DELIVERY_LIST.GUID] = txtGUID.Text;
                dtlist[CU_I_DELIVERY_LIST.G_NUM] = txtG_NUM.Text;
                dtlist[CU_I_DELIVERY_LIST.LOGISTICS_NO] = txtLOGISTICS_NO.EditValue;
                dtlist[CU_I_DELIVERY_LIST.NOTE] = txtNOTE.Text;
                //绑定数据

                this.Close();
            }
            else
                dtlist = null;
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            dtlist = null;
            this.Close();
        }

        //保存前检查
        protected bool ValidateBeforSave()
        {
            bool Validate = true;

            Validate = LibraryTools.IsNotEmpBaseEdit(txtLOGISTICS_NO, "物流运单编号不能为空！")
            ;

            return Validate;
        }

    }
}
