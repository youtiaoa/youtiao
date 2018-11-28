using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GZFramework.DB.Lib;
using GZFramework.UI.Dev.Common;
using GZFramework.UI.Dev.LibForm;
using CBSP.Business;
using static CBSP.Business.ParaSql;
using DevExpress.XtraEditors;
using GZFramework.UI.Core;
using GZFramework.UI.Dev;
using CBSP.Library.Config.RibbonButtons;
using DevExpress.XtraBars;
using CBSP.Models.Import;

namespace CBSP.Import.ImportOthers
{
    public partial class frmCuStatus : frmBaseDataBusiness
    {
        bllCuStatus bll;
        public frmCuStatus()
        {
            InitializeComponent();
            this.Load += frm_Load;
            //实例化必须，bllBusinessBase必须替换为bll层自己继承的子类(指定正确的dal.DBCode)，建议封装重写到项目bll层

            _bll = bll = new bllCuStatus();
        }
        private void frm_Load(object sender, EventArgs e)
        {
            _SummaryView = gvMainData;//必须赋值
            base.AddControlsOnAddKey();

            ParaDataset = bll.CusParaDateset_CuStatus();

            this.txts_ORDER_STATUS.Properties.DataSource = ParaDataset.Tables[CusParaName.PARA_PLAT_STATUS];
            this.txts_WAYBILL_STATUS.Properties.DataSource = ParaDataset.Tables[CusParaName.PARA_PLAT_STATUS];
            this.txts_DECLARE_STATUS.Properties.DataSource = ParaDataset.Tables[CusParaName.PARA_PLAT_STATUS];

            Library.DataBinderTools.Bound.BoundCommonDictDataID(lueStatus, Business.CustomerEnum.EnumCommonDicData.平台状态, false, false);

            base.AddControlsOnlyRead();
        }
        //查询
        private void btn_Search_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            if (!String.IsNullOrEmpty(txts_ORDER_NO.Text)) dic.Add("ORDER_NO", txts_ORDER_NO.Text);
            if (!String.IsNullOrEmpty(txts_ORDER_STATUS.Text)) dic.Add("ORDER_STATUS", txts_ORDER_STATUS.EditValue);
            if (!String.IsNullOrEmpty(txts_WAYBILL_STATUS.Text)) dic.Add("WAYBILL_STATUS", txts_WAYBILL_STATUS.EditValue);
            if (!String.IsNullOrEmpty(txts_DECLARE_STATUS.Text)) dic.Add("DECLARE_STATUS", txts_DECLARE_STATUS.EditValue);

            DataTable dt = bll.Search(dic);

            gcMainData.DataSource = dt;
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
            bool Validate = true;

            ;


            return Validate;
        }


        #region 其他常用
        //绑定明细页数据
        public override void DoBoundEditData()
        {
            //base.DoBoundEditData();
            LibraryTools.DoBindingEditorPanel(pan_Summary, EditData.Tables[_bll.SummaryModel.TableName], "txt");

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
                return base.CustomerAuthority + FunctionAuthorityCommon.ADD + FunctionAuthorityCommon.FVIEW + FunctionAuthorityCommon.EDIT - FunctionAuthorityCommon.DELETE + FunctionAuthority.EX_04;
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
                    name = "DeleteByExcel",
                    Caption = "导入删除",
                    ImgFileName = "import_32px.ico",
                    BeginGroup = false,
                    Shortcut = Keys.None//这里是快捷键
                };
                InsertAfterButton(ButtonNames.btnDelete, btn, FunctionAuthority.EX_04, "导入删除", DeleteByExcel);
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
            try
            {
                string list = "(''";
                int[] rows = _SummaryView.GetSelectedRows();
                if (rows.Length < 1)
                {
                    Msg.Warning("请选择至少1条数据！");
                    return;
                }
                else if (chs_Check_Order.Checked || chs_Check_Waybill.Checked || chs_Check_Declare.Checked || chs_Check_EciqOrder.Checked)
                {
                    if (Msg.AskQuestion("确定要删除选中记录吗？") == false) return;
                    WaiteServer.ShowWaite(this);
                    EditData = GetEditData("");
                    int sucess = 0;
                    foreach (int k in rows)
                    {
                        string Key = ConvertEx.ToString(_SummaryView.GetDataRow(k)[_bll.SummaryModel.PrimaryKey]);
                        DataRow dt = _SummaryView.GetDataRow(k);
                        //是否允许删除
                        if (IsDeclare(dt["DECLARE_STATUS"].ToString()))
                        {
                            sucess++;
                            list += ",'" + Key + "'";
                        }
                    }
                    list += ")";
                    bll.DeleteByOrderNo(list, chs_Check_Order.Checked, chs_Check_Waybill.Checked, chs_Check_Declare.Checked, chs_Check_EciqOrder.Checked);

                    WaiteServer.CloseWaite();
                    RefreshDataCache();//刷新缓存数据
                    Msg.ShowInformation("删除完成！\r\n 共" + rows.Length + "条,删除" + sucess + "条。");
                }
                else
                {
                    Msg.Warning("请选择至少1项单据项！");
                    return;
                    //base.DoDelete(sender);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }

        private void DeleteByExcel(object sender, ItemClickEventArgs e)
        {
            try
            {
                EditData = frmBaseExcel.ShowForm(vw_Cu_Status._TableName);
                int count = EditData.Tables[vw_Cu_Status._TableName].Rows.Count;
                if (EditData.Tables[vw_Cu_Status._TableName] != null && count > 0)
                {
                    if (chs_Check_Order.Checked || chs_Check_Waybill.Checked || chs_Check_Declare.Checked || chs_Check_EciqOrder.Checked)
                    {
                        string list = "(''";
                        foreach (DataRow row in EditData.Tables[vw_Cu_Status._TableName].Rows)
                        {
                            string Key = row[vw_Cu_Status.ORDER_NO].ToString();
                            list += ",'" + Key + "'";
                        }
                        list += ")";

                        //返回不允许删除的订单列表
                        DataTable dt = bll.IsDelete(list);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                list = list.Replace(row[0].ToString(), "");
                            }
                        }

                        bll.DeleteByOrderNo(list, chs_Check_Order.Checked, chs_Check_Waybill.Checked, chs_Check_Declare.Checked, chs_Check_EciqOrder.Checked);

                        WaiteServer.CloseWaite();
                        RefreshDataCache();//刷新缓存数据
                        Msg.ShowInformation("删除完成！\r\n 共" + count + "条,删除" + (count - dt.Rows.Count) + "条。");

                    }
                    else
                    {
                        Msg.Warning("请选择至少1项单据项！");
                        return;
                        //base.DoDelete(sender);
                    }
                }
                else
                {
                    Msg.Warning("表格中未读取到内容！");
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            
        }

        /// <summary>
        /// 修改
        /// </summary>
        protected override void DoEdit(object sender)
        {
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
