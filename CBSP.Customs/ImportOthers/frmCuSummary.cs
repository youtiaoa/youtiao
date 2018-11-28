using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using GZFramework.UI.Dev.Common;
using GZFramework.UI.Dev.LibForm;
using static CBSP.Business.ParaSql;
using DevExpress.XtraEditors;
using GZFramework.UI.Core;
using CBSP.Business.Others;
using GZFramework.UI.Dev;
using CBSP.UI.Dev.Common;

namespace CBSP.Import.ImportOthers
{
    public partial class frmCuSummary : frmBaseDataBusiness
    {
        bllCuSummary bll;
        public frmCuSummary()
        {
            InitializeComponent();
            this.Load += frm_Load;
            //实例化必须，bllBusinessBase必须替换为bll层自己继承的子类(指定正确的dal.DBCode)，建议封装重写到项目bll层

            _bll = bll = new bllCuSummary();
        }
        private void frm_Load(object sender, EventArgs e)
        {
            _SummaryView = gvMainData;//必须赋值
            base.AddControlsOnAddKey();

            ParaDataset = bll.CusParaDateset_CuSummary();

            this.txts_RETURN_STATUS.Properties.DataSource = ParaDataset.Tables[CusParaName.PARA_CU_RESPONSE_STATUS];

            base.AddControlsOnlyRead();

            txts_STAR_TIME.EditValue = DateTime.Now.AddDays(-1);
        }
        //查询
        private void btn_Search_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            if (!String.IsNullOrEmpty(txts_G_NAME.Text)) dic.Add("G_NAME", txts_G_NAME.Text);
            if (!String.IsNullOrEmpty(txts_RETURN_STATUS.Text)) dic.Add("RETURN_STATUS", txts_RETURN_STATUS.EditValue);
            if (!String.IsNullOrEmpty(txts_STAR_TIME.Text)) dic.Add("STAR_TIME", txts_STAR_TIME.Text);
            if (!String.IsNullOrEmpty(txts_END_TIME.Text)) dic.Add("END_TIME", txts_END_TIME.Text);

            DataTable dt = bll.GetSearch(dic);

            gcMainData.DataSource = dt;
            //if (gvMainData.RowCount < 100)//行数过多会很耗时
                gvMainData.BestFitColumns();
        }
        //清空条件
        private void btn_Clear_Click(object sender, EventArgs e)
        {
            LibraryTools.DoClearPanel(LC_Search);
        }


        #region 其他常用

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
                return base.CustomerAuthority + FunctionAuthorityCommon.ADD + FunctionAuthorityCommon.FVIEW + FunctionAuthorityCommon.EDIT - FunctionAuthorityCommon.DELETE + FunctionAuthorityCommon.Export;
            }
        }

        //自定义窗体权限按钮
        public override void IniButton()
        {
            base.IniButton();

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
            DataRow dr = _SummaryView.GetFocusedDataRow();
            if (dr == null)
            {
                Msg.Warning("没有选择数据！");
                return;
            }

            CurrentDataState = FormDataState.View;//设置状态
            string Key = ConvertEx.ToString(dr[_bll.SummaryModel.PrimaryKey]);
            //2015年10月21日11:55:26 当双击不同记录的时候再重新获取数据，避免重复读取数据库
            bool Repeat = EditData != null && Object.Equals(Key, EditData.Tables[_bll.SummaryModel.TableName].Rows[0][_bll.SummaryModel.PrimaryKey]);
            if (!Repeat)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                if (!String.IsNullOrEmpty(txts_RETURN_STATUS.Text)) dic.Add("RETURN_STATUS", txts_RETURN_STATUS.EditValue);
                if (!String.IsNullOrEmpty(txts_STAR_TIME.Text)) dic.Add("STAR_TIME", txts_STAR_TIME.EditValue);
                if (!String.IsNullOrEmpty(txts_END_TIME.Text)) dic.Add("END_TIME", txts_END_TIME.EditValue);
                if (!String.IsNullOrEmpty(Key)) dic.Add("G_NAME", ",'" + Key + "'");

                DataTable dt = bll.GetListData(dic);//获得需要绑定的数据
                this.gc_Detail_CU_I_DECLFORM_LIST.DataSource = dt;//绑定数据
                this.gv_Detail_CU_I_DECLFORM_LIST.BestFitColumns();//绑定数据
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        protected override void DoRefresh(object sender)
        {
            base.DoRefresh(sender);
        }

        /// <summary>
        /// 返回取消
        /// </summary>
        protected override void DoCancel(object sender)
        {
            base.DoCancel(sender);
        }


        /// <summary>
        /// 导出数据
        /// </summary>
        protected override void DoExport(object sender)
        {
            DataTable dt = new DataTable();

            int[] rows = _SummaryView.GetSelectedRows();
            if (rows.Length < 1)
            {
                Msg.Warning("请选择至少1条数据！");
                return;
            }
            else
            {
                WaiteServer.ShowWaite(this);

                int sucess = 0;

                Dictionary<string, object> dic = new Dictionary<string, object>();

                if (!String.IsNullOrEmpty(txts_RETURN_STATUS.Text)) dic.Add("RETURN_STATUS", txts_RETURN_STATUS.EditValue);
                if (!String.IsNullOrEmpty(txts_STAR_TIME.Text)) dic.Add("STAR_TIME", txts_STAR_TIME.EditValue);
                if (!String.IsNullOrEmpty(txts_END_TIME.Text)) dic.Add("END_TIME", txts_END_TIME.EditValue);

                foreach (int k in rows)
                {
                    string Key = ConvertEx.ToString(_SummaryView.GetDataRow(k)[_bll.SummaryModel.PrimaryKey]);

                    if (!String.IsNullOrEmpty(Key)) dic.Add("G_NAME" + sucess.ToString(), ",'" + Key + "'");

                    sucess++;
                }

                dt = bll.GetListData(dic);//获得需要绑定的数据

            }

            if (DataToExcel.ExportToExcel(dt, "商品汇总", Lists))
            {
                WaiteServer.CloseWaite();
                Msg.ShowInformation("导出成功！");
            }

            WaiteServer.CloseWaite();
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

        private void gc_Detail_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {

        }
    }
}
