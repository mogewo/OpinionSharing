namespace OpinionSharingForm.GUI
{
    partial class FigurePanel
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.showSensorNetworkCB = new System.Windows.Forms.CheckBox();
            this.showOnlySensorNetworkCB = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // showSensorNetworkCB
            // 
            this.showSensorNetworkCB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.showSensorNetworkCB.AutoSize = true;
            this.showSensorNetworkCB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showSensorNetworkCB.Location = new System.Drawing.Point(193, 300);
            this.showSensorNetworkCB.Name = "showSensorNetworkCB";
            this.showSensorNetworkCB.Size = new System.Drawing.Size(136, 16);
            this.showSensorNetworkCB.TabIndex = 0;
            this.showSensorNetworkCB.Text = "Show Sensor Network";
            this.showSensorNetworkCB.UseVisualStyleBackColor = true;
            this.showSensorNetworkCB.CheckedChanged += new System.EventHandler(this.showSensorNetworkCB_CheckedChanged);
            // 
            // showOnlySensorNetworkCB
            // 
            this.showOnlySensorNetworkCB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.showOnlySensorNetworkCB.AutoSize = true;
            this.showOnlySensorNetworkCB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.showOnlySensorNetworkCB.Location = new System.Drawing.Point(193, 316);
            this.showOnlySensorNetworkCB.Name = "showOnlySensorNetworkCB";
            this.showOnlySensorNetworkCB.Size = new System.Drawing.Size(47, 16);
            this.showOnlySensorNetworkCB.TabIndex = 1;
            this.showOnlySensorNetworkCB.Text = "Only";
            this.showOnlySensorNetworkCB.UseVisualStyleBackColor = true;
            this.showOnlySensorNetworkCB.CheckedChanged += new System.EventHandler(this.showOnlySensorNetworkCB_CheckedChanged);
            // 
            // FigurePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.showOnlySensorNetworkCB);
            this.Controls.Add(this.showSensorNetworkCB);
            this.DoubleBuffered = true;
            this.Name = "FigurePanel";
            this.Size = new System.Drawing.Size(332, 332);
            this.Click += new System.EventHandler(this.FigurePanel_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FigurePanel_Paint);
            this.Resize += new System.EventHandler(this.FigurePanel_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox showSensorNetworkCB;
        private System.Windows.Forms.CheckBox showOnlySensorNetworkCB;

    }
}
