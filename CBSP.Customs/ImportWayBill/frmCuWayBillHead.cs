using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using GZFramework.UI.Dev.Common;
using GZFramework.UI.Dev.LibForm;
using CBSP.Business;
using static CBSP.Business.ConstantInfo;
using GZFramework.UI.Core;
using CBSP.Models.Import;
using DevExpress.XtraEditors;
using CBSP.Library.Config.RibbonButtons;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using static CBSP.Business.ParaSql;
using CBSP.Library.MyClass;

namespace CBSP.Customs.ImportWayBill
{
    public partial class frmCuWayBillHead : frmBaseDataBusiness
    {
        bllImWayBill bll;
        public frmCuWayBillHead()
        {
            InitializeComponent();
            this.Load += frm_Load;
            //实例化必须，bllBusinessBase必须替换为bll层自己继承的子类(指定正确的dal.DBCode)，建议封装重写到项目bll层

            _bll = bll = new bllImWayBill();
        }
        private void frm_Load(object sender, EventArgs e)
        {
            _SummaryView = gvMainData;//必须赋值
            base.AddControlsOnAddKey();

            base.AddControlsOnlyRead( this.txtCURRENCY);

            ParaDataset = bll.CusParaDateset_Waybill();

            this.txts_RETURN_STATUS.Properties.DataSource = ParaDataset.Tables[CusParaName.PARA_CU_RESPONSE_STATUS];
            this.txtLOGISTICS_CODE.Properties.DataSource = ParaDataset.Tables[CusParaName.COP_TYPE_L];
            this.txtCURRENCY.Properties.DataSource = ParaDataset.Tables[CusParaName.CURR];
            this.txts_STATUS.Properties.DataSource = ParaDataset.Tables[CusParaName.PARA_PLAT_STATUS];

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

            if (!String.IsNullOrEmpty(txts_LOGISTICS_NO.Text)) dic.Add("LOGISTICS_NO", txts_LOGISTICS_NO.Text);
            if (!String.IsNullOrEmpty(txts_CONSIGNEE.Text)) dic.Add("CONSIGNEE", txts_CONSIGNEE.Text);
            if (!String.IsNullOrEmpty(txts_RETURN_STATUS.Text)) dic.Add("RETURN_STATUS", txts_RETURN_STATUS.EditValue);
            if (!String.IsNullOrEmpty(txts_STAR_TIME.Text)) dic.Add("STAR_TIME", txts_STAR_TIME.Text);
            if (!String.IsNullOrEmpty(txts_END_TIME.Text)) dic.Add("END_TIME", txts_END_TIME.Text);
            if (!String.IsNullOrEmpty(txts_STATUS.Text)) dic.Add("STATUS", txts_STATUS.EditValue);

            DataTable dt = bll.GetDateList(dic, CU_I_WAYBILL_HEAD._TableName);

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
                       & LibraryTools.IsNotEmpBaseEdit(txtLOGISTICS_NO, "运单编号不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtFREIGHT, "运费不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtINSURE_FEE, "保价费不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtCURRENCY, "币值不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtWEIGHT, "毛重不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtPACK_NO, "件数不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtCONSIGNEE, "收货人姓名不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtCONSIGNEE_ADDRESS, "收货地址不能为空！")
                         & LibraryTools.IsNotEmpBaseEdit(txtORDER_NO, "订单编号不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtCONSIGNEE_TELEPHONE, "收货人电话不能为空！");
            ;
            return Validate;
        }


        #region 其他常用
        //绑定明细页数据
        public override void DoBoundEditData()
        {
            //base.DoBoundEditData();
            LibraryTools.DoBindingEditorPanel(pan_Summary, EditData.Tables[_bll.SummaryModel.TableName], "txt");

            EditData.Tables[_bll.SummaryModel.TableName].Rows[0][CU_I_WAYBILL_HEAD.CURRENCY] = CU_CURRENCY.CU_CURRENCY_142;

            if (CurrentDataState == FormDataState.Add)
            {
                EditData.Tables[_bll.SummaryModel.TableName].Rows[0][CU_I_WAYBILL_HEAD.GUID] = BillNoGenerator.NewWayBillNo();
                Library.DataBinderTools.Bound.DoBindingEntity(EditData.Tables[_bll.SummaryModel.TableName].Rows[0]);

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
                return base.CustomerAuthority + FunctionAuthority.EX_04 + FunctionAuthorityCommon.Export + FunctionAuthority.EX_05;//扩展申报;
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
            if (CurrentAuthorityExist(FunctionAuthority.EX_05))//如果有扩展权限
            {
                var btn = new RibbonItemButton()
                {
                    name = "Import",
                    Caption = "导入",
                    ImgFileName = "import_32px.ico",
                    BeginGroup = false,
                    Shortcut = Keys.None//这里是快捷键
                };
                InsertBeforeButton(ButtonNames.btnExport, btn, FunctionAuthority.EX_05, "导入", DoImport);
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
                    frmCuWayBillFlow status = new frmCuWayBillFlow();
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
            DevExpress.XtraGrid.Columns.GridColumn LogisticsColumn = gvMainData.Columns["LOGISTICS_NO"];
            LogisticsColumn.SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Count, "总数：{0}");

            DevExpress.XtraGrid.Columns.GridColumn WerghtColumn = gvMainData.Columns["WEIGHT"];
            WerghtColumn.SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "总毛重：{0:N2}");

            DevExpress.XtraGrid.Columns.GridColumn FreightColumn = gvMainData.Columns["FREIGHT"];
            FreightColumn.SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "总运费：{0:C2}");

            DevExpress.XtraGrid.Columns.GridColumn InsureColumn = gvMainData.Columns["INSURE_FEE"];
            InsureColumn.SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "总保费：{0:C2}");

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
        /// 申报
        /// </summary>
        private void DoDeclare(object sender, ItemClickEventArgs e)
        {
            base.DoDeclare(sender);
        }

        /// <summary>
        /// 导入
        /// </summary>
        private void DoImport(object sender, ItemClickEventArgs e)
        {
            EditData = frmBaseExcel.ShowForm(CU_I_WAYBILL_HEAD._TableName);
            if (EditData != null)
                base.DoImport(sender);
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
