using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using GZFramework.UI.Dev.Common;
using GZFramework.UI.Dev.LibForm;
using CBSP.Business;
using CBSP.Models.Import;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using CBSP.Models.Para;
using static CBSP.Business.ConstantInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using GZFramework.UI.Core;
using CBSP.Library.Config.RibbonButtons;
using DevExpress.XtraBars;
using static CBSP.Business.ParaSql;

namespace CBSP.Customs.ImportOrder
{
    public partial class frmCuOrderHead : frmBaseDataBusiness
    {
        bllImOrder bll;
        public frmCuOrderHead()
        {
            InitializeComponent();
            this.Load += frm_Load;
            //实例化必须，bllBusinessBase必须替换为bll层自己继承的子类(指定正确的dal.DBCode)，建议封装重写到项目bll层

            _bll = bll = new bllImOrder();
        }
        private void frm_Load(object sender, EventArgs e)
        {
            _SummaryView = gvMainData;//必须赋值
            base.AddControlsOnAddKey();

            base.AddControlsOnlyRead(this.txtORDER_TYPE, this.txtCURRENCY, this.txtBUYER_ID_TYPE);

            ParaDataset = bll.CusParaDateset_ImOrder();

            this.txtORDER_TYPE.Properties.DataSource = ParaDataset.Tables[CusParaName.PARA_IE_FLAG];
            this.txtEBP_CODE.Properties.DataSource = ParaDataset.Tables[CusParaName.COP_TYPE_EP];
            this.txtEBC_CODE.Properties.DataSource = ParaDataset.Tables[CusParaName.COP_TYPE_E];
            this.txtPAY_CODE.Properties.DataSource = ParaDataset.Tables[CusParaName.COP_TYPE_P];
            this.txtCURRENCY.Properties.DataSource = ParaDataset.Tables[CusParaName.CURR];
            this.txtBUYER_ID_TYPE.Properties.DataSource = ParaDataset.Tables[CusParaName.CERT_TYPE];
            this.txtCONSIGNEE_DITRICT.Properties.DataSource = ParaDataset.Tables[CusParaName.PARA_DISTRICT];
            this.txts_STATUS.Properties.DataSource = ParaDataset.Tables[CusParaName.PARA_PLAT_STATUS];
            this.txts_ORDER_BILL_TYPE.Properties.DataSource = ParaDataset.Tables[CusParaName.PARA_CU_TRADE_MODE];
            this.txts_RETURN_STATUS.Properties.DataSource = ParaDataset.Tables[CusParaName.PARA_CU_RESPONSE_STATUS];

            Library.DataBinderTools.Bound.BoundCommonDictDataID(lueOrderType, Business.CustomerEnum.EnumCommonDicData.进出口标志, false, false);
            Library.DataBinderTools.Bound.BoundCommonDictDataID(lueIDType, Business.CustomerEnum.EnumCommonDicData.证件类型, false, false);
            Library.DataBinderTools.Bound.BoundCommonDictDataID(lueInputFlag, Business.CustomerEnum.EnumCommonDicData.录入标志, false, false);
            Library.DataBinderTools.Bound.BoundCommonDictDataID(lueReturnStatus, Business.CustomerEnum.EnumCommonDicData.海关状态, false, false);
            Library.DataBinderTools.Bound.BoundCommonDictDataID(lueStatus, Business.CustomerEnum.EnumCommonDicData.平台状态, false, false);
            Library.DataBinderTools.Bound.BoundCommonDictDataID(lueUploadFlag, Business.CustomerEnum.EnumCommonDicData.上传标志, false, false);
            Library.DataBinderTools.Bound.BoundCommonDictDataID(lueOrderBillType, Business.CustomerEnum.EnumCommonDicData.贸易方式, false, false);
            Library.DataBinderTools.Bound.BoundCommonDictDataID(lueEmsFlag, Business.CustomerEnum.EnumCommonDicData.上载标志, false, false);

            txts_STAR_TIME.EditValue = DateTime.Now.AddDays(-1);
        }
        //查询
        private void btn_Search_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            if (!String.IsNullOrEmpty(txts_ORDER_NO.Text)) dic.Add("ORDER_NO", txts_ORDER_NO.Text);
            if (!String.IsNullOrEmpty(txts_ORDER_BILL_TYPE.Text)) dic.Add("ORDER_BILL_TYPE", txts_ORDER_BILL_TYPE.EditValue);
            if (!String.IsNullOrEmpty(txts_STATUS.Text)) dic.Add("STATUS", txts_STATUS.EditValue);
            if (!String.IsNullOrEmpty(txts_RETURN_STATUS.Text)) dic.Add("RETURN_STATUS", txts_RETURN_STATUS.EditValue);
            if (!String.IsNullOrEmpty(txts_STAR_TIME.Text)) dic.Add("STAR_TIME", txts_STAR_TIME.Text);
            if (!String.IsNullOrEmpty(txts_END_TIME.Text)) dic.Add("END_TIME", txts_END_TIME.Text);

            DataTable dt = bll.GetDateList(dic, CU_I_ORDER_HEAD._TableName);

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
                       & LibraryTools.IsNotEmpBaseEdit(txtORDER_TYPE, "订单类型不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtORDER_NO, "订单号不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtEBP_CODE, "电商平台不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtEBC_CODE, "电商企业不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtGOODS_VALUE, "商品价格不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtFREIGHT, "运杂费不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtDISCOUNT, "非现金抵扣金额不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtTAXTOTAL, "代扣税款不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtACTURAL_PAID, "实际支付金额不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtCURRENCY, "币制不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtBUYER_REGNO, "订购人注册号不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtBUYER_NAME, "订购人姓名不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtBUYER_TELEPHONE, "订购人电话不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtBUYER_ID_TYPE, "订购人证件类型不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtBUYER_ID_NUMBER, "订购人证件号不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtCONSIGNEE, "收货人姓名不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtCONSIGNEE_TELEPHONE, "收货人电话不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtCONSIGNEE_ADDRESS, "收货人地址不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtCONSIGNEE_DITRICT, "收货人行政区不能为空！");

            Validate = Validate & !gv_Detail_CU_I_ORDER_LIST.HasColumnErrors & gv_Detail_CU_I_ORDER_LIST.ValidateEditor();
            ;


            return Validate;
        }


        private void gv_Detail_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            string TableName = (sender as DevExpress.XtraGrid.Views.Grid.GridView).GridControl.DataSource.ToString();
            //(sender as DevExpress.XtraGrid.Views.Grid.GridView).SetRowCellValue(e.RowHandle, _bll.DetailModel[TableName].ForeignKey, EditData.Tables[_bll.SummaryModel.TableName].Rows[0][_bll.SummaryModel.PrimaryKey]);            
            (sender as DevExpress.XtraGrid.Views.Grid.GridView).GetFocusedDataRow()[_bll.DetailModel[TableName].ForeignKey] = EditData.Tables[_bll.SummaryModel.TableName].Rows[0][_bll.SummaryModel.PrimaryKey];

        }
        private void gc_Detail_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
            {
                e.Handled = Msg.AskQuestion("是否确定要删除选中行？") == false;
            }
            if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Append)
            {
                var count = (EditData.Tables[CU_I_ORDER_LIST._TableName].Rows.Count + 1).ToString();
                var v = frmCuOrderList.ShowForm(count, false, null, EditData.Tables[_bll.SummaryModel.TableName].Rows[0][CU_I_ORDER_HEAD.GUID].ToString(),ParaDataset);
                if (v != null)
                {
                    EditData.Tables[CU_I_ORDER_LIST._TableName].Rows.Add(v.ItemArray);
                }
                e.Handled = true;//标记为已处理，不再向下执行
            }
        }

        private void gv_Detail_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            var view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            if (Object.Equals(view.FocusedColumn.Tag, "NotNull"))
            {
                if (Object.Equals(string.Empty, e.Value) || Object.Equals(null, e.Value) || Object.Equals(DBNull.Value, e.Value))
                {
                    e.Valid = false;
                    e.ErrorText = view.FocusedColumn.Caption + "不能为空！";
                }
            }
        }

        private void gv_Detail_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            var view = sender as DevExpress.XtraGrid.Views.Grid.GridView;

            bool V = true;

            foreach (DevExpress.XtraGrid.Columns.GridColumn column in view.Columns)
            {
                if (Object.Equals(column.Tag, "NotNull"))
                {
                    if (String.IsNullOrEmpty(view.GetFocusedRowCellDisplayText(column)))
                    {
                        view.SetColumnError(column, column.Caption + "不能为空!");
                        V = V & false;
                    }
                }
            }
            e.Valid = V;
        }

        private void gv_Detail_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            //去掉验证行失败时弹出的确认框
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }


        #region 其他常用
        //绑定明细页数据
        public override void DoBoundEditData()
        {
            //base.DoBoundEditData();
            LibraryTools.DoBindingEditorPanel(pan_Summary, EditData.Tables[_bll.SummaryModel.TableName], "txt");
            this.gc_Detail_CU_I_ORDER_LIST.DataSource = EditData.Tables[CU_I_ORDER_LIST._TableName];
            this.gv_Detail_CU_I_ORDER_LIST.BestFitColumns();
            //数据绑定，两个地方任选一处都可以
            EditData.Tables[_bll.SummaryModel.TableName].Rows[0][CU_I_ORDER_HEAD.BUYER_ID_TYPE] = CU_CERT_TYPE.CU_CERT_TYPE_1;
            EditData.Tables[_bll.SummaryModel.TableName].Rows[0][CU_I_ORDER_HEAD.CURRENCY] = CU_CURRENCY.CU_CURRENCY_142;
            EditData.Tables[_bll.SummaryModel.TableName].Rows[0][CU_I_ORDER_HEAD.ORDER_TYPE] = Type_FLAG.Type_FLAG_I;

            if (CurrentDataState == FormDataState.Add)
            {
                EditData.Tables[_bll.SummaryModel.TableName].Rows[0][CU_I_ORDER_HEAD.GUID] = BillNoGenerator.NewOrderBillNo();
                Library.DataBinderTools.Bound.DoBindingEntity(EditData.Tables[_bll.SummaryModel.TableName].Rows[0]);

                this.txtBUYER_ID_TYPE.EditValue = CU_CERT_TYPE.CU_CERT_TYPE_1;
                this.txtCURRENCY.EditValue = CU_CURRENCY.CU_CURRENCY_142;
                this.txtORDER_TYPE.EditValue = Type_FLAG.Type_FLAG_I;
            }
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
                return base.CustomerAuthority + FunctionAuthority.EX_04 + FunctionAuthorityCommon.Export + FunctionAuthority.EX_05 + FunctionAuthority.EX_06;//扩展申报;
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
            if (CurrentAuthorityExist(FunctionAuthority.EX_06))//如果有扩展权限
            {
                var btn = new RibbonItemButton()
                {
                    name = "GetBillNo",
                    Caption = "取号",
                    ImgFileName = "download_32px.png",
                    BeginGroup = false,
                    Shortcut = Keys.None//这里是快捷键
                };
                InsertAfterButton(ButtonNames.btnExport, btn, FunctionAuthority.EX_06, "取号", DoGetBill);
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
                    frmCuOrderFlow status = new frmCuOrderFlow();
                    status.ShowForm(Key);
                }
            }
        }

        private void gv_Detail_CU_I_ORDER_LIST_RowClick(object sender, RowClickEventArgs e)
        {
            bool flag = false;
            if (CurrentDataState == FormDataState.Edit || CurrentDataState == FormDataState.Add)
            {
                flag = true;
            }
            GridView gridView = (GridView)sender;
            DataRow listrow = gridView.GetFocusedDataRow(); 
            var v = frmCuOrderList.ShowForm(null, flag, listrow, null, ParaDataset);
            if (v != null)
            {
                CommonTools.CopyDatarowValue(v, listrow);
                gridView.UpdateCurrentRow();
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

            DevExpress.XtraGrid.Columns.GridColumn GVColumn = gvMainData.Columns["GOODS_VALUE"];
            GVColumn.SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "总货值：{0:C2}");

            DevExpress.XtraGrid.Columns.GridColumn FreightColumn = gvMainData.Columns["FREIGHT"];
            FreightColumn.SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "总运费：{0:C2}");

            DevExpress.XtraGrid.Columns.GridColumn DiscountColumn = gvMainData.Columns["DISCOUNT"];
            DiscountColumn.SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "总优免：{0:C2}");

            DevExpress.XtraGrid.Columns.GridColumn TaxtotalColumn = gvMainData.Columns["TAXTOTAL"];
            TaxtotalColumn.SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "总税款：{0:C2}");

            DevExpress.XtraGrid.Columns.GridColumn ActuralColumn = gvMainData.Columns["ACTURAL_PAID"];
            ActuralColumn.SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "总实付：{0:C2}");
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
        private void DoDeclare(object sender, ItemClickEventArgs e )
        {
            base.DoDeclare(sender);
        }

        /// <summary>
        /// 取号
        /// </summary>
        private void DoGetBill(object sender, ItemClickEventArgs e)
        {
            base.DoGetBill(sender);
        }

        /// <summary>
        /// 导入
        /// </summary>
        private void DoImport(object sender, ItemClickEventArgs e)
        {
            EditData = frmBaseExcel.ShowForm(CU_I_ORDER_HEAD._TableName);
            if (EditData != null && EditData.Tables.Count > 0)
                base.DoImport(sender);
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
            Lists = bll.GetDateList(((System.Data.DataView)_SummaryView.DataSource).Table, CU_I_ORDER_LIST._TableName, CU_I_ORDER_LIST.GUID,CU_I_ORDER_HEAD.ORDER_NO,CU_I_ORDER_HEAD._TableName);
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
