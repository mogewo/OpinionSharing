namespace OpinionSharingForm
{
    partial class MyForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 0.6D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 0.3D);
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint3 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(0D, 0.1D);
            this.AnimationTimer = new System.Windows.Forms.Timer(this.components);
            this.SplitContainer = new System.Windows.Forms.SplitContainer();
            this.AccuracyChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.expAccuracyBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.SettingTabControl = new System.Windows.Forms.TabControl();
            this.NetworkTab = new System.Windows.Forms.TabPage();
            this.GeneratorCB = new System.Windows.Forms.ComboBox();
            this.PrepareButton = new System.Windows.Forms.Button();
            this.SensorNumTB = new System.Windows.Forms.TextBox();
            this.AgentNumLabel = new System.Windows.Forms.Label();
            this.AgentNumTB = new System.Windows.Forms.TextBox();
            this.SeedLabel = new System.Windows.Forms.Label();
            this.SensorNumLabel = new System.Windows.Forms.Label();
            this.PRewireLabel = new System.Windows.Forms.Label();
            this.ExpectedDegreeLabel = new System.Windows.Forms.Label();
            this.SeedTB = new System.Windows.Forms.TextBox();
            this.ExpectedDegreeTB = new System.Windows.Forms.TextBox();
            this.PRewireTB = new System.Windows.Forms.TextBox();
            this.LearningTab = new System.Windows.Forms.TabPage();
            this.SetAlgo_Button = new System.Windows.Forms.Button();
            this.TargetAwarenessRateLabel = new System.Windows.Forms.Label();
            this.LearningProcess = new System.Windows.Forms.ProgressBar();
            this.AlgoLabel = new System.Windows.Forms.Label();
            this.TargetAwarenessRatTB = new System.Windows.Forms.TextBox();
            this.AlgoCB = new System.Windows.Forms.ComboBox();
            this.LearningStart = new System.Windows.Forms.Button();
            this.Animation = new System.Windows.Forms.TabPage();
            this.SeedUpDown = new System.Windows.Forms.NumericUpDown();
            this.StepButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.WhiteRadio = new System.Windows.Forms.RadioButton();
            this.AnimationSeedLabel = new System.Windows.Forms.Label();
            this.BlackRadio = new System.Windows.Forms.RadioButton();
            this.InitButton = new System.Windows.Forms.Button();
            this.FactLabel = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.StartButton = new System.Windows.Forms.Button();
            this.Step = new System.Windows.Forms.Label();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.figurePanel = new OpinionSharingForm.GUI.FigurePanel();
            this.agentStatePanel = new OpinionSharingForm.GUI.AgentStatePanel();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).BeginInit();
            this.SplitContainer.Panel1.SuspendLayout();
            this.SplitContainer.Panel2.SuspendLayout();
            this.SplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AccuracyChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.expAccuracyBindingSource)).BeginInit();
            this.SettingTabControl.SuspendLayout();
            this.NetworkTab.SuspendLayout();
            this.LearningTab.SuspendLayout();
            this.Animation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SeedUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // AnimationTimer
            // 
            this.AnimationTimer.Tick += new System.EventHandler(this.AnimationTimer_Tick);
            // 
            // SplitContainer
            // 
            this.SplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.SplitContainer.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer.Name = "SplitContainer";
            // 
            // SplitContainer.Panel1
            // 
            this.SplitContainer.Panel1.Controls.Add(this.figurePanel);
            this.SplitContainer.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.SplitContainer_Panel1_Paint);
            // 
            // SplitContainer.Panel2
            // 
            this.SplitContainer.Panel2.Controls.Add(this.AccuracyChart);
            this.SplitContainer.Panel2.Controls.Add(this.agentStatePanel);
            this.SplitContainer.Panel2.Controls.Add(this.SettingTabControl);
            this.SplitContainer.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.SplitContainer_Panel2_Paint);
            this.SplitContainer.Size = new System.Drawing.Size(1451, 707);
            this.SplitContainer.SplitterDistance = 1037;
            this.SplitContainer.TabIndex = 12;
            // 
            // AccuracyChart
            // 
            this.AccuracyChart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.AccuracyChart.ChartAreas.Add(chartArea1);
            this.AccuracyChart.DataSource = this.expAccuracyBindingSource;
            legend1.Name = "Legend1";
            this.AccuracyChart.Legends.Add(legend1);
            this.AccuracyChart.Location = new System.Drawing.Point(38, 523);
            this.AccuracyChart.Name = "AccuracyChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1.IsXValueIndexed = true;
            series1.LabelBackColor = System.Drawing.Color.Transparent;
            series1.Legend = "Legend1";
            series1.Name = "Accuracy";
            dataPoint1.AxisLabel = "Correct";
            dataPoint1.LabelBackColor = System.Drawing.Color.Transparent;
            dataPoint2.AxisLabel = "Incorrect";
            dataPoint2.LabelBackColor = System.Drawing.Color.Transparent;
            dataPoint3.AxisLabel = "Undeter";
            dataPoint3.LabelBackColor = System.Drawing.Color.Transparent;
            series1.Points.Add(dataPoint1);
            series1.Points.Add(dataPoint2);
            series1.Points.Add(dataPoint3);
            this.AccuracyChart.Series.Add(series1);
            this.AccuracyChart.Size = new System.Drawing.Size(372, 190);
            this.AccuracyChart.TabIndex = 4;
            this.AccuracyChart.Text = "chart1";
            this.AccuracyChart.Click += new System.EventHandler(this.AccuracyChart_Click);
            // 
            // expAccuracyBindingSource
            // 
            this.expAccuracyBindingSource.DataSource = typeof(OpinionSharing.Env.ExpAccuracy);
            // 
            // SettingTabControl
            // 
            this.SettingTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SettingTabControl.Controls.Add(this.NetworkTab);
            this.SettingTabControl.Controls.Add(this.LearningTab);
            this.SettingTabControl.Controls.Add(this.Animation);
            this.SettingTabControl.Location = new System.Drawing.Point(38, 12);
            this.SettingTabControl.Name = "SettingTabControl";
            this.SettingTabControl.SelectedIndex = 0;
            this.SettingTabControl.Size = new System.Drawing.Size(372, 158);
            this.SettingTabControl.TabIndex = 3;
            // 
            // NetworkTab
            // 
            this.NetworkTab.Controls.Add(this.GeneratorCB);
            this.NetworkTab.Controls.Add(this.PrepareButton);
            this.NetworkTab.Controls.Add(this.SensorNumTB);
            this.NetworkTab.Controls.Add(this.AgentNumLabel);
            this.NetworkTab.Controls.Add(this.AgentNumTB);
            this.NetworkTab.Controls.Add(this.SeedLabel);
            this.NetworkTab.Controls.Add(this.SensorNumLabel);
            this.NetworkTab.Controls.Add(this.PRewireLabel);
            this.NetworkTab.Controls.Add(this.ExpectedDegreeLabel);
            this.NetworkTab.Controls.Add(this.SeedTB);
            this.NetworkTab.Controls.Add(this.ExpectedDegreeTB);
            this.NetworkTab.Controls.Add(this.PRewireTB);
            this.NetworkTab.Location = new System.Drawing.Point(4, 22);
            this.NetworkTab.Name = "NetworkTab";
            this.NetworkTab.Padding = new System.Windows.Forms.Padding(3);
            this.NetworkTab.Size = new System.Drawing.Size(364, 132);
            this.NetworkTab.TabIndex = 0;
            this.NetworkTab.Text = "Network";
            this.NetworkTab.UseVisualStyleBackColor = true;
            this.NetworkTab.Click += new System.EventHandler(this.NetworkTab_Click);
            // 
            // GeneratorCB
            // 
            this.GeneratorCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GeneratorCB.FormattingEnabled = true;
            this.GeneratorCB.Items.AddRange(new object[] {
            "BA",
            "WS",
            "Random",
            "Leader"});
            this.GeneratorCB.Location = new System.Drawing.Point(102, 57);
            this.GeneratorCB.Name = "GeneratorCB";
            this.GeneratorCB.Size = new System.Drawing.Size(105, 20);
            this.GeneratorCB.TabIndex = 17;
            this.GeneratorCB.SelectedIndexChanged += new System.EventHandler(this.GeneratorCB_SelectedIndexChanged);
            // 
            // PrepareButton
            // 
            this.PrepareButton.Location = new System.Drawing.Point(117, 100);
            this.PrepareButton.Name = "PrepareButton";
            this.PrepareButton.Size = new System.Drawing.Size(75, 20);
            this.PrepareButton.TabIndex = 5;
            this.PrepareButton.Text = "Prepare";
            this.PrepareButton.UseVisualStyleBackColor = true;
            this.PrepareButton.Click += new System.EventHandler(this.PrepareButton_Click);
            // 
            // SensorNumTB
            // 
            this.SensorNumTB.Location = new System.Drawing.Point(67, 31);
            this.SensorNumTB.Name = "SensorNumTB";
            this.SensorNumTB.Size = new System.Drawing.Size(25, 19);
            this.SensorNumTB.TabIndex = 7;
            this.SensorNumTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.settings_KeyDown);
            // 
            // AgentNumLabel
            // 
            this.AgentNumLabel.AutoSize = true;
            this.AgentNumLabel.Location = new System.Drawing.Point(3, 7);
            this.AgentNumLabel.Name = "AgentNumLabel";
            this.AgentNumLabel.Size = new System.Drawing.Size(58, 12);
            this.AgentNumLabel.TabIndex = 8;
            this.AgentNumLabel.Text = "AgentNum";
            // 
            // AgentNumTB
            // 
            this.AgentNumTB.Location = new System.Drawing.Point(67, 4);
            this.AgentNumTB.Name = "AgentNumTB";
            this.AgentNumTB.Size = new System.Drawing.Size(25, 19);
            this.AgentNumTB.TabIndex = 6;
            this.AgentNumTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.settings_KeyDown);
            // 
            // SeedLabel
            // 
            this.SeedLabel.AutoSize = true;
            this.SeedLabel.Location = new System.Drawing.Point(98, 34);
            this.SeedLabel.Name = "SeedLabel";
            this.SeedLabel.Size = new System.Drawing.Size(30, 12);
            this.SeedLabel.TabIndex = 14;
            this.SeedLabel.Text = "Seed";
            // 
            // SensorNumLabel
            // 
            this.SensorNumLabel.AutoSize = true;
            this.SensorNumLabel.Location = new System.Drawing.Point(3, 34);
            this.SensorNumLabel.Name = "SensorNumLabel";
            this.SensorNumLabel.Size = new System.Drawing.Size(63, 12);
            this.SensorNumLabel.TabIndex = 9;
            this.SensorNumLabel.Text = "SensorNum";
            // 
            // PRewireLabel
            // 
            this.PRewireLabel.AutoSize = true;
            this.PRewireLabel.Location = new System.Drawing.Point(98, 7);
            this.PRewireLabel.Name = "PRewireLabel";
            this.PRewireLabel.Size = new System.Drawing.Size(46, 12);
            this.PRewireLabel.TabIndex = 13;
            this.PRewireLabel.Text = "p_rewire";
            // 
            // ExpectedDegreeLabel
            // 
            this.ExpectedDegreeLabel.AutoSize = true;
            this.ExpectedDegreeLabel.Location = new System.Drawing.Point(4, 61);
            this.ExpectedDegreeLabel.Name = "ExpectedDegreeLabel";
            this.ExpectedDegreeLabel.Size = new System.Drawing.Size(23, 12);
            this.ExpectedDegreeLabel.TabIndex = 4;
            this.ExpectedDegreeLabel.Text = "<d>";
            // 
            // SeedTB
            // 
            this.SeedTB.Location = new System.Drawing.Point(167, 31);
            this.SeedTB.Name = "SeedTB";
            this.SeedTB.Size = new System.Drawing.Size(25, 19);
            this.SeedTB.TabIndex = 12;
            this.SeedTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.settings_KeyDown);
            // 
            // ExpectedDegreeTB
            // 
            this.ExpectedDegreeTB.Location = new System.Drawing.Point(67, 58);
            this.ExpectedDegreeTB.Name = "ExpectedDegreeTB";
            this.ExpectedDegreeTB.Size = new System.Drawing.Size(25, 19);
            this.ExpectedDegreeTB.TabIndex = 10;
            this.ExpectedDegreeTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.settings_KeyDown);
            // 
            // PRewireTB
            // 
            this.PRewireTB.Location = new System.Drawing.Point(167, 4);
            this.PRewireTB.Name = "PRewireTB";
            this.PRewireTB.Size = new System.Drawing.Size(25, 19);
            this.PRewireTB.TabIndex = 11;
            this.PRewireTB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.settings_KeyDown);
            // 
            // LearningTab
            // 
            this.LearningTab.Controls.Add(this.SetAlgo_Button);
            this.LearningTab.Controls.Add(this.TargetAwarenessRateLabel);
            this.LearningTab.Controls.Add(this.LearningProcess);
            this.LearningTab.Controls.Add(this.AlgoLabel);
            this.LearningTab.Controls.Add(this.TargetAwarenessRatTB);
            this.LearningTab.Controls.Add(this.AlgoCB);
            this.LearningTab.Controls.Add(this.LearningStart);
            this.LearningTab.Location = new System.Drawing.Point(4, 22);
            this.LearningTab.Name = "LearningTab";
            this.LearningTab.Padding = new System.Windows.Forms.Padding(3);
            this.LearningTab.Size = new System.Drawing.Size(1024, 132);
            this.LearningTab.TabIndex = 1;
            this.LearningTab.Text = "Learning";
            this.LearningTab.UseVisualStyleBackColor = true;
            // 
            // SetAlgo_Button
            // 
            this.SetAlgo_Button.Location = new System.Drawing.Point(6, 48);
            this.SetAlgo_Button.Name = "SetAlgo_Button";
            this.SetAlgo_Button.Size = new System.Drawing.Size(196, 23);
            this.SetAlgo_Button.TabIndex = 17;
            this.SetAlgo_Button.Text = "Set Algorithm";
            this.SetAlgo_Button.UseVisualStyleBackColor = true;
            this.SetAlgo_Button.Click += new System.EventHandler(this.SetAlgo_Button_Click);
            // 
            // TargetAwarenessRateLabel
            // 
            this.TargetAwarenessRateLabel.AutoSize = true;
            this.TargetAwarenessRateLabel.Location = new System.Drawing.Point(120, 3);
            this.TargetAwarenessRateLabel.Name = "TargetAwarenessRateLabel";
            this.TargetAwarenessRateLabel.Size = new System.Drawing.Size(29, 12);
            this.TargetAwarenessRateLabel.TabIndex = 13;
            this.TargetAwarenessRateLabel.Text = "h_trg";
            this.TargetAwarenessRateLabel.Click += new System.EventHandler(this.TargetAwarenessRateLabel_Click);
            // 
            // LearningProcess
            // 
            this.LearningProcess.Location = new System.Drawing.Point(6, 106);
            this.LearningProcess.Name = "LearningProcess";
            this.LearningProcess.Size = new System.Drawing.Size(196, 20);
            this.LearningProcess.TabIndex = 2;
            // 
            // AlgoLabel
            // 
            this.AlgoLabel.AutoSize = true;
            this.AlgoLabel.BackColor = System.Drawing.SystemColors.Control;
            this.AlgoLabel.Location = new System.Drawing.Point(3, 3);
            this.AlgoLabel.Name = "AlgoLabel";
            this.AlgoLabel.Size = new System.Drawing.Size(54, 12);
            this.AlgoLabel.TabIndex = 15;
            this.AlgoLabel.Text = "Algorithm";
            // 
            // TargetAwarenessRatTB
            // 
            this.TargetAwarenessRatTB.Location = new System.Drawing.Point(122, 18);
            this.TargetAwarenessRatTB.Name = "TargetAwarenessRatTB";
            this.TargetAwarenessRatTB.Size = new System.Drawing.Size(68, 19);
            this.TargetAwarenessRatTB.TabIndex = 14;
            this.TargetAwarenessRatTB.Text = "0.9";
            this.TargetAwarenessRatTB.TextChanged += new System.EventHandler(this.TargetAwarenessRatTB_TextChanged);
            // 
            // AlgoCB
            // 
            this.AlgoCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AlgoCB.FormattingEnabled = true;
            this.AlgoCB.Location = new System.Drawing.Point(6, 18);
            this.AlgoCB.Name = "AlgoCB";
            this.AlgoCB.Size = new System.Drawing.Size(105, 20);
            this.AlgoCB.TabIndex = 16;
            this.AlgoCB.SelectedIndexChanged += new System.EventHandler(this.AlgoCB_SelectedIndexChanged);
            // 
            // LearningStart
            // 
            this.LearningStart.Location = new System.Drawing.Point(6, 77);
            this.LearningStart.Name = "LearningStart";
            this.LearningStart.Size = new System.Drawing.Size(196, 23);
            this.LearningStart.TabIndex = 0;
            this.LearningStart.Text = "LearningStart";
            this.LearningStart.UseVisualStyleBackColor = true;
            this.LearningStart.Click += new System.EventHandler(this.LearningStart_Click);
            // 
            // Animation
            // 
            this.Animation.BackColor = System.Drawing.Color.White;
            this.Animation.Controls.Add(this.SeedUpDown);
            this.Animation.Controls.Add(this.StepButton);
            this.Animation.Controls.Add(this.label1);
            this.Animation.Controls.Add(this.WhiteRadio);
            this.Animation.Controls.Add(this.AnimationSeedLabel);
            this.Animation.Controls.Add(this.BlackRadio);
            this.Animation.Controls.Add(this.InitButton);
            this.Animation.Controls.Add(this.FactLabel);
            this.Animation.Controls.Add(this.trackBar1);
            this.Animation.Controls.Add(this.StartButton);
            this.Animation.Controls.Add(this.Step);
            this.Animation.Location = new System.Drawing.Point(4, 22);
            this.Animation.Name = "Animation";
            this.Animation.Size = new System.Drawing.Size(1024, 132);
            this.Animation.TabIndex = 2;
            this.Animation.Text = "Animation";
            // 
            // SeedUpDown
            // 
            this.SeedUpDown.Location = new System.Drawing.Point(44, 16);
            this.SeedUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.SeedUpDown.Name = "SeedUpDown";
            this.SeedUpDown.Size = new System.Drawing.Size(58, 19);
            this.SeedUpDown.TabIndex = 3;
            // 
            // StepButton
            // 
            this.StepButton.Location = new System.Drawing.Point(76, 59);
            this.StepButton.Name = "StepButton";
            this.StepButton.Size = new System.Drawing.Size(24, 23);
            this.StepButton.TabIndex = 12;
            this.StepButton.Text = ">";
            this.StepButton.UseVisualStyleBackColor = true;
            this.StepButton.Click += new System.EventHandler(this.StepButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 12);
            this.label1.TabIndex = 21;
            this.label1.Text = "Speed";
            // 
            // WhiteRadio
            // 
            this.WhiteRadio.AutoSize = true;
            this.WhiteRadio.Checked = true;
            this.WhiteRadio.Location = new System.Drawing.Point(44, 39);
            this.WhiteRadio.Name = "WhiteRadio";
            this.WhiteRadio.Size = new System.Drawing.Size(51, 16);
            this.WhiteRadio.TabIndex = 13;
            this.WhiteRadio.TabStop = true;
            this.WhiteRadio.Text = "White";
            this.WhiteRadio.UseVisualStyleBackColor = true;
            this.WhiteRadio.CheckedChanged += new System.EventHandler(this.WhiteRadio_CheckedChanged);
            // 
            // AnimationSeedLabel
            // 
            this.AnimationSeedLabel.AutoSize = true;
            this.AnimationSeedLabel.Location = new System.Drawing.Point(7, 18);
            this.AnimationSeedLabel.Name = "AnimationSeedLabel";
            this.AnimationSeedLabel.Size = new System.Drawing.Size(30, 12);
            this.AnimationSeedLabel.TabIndex = 20;
            this.AnimationSeedLabel.Text = "Seed";
            // 
            // BlackRadio
            // 
            this.BlackRadio.AutoSize = true;
            this.BlackRadio.Location = new System.Drawing.Point(118, 37);
            this.BlackRadio.Name = "BlackRadio";
            this.BlackRadio.Size = new System.Drawing.Size(52, 16);
            this.BlackRadio.TabIndex = 14;
            this.BlackRadio.Text = "Black";
            this.BlackRadio.UseVisualStyleBackColor = true;
            this.BlackRadio.CheckedChanged += new System.EventHandler(this.BlackRadio_CheckedChanged);
            // 
            // InitButton
            // 
            this.InitButton.Location = new System.Drawing.Point(9, 59);
            this.InitButton.Name = "InitButton";
            this.InitButton.Size = new System.Drawing.Size(29, 23);
            this.InitButton.TabIndex = 18;
            this.InitButton.Text = "■";
            this.InitButton.UseVisualStyleBackColor = true;
            this.InitButton.Click += new System.EventHandler(this.InitButton_Click);
            // 
            // FactLabel
            // 
            this.FactLabel.AutoSize = true;
            this.FactLabel.Location = new System.Drawing.Point(6, 41);
            this.FactLabel.Name = "FactLabel";
            this.FactLabel.Size = new System.Drawing.Size(28, 12);
            this.FactLabel.TabIndex = 3;
            this.FactLabel.Text = "Fact";
            // 
            // trackBar1
            // 
            this.trackBar1.AutoSize = false;
            this.trackBar1.BackColor = System.Drawing.Color.White;
            this.trackBar1.Location = new System.Drawing.Point(44, 88);
            this.trackBar1.Maximum = 1000;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(144, 27);
            this.trackBar1.SmallChange = 50;
            this.trackBar1.TabIndex = 17;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar1.Value = 150;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(44, 59);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(26, 23);
            this.StartButton.TabIndex = 0;
            this.StartButton.Text = ">>";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.start_Click);
            // 
            // Step
            // 
            this.Step.AutoSize = true;
            this.Step.Location = new System.Drawing.Point(116, 64);
            this.Step.Name = "Step";
            this.Step.Size = new System.Drawing.Size(28, 12);
            this.Step.TabIndex = 2;
            this.Step.Text = "Step";
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // figurePanel
            // 
            this.figurePanel.AgentStatePanel = null;
            this.figurePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.figurePanel.DrawLinks = true;
            this.figurePanel.Environment = null;
            this.figurePanel.Location = new System.Drawing.Point(3, 0);
            this.figurePanel.Name = "figurePanel";
            this.figurePanel.Size = new System.Drawing.Size(1031, 704);
            this.figurePanel.TabIndex = 0;
            // 
            // agentStatePanel
            // 
            this.agentStatePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.agentStatePanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.agentStatePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.agentStatePanel.Location = new System.Drawing.Point(38, 176);
            this.agentStatePanel.Name = "agentStatePanel";
            this.agentStatePanel.Size = new System.Drawing.Size(372, 341);
            this.agentStatePanel.TabIndex = 1;
            this.agentStatePanel.Load += new System.EventHandler(this.agentStatePanel_Load);
            // 
            // MyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1451, 707);
            this.Controls.Add(this.SplitContainer);
            this.Name = "MyForm";
            this.Text = "OpinionSharing";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MyForm_FormClosed);
            this.SplitContainer.Panel1.ResumeLayout(false);
            this.SplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).EndInit();
            this.SplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AccuracyChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.expAccuracyBindingSource)).EndInit();
            this.SettingTabControl.ResumeLayout(false);
            this.NetworkTab.ResumeLayout(false);
            this.NetworkTab.PerformLayout();
            this.LearningTab.ResumeLayout(false);
            this.LearningTab.PerformLayout();
            this.Animation.ResumeLayout(false);
            this.Animation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SeedUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Label Step;
        private System.Windows.Forms.Label FactLabel;
        private System.Windows.Forms.Label ExpectedDegreeLabel;
        private System.Windows.Forms.Button PrepareButton;
        private System.Windows.Forms.TextBox AgentNumTB;
        private System.Windows.Forms.TextBox SensorNumTB;
        private System.Windows.Forms.Label AgentNumLabel;
        private System.Windows.Forms.Label SensorNumLabel;
        private System.Windows.Forms.TextBox ExpectedDegreeTB;
        private System.Windows.Forms.SplitContainer SplitContainer;
        private System.Windows.Forms.ProgressBar LearningProcess;
        private System.Windows.Forms.Button LearningStart;
        private System.Windows.Forms.TextBox TargetAwarenessRatTB;
        private System.Windows.Forms.Label TargetAwarenessRateLabel;
        private System.Windows.Forms.Button StepButton;
        private GUI.AgentStatePanel agentStatePanel;
        private System.Windows.Forms.Label SeedLabel;
        private System.Windows.Forms.Label PRewireLabel;
        private System.Windows.Forms.TextBox SeedTB;
        private System.Windows.Forms.TextBox PRewireTB;
        private System.Windows.Forms.RadioButton BlackRadio;
        private System.Windows.Forms.RadioButton WhiteRadio;
        private System.Windows.Forms.TrackBar trackBar1;

        private System.Windows.Forms.Label AlgoLabel;
        private System.Windows.Forms.ComboBox AlgoCB;
        private System.Windows.Forms.Button InitButton;
        private System.Windows.Forms.Timer AnimationTimer;
        private System.Windows.Forms.Label AnimationSeedLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown SeedUpDown;
        private System.Windows.Forms.TabControl SettingTabControl;
        private System.Windows.Forms.TabPage NetworkTab;
        private System.Windows.Forms.TabPage LearningTab;
        private System.Windows.Forms.TabPage Animation;
        private System.Windows.Forms.DataVisualization.Charting.Chart AccuracyChart;
        private System.Windows.Forms.BindingSource expAccuracyBindingSource;
        private System.Windows.Forms.Button SetAlgo_Button;
        private System.Windows.Forms.ComboBox GeneratorCB;
        private GUI.FigurePanel figurePanel;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
    }
}