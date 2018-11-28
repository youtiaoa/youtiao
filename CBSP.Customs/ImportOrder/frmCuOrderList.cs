using CBSP.Business;
using CBSP.Models.Import;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using GZFramework.UI.Dev.Common;
using GZFramework.UI.Dev.LibForm;
using System;
using System.Data;
using System.Windows.Forms;
using static CBSP.Business.ConstantInfo;
using static CBSP.Business.ParaSql;

namespace CBSP.Customs.ImportOrder
{
    public partial class frmCuOrderList : frmBaseDialog
    {
        bllImOrder bll = new bllImOrder();
        DataRow dtlist;

        public frmCuOrderList()
        {
            InitializeComponent();
        }

        public static DataRow ShowForm(string count, bool isflag , DataRow row, string GUID, DataSet para)
        {
            frmCuOrderList form = new frmCuOrderList();
            form.txtCURRENCY.Properties.DataSource = para.Tables[CusParaName.CURR];
            form.txtCURRENCY.EditValue = CU_CURRENCY.CU_CURRENCY_142;
            form.txtCOUNTRY.Properties.DataSource = para.Tables[CusParaName.PARA_COUNTRY];
            form.txtUNIT.Properties.DataSource = para.Tables[CusParaName.PARA_UNIT];

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
                form.txtG_NUM.Text = count;
                form.txtGUID.Text = GUID;
            }

            form.ShowDialog();
            return form.dtlist;

        }



        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (ValidateBeforSave())
            {
                dtlist = bll.GetTable().Rows.Add();
                dtlist[CU_I_ORDER_LIST.GUID] = txtGUID.Text;
                dtlist[CU_I_ORDER_LIST.G_NUM] = txtG_NUM.Text;
                dtlist[CU_I_ORDER_LIST.ITEM_NO] = txtITEM_NO.Text;
                dtlist[CU_I_ORDER_LIST.ITEM_NAME] = txtITEM_NAME.Text;
                dtlist[CU_I_ORDER_LIST.G_MODEL] = txtG_MODEL.Text;
                dtlist[CU_I_ORDER_LIST.ITEM_DESCRIBE] = txtITEM_DESCRIBE.Text;
                dtlist[CU_I_ORDER_LIST.BAR_CODE] = txtBAR_CODE.Text;
                dtlist[CU_I_ORDER_LIST.QTY] = txtQTY.Text;
                dtlist[CU_I_ORDER_LIST.UNIT] = txtUNIT.EditValue;
                dtlist[CU_I_ORDER_LIST.PRICE] = txtPRICE.Text;
                dtlist[CU_I_ORDER_LIST.TOTAL_PRICE] = txtTOTAL_PRICE.Text;
                dtlist[CU_I_ORDER_LIST.CURRENCY] = txtCURRENCY.EditValue;
                dtlist[CU_I_ORDER_LIST.COUNTRY] = txtCOUNTRY.EditValue;
                dtlist[CU_I_ORDER_LIST.NOTE] = txtNOTE.Text;
                /*== null ? DBNull.Value : txtSecondQty.EditValue;*/
                
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

            Validate = LibraryTools.IsNotEmpBaseEdit(txtITEM_NAME, "商品名称不能为空！")
                     & LibraryTools.IsNotEmpBaseEdit(txtUNIT, "单位不能为空！")
                     & LibraryTools.IsNotEmpBaseEdit(txtQTY, "数量不能为空！")
                     & LibraryTools.IsNotEmpBaseEdit(txtPRICE, "单价不能为空！")
                     & LibraryTools.IsNotEmpBaseEdit(txtTOTAL_PRICE, "总价不能为空！")
                     & LibraryTools.IsNotEmpBaseEdit(txtCURRENCY, "币制不能为空！")
                     & LibraryTools.IsNotEmpBaseEdit(txtCOUNTRY, "原产国不能为空！")
                     & LibraryTools.IsNotEmpBaseEdit(txtG_MODEL, "规格型号不能为空！")
            ;

            return Validate;
        }

        #region 绑定lookUpEdit

        private void UpEdit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            //防止在窗口句柄初始化之前就走到下面的代码 
            if (this.IsHandleCreated)
            {
                BeginInvoke(new MethodInvoker(delegate ()
                {
                    Library.DataBinderTools.Bound.FilterLookup(sender);

                }));
            }
        }

        /// <summary>
        /// 焦点事件
        /// </summary>
        private void UpEdit_Enter(object sender, EventArgs e)
        {
            //防止在窗口句柄初始化之前就走到下面的代码 
            if (this.IsHandleCreated)
            {
                GridLookUpEdit UpEdit = (GridLookUpEdit)sender;
                base.OnEnter(e);
                BeginInvoke(new Action(() => { Library.DataBinderTools.Bound.BoundData(UpEdit); }));
            }
           
        }

        #endregion
    }
}
