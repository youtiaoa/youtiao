namespace CBSP.Customs.ImportDeclare
{
    partial class frmCuDeclareFlow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCuDeclareFlow));
            this.gcMainData = new DevExpress.XtraGrid.GridControl();
            this.gvMainData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gc_STATUS_SOURCE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lueStatusSouce = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gc_STATUS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lueStatus = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gc_STATUS_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gc_STATUS_DESC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gc_RECE_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gc_FILE_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gc_UPLOAD_FILE_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gcMainData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMainData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueStatusSouce)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // gcMainData
            // 
            this.gcMainData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcMainData.Location = new System.Drawing.Point(0, 0);
            this.gcMainData.MainView = this.gvMainData;
            this.gcMainData.Name = "gcMainData";
            this.gcMainData.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.lueStatusSouce,
            this.lueStatus});
            this.gcMainData.Size = new System.Drawing.Size(715, 271);
            this.gcMainData.TabIndex = 2;
            this.gcMainData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMainData});
            // 
            // gvMainData
            // 
            this.gvMainData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gc_STATUS_SOURCE,
            this.gc_STATUS,
            this.gc_STATUS_DATE,
            this.gc_STATUS_DESC,
            this.gc_RECE_DATE,
            this.gc_FILE_NAME,
            this.gc_UPLOAD_FILE_NAME});
            this.gvMainData.GridControl = this.gcMainData;
            this.gvMainData.Name = "gvMainData";
            this.gvMainData.OptionsBehavior.Editable = false;
            this.gvMainData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvMainData.OptionsView.ColumnAutoWidth = false;
            this.gvMainData.OptionsView.ShowGroupPanel = false;
            // 
            // gc_STATUS_SOURCE
            // 
            this.gc_STATUS_SOURCE.Caption = "状态来源";
            this.gc_STATUS_SOURCE.ColumnEdit = this.lueStatusSouce;
            this.gc_STATUS_SOURCE.FieldName = "STATUS_SOURCE";
            this.gc_STATUS_SOURCE.Name = "gc_STATUS_SOURCE";
            this.gc_STATUS_SOURCE.Visible = true;
            this.gc_STATUS_SOURCE.VisibleIndex = 0;
            // 
            // lueStatusSouce
            // 
            this.lueStatusSouce.AutoHeight = false;
            this.lueStatusSouce.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueStatusSouce.Name = "lueStatusSouce";
            // 
            // gc_STATUS
            // 
            this.gc_STATUS.Caption = "状态";
            this.gc_STATUS.ColumnEdit = this.lueStatus;
            this.gc_STATUS.FieldName = "STATUS";
            this.gc_STATUS.Name = "gc_STATUS";
            this.gc_STATUS.Visible = true;
            this.gc_STATUS.VisibleIndex = 1;
            // 
            // lueStatus
            // 
            this.lueStatus.AutoHeight = false;
            this.lueStatus.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueStatus.Name = "lueStatus";
            // 
            // gc_STATUS_DATE
            // 
            this.gc_STATUS_DATE.Caption = "状态时间";
            this.gc_STATUS_DATE.DisplayFormat.FormatString = "MM-dd HH:mm:ss";
            this.gc_STATUS_DATE.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gc_STATUS_DATE.FieldName = "STATUS_DATE";
            this.gc_STATUS_DATE.Name = "gc_STATUS_DATE";
            this.gc_STATUS_DATE.Visible = true;
            this.gc_STATUS_DATE.VisibleIndex = 2;
            // 
            // gc_STATUS_DESC
            // 
            this.gc_STATUS_DESC.Caption = "状态描述";
            this.gc_STATUS_DESC.FieldName = "STATUS_DESC";
            this.gc_STATUS_DESC.Name = "gc_STATUS_DESC";
            this.gc_STATUS_DESC.Visible = true;
            this.gc_STATUS_DESC.VisibleIndex = 3;
            // 
            // gc_RECE_DATE
            // 
            this.gc_RECE_DATE.Caption = "接收时间";
            this.gc_RECE_DATE.DisplayFormat.FormatString = "MM-dd HH:mm:ss";
            this.gc_RECE_DATE.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gc_RECE_DATE.FieldName = "RECE_DATE";
            this.gc_RECE_DATE.Name = "gc_RECE_DATE";
            this.gc_RECE_DATE.Visible = true;
            this.gc_RECE_DATE.VisibleIndex = 4;
            // 
            // gc_FILE_NAME
            // 
            this.gc_FILE_NAME.Caption = "接收文件";
            this.gc_FILE_NAME.FieldName = "FILE_NAME";
            this.gc_FILE_NAME.Name = "gc_FILE_NAME";
            this.gc_FILE_NAME.Visible = true;
            this.gc_FILE_NAME.VisibleIndex = 5;
            // 
            // gc_UPLOAD_FILE_NAME
            // 
            this.gc_UPLOAD_FILE_NAME.Caption = "上传文件";
            this.gc_UPLOAD_FILE_NAME.FieldName = "UPLOAD_FILE_NAME";
            this.gc_UPLOAD_FILE_NAME.Name = "gc_UPLOAD_FILE_NAME";
            this.gc_UPLOAD_FILE_NAME.Visible = true;
            this.gc_UPLOAD_FILE_NAME.VisibleIndex = 6;
            // 
            // frmCuDeclareFlow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 271);
            this.Controls.Add(this.gcMainData);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCuDeclareFlow";
            this.Text = "统一版清单状态流";
            ((System.ComponentModel.ISupportInitialize)(this.gcMainData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMainData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueStatusSouce)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueStatus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcMainData;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMainData;
        private DevExpress.XtraGrid.Columns.GridColumn gc_STATUS_SOURCE;
        private DevExpress.XtraGrid.Columns.GridColumn gc_STATUS;
        private DevExpress.XtraGrid.Columns.GridColumn gc_STATUS_DATE;
        private DevExpress.XtraGrid.Columns.GridColumn gc_STATUS_DESC;
        private DevExpress.XtraGrid.Columns.GridColumn gc_RECE_DATE;
        private DevExpress.XtraGrid.Columns.GridColumn gc_FILE_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn gc_UPLOAD_FILE_NAME;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lueStatusSouce;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lueStatus;
    }
}