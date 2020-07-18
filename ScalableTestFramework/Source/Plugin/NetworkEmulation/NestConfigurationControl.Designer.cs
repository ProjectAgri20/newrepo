namespace HP.ScalableTest.Plugin.NetworkEmulation
{
    partial class NestConfigurationControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NestConfigurationControl));
            this.tabControlNewt = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.description_richTextBox = new System.Windows.Forms.RichTextBox();
            this.comboBoxBWUpStream = new System.Windows.Forms.ComboBox();
            this.textBoxBWUpStream = new System.Windows.Forms.TextBox();
            this.upstream_label = new System.Windows.Forms.Label();
            this.comboBoxBWDownStream = new System.Windows.Forms.ComboBox();
            this.downstream_label = new System.Windows.Forms.Label();
            this.textBoxBWDownStream = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.latencyvalue_label = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.latencyNDDev_label = new System.Windows.Forms.Label();
            this.textBoxLatencyNormalDev = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.latencyNDAvg_label = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxLatencyNormalAvg = new System.Windows.Forms.TextBox();
            this.radioButtonLatencyNormal = new System.Windows.Forms.RadioButton();
            this.latencyUDMax_label = new System.Windows.Forms.Label();
            this.textBoxLatencyUniformMax = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.latencyUDMin_label = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxLatencyUniformMin = new System.Windows.Forms.TextBox();
            this.radioButtonLatencyUniform = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxLatencyFixed = new System.Windows.Forms.TextBox();
            this.radioButtonLatencyFixed = new System.Windows.Forms.RadioButton();
            this.radioButtonLatencyNo = new System.Windows.Forms.RadioButton();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.loss_rate_value_label = new System.Windows.Forms.Label();
            this.radioButtonLossNo = new System.Windows.Forms.RadioButton();
            this.textBoxLossRate = new System.Windows.Forms.TextBox();
            this.loss_periodic_packet_label = new System.Windows.Forms.Label();
            this.loss_rate_label = new System.Windows.Forms.Label();
            this.radioButtonLossRandom = new System.Windows.Forms.RadioButton();
            this.textBoxLossPeriodic = new System.Windows.Forms.TextBox();
            this.loss_periodic_label = new System.Windows.Forms.Label();
            this.radioButtonLossPeriodic = new System.Windows.Forms.RadioButton();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.error_rate_value_label = new System.Windows.Forms.Label();
            this.textBoxErrorRate = new System.Windows.Forms.TextBox();
            this.error_rate_label = new System.Windows.Forms.Label();
            this.radioButtonErrorRandom = new System.Windows.Forms.RadioButton();
            this.radioButtonErrorNo = new System.Windows.Forms.RadioButton();
            this.comboBox_networkprofiles = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControlNewt.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlNewt
            // 
            this.tabControlNewt.Controls.Add(this.tabPage1);
            this.tabControlNewt.Controls.Add(this.tabPage2);
            this.tabControlNewt.Controls.Add(this.tabPage3);
            this.tabControlNewt.Controls.Add(this.tabPage4);
            this.tabControlNewt.Location = new System.Drawing.Point(15, 69);
            this.tabControlNewt.Name = "tabControlNewt";
            this.tabControlNewt.SelectedIndex = 0;
            this.tabControlNewt.Size = new System.Drawing.Size(464, 318);
            this.tabControlNewt.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.description_richTextBox);
            this.tabPage1.Controls.Add(this.comboBoxBWUpStream);
            this.tabPage1.Controls.Add(this.textBoxBWUpStream);
            this.tabPage1.Controls.Add(this.upstream_label);
            this.tabPage1.Controls.Add(this.comboBoxBWDownStream);
            this.tabPage1.Controls.Add(this.downstream_label);
            this.tabPage1.Controls.Add(this.textBoxBWDownStream);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(456, 292);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Bandwidth";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // description_richTextBox
            // 
            this.description_richTextBox.Location = new System.Drawing.Point(55, 120);
            this.description_richTextBox.Name = "description_richTextBox";
            this.description_richTextBox.ReadOnly = true;
            this.description_richTextBox.Size = new System.Drawing.Size(368, 96);
            this.description_richTextBox.TabIndex = 7;
            this.description_richTextBox.Text = resources.GetString("description_richTextBox.Text");
            // 
            // comboBoxBWUpStream
            // 
            this.comboBoxBWUpStream.FormattingEnabled = true;
            this.comboBoxBWUpStream.Items.AddRange(new object[] {
            "Mbps",
            "Kbps"});
            this.comboBoxBWUpStream.Location = new System.Drawing.Point(241, 64);
            this.comboBoxBWUpStream.Name = "comboBoxBWUpStream";
            this.comboBoxBWUpStream.Size = new System.Drawing.Size(87, 21);
            this.comboBoxBWUpStream.TabIndex = 5;
            // 
            // textBoxBWUpStream
            // 
            this.textBoxBWUpStream.Location = new System.Drawing.Point(124, 65);
            this.textBoxBWUpStream.Name = "textBoxBWUpStream";
            this.textBoxBWUpStream.Size = new System.Drawing.Size(100, 20);
            this.textBoxBWUpStream.TabIndex = 4;
            // 
            // upstream_label
            // 
            this.upstream_label.AutoSize = true;
            this.upstream_label.Location = new System.Drawing.Point(52, 68);
            this.upstream_label.Name = "upstream_label";
            this.upstream_label.Size = new System.Drawing.Size(52, 13);
            this.upstream_label.TabIndex = 3;
            this.upstream_label.Text = "Upstream";
            // 
            // comboBoxBWDownStream
            // 
            this.comboBoxBWDownStream.FormattingEnabled = true;
            this.comboBoxBWDownStream.Items.AddRange(new object[] {
            "Mbps",
            "Kbps"});
            this.comboBoxBWDownStream.Location = new System.Drawing.Point(241, 24);
            this.comboBoxBWDownStream.Name = "comboBoxBWDownStream";
            this.comboBoxBWDownStream.Size = new System.Drawing.Size(87, 21);
            this.comboBoxBWDownStream.TabIndex = 2;
            // 
            // downstream_label
            // 
            this.downstream_label.AutoSize = true;
            this.downstream_label.Location = new System.Drawing.Point(52, 27);
            this.downstream_label.Name = "downstream_label";
            this.downstream_label.Size = new System.Drawing.Size(66, 13);
            this.downstream_label.TabIndex = 1;
            this.downstream_label.Text = "Downstream";
            // 
            // textBoxBWDownStream
            // 
            this.textBoxBWDownStream.Location = new System.Drawing.Point(124, 25);
            this.textBoxBWDownStream.Name = "textBoxBWDownStream";
            this.textBoxBWDownStream.Size = new System.Drawing.Size(100, 20);
            this.textBoxBWDownStream.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.latencyvalue_label);
            this.tabPage2.Controls.Add(this.textBox1);
            this.tabPage2.Controls.Add(this.latencyNDDev_label);
            this.tabPage2.Controls.Add(this.textBoxLatencyNormalDev);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.latencyNDAvg_label);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.textBoxLatencyNormalAvg);
            this.tabPage2.Controls.Add(this.radioButtonLatencyNormal);
            this.tabPage2.Controls.Add(this.latencyUDMax_label);
            this.tabPage2.Controls.Add(this.textBoxLatencyUniformMax);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.latencyUDMin_label);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.textBoxLatencyUniformMin);
            this.tabPage2.Controls.Add(this.radioButtonLatencyUniform);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.textBoxLatencyFixed);
            this.tabPage2.Controls.Add(this.radioButtonLatencyFixed);
            this.tabPage2.Controls.Add(this.radioButtonLatencyNo);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(456, 292);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Latency";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // latencyvalue_label
            // 
            this.latencyvalue_label.AutoSize = true;
            this.latencyvalue_label.Location = new System.Drawing.Point(210, 83);
            this.latencyvalue_label.Name = "latencyvalue_label";
            this.latencyvalue_label.Size = new System.Drawing.Size(20, 13);
            this.latencyvalue_label.TabIndex = 19;
            this.latencyvalue_label.Text = "ms";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(35, 235);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(389, 39);
            this.textBox1.TabIndex = 18;
            this.textBox1.Text = "NOTE:The latency applies to both upstream and downstream, so this will result in " +
    "2x latency of entered values.";
            // 
            // latencyNDDev_label
            // 
            this.latencyNDDev_label.AutoSize = true;
            this.latencyNDDev_label.Location = new System.Drawing.Point(374, 203);
            this.latencyNDDev_label.Name = "latencyNDDev_label";
            this.latencyNDDev_label.Size = new System.Drawing.Size(20, 13);
            this.latencyNDDev_label.TabIndex = 17;
            this.latencyNDDev_label.Text = "ms";
            // 
            // textBoxLatencyNormalDev
            // 
            this.textBoxLatencyNormalDev.Enabled = false;
            this.textBoxLatencyNormalDev.Location = new System.Drawing.Point(290, 200);
            this.textBoxLatencyNormalDev.Name = "textBoxLatencyNormalDev";
            this.textBoxLatencyNormalDev.Size = new System.Drawing.Size(80, 20);
            this.textBoxLatencyNormalDev.TabIndex = 8;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(250, 200);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(27, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Dev";
            // 
            // latencyNDAvg_label
            // 
            this.latencyNDAvg_label.AutoSize = true;
            this.latencyNDAvg_label.Location = new System.Drawing.Point(210, 202);
            this.latencyNDAvg_label.Name = "latencyNDAvg_label";
            this.latencyNDAvg_label.Size = new System.Drawing.Size(20, 13);
            this.latencyNDAvg_label.TabIndex = 14;
            this.latencyNDAvg_label.Text = "ms";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(50, 200);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(26, 13);
            this.label11.TabIndex = 13;
            this.label11.Text = "Avg";
            // 
            // textBoxLatencyNormalAvg
            // 
            this.textBoxLatencyNormalAvg.Enabled = false;
            this.textBoxLatencyNormalAvg.Location = new System.Drawing.Point(125, 200);
            this.textBoxLatencyNormalAvg.Name = "textBoxLatencyNormalAvg";
            this.textBoxLatencyNormalAvg.Size = new System.Drawing.Size(80, 20);
            this.textBoxLatencyNormalAvg.TabIndex = 7;
            // 
            // radioButtonLatencyNormal
            // 
            this.radioButtonLatencyNormal.AutoSize = true;
            this.radioButtonLatencyNormal.Location = new System.Drawing.Point(35, 170);
            this.radioButtonLatencyNormal.Name = "radioButtonLatencyNormal";
            this.radioButtonLatencyNormal.Size = new System.Drawing.Size(111, 17);
            this.radioButtonLatencyNormal.TabIndex = 3;
            this.radioButtonLatencyNormal.TabStop = true;
            this.radioButtonLatencyNormal.Text = "Normal Distributed";
            this.radioButtonLatencyNormal.UseVisualStyleBackColor = true;
            this.radioButtonLatencyNormal.CheckedChanged += new System.EventHandler(this.radioButtonLatencyNormal_CheckedChanged);
            // 
            // latencyUDMax_label
            // 
            this.latencyUDMax_label.AutoSize = true;
            this.latencyUDMax_label.Location = new System.Drawing.Point(375, 144);
            this.latencyUDMax_label.Name = "latencyUDMax_label";
            this.latencyUDMax_label.Size = new System.Drawing.Size(20, 13);
            this.latencyUDMax_label.TabIndex = 10;
            this.latencyUDMax_label.Text = "ms";
            // 
            // textBoxLatencyUniformMax
            // 
            this.textBoxLatencyUniformMax.Enabled = false;
            this.textBoxLatencyUniformMax.Location = new System.Drawing.Point(290, 140);
            this.textBoxLatencyUniformMax.Name = "textBoxLatencyUniformMax";
            this.textBoxLatencyUniformMax.Size = new System.Drawing.Size(80, 20);
            this.textBoxLatencyUniformMax.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(250, 140);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Max";
            // 
            // latencyUDMin_label
            // 
            this.latencyUDMin_label.AutoSize = true;
            this.latencyUDMin_label.Location = new System.Drawing.Point(210, 143);
            this.latencyUDMin_label.Name = "latencyUDMin_label";
            this.latencyUDMin_label.Size = new System.Drawing.Size(20, 13);
            this.latencyUDMin_label.TabIndex = 7;
            this.latencyUDMin_label.Text = "ms";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(50, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Min";
            // 
            // textBoxLatencyUniformMin
            // 
            this.textBoxLatencyUniformMin.Enabled = false;
            this.textBoxLatencyUniformMin.Location = new System.Drawing.Point(125, 140);
            this.textBoxLatencyUniformMin.Name = "textBoxLatencyUniformMin";
            this.textBoxLatencyUniformMin.Size = new System.Drawing.Size(80, 20);
            this.textBoxLatencyUniformMin.TabIndex = 5;
            // 
            // radioButtonLatencyUniform
            // 
            this.radioButtonLatencyUniform.AutoSize = true;
            this.radioButtonLatencyUniform.Location = new System.Drawing.Point(35, 110);
            this.radioButtonLatencyUniform.Name = "radioButtonLatencyUniform";
            this.radioButtonLatencyUniform.Size = new System.Drawing.Size(114, 17);
            this.radioButtonLatencyUniform.TabIndex = 2;
            this.radioButtonLatencyUniform.TabStop = true;
            this.radioButtonLatencyUniform.Text = "Uniform Distributed";
            this.radioButtonLatencyUniform.UseVisualStyleBackColor = true;
            this.radioButtonLatencyUniform.CheckedChanged += new System.EventHandler(this.radioButtonLatencyUniform_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(50, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Latency";
            // 
            // textBoxLatencyFixed
            // 
            this.textBoxLatencyFixed.Enabled = false;
            this.textBoxLatencyFixed.Location = new System.Drawing.Point(125, 80);
            this.textBoxLatencyFixed.Name = "textBoxLatencyFixed";
            this.textBoxLatencyFixed.Size = new System.Drawing.Size(80, 20);
            this.textBoxLatencyFixed.TabIndex = 4;
            // 
            // radioButtonLatencyFixed
            // 
            this.radioButtonLatencyFixed.AutoSize = true;
            this.radioButtonLatencyFixed.Location = new System.Drawing.Point(35, 50);
            this.radioButtonLatencyFixed.Name = "radioButtonLatencyFixed";
            this.radioButtonLatencyFixed.Size = new System.Drawing.Size(50, 17);
            this.radioButtonLatencyFixed.TabIndex = 1;
            this.radioButtonLatencyFixed.TabStop = true;
            this.radioButtonLatencyFixed.Text = "Fixed";
            this.radioButtonLatencyFixed.UseVisualStyleBackColor = true;
            this.radioButtonLatencyFixed.CheckedChanged += new System.EventHandler(this.radioButtonLatencyFixed_CheckedChanged);
            // 
            // radioButtonLatencyNo
            // 
            this.radioButtonLatencyNo.AutoSize = true;
            this.radioButtonLatencyNo.Location = new System.Drawing.Point(35, 20);
            this.radioButtonLatencyNo.Name = "radioButtonLatencyNo";
            this.radioButtonLatencyNo.Size = new System.Drawing.Size(80, 17);
            this.radioButtonLatencyNo.TabIndex = 0;
            this.radioButtonLatencyNo.TabStop = true;
            this.radioButtonLatencyNo.Text = "No Latency";
            this.radioButtonLatencyNo.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.loss_rate_value_label);
            this.tabPage3.Controls.Add(this.radioButtonLossNo);
            this.tabPage3.Controls.Add(this.textBoxLossRate);
            this.tabPage3.Controls.Add(this.loss_periodic_packet_label);
            this.tabPage3.Controls.Add(this.loss_rate_label);
            this.tabPage3.Controls.Add(this.radioButtonLossRandom);
            this.tabPage3.Controls.Add(this.textBoxLossPeriodic);
            this.tabPage3.Controls.Add(this.loss_periodic_label);
            this.tabPage3.Controls.Add(this.radioButtonLossPeriodic);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(456, 292);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Loss";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // loss_rate_value_label
            // 
            this.loss_rate_value_label.AutoSize = true;
            this.loss_rate_value_label.Location = new System.Drawing.Point(284, 143);
            this.loss_rate_value_label.Name = "loss_rate_value_label";
            this.loss_rate_value_label.Size = new System.Drawing.Size(15, 13);
            this.loss_rate_value_label.TabIndex = 8;
            this.loss_rate_value_label.Text = "%";
            // 
            // radioButtonLossNo
            // 
            this.radioButtonLossNo.AutoSize = true;
            this.radioButtonLossNo.Checked = true;
            this.radioButtonLossNo.Location = new System.Drawing.Point(35, 20);
            this.radioButtonLossNo.Name = "radioButtonLossNo";
            this.radioButtonLossNo.Size = new System.Drawing.Size(39, 17);
            this.radioButtonLossNo.TabIndex = 7;
            this.radioButtonLossNo.TabStop = true;
            this.radioButtonLossNo.Text = "No";
            this.radioButtonLossNo.UseVisualStyleBackColor = true;
            // 
            // textBoxLossRate
            // 
            this.textBoxLossRate.Enabled = false;
            this.textBoxLossRate.Location = new System.Drawing.Point(200, 140);
            this.textBoxLossRate.Name = "textBoxLossRate";
            this.textBoxLossRate.Size = new System.Drawing.Size(80, 20);
            this.textBoxLossRate.TabIndex = 6;
            // 
            // loss_periodic_packet_label
            // 
            this.loss_periodic_packet_label.AutoSize = true;
            this.loss_periodic_packet_label.Location = new System.Drawing.Point(286, 83);
            this.loss_periodic_packet_label.Name = "loss_periodic_packet_label";
            this.loss_periodic_packet_label.Size = new System.Drawing.Size(45, 13);
            this.loss_periodic_packet_label.TabIndex = 5;
            this.loss_periodic_packet_label.Text = "packets";
            // 
            // loss_rate_label
            // 
            this.loss_rate_label.AutoSize = true;
            this.loss_rate_label.Location = new System.Drawing.Point(50, 140);
            this.loss_rate_label.Name = "loss_rate_label";
            this.loss_rate_label.Size = new System.Drawing.Size(55, 13);
            this.loss_rate_label.TabIndex = 4;
            this.loss_rate_label.Text = "Loss Rate";
            // 
            // radioButtonLossRandom
            // 
            this.radioButtonLossRandom.AutoSize = true;
            this.radioButtonLossRandom.Location = new System.Drawing.Point(35, 110);
            this.radioButtonLossRandom.Name = "radioButtonLossRandom";
            this.radioButtonLossRandom.Size = new System.Drawing.Size(90, 17);
            this.radioButtonLossRandom.TabIndex = 3;
            this.radioButtonLossRandom.Text = "Random Loss";
            this.radioButtonLossRandom.UseVisualStyleBackColor = true;
            this.radioButtonLossRandom.CheckedChanged += new System.EventHandler(this.radioButtonLossRandom_CheckedChanged);
            // 
            // textBoxLossPeriodic
            // 
            this.textBoxLossPeriodic.Enabled = false;
            this.textBoxLossPeriodic.Location = new System.Drawing.Point(200, 80);
            this.textBoxLossPeriodic.Name = "textBoxLossPeriodic";
            this.textBoxLossPeriodic.Size = new System.Drawing.Size(80, 20);
            this.textBoxLossPeriodic.TabIndex = 2;
            // 
            // loss_periodic_label
            // 
            this.loss_periodic_label.AutoSize = true;
            this.loss_periodic_label.Location = new System.Drawing.Point(50, 80);
            this.loss_periodic_label.Name = "loss_periodic_label";
            this.loss_periodic_label.Size = new System.Drawing.Size(116, 13);
            this.loss_periodic_label.TabIndex = 1;
            this.loss_periodic_label.Text = "Lose one packet every";
            // 
            // radioButtonLossPeriodic
            // 
            this.radioButtonLossPeriodic.AutoSize = true;
            this.radioButtonLossPeriodic.Location = new System.Drawing.Point(35, 50);
            this.radioButtonLossPeriodic.Name = "radioButtonLossPeriodic";
            this.radioButtonLossPeriodic.Size = new System.Drawing.Size(88, 17);
            this.radioButtonLossPeriodic.TabIndex = 0;
            this.radioButtonLossPeriodic.Text = "Periodic Loss";
            this.radioButtonLossPeriodic.UseVisualStyleBackColor = true;
            this.radioButtonLossPeriodic.CheckedChanged += new System.EventHandler(this.radioButtonLossPeriodic_CheckedChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.error_rate_value_label);
            this.tabPage4.Controls.Add(this.textBoxErrorRate);
            this.tabPage4.Controls.Add(this.error_rate_label);
            this.tabPage4.Controls.Add(this.radioButtonErrorRandom);
            this.tabPage4.Controls.Add(this.radioButtonErrorNo);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(456, 292);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Error";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // error_rate_value_label
            // 
            this.error_rate_value_label.AutoSize = true;
            this.error_rate_value_label.Location = new System.Drawing.Point(233, 84);
            this.error_rate_value_label.Name = "error_rate_value_label";
            this.error_rate_value_label.Size = new System.Drawing.Size(15, 13);
            this.error_rate_value_label.TabIndex = 4;
            this.error_rate_value_label.Text = "%";
            // 
            // textBoxErrorRate
            // 
            this.textBoxErrorRate.Enabled = false;
            this.textBoxErrorRate.Location = new System.Drawing.Point(150, 80);
            this.textBoxErrorRate.Name = "textBoxErrorRate";
            this.textBoxErrorRate.Size = new System.Drawing.Size(80, 20);
            this.textBoxErrorRate.TabIndex = 3;
            // 
            // error_rate_label
            // 
            this.error_rate_label.AutoSize = true;
            this.error_rate_label.Location = new System.Drawing.Point(50, 80);
            this.error_rate_label.Name = "error_rate_label";
            this.error_rate_label.Size = new System.Drawing.Size(55, 13);
            this.error_rate_label.TabIndex = 2;
            this.error_rate_label.Text = "Error Rate";
            // 
            // radioButtonErrorRandom
            // 
            this.radioButtonErrorRandom.AutoSize = true;
            this.radioButtonErrorRandom.Location = new System.Drawing.Point(35, 50);
            this.radioButtonErrorRandom.Name = "radioButtonErrorRandom";
            this.radioButtonErrorRandom.Size = new System.Drawing.Size(90, 17);
            this.radioButtonErrorRandom.TabIndex = 1;
            this.radioButtonErrorRandom.TabStop = true;
            this.radioButtonErrorRandom.Text = "Random Error";
            this.radioButtonErrorRandom.UseVisualStyleBackColor = true;
            this.radioButtonErrorRandom.CheckedChanged += new System.EventHandler(this.radioButtonErrorRandom_CheckedChanged);
            // 
            // radioButtonErrorNo
            // 
            this.radioButtonErrorNo.AutoSize = true;
            this.radioButtonErrorNo.Checked = true;
            this.radioButtonErrorNo.Location = new System.Drawing.Point(35, 20);
            this.radioButtonErrorNo.Name = "radioButtonErrorNo";
            this.radioButtonErrorNo.Size = new System.Drawing.Size(64, 17);
            this.radioButtonErrorNo.TabIndex = 0;
            this.radioButtonErrorNo.TabStop = true;
            this.radioButtonErrorNo.Text = "No Error";
            this.radioButtonErrorNo.UseVisualStyleBackColor = true;
            // 
            // comboBox_networkprofiles
            // 
            this.comboBox_networkprofiles.FormattingEnabled = true;
            this.comboBox_networkprofiles.Location = new System.Drawing.Point(16, 42);
            this.comboBox_networkprofiles.Name = "comboBox_networkprofiles";
            this.comboBox_networkprofiles.Size = new System.Drawing.Size(196, 21);
            this.comboBox_networkprofiles.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Emulation Profiles";
            // 
            // NestConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_networkprofiles);
            this.Controls.Add(this.tabControlNewt);
            this.Name = "NestConfigurationControl";
            this.Size = new System.Drawing.Size(503, 450);
            this.tabControlNewt.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlNewt;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ComboBox comboBoxBWUpStream;
        private System.Windows.Forms.TextBox textBoxBWUpStream;
        private System.Windows.Forms.Label upstream_label;
        private System.Windows.Forms.ComboBox comboBoxBWDownStream;
        private System.Windows.Forms.Label downstream_label;
        private System.Windows.Forms.TextBox textBoxBWDownStream;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label latencyNDDev_label;
        private System.Windows.Forms.TextBox textBoxLatencyNormalDev;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label latencyNDAvg_label;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxLatencyNormalAvg;
        private System.Windows.Forms.RadioButton radioButtonLatencyNormal;
        private System.Windows.Forms.Label latencyUDMax_label;
        private System.Windows.Forms.TextBox textBoxLatencyUniformMax;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label latencyUDMin_label;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxLatencyUniformMin;
        private System.Windows.Forms.RadioButton radioButtonLatencyUniform;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxLatencyFixed;
        private System.Windows.Forms.RadioButton radioButtonLatencyFixed;
        private System.Windows.Forms.RadioButton radioButtonLatencyNo;
        private System.Windows.Forms.TextBox textBoxLossRate;
        private System.Windows.Forms.Label loss_periodic_packet_label;
        private System.Windows.Forms.Label loss_rate_label;
        private System.Windows.Forms.RadioButton radioButtonLossRandom;
        private System.Windows.Forms.TextBox textBoxLossPeriodic;
        private System.Windows.Forms.Label loss_periodic_label;
        private System.Windows.Forms.RadioButton radioButtonLossPeriodic;
        private System.Windows.Forms.TextBox textBoxErrorRate;
        private System.Windows.Forms.Label error_rate_label;
        private System.Windows.Forms.RadioButton radioButtonErrorRandom;
        private System.Windows.Forms.RadioButton radioButtonErrorNo;
        private System.Windows.Forms.RadioButton radioButtonLossNo;
        private System.Windows.Forms.Label loss_rate_value_label;
        private System.Windows.Forms.Label error_rate_value_label;
        private System.Windows.Forms.Label latencyvalue_label;
        private System.Windows.Forms.RichTextBox description_richTextBox;
        private System.Windows.Forms.ComboBox comboBox_networkprofiles;
        private System.Windows.Forms.Label label1;
    }
}
