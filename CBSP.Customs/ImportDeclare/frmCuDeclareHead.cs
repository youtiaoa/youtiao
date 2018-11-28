using CBSP.Business;
using CBSP.Library.Config.RibbonButtons;
using CBSP.Models.Import;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using GZFramework.UI.Core;
using GZFramework.UI.Dev;
using GZFramework.UI.Dev.Common;
using GZFramework.UI.Dev.LibForm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static CBSP.Business.ConstantInfo;
using static CBSP.Business.ParaSql;

namespace CBSP.Customs.ImportDeclare
{
    public partial class frmCuDeclareHead : frmBaseDataBusiness
    {
        bllImDeclare bll;
        public frmCuDeclareHead()
        {
            InitializeComponent();
            this.Load += frm_Load;
            //实例化必须，bllBusinessBase必须替换为bll层自己继承的子类(指定正确的dal.DBCode)，建议封装重写到项目bll层

            _bll = bll = new bllImDeclare();
        }
        private void frm_Load(object sender, EventArgs e)
        {
            _SummaryView = gvMainData;//必须赋值
            base.AddControlsOnAddKey();

            base.AddControlsOnlyRead(this.txtCOP_NO, this.txtBUYER_ID_TYPE, this.txtCURRENCY, txtPRE_NO, txtINVT_NO);

            ParaDataset = bll.CusParaDateset_Declare();

            this.txts_RETURN_STATUS.Properties.DataSource = ParaDataset.Tables[CusParaName.PARA_CU_RESPONSE_STATUS];
            this.txtLOCT_NO.Properties.DataSource = ParaDataset.Tables[CusParaName.PARA_SUPER_PLACE];
            this.txtWRAP_TYPE.Properties.DataSource = ParaDataset.Tables[CusParaName.PARA_WRAP_TYPE];
            this.txtCOUNTRY.Properties.DataSource = ParaDataset.Tables[CusParaName.PARA_COUNTRY];
            this.txtTRAF_MODE.Properties.DataSource = ParaDataset.Tables[CusParaName.PARA_CU_TRAF_MODE];
            this.txtTRADE_MODE.Properties.DataSource = ParaDataset.Tables[CusParaName.PARA_CU_TRADE_MODE];
            this.txtPORT_CODE.Properties.DataSource = ParaDataset.Tables[CusParaName.PARA_CUSTOMS];
            this.txtBUYER_ID_TYPE.Properties.DataSource = ParaDataset.Tables[CusParaName.CERT_TYPE];
            this.txtCURRENCY.Properties.DataSource = ParaDataset.Tables[CusParaName.CURR];
            this.txtAREA_CODE.Properties.DataSource = ParaDataset.Tables[CusParaName.COP_TYPE_R];
            this.txtAGENT_CODE.Properties.DataSource = ParaDataset.Tables[CusParaName.COP_TYPE_A];
            this.txtASSURE_CODE.Properties.DataSource = ParaDataset.Tables[CusParaName.COP_TYPE_A];
            this.txtLOGISTICS_CODE.Properties.DataSource = ParaDataset.Tables[CusParaName.COP_TYPE_L];
            this.txtEBC_CODE.Properties.DataSource = ParaDataset.Tables[CusParaName.COP_TYPE_E];
            this.txtEBP_CODE.Properties.DataSource = ParaDataset.Tables[CusParaName.COP_TYPE_EP];
            this.txtCUSTOMS_CODE.Properties.DataSource = ParaDataset.Tables[CusParaName.PARA_CUSTOMS];
            this.txts_TRADE_MODE.Properties.DataSource = ParaDataset.Tables[CusParaName.PARA_CU_TRADE_MODE];
            this.txts_STATUS.Properties.DataSource = ParaDataset.Tables[CusParaName.PARA_PLAT_STATUS];

            Library.DataBinderTools.Bound.BoundCommonDictDataID(lueIdType, Business.CustomerEnum.EnumCommonDicData.证件类型, false, false);
            Library.DataBinderTools.Bound.BoundCommonDictDataID(lueInputFlag, Business.CustomerEnum.EnumCommonDicData.录入标志, false, false);
            Library.DataBinderTools.Bound.BoundCommonDictDataID(lueReturnStatus, Business.CustomerEnum.EnumCommonDicData.海关状态, false, false);
            Library.DataBinderTools.Bound.BoundCommonDictDataID(lueStatus, Business.CustomerEnum.EnumCommonDicData.平台状态, false, false);
            Library.DataBinderTools.Bound.BoundCommonDictDataID(lueUploadFlag, Business.CustomerEnum.EnumCommonDicData.上传标志, false, false);
            Library.DataBinderTools.Bound.BoundCommonDictDataID(lueTradeMode, Business.CustomerEnum.EnumCommonDicData.贸易方式, false, false);
            Library.DataBinderTools.Bound.BoundCommonDictDataID(luePrint, Business.CustomerEnum.EnumCommonDicData.上载标志, false, false);

            txts_STAR_TIME.EditValue = DateTime.Now.AddDays(-1);
        }
        //查询
        private void btn_Search_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            if (!String.IsNullOrEmpty(txts_TRADE_MODE.Text)) dic.Add("TRADE_MODE", txts_TRADE_MODE.EditValue);
            if (!String.IsNullOrEmpty(txts_ORDER_NO.Text)) dic.Add("ORDER_NO", txts_ORDER_NO.Text);
            if (!String.IsNullOrEmpty(txts_STATUS.Text)) dic.Add("STATUS", txts_STATUS.EditValue);
            if (!String.IsNullOrEmpty(txts_RETURN_STATUS.Text)) dic.Add("RETURN_STATUS", txts_RETURN_STATUS.EditValue);
            if (!String.IsNullOrEmpty(txts_STAR_TIME.Text)) dic.Add("STAR_TIME", txts_STAR_TIME.Text);
            if (!String.IsNullOrEmpty(txts_END_TIME.Text)) dic.Add("END_TIME", txts_END_TIME.Text);

            DataTable dt = bll.GetDateList(dic,CU_I_DECLFORM_HEAD._TableName);

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
                       & LibraryTools.IsNotEmpBaseEdit(txtCOP_NO, "内部编号不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtORDER_NO, "订单编号不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtEBP_CODE, "电商平台不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtEBC_CODE, "电商企业不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtLOGISTICS_NO, "运单编号不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtLOGISTICS_CODE, "物流企业不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtASSURE_CODE, "担保企业不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtDECL_TIME, "申报日期不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtCUSTOMS_CODE, "申报海关不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtPORT_CODE, "口岸海关不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtBUYER_ID_TYPE, "订购人证件类型不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtBUYER_ID_NUMBER, "订购人证件号不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtBUYER_NAME, "订购人姓名不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtBUYER_TELEPHONE, "订购人电话不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtCONSIGNEE_ADDRESS, "收件地址不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtAGENT_CODE, "申报企业不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtTRADE_MODE, "贸易方式不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtTRAF_MODE, "运输方式不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtCOUNTRY, "起运国不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtFREIGHT, "运费不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtINSURED_FEE, "保费不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtCURRENCY, "币值不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtPACK_NO, "件数不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtGROSS_WEIGHT, "毛重不能为空！")
                       & LibraryTools.IsNotEmpBaseEdit(txtNET_WEIGHT, "净重不能为空！")
                       &LibraryTools.IsNotEmpBaseEdit(txtIE_DATE, "进出口日期不能为空！")
                       &LibraryTools.IsNotEmpBaseEdit(txtWRAP_TYPE, "包装种类代码不能为空！");

            if (txtTRADE_MODE.EditValue.ToString() == CU_TRADE_MODE.CU_TRADE_MODE_1210)
            {
                Validate = Validate & LibraryTools.IsNotEmpBaseEdit(txtEMS_NO, "账册编号(保税)不能为空！")
                    & LibraryTools.IsNotEmpBaseEdit(txtAREA_CODE, "区内企业(保税)不能为空！");
            }
            if(txtTRADE_MODE.EditValue.ToString() == CU_TRADE_MODE.CU_TRADE_MODE_9610)
            {
                Validate = Validate & LibraryTools.IsNotEmpBaseEdit(txtTRAF_MODE, "运输工具编号(直购)不能为空！")
                    & LibraryTools.IsNotEmpBaseEdit(txtVOYAGE_NO, "航班航次(直购)不能为空！")
                    & LibraryTools.IsNotEmpBaseEdit(txtBILL_NO, "提运单号(直购)不能为空！");
            }
            Validate = Validate & !gv_Detail_CU_I_DECLFORM_LIST.HasColumnErrors & gv_Detail_CU_I_DECLFORM_LIST.ValidateEditor();
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
                var count = (EditData.Tables[CU_I_DECLFORM_LIST._TableName].Rows.Count + 1).ToString();
                var v = frmCuDeclareList.ShowForm(count, false, null, EditData.Tables[_bll.SummaryModel.TableName].Rows[0][CU_I_ORDER_HEAD.GUID].ToString(),ParaDataset);
                if (v != null)
                {
                    EditData.Tables[CU_I_DECLFORM_LIST._TableName].Rows.Add(v.ItemArray);
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
            this.gc_Detail_CU_I_DECLFORM_LIST.DataSource = EditData.Tables[CU_I_DECLFORM_LIST._TableName];
            this.gv_Detail_CU_I_DECLFORM_LIST.BestFitColumns();

            //数据绑定，两个地方任选一处都可以
            EditData.Tables[_bll.SummaryModel.TableName].Rows[0][CU_I_DECLFORM_HEAD.BUYER_ID_TYPE] = CU_CERT_TYPE.CU_CERT_TYPE_1;
            EditData.Tables[_bll.SummaryModel.TableName].Rows[0][CU_I_DECLFORM_HEAD.CURRENCY] = CU_CURRENCY.CU_CURRENCY_142;

            if (CurrentDataState == FormDataState.Add)
            {
                EditData.Tables[_bll.SummaryModel.TableName].Rows[0][CU_I_DECLFORM_HEAD.GUID] = BillNoGenerator.NewDeclformBillNo();
                EditData.Tables[_bll.SummaryModel.TableName].Rows[0][CU_I_DECLFORM_HEAD.COP_NO] = BillNoGenerator.NewDeclformCopNo();
                Library.DataBinderTools.Bound.DoBindingEntity(EditData.Tables[_bll.SummaryModel.TableName].Rows[0]);

                this.txtBUYER_ID_TYPE.EditValue = CU_CERT_TYPE.CU_CERT_TYPE_1;
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
                return base.CustomerAuthority + FunctionAuthority.EX_04 + FunctionAuthorityCommon.Export + FunctionAuthority.EX_05 + FunctionAuthority.EX_07;//扩展申报;
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
            if (CurrentAuthorityExist(FunctionAuthority.EX_07))//如果有扩展权限
            {
                var btn = new RibbonItemButton()
                {
                    name = "Print",
                    Caption = "打印",
                    ImgFileName = "print_printer_32px.png",
                    BeginGroup = false,
                    Shortcut = Keys.None//这里是快捷键
                };
                InsertAfterButton(ButtonNames.btnExport, btn, FunctionAuthority.EX_07, "打印", DoPrint);
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
                    frmCuDeclareFlow status = new frmCuDeclareFlow();
                    status.ShowForm(Key);

                }
            }
        }
        private void gv_Detail_RowClick(object sender, RowClickEventArgs e)
        {
            bool flag = false;
            if (CurrentDataState == FormDataState.Edit || CurrentDataState == FormDataState.Add)
            {
                flag = true;
            }
            GridView gridView = (GridView)sender;
            DataRow listrow = gridView.GetFocusedDataRow();
            var v = frmCuDeclareList.ShowForm(null, flag, listrow, null, ParaDataset);
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

            DevExpress.XtraGrid.Columns.GridColumn FreightColumn = gvMainData.Columns["FREIGHT"];
            FreightColumn.SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "总运费：{0:C2}");

            DevExpress.XtraGrid.Columns.GridColumn GrossWeightColumn = gvMainData.Columns["GROSS_WEIGHT"];
            GrossWeightColumn.SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "总毛重：{0:N2}");

            DevExpress.XtraGrid.Columns.GridColumn NetWeightColumn = gvMainData.Columns["NET_WEIGHT"];
            NetWeightColumn.SummaryItem.SetSummary(DevExpress.Data.SummaryItemType.Sum, "总净重：{0:N2}");

            DevExpress.XtraGrid.Columns.GridColumn InsureColumn = gvMainData.Columns["INSURED_FEE"];
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
        /// 申报
        /// </summary>
        private void DoDeclare(object sender, ItemClickEventArgs e)
        {
            base.DoDeclare(sender);
        }

        /// <summary>
        /// 审核
        /// </summary>
        protected override void DoApproval(object sender)
        {
            base.DoApproval(sender);
        }

        /// <summary>
        /// 导入
        /// </summary>
        private void DoImport(object sender, ItemClickEventArgs e)
        {
            EditData = frmBaseExcel.ShowForm(CU_I_DECLFORM_HEAD._TableName);
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
        /// 打印
        /// </summary>
        private void DoPrint(object sender, ItemClickEventArgs e)
        {
            base.DoPrint(sender);

            FrxDataSet dataset = new FrxDataSet();
            FillDataSetWithSampleData(dataset);
            if(dataset.Tables.Count > 0 && dataset.Tables["frxEms"].Rows.Count > 0)
            {
                if (PrintService(dataset))
                {
                    bll.UpdatePrint();
                }
            }
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
            Lists = bll.GetDateList(((System.Data.DataView)_SummaryView.DataSource).Table, CU_I_DECLFORM_LIST._TableName, CU_I_DECLFORM_LIST.GUID, CU_I_DECLFORM_HEAD.ORDER_NO,CU_I_DECLFORM_HEAD._TableName);
            base.DoExport(sender);
        }

        //为FrxDataSet加载数据   
        private void FillDataSetWithSampleData(FrxDataSet dataset)
        {

            SqlConnection conn = new SqlConnection("server=27.223.91.14;database=CBSP_Business;UID=sa;PWD=world$2016");
            SqlDataAdapter da = new SqlDataAdapter("select * from vw_Print ", conn);
            da.Fill(dataset, "frxEms");
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
