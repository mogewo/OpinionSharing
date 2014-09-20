namespace OpinionSharingForm.GUI
{
    partial class AgentStatePanel
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
            this.WhiteButton = new System.Windows.Forms.Button();
            this.BlackButton = new System.Windows.Forms.Button();
            this.Candidates_CB = new System.Windows.Forms.ComboBox();
            this.IDLabel = new System.Windows.Forms.Label();
            this.AlgorithmLabel = new System.Windows.Forms.Label();
            this.TargetAwarenessRateLabel = new System.Windows.Forms.Label();
            this.otherStates = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // WhiteButton
            // 
            this.WhiteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.WhiteButton.Location = new System.Drawing.Point(117, 24);
            this.WhiteButton.Name = "WhiteButton";
            this.WhiteButton.Size = new System.Drawing.Size(46, 20);
            this.WhiteButton.TabIndex = 4;
            this.WhiteButton.Text = "White";
            this.WhiteButton.UseVisualStyleBackColor = true;
            this.WhiteButton.Click += new System.EventHandler(this.WhiteButton_Click);
            // 
            // BlackButton
            // 
            this.BlackButton.Location = new System.Drawing.Point(5, 24);
            this.BlackButton.Name = "BlackButton";
            this.BlackButton.Size = new System.Drawing.Size(46, 20);
            this.BlackButton.TabIndex = 3;
            this.BlackButton.Text = "Black";
            this.BlackButton.UseVisualStyleBackColor = true;
            this.BlackButton.Click += new System.EventHandler(this.BlackButton_Click);
            // 
            // Candidates_CB
            // 
            this.Candidates_CB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Candidates_CB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.Candidates_CB.FormattingEnabled = true;
            this.Candidates_CB.Location = new System.Drawing.Point(5, 72);
            this.Candidates_CB.Name = "Candidates_CB";
            this.Candidates_CB.Size = new System.Drawing.Size(158, 187);
            this.Candidates_CB.TabIndex = 6;
            this.Candidates_CB.SelectedIndexChanged += new System.EventHandler(this.Candidates_CB_SelectedIndexChanged);
            // 
            // IDLabel
            // 
            this.IDLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IDLabel.AutoSize = true;
            this.IDLabel.Location = new System.Drawing.Point(3, 9);
            this.IDLabel.Name = "IDLabel";
            this.IDLabel.Size = new System.Drawing.Size(16, 12);
            this.IDLabel.TabIndex = 7;
            this.IDLabel.Text = "ID";
            // 
            // AlgorithmLabel
            // 
            this.AlgorithmLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.AlgorithmLabel.AutoSize = true;
            this.AlgorithmLabel.Location = new System.Drawing.Point(43, 9);
            this.AlgorithmLabel.Name = "AlgorithmLabel";
            this.AlgorithmLabel.Size = new System.Drawing.Size(54, 12);
            this.AlgorithmLabel.TabIndex = 8;
            this.AlgorithmLabel.Text = "Algorithm";
            // 
            // TargetAwarenessRateLabel
            // 
            this.TargetAwarenessRateLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.TargetAwarenessRateLabel.AutoSize = true;
            this.TargetAwarenessRateLabel.Location = new System.Drawing.Point(115, 9);
            this.TargetAwarenessRateLabel.Name = "TargetAwarenessRateLabel";
            this.TargetAwarenessRateLabel.Size = new System.Drawing.Size(29, 12);
            this.TargetAwarenessRateLabel.TabIndex = 9;
            this.TargetAwarenessRateLabel.Text = "h_trg";
            // 
            // otherStates
            // 
            this.otherStates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otherStates.Location = new System.Drawing.Point(5, 256);
            this.otherStates.Multiline = true;
            this.otherStates.Name = "otherStates";
            this.otherStates.Size = new System.Drawing.Size(158, 54);
            this.otherStates.TabIndex = 10;
            // 
            // AgentStatePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.otherStates);
            this.Controls.Add(this.TargetAwarenessRateLabel);
            this.Controls.Add(this.AlgorithmLabel);
            this.Controls.Add(this.IDLabel);
            this.Controls.Add(this.Candidates_CB);
            this.Controls.Add(this.WhiteButton);
            this.Controls.Add(this.BlackButton);
            this.Name = "AgentStatePanel";
            this.Size = new System.Drawing.Size(166, 310);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BlackButton;
        private System.Windows.Forms.Button WhiteButton;
        private System.Windows.Forms.ComboBox Candidates_CB;
        private System.Windows.Forms.Label IDLabel;
        private System.Windows.Forms.Label AlgorithmLabel;
        private System.Windows.Forms.Label TargetAwarenessRateLabel;
        private System.Windows.Forms.TextBox otherStates;
    }
}
