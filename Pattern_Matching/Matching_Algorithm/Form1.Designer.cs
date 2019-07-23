namespace Matching_Algorithm
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.stockchart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.findbtn = new System.Windows.Forms.Button();
            this.hlRate_textbox = new System.Windows.Forms.TextBox();
            this.devRate_textbox = new System.Windows.Forms.TextBox();
            this.slopeRate_textbox = new System.Windows.Forms.TextBox();
            this.ResultView = new System.Windows.Forms.ListView();
            this.종목코드 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.날짜 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.고저점 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.편차 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.기울기 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.총합 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.인덱스 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DrawPannel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.Default_MRate_btn = new System.Windows.Forms.Button();
            this.Clear_UserBoard_btn = new System.Windows.Forms.Button();
            this.Period_20btn = new System.Windows.Forms.Button();
            this.Period_60btn = new System.Windows.Forms.Button();
            this.Period_120btn = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.stockchart)).BeginInit();
            this.SuspendLayout();
            // 
            // stockchart
            // 
            this.stockchart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.stockchart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.stockchart.Legends.Add(legend1);
            this.stockchart.Location = new System.Drawing.Point(416, 17);
            this.stockchart.Name = "stockchart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.stockchart.Series.Add(series1);
            this.stockchart.Size = new System.Drawing.Size(934, 515);
            this.stockchart.TabIndex = 0;
            this.stockchart.Text = "chart1";
            // 
            // findbtn
            // 
            this.findbtn.Location = new System.Drawing.Point(44, 505);
            this.findbtn.Name = "findbtn";
            this.findbtn.Size = new System.Drawing.Size(313, 55);
            this.findbtn.TabIndex = 2;
            this.findbtn.Text = "검색";
            this.findbtn.UseVisualStyleBackColor = true;
            this.findbtn.Click += new System.EventHandler(this.findbtn_Click);
            // 
            // hlRate_textbox
            // 
            this.hlRate_textbox.Location = new System.Drawing.Point(86, 473);
            this.hlRate_textbox.Name = "hlRate_textbox";
            this.hlRate_textbox.Size = new System.Drawing.Size(49, 21);
            this.hlRate_textbox.TabIndex = 6;
            // 
            // devRate_textbox
            // 
            this.devRate_textbox.Location = new System.Drawing.Point(176, 473);
            this.devRate_textbox.Name = "devRate_textbox";
            this.devRate_textbox.Size = new System.Drawing.Size(49, 21);
            this.devRate_textbox.TabIndex = 7;
            // 
            // slopeRate_textbox
            // 
            this.slopeRate_textbox.Location = new System.Drawing.Point(276, 473);
            this.slopeRate_textbox.Name = "slopeRate_textbox";
            this.slopeRate_textbox.Size = new System.Drawing.Size(49, 21);
            this.slopeRate_textbox.TabIndex = 8;
            // 
            // ResultView
            // 
            this.ResultView.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ResultView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.종목코드,
            this.날짜,
            this.고저점,
            this.편차,
            this.기울기,
            this.총합,
            this.인덱스});
            this.ResultView.FullRowSelect = true;
            this.ResultView.Location = new System.Drawing.Point(8, 566);
            this.ResultView.Name = "ResultView";
            this.ResultView.Size = new System.Drawing.Size(1342, 165);
            this.ResultView.TabIndex = 11;
            this.ResultView.UseCompatibleStateImageBehavior = false;
            this.ResultView.View = System.Windows.Forms.View.Details;
            this.ResultView.SelectedIndexChanged += new System.EventHandler(this.ResultView_SelectedIndexChanged);
            // 
            // 종목코드
            // 
            this.종목코드.Text = "종목코드";
            this.종목코드.Width = 115;
            // 
            // 날짜
            // 
            this.날짜.Text = "날짜";
            this.날짜.Width = 126;
            // 
            // 고저점
            // 
            this.고저점.Text = "고저점";
            this.고저점.Width = 122;
            // 
            // 편차
            // 
            this.편차.Text = "편차";
            this.편차.Width = 127;
            // 
            // 기울기
            // 
            this.기울기.Text = "기울기";
            this.기울기.Width = 145;
            // 
            // 총합
            // 
            this.총합.Text = "총합";
            this.총합.Width = 134;
            // 
            // 인덱스
            // 
            this.인덱스.Text = "인덱스";
            this.인덱스.Width = 7;
            // 
            // DrawPannel
            // 
            this.DrawPannel.BackColor = System.Drawing.Color.Gainsboro;
            this.DrawPannel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DrawPannel.Location = new System.Drawing.Point(44, 32);
            this.DrawPannel.Name = "DrawPannel";
            this.DrawPannel.Size = new System.Drawing.Size(340, 340);
            this.DrawPannel.TabIndex = 12;
            this.DrawPannel.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawPannel_Paint);
            this.DrawPannel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DrawPannel_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "Price";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(352, 384);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 14;
            this.label2.Text = "Period";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 476);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "고저점";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(141, 476);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 16;
            this.label4.Text = "편차";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(232, 476);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "기울기";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(42, 375);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(11, 12);
            this.label6.TabIndex = 18;
            this.label6.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 360);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(27, 12);
            this.label7.TabIndex = 19;
            this.label7.Text = "90%";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 188);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 12);
            this.label8.TabIndex = 20;
            this.label8.Text = "100%";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 29);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(33, 12);
            this.label9.TabIndex = 21;
            this.label9.Text = "110%";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 458);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 12);
            this.label10.TabIndex = 22;
            this.label10.Text = " :: 매칭율 비율";
            // 
            // Default_MRate_btn
            // 
            this.Default_MRate_btn.Location = new System.Drawing.Point(335, 471);
            this.Default_MRate_btn.Name = "Default_MRate_btn";
            this.Default_MRate_btn.Size = new System.Drawing.Size(75, 23);
            this.Default_MRate_btn.TabIndex = 23;
            this.Default_MRate_btn.Text = "기본값";
            this.Default_MRate_btn.UseVisualStyleBackColor = true;
            this.Default_MRate_btn.Click += new System.EventHandler(this.Default_MRate_btn_Click);
            // 
            // Clear_UserBoard_btn
            // 
            this.Clear_UserBoard_btn.Location = new System.Drawing.Point(271, 379);
            this.Clear_UserBoard_btn.Name = "Clear_UserBoard_btn";
            this.Clear_UserBoard_btn.Size = new System.Drawing.Size(75, 23);
            this.Clear_UserBoard_btn.TabIndex = 24;
            this.Clear_UserBoard_btn.Text = "초기화";
            this.Clear_UserBoard_btn.UseVisualStyleBackColor = true;
            this.Clear_UserBoard_btn.Click += new System.EventHandler(this.Clear_UserBoard_btn_Click);
            // 
            // Period_20btn
            // 
            this.Period_20btn.Location = new System.Drawing.Point(150, 430);
            this.Period_20btn.Name = "Period_20btn";
            this.Period_20btn.Size = new System.Drawing.Size(75, 23);
            this.Period_20btn.TabIndex = 25;
            this.Period_20btn.Text = "20일";
            this.Period_20btn.UseVisualStyleBackColor = true;
            this.Period_20btn.Click += new System.EventHandler(this.Period_20btn_Click);
            // 
            // Period_60btn
            // 
            this.Period_60btn.Location = new System.Drawing.Point(231, 430);
            this.Period_60btn.Name = "Period_60btn";
            this.Period_60btn.Size = new System.Drawing.Size(75, 23);
            this.Period_60btn.TabIndex = 26;
            this.Period_60btn.Text = "60일";
            this.Period_60btn.UseVisualStyleBackColor = true;
            this.Period_60btn.Click += new System.EventHandler(this.Period_60btn_Click);
            // 
            // Period_120btn
            // 
            this.Period_120btn.Location = new System.Drawing.Point(312, 430);
            this.Period_120btn.Name = "Period_120btn";
            this.Period_120btn.Size = new System.Drawing.Size(75, 23);
            this.Period_120btn.TabIndex = 27;
            this.Period_120btn.Text = "120일";
            this.Period_120btn.UseVisualStyleBackColor = true;
            this.Period_120btn.Click += new System.EventHandler(this.Period_120btn_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 413);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(101, 12);
            this.label11.TabIndex = 28;
            this.label11.Text = " :: 검색 기간 설정";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(84, 435);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(33, 12);
            this.label12.TabIndex = 29;
            this.label12.Text = "20 일";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(39, 435);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 12);
            this.label13.TabIndex = 30;
            this.label13.Text = "기간 : ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuBar;
            this.ClientSize = new System.Drawing.Size(1362, 741);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.Period_120btn);
            this.Controls.Add(this.Period_60btn);
            this.Controls.Add(this.Period_20btn);
            this.Controls.Add(this.Clear_UserBoard_btn);
            this.Controls.Add(this.Default_MRate_btn);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DrawPannel);
            this.Controls.Add(this.ResultView);
            this.Controls.Add(this.slopeRate_textbox);
            this.Controls.Add(this.devRate_textbox);
            this.Controls.Add(this.hlRate_textbox);
            this.Controls.Add(this.findbtn);
            this.Controls.Add(this.stockchart);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.stockchart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart stockchart;
        private System.Windows.Forms.Button findbtn;
        private System.Windows.Forms.TextBox hlRate_textbox;
        private System.Windows.Forms.TextBox devRate_textbox;
        private System.Windows.Forms.TextBox slopeRate_textbox;
        private System.Windows.Forms.ListView ResultView;
        private System.Windows.Forms.Panel DrawPannel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button Default_MRate_btn;
        private System.Windows.Forms.Button Clear_UserBoard_btn;
        private System.Windows.Forms.Button Period_20btn;
        private System.Windows.Forms.Button Period_60btn;
        private System.Windows.Forms.Button Period_120btn;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ColumnHeader 종목코드;
        private System.Windows.Forms.ColumnHeader 날짜;
        private System.Windows.Forms.ColumnHeader 고저점;
        private System.Windows.Forms.ColumnHeader 편차;
        private System.Windows.Forms.ColumnHeader 기울기;
        private System.Windows.Forms.ColumnHeader 총합;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ColumnHeader 인덱스;
    }
}

