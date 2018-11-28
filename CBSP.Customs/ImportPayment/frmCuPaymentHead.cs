using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using GZFramework.UI.Dev.Common;
using GZFramework.UI.Dev.LibForm;
using CBSP.Business;
using CBSP.Models.Import;
using static CBSP.Business.ConstantInfo;
using GZFramework.UI.Core;
using CBSP.Library.Config.RibbonButtons;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using static CBSP.Business.ParaSql;

namespace CBSP.Customs.ImportPayment
{
    public partial class frmCuPaymentHead : frmBaseDataBusiness
    {
        bllImPayment bll;
        public frmCuPaymentHead()
        {
            InitializeComponent();
            this.Load += frm_Load;
            //实例化必须，bllBusinessBase必须替换为bll层自己继承的子类(指定正确的dal.DBCode)，建议封装重写到项目bll层

            _bll = bll = new bllImPayment();
        }
        private void frm_Load(object sender, EventArgs e)
        {
            _SummaryView = gvMainData;//必须赋值
            base.AddControlsOnAddKey();

            base.AddControlsOnlyRead(this.txtPAYER_ID_TYPE, txtCURRENCY);

            ParaDataset = bll.CusParaDateset_Payment();

            this.txts_RETURN_STATUS.Properties.DataSource = ParaDataset.Tables[CusParaName.PARA_CU_RESPONSE_STATUS];
            this.txtEBP_CODE.Properties.DataSource = ParaDataset.Tables[CusParaName.COP_TYPE_EP];
            this.txtPAY_CODE.Properties.DataSource = ParaDataset.Tables[CusParaName.COP_TYPE_P];
            this.txtCURRENCY.Properties.DataSource = ParaDataset.Tables[CusParaName.CURR];
            this.txtPAYER_ID_TYPE.Properties.DataSource = ParaDataset.Tables[CusParaName.CERT_TYPE];

            Library.DataBinderTools.Bound.BoundCommonDictDataID(lueIDType, Business.CustomerEnum.EnumCommonDicData.证件类型, false, false);
            Library.DataBinderTools.Bound.BoundCommonDictDataID(lueInputFlag, Business.CustomerEnum.EnumCommonDicData.录入标志, false, false);
            Library.DataBinderTools.Bound.BoundCommonDictDataID(lueReturnStatus, Business.CustomerEnum.EnumCommonDicData.海关状态, false, false);
            Library.DataBinderTools.Bound.BoundCommonDictDataID(lueStatus, Business.CustomerEnum.EnumCommonDicData.平台状态, false, false);
            Library.DataBinderTools.Bound.BoundCommonDictDataID(lueUploadFlag, Business.CustomerEnum.EnumCommonDicData.上传标志, false, false);

            txts_STAR_TIME.EditValue = DateTime.Now.AddDays(-1);
        }
        //查询
        private void btn_Search_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            if (!String.IsNullOrEmpty(txts_ORDER_NO.Text)) dic.Add("ORDER_NO", txts_ORDER_NO.Text);
            if (!String.IsNullOrEmpty(txts_PAYER_NAME.Text)) dic.Add("PAYER_NAME", txts_PAYER_NAME.Text);
            if (!String.IsNullOrEmpty(txts_RETURN_STATUS.Text)) dic.Add("RETURN_STATUS", txts_RETURN_STATUS.Text);
            if (!String.IsNullOrEmpty(txts_STAR_TIME.Text)) dic.Add("STAR_TIME", txts_STAR_TIME.Text);
            if (!String.IsNullOrEmpty(txts_END_TIME.Text)) dic.Add("END_TIME", txts_END_TIME.Text);

            DataTable dt = bll.GetDateList(dic, CU_I_PAYMENT_HEAD._TableName);

            gcMainData.DataSource = dt;
            CreateSummary();
            if (gvMainData.RowCount < 300)//行数过多会很耗时
                gvMainData.BestFitColumns();
        }
        //清空条件
        private void btn_Clear_Click(object sender, EventArgs e)
        {
            LibraryTools.DoClearPanel(LC_Search);
        }


        //保存前检查
        protected override bool ValidateBeforSave()
        {
            bool Validate = true
                       & LibraryTools.IsNotEmpBaseEdit(txtPAY_CODE, "支付企业不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtPAY_TRANSACTION_ID, "支付交易流水号不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtORDER_NO, "订单编号不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtEBP_CODE, "电商平台不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtPAYER_ID_TYPE, "支付人证件类型不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtPAYER_ID_NUMBER, "支付人证件号码不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtPAYER_NAME, "支付人姓名不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtAMOUNT_PAID, "支付金额不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtCURRENCY, "支付币制不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtPAY_TIME, "支付时间不能为空！");

            ;


            return Validate;
        }


        #region 其他常用
        //绑定明细页数据
        public override void DoBoundEditData()
        {
            //base.DoBoundEditData();
            LibraryTools.DoBindingEditorPanel(pan_Summary, EditData.Tables[_bll.SummaryModel.TableName], "txt");

            //数据绑定，两个地方任选一处都可以
            EditData.Tables[_bll.SummaryModel.TableName].Rows[0][CU_I_PAYMENT_HEAD.PAYER_ID_TYPE] = CU_CERT_TYPE.CU_CERT_TYPE_1;
            EditData.Tables[_bll.SummaryModel.TableName].Rows[0][CU_I_PAYMENT_HEAD.CURRENCY] = CU_CURRENCY.CU_CURRENCY_142;

            if (CurrentDataState == FormDataState.Add)
            {
                EditData.Tables[_bll.SummaryModel.TableName].Rows[0][CU_I_PAYMENT_HEAD.GUID] = BillNoGenerator.NewPaymentBillNo();
                Library.DataBinderTools.Bound.DoBindingEntity(EditData.Tables[_bll.SummaryModel.TableName].Rows[0]);

                this.txtPAYER_ID_TYPE.EditValue = CU_CERT_TYPE.CU_CERT_TYPE_1;
                this.txtCURRENCY.EditValue = CU_CURRENCY.CU_CURRENCY_142;
            }

            //其他绑定
            //LibraryTools.DoBindingEditorPanel(pan_Summary, EditData.Tables[_bll.SummaryModel.TableName], "txt");
            //txxtPassword.EditValue = EditData.Tables[_bll.SummaryTableName].Rows[0][dt_MyUser.Password];
            //gc_Detail.DataSource = EditData.Tables[dt_MyUserRole._TableName];    
        }

        //获得详细数据，明细也
        public override DataSet GetEditData(string KeyValue)
        {
            return base.GetEditData(KeyValue);
        }

        /// <summary>
        /// 设置窗体的基础权限从FunctionAuthorityCommon类中取，例如(默认)：
        /// return FunctionAuthorityCommon.VIEW//查看
        ///       + FunctionAuthorityCommon.ADD//新增
        ///       + FunctionAuthorityCommon.EDIT//修改
        ///       + FunctionAuthorityCommon.DELETE//删除
        ///       + FunctionAuthorityCommon.Save//保存
        ///       + FunctionAuthorityCommon.Cancel;//取消
        /// </summary>
        protected override int CustomerAuthority
        {
            get
            {
                return base.CustomerAuthority + FunctionAuthority.EX_04 + FunctionAuthorityCommon.Export;//扩展申报;
            }
        }

        //自定义窗体权限按钮
        public override void IniButton()
        {
            base.IniButton();
            if (CurrentAuthorityExist(FunctionAuthority.EX_04))//如果有扩展权限
            {
                var btn = new RibbonItemButton()
                {
                    name = "Declare",
                    Caption = "申报",
                    ImgFileName = "upload_32px.ico",
                    BeginGroup = false,
                    Shortcut = Keys.None//这里是快捷键
                };
                InsertAfterButton(ButtonNames.btnExport, btn, FunctionAuthority.EX_04, "申报", DoDeclare);
            }
        }

        //窗体状态改变后
        protected override void DataStateChanged(GZFramework.UI.Core.FormDataState NewState)
        {
            base.DataStateChanged(NewState);
        }
        //窗体状态改时
        protected override void DataStateChanging(GZFramework.UI.Core.FormDataState OldState, GZFramework.UI.Core.FormDataState NewState)
        {
            base.DataStateChanging(OldState, NewState);
        }



        /// <summary>
        /// 设置按钮可用状态，如果已经在ControlOnlyReads或SetControlAccessable中添加，这里不需要重新设置
        /// </summary>
        /// <param name="Edit"></param>
        protected override void SetControlAccessable(bool Edit)
        {
            //LibraryTools.SetControlAccessable(tp_Edit, Edit);
            base.SetControlAccessable(Edit);

        }

        /// <summary>
        /// 超链事件
        /// </summary>
        private void gvMainData_RowClick(object sender, RowClickEventArgs e)
        {
            GridView gridView = (GridView)sender;
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                GridHitInfo hitInfo = gridView.CalcHitInfo(e.Location);
                if (hitInfo.InRowCell && hitInfo.Column == this.gc_LINK)
                {
                    //展示状态流
                    DataRow dr = _SummaryView.GetFocusedDataRow();
                    string Key = ConvertEx.ToString(dr[_bll.SummaryModel.PrimaryKey]);
                    frmCuPaymentFlow status = new frmCuPaymentFlow();
                    status.ShowForm(Key);
                }
            }
        }

        private void gvMainData_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            base.CustomSummaryCalculate(e);
        }
        /// <summary>
        /// 明细增加汇总信息
        /// </summary>
        private void CreateSummary()
        {
            DevExpress.XtraGrid.Columns.GridColumn OrderColumn = gvMainData.Columns["ORDER_NO"];
            OrderColumn.SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "总数：{0}");

            DevExpress.XtraGrid.Columns.GridColumn AmountColumn = gvMainData.Columns["AMOUNT_PAID"];
            AmountColumn.SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "总实付：{0:C2}");

        }

        #endregion

        #region 操作事件，不需要的可以删除
        /// <summary>
        /// 查询
        /// </summary>
        protected override void DoView(object sender)
        {
            base.DoView(sender);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        protected override void DoRefresh(object sender)
        {
            base.DoRefresh(sender);
        }

        /// <summary>
        /// 新增
        /// </summary>
        protected override void DoAdd(object sender)
        {
            base.DoAdd(sender);
        }

        /// <summary>
        /// 删除
        /// </summary>
        protected override void DoDelete(object sender)
        {
            base.DoDelete(sender);
        }

        /// <summary>
        /// 修改
        /// </summary>
        protected override void DoEdit(object sender)
        {
            int[] rows = _SummaryView.GetSelectedRows();
            if (rows.Length > 1)
            {
                Msg.Warning("请选择唯一一条数据编辑！");
                return;
            }
            base.DoEdit(sender);
        }

        /// <summary>
        /// 保存
        /// </summary>
        protected override void DoSave(object sender)
        {
            base.DoSave(sender);
        }

        /// <summary>
        /// 保存并关闭
        /// </summary>
        protected override void DoSaveAndClose(object sender)
        {
            base.DoSaveAndClose(sender);
        }

        /// <summary>
        /// 审核
        /// </summary>
        protected override void DoApproval(object sender)
        {
            base.DoApproval(sender);
        }

        /// <summary>
        /// 申报
        /// </summary>
        private void DoDeclare(object sender, ItemClickEventArgs e)
        {
            base.DoDeclare(sender);
        }

        /// <summary>
        /// 返回取消
        /// </summary>
        protected override void DoCancel(object sender)
        {
            base.DoCancel(sender);
        }

        /// <summary>
        /// 打印预览
        /// </summary>
        protected override void DoPreview(object sender)
        {
            base.DoPreview(sender);
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        protected override void DoExport(object sender)
        {
            base.DoExport(sender);
        }

        #endregion

        #region 绑定lookUpEdit

        private void UpEdit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate ()
            {
                Library.DataBinderTools.Bound.FilterLookup(sender);

            }));
        }

        /// <summary>
        /// 焦点事件
        /// </summary>
        private void UpEdit_Enter(object sender, EventArgs e)
        {
            GridLookUpEdit UpEdit = (GridLookUpEdit)sender;
            base.OnEnter(e);
            BeginInvoke(new Action(() => { Library.DataBinderTools.Bound.BoundData(UpEdit); }));
        }

        #endregion


    }
}
