
namespace Astana
{
    partial class GraphicEventJournalForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btn_closeForm = new System.Windows.Forms.Button();
            this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.lb_nameRO = new System.Windows.Forms.Label();
            this.btn_success = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_CheckPoint = new System.Windows.Forms.CheckBox();
            this.cb_Flushing = new System.Windows.Forms.CheckBox();
            this.cb_ROname = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chart1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(51)))));
            chartArea1.AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea1.AxisX.ScaleView.MinSize = 15D;
            chartArea1.AxisX.ScrollBar.IsPositionedInside = false;
            chartArea1.AxisY.Interval = 10D;
            chartArea1.AxisY.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea1.AxisY.Maximum = 60D;
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(51)))));
            chartArea1.Name = "ChartArea1";
            chartArea2.AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea2.AxisX.ScaleView.MinSize = 15D;
            chartArea2.AxisX.ScrollBar.IsPositionedInside = false;
            chartArea2.AxisY.Interval = 10D;
            chartArea2.AxisY.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea2.AxisY.Maximum = 60D;
            chartArea2.AxisY.Minimum = 0D;
            chartArea2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(51)))));
            chartArea2.Name = "ChartArea2";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.ChartAreas.Add(chartArea2);
            legend1.Alignment = System.Drawing.StringAlignment.Far;
            legend1.BackColor = System.Drawing.Color.Transparent;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            legend1.ForeColor = System.Drawing.Color.White;
            legend1.IsTextAutoFit = false;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(12, 89);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            series1.IsValueShownAsLabel = true;
            series1.LabelForeColor = System.Drawing.Color.White;
            series1.Legend = "Legend1";
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series1.Name = "Норм. расход (К)";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            series2.IsValueShownAsLabel = true;
            series2.LabelForeColor = System.Drawing.Color.White;
            series2.Legend = "Legend1";
            series2.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series2.Name = "Расход пермиата (К)";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series2.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series3.ChartArea = "ChartArea2";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series3.IsValueShownAsLabel = true;
            series3.LabelForeColor = System.Drawing.Color.White;
            series3.Legend = "Legend1";
            series3.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series3.Name = "Норм. расход (П)";
            series3.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series3.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series4.ChartArea = "ChartArea2";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series4.IsValueShownAsLabel = true;
            series4.LabelForeColor = System.Drawing.Color.White;
            series4.Legend = "Legend1";
            series4.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series4.Name = "Расход пермиата (П)";
            series4.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series4.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Series.Add(series3);
            this.chart1.Series.Add(series4);
            this.chart1.Size = new System.Drawing.Size(1362, 687);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            title1.Alignment = System.Drawing.ContentAlignment.TopCenter;
            title1.DockedToChartArea = "ChartArea1";
            title1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            title1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            title1.IsDockedInsideChartArea = false;
            title1.Name = "Title1";
            title1.Text = "Контрольные точки";
            title2.Alignment = System.Drawing.ContentAlignment.TopCenter;
            title2.DockedToChartArea = "ChartArea2";
            title2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            title2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            title2.IsDockedInsideChartArea = false;
            title2.Name = "Title2";
            title2.Text = "Химическая регенерация";
            this.chart1.Titles.Add(title1);
            this.chart1.Titles.Add(title2);
            // 
            // btn_closeForm
            // 
            this.btn_closeForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_closeForm.AutoSize = true;
            this.btn_closeForm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_closeForm.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_closeForm.ForeColor = System.Drawing.SystemColors.InfoText;
            this.btn_closeForm.Location = new System.Drawing.Point(1342, 12);
            this.btn_closeForm.Name = "btn_closeForm";
            this.btn_closeForm.Size = new System.Drawing.Size(32, 32);
            this.btn_closeForm.TabIndex = 28;
            this.btn_closeForm.Text = "X";
            this.btn_closeForm.UseVisualStyleBackColor = true;
            this.btn_closeForm.Click += new System.EventHandler(this.btn_closeForm_Click);
            // 
            // dateTimePickerStart
            // 
            this.dateTimePickerStart.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            this.dateTimePickerStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePickerStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerStart.Location = new System.Drawing.Point(12, 15);
            this.dateTimePickerStart.Name = "dateTimePickerStart";
            this.dateTimePickerStart.Size = new System.Drawing.Size(203, 29);
            this.dateTimePickerStart.TabIndex = 29;
            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            this.dateTimePickerEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePickerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEnd.Location = new System.Drawing.Point(244, 15);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.Size = new System.Drawing.Size(203, 29);
            this.dateTimePickerEnd.TabIndex = 30;
            // 
            // lb_nameRO
            // 
            this.lb_nameRO.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lb_nameRO.AutoSize = true;
            this.lb_nameRO.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lb_nameRO.ForeColor = System.Drawing.SystemColors.Window;
            this.lb_nameRO.Location = new System.Drawing.Point(585, 9);
            this.lb_nameRO.Name = "lb_nameRO";
            this.lb_nameRO.Size = new System.Drawing.Size(154, 37);
            this.lb_nameRO.TabIndex = 31;
            this.lb_nameRO.Text = "Название";
            this.lb_nameRO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_success
            // 
            this.btn_success.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_success.Location = new System.Drawing.Point(454, 47);
            this.btn_success.Name = "btn_success";
            this.btn_success.Size = new System.Drawing.Size(121, 34);
            this.btn_success.TabIndex = 32;
            this.btn_success.Text = "Показать";
            this.btn_success.UseVisualStyleBackColor = true;
            this.btn_success.Click += new System.EventHandler(this.btn_success_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(221, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 24);
            this.label1.TabIndex = 33;
            this.label1.Text = "-";
            // 
            // cb_CheckPoint
            // 
            this.cb_CheckPoint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.cb_CheckPoint.Checked = true;
            this.cb_CheckPoint.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_CheckPoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cb_CheckPoint.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cb_CheckPoint.Location = new System.Drawing.Point(12, 48);
            this.cb_CheckPoint.Name = "cb_CheckPoint";
            this.cb_CheckPoint.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.cb_CheckPoint.Size = new System.Drawing.Size(145, 32);
            this.cb_CheckPoint.TabIndex = 34;
            this.cb_CheckPoint.Text = "Конт. точки";
            this.cb_CheckPoint.UseVisualStyleBackColor = false;
            this.cb_CheckPoint.CheckedChanged += new System.EventHandler(this.cb_CheckPoint_CheckedChanged);
            // 
            // cb_Flushing
            // 
            this.cb_Flushing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.cb_Flushing.Checked = true;
            this.cb_Flushing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_Flushing.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cb_Flushing.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cb_Flushing.Location = new System.Drawing.Point(163, 48);
            this.cb_Flushing.Name = "cb_Flushing";
            this.cb_Flushing.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.cb_Flushing.Size = new System.Drawing.Size(157, 32);
            this.cb_Flushing.TabIndex = 35;
            this.cb_Flushing.Text = "Регенерация";
            this.cb_Flushing.UseVisualStyleBackColor = false;
            this.cb_Flushing.CheckedChanged += new System.EventHandler(this.cb_Flushing_CheckedChanged);
            // 
            // cb_ROname
            // 
            this.cb_ROname.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cb_ROname.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cb_ROname.FormattingEnabled = true;
            this.cb_ROname.Items.AddRange(new object[] {
            "RO1",
            "RO2",
            "RO3",
            "RO4",
            "RO5",
            "RO6",
            "ROобщ"});
            this.cb_ROname.Location = new System.Drawing.Point(328, 48);
            this.cb_ROname.Name = "cb_ROname";
            this.cb_ROname.Size = new System.Drawing.Size(121, 32);
            this.cb_ROname.TabIndex = 36;
            // 
            // GraphicEventJournalForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(41)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(1386, 788);
            this.Controls.Add(this.cb_ROname);
            this.Controls.Add(this.cb_Flushing);
            this.Controls.Add(this.cb_CheckPoint);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_success);
            this.Controls.Add(this.lb_nameRO);
            this.Controls.Add(this.dateTimePickerEnd);
            this.Controls.Add(this.dateTimePickerStart);
            this.Controls.Add(this.btn_closeForm);
            this.Controls.Add(this.chart1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GraphicEventJournalForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GraphicEventJournalForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button btn_closeForm;
        private System.Windows.Forms.DateTimePicker dateTimePickerStart;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private System.Windows.Forms.Label lb_nameRO;
        private System.Windows.Forms.Button btn_success;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cb_CheckPoint;
        private System.Windows.Forms.CheckBox cb_Flushing;
        private System.Windows.Forms.ComboBox cb_ROname;
    }
}