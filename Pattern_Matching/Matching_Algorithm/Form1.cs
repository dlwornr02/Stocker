


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Windows.Forms.DataVisualization.Charting;

namespace Matching_Algorithm
{
    public partial class Form1 : Form
    {
        private List<Stock> stockList = new List<Stock>();
        private List<UserStock> userDrawList = new List<UserStock>();
        private List<Stock> tempStockList = new List<Stock>();
        private List<int> matchIdxList = new List<int>();
        private List<string> subjectList = new List<string>();

        private int period = 20;
        private int defaultHighLowRate = 30;
        private int defaultDeviRate = 30;
        private int defaultslopeRate = 40;

        private List<Point> drawptList = new List<Point>();

        public Form1()
        {
            InitializeComponent();

            LoadSubjectCode();//DB로 부터 주식 종목코드 로드하는 함수

            stockchart.Series.Clear();

            Series data = new Series();

            data.ChartType = SeriesChartType.Line;
            data.Name = "Price";

            hlRate_textbox.Text = "30";
            devRate_textbox.Text = "30";
            slopeRate_textbox.Text = "40";

            drawptList.Add(new Point(0, DrawPannel.Height / 2));
            drawptList.Add(new Point(DrawPannel.Width, DrawPannel.Height / 2));
        }


        public void LoadSubjectCode()
        {
            string connectionString = "server = 127.0.0.1,21433; uid = sa; pwd = sj12sj12; database = StockInformation1;";
            SqlConnection scon = null;
            SqlCommand scom = null;
            SqlDataReader sdr = null;

            try
            {
                scon = new SqlConnection(connectionString);
                scom = new SqlCommand();
                scom.Connection = scon;
                scom.CommandText = "select 종목코드 from Stock_Main";
                scon.Open();

                sdr = scom.ExecuteReader();

                while (sdr.Read())
                {
                    string sc = sdr["종목코드"].ToString();

                    subjectList.Add(sc);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Source);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                if (scon != null)
                {
                    scon.Close();
                }
            }
        }

        private void DrawPannel_MouseDown(object sender, MouseEventArgs e)
        {
            Point pt = new Point(e.Location.X, e.Location.Y);

            for (int x = 0; x <= 100; x += 5)
            {
                if (x - 2.5 < (double)e.Location.X * (100 / (double)DrawPannel.Size.Width) && (double)e.Location.X * (100 / (double)DrawPannel.Size.Width) <= x + 2.5)
                {
                    if (e.Location.X <= DrawPannel.Size.Width / 20)
                        pt.X = DrawPannel.Size.Width / 20;
                    else
                    {
                        pt.X = x * DrawPannel.Size.Width / 100;
                    }
                }

                if (x - 2.5 < (double)e.Location.Y * (100 / (double)DrawPannel.Size.Height) && (double)e.Location.Y * (100 / (double)DrawPannel.Size.Height) <= x + 2.5)
                { pt.Y = x * DrawPannel.Size.Height / 100; }
            }

            for (int j = 0; j < drawptList.Count; j++)
                if (pt.X == drawptList[j].X)
                {
                    drawptList.Remove(drawptList[j]);
                    break;
                }

            drawptList.Add(pt);
            drawptList.Sort(delegate(Point p1, Point p2) { return p1.X.CompareTo(p2.X); }); //입력받은 데이터 정렬


            DrawPannel.Invalidate();
        }

        private void DrawPannel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = DrawPannel.CreateGraphics();
            g.Clear(Color.Gainsboro);
            Pen pen1 = new Pen(Color.DarkBlue, 1f);
            pen1.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            Pen pen2 = new Pen(Color.Black, 1f);
            pen2.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            Pen pen3 = new Pen(Color.Red, 2f);

            for (int i = 1; i < 20; i++) //DrawPanner 바탕그리기
            {
                g.DrawLine(pen1, 0, i * (DrawPannel.Height / 20), DrawPannel.Width, i * (DrawPannel.Height / 20));
                g.DrawLine(pen1, i * (DrawPannel.Width / 20), 0, i * (DrawPannel.Width / 20), DrawPannel.Height);
            }
            for (int i = 1; i < 4; i++) //DrawPanner 바탕그리기
            {
                g.DrawLine(pen2, 0, i * (DrawPannel.Height / 4), DrawPannel.Width, i * (DrawPannel.Height / 4));
                g.DrawLine(pen2, i * (DrawPannel.Width / 4), 0, i * (DrawPannel.Width / 4), DrawPannel.Height);
            }

            for (int i = 0; i < drawptList.Count - 1; i++) //사용자 입력한 데이터 그리기
            {
                g.DrawLine(pen3, drawptList[i].X, drawptList[i].Y, drawptList[i + 1].X, drawptList[i + 1].Y);
            }
        }









        private void findbtn_Click(object sender, EventArgs e)
        {
            ResultView.Items.Clear();
            userDrawList.Clear();

            for (int i = 0; i < drawptList.Count(); i++)
            {
                UserStock ust = new UserStock();
                if (i == 0)
                {
                    ust.Day = 0;
                    ust.Semester = 1;
                    ust.Price = 100;
                }

                ust.Day = sort_Draw_Day(drawptList[i].X);
                if (ust.Day >= period / 4 * 0 && ust.Day < period / 4 * 1)
                    ust.Semester = 1;
                else if (ust.Day >= period / 4 * 1 && ust.Day < period / 4 * 2)
                    ust.Semester = 2;
                else if (ust.Day >= period / 4 * 2 && ust.Day < period / 4 * 3)
                    ust.Semester = 3;
                else if (ust.Day >= period / 4 * 3 && ust.Day < period / 4 * 4)
                    ust.Semester = 4;

                ust.Price = sort_Draw_Price(drawptList[i].Y);

                userDrawList.Add(ust);
            }

            defaultHighLowRate = int.Parse(hlRate_textbox.Text);
            defaultDeviRate = int.Parse(devRate_textbox.Text);
            defaultslopeRate = int.Parse(slopeRate_textbox.Text);

            for (int i = 0; i < 6; i++)
            {
                LoadDataFromDB(subjectList[i]);

                UserPatternMatch();

                GetMatchRate_Algorithm(subjectList[i]);
            }

            MessageBox.Show("Complete");
        }

        private int sort_Draw_Day(int d)
        {
            int day = 0;
            int divDay = DrawPannel.Width / 20;

            if (d > day * 0 && d <= divDay * 1)
                day = period / 20 * 1;
            else if (d > day * 1 && d <= divDay * 2)
                day = period / 20 * 2;
            else if (d > day * 2 && d <= divDay * 3)
                day = period / 20 * 3;
            else if (d > day * 3 && d <= divDay * 4)
                day = period / 20 * 4;
            else if (d > day * 4 && d <= divDay * 5)
                day = period / 20 * 5;
            else if (d > day * 5 && d <= divDay * 6)
                day = period / 20 * 6;
            else if (d > day * 6 && d <= divDay * 7)
                day = period / 20 * 7;
            else if (d > day * 7 && d <= divDay * 8)
                day = period / 20 * 8;
            else if (d > day * 8 && d <= divDay * 9)
                day = period / 20 * 9;
            else if (d > day * 9 && d <= divDay * 10)
                day = period / 20 * 10;
            else if (d > day * 10 && d <= divDay * 11)
                day = period / 20 * 11;
            else if (d > day * 11 && d <= divDay * 12)
                day = period / 20 * 12;
            else if (d > day * 12 && d <= divDay * 13)
                day = period / 20 * 13;
            else if (d > day * 13 && d <= divDay * 14)
                day = period / 20 * 14;
            else if (d > day * 14 && d <= divDay * 15)
                day = period / 20 * 15;
            else if (d > day * 15 && d <= divDay * 16)
                day = period / 20 * 16;
            else if (d > day * 16 && d <= divDay * 17)
                day = period / 20 * 17;
            else if (d > day * 17 && d <= divDay * 18)
                day = period / 20 * 18;
            else if (d > day * 18 && d <= divDay * 19)
                day = period / 20 * 19;
            else if (d > day * 19 && d <= divDay * 20)
                day = period / 20 * 20;

            return day;
        }
        private int sort_Draw_Price(int p)
        {
            int price = 0;
            int divPrice = DrawPannel.Height / 20;

            if (p == 0)
                price = 110;
            else if (p > divPrice * 0 && p <= divPrice * 1)
                price = 109;
            else if (p > divPrice * 1 && p <= divPrice * 2)
                price = 108;
            else if (p > divPrice * 2 && p <= divPrice * 3)
                price = 107;
            else if (p > divPrice * 3 && p <= divPrice * 4)
                price = 106;
            else if (p > divPrice * 4 && p <= divPrice * 5)
                price = 105;
            else if (p > divPrice * 5 && p <= divPrice * 6)
                price = 104;
            else if (p > divPrice * 6 && p <= divPrice * 7)
                price = 103;
            else if (p > divPrice * 7 && p <= divPrice * 8)
                price = 102;
            else if (p > divPrice * 8 && p <= divPrice * 9)
                price = 101;
            else if (p > divPrice * 9 && p <= divPrice * 10)
                price = 100;
            else if (p > divPrice * 10 && p <= divPrice * 11)
                price = 99;
            else if (p > divPrice * 11 && p <= divPrice * 12)
                price = 98;
            else if (p > divPrice * 12 && p <= divPrice * 13)
                price = 97;
            else if (p > divPrice * 13 && p <= divPrice * 14)
                price = 96;
            else if (p > divPrice * 14 && p <= divPrice * 15)
                price = 95;
            else if (p > divPrice * 15 && p <= divPrice * 16)
                price = 94;
            else if (p > divPrice * 16 && p <= divPrice * 17)
                price = 93;
            else if (p > divPrice * 17 && p <= divPrice * 18)
                price = 92;
            else if (p > divPrice * 18 && p <= divPrice * 19)
                price = 91;
            else if (p > divPrice * 19 && p <= divPrice * 20)
                price = 90;

            return price;
        }

        public void LoadDataFromDB(string code)
        {
            string connectionString =
                "server = 127.0.0.1,21433; uid = sa; pwd = sj12sj12; database = StockInformation1;";
            SqlConnection scon = null;
            SqlCommand scom = null;
            SqlDataReader sdr = null;

            stockList.Clear();
            matchIdxList.Clear();

            try
            {
                scon = new SqlConnection(connectionString);
                scom = new SqlCommand();
                scom.Connection = scon;

                string s = string.Format("select 일자, 시가 from Day_C_Data where 종목코드 in ({0}) order by 일자 asc", code);
                scom.CommandText = s;
                scon.Open();

                sdr = scom.ExecuteReader();

                while (sdr.Read())
                {
                    string d = sdr["일자"].ToString();
                    int sp = int.Parse(sdr["시가"].ToString());

                    stockList.Add(new Stock(d, sp));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Source);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                if (scon != null)
                {
                    scon.Close();
                }
            }
        }

        void UserPatternMatch()
        {
            int userhidx = 0, userlidx = 0;
            for (int i = 0; i < userDrawList.Count(); i++)
            {
                if (userDrawList[userhidx].Price < userDrawList[i].Price)
                {
                    userhidx = i;
                }
                if (userDrawList[userlidx].Price > userDrawList[i].Price)
                {
                    userlidx = i;
                }
            }
            userDrawList[userhidx].Highpt = true;
            userDrawList[userlidx].Lowpt = true;

            for (int i = 0; i < userDrawList.Count(); i++)
            {
                if (userDrawList[userhidx].Price * 1.03 >= userDrawList[i].Price && userDrawList[userhidx].Price * 0.97 <= userDrawList[i].Price)
                {
                    userDrawList[i].Highpt = true;
                }
                if (userDrawList[userlidx].Price * 1.03 >= userDrawList[i].Price && userDrawList[userlidx].Price * 0.97 <= userDrawList[i].Price)
                {
                    userDrawList[i].Lowpt = true;
                }
            }

            for (int scanIdx = 0; scanIdx < stockList.Count() - period; scanIdx++)
            {
                for (int i = scanIdx; i < scanIdx + period; i++)
                    tempStockList.Add(stockList[i]);

                int stockhidx = 0, stocklidx = 0; ;
                for (int i = 0; i < tempStockList.Count(); i++)
                {
                    if (tempStockList[stockhidx].StPrice < tempStockList[i].StPrice)
                        stockhidx = i;
                    if (tempStockList[stocklidx].StPrice > tempStockList[i].StPrice)
                        stocklidx = i;
                }
                tempStockList[stockhidx].Highpt = true;
                tempStockList[stocklidx].Lowpt = true;

                for (int i = 0; i < tempStockList.Count(); i++)
                {
                    if (tempStockList[stockhidx].StPrice * 1.0002 >= tempStockList[i].StPrice && tempStockList[stockhidx].StPrice * 0.9998 <= tempStockList[i].StPrice)
                        tempStockList[i].Highpt = true;
                    if (tempStockList[stocklidx].StPrice * 1.0002 >= tempStockList[i].StPrice && tempStockList[stocklidx].StPrice * 0.9998 <= tempStockList[i].StPrice)
                        tempStockList[i].Lowpt = true;
                }

                for (int i = 0; i < tempStockList.Count() - 1; i++)
                {
                    if (tempStockList[i].StPrice < tempStockList[i + 1].StPrice)
                        tempStockList[i + 1].Up_down = "Up";
                    else if (tempStockList[i].StPrice > tempStockList[i + 1].StPrice)
                        tempStockList[i + 1].Up_down = "Down";
                    else
                        tempStockList[i + 1].Up_down = "Same";
                }

                List<int> h_compareList = new List<int>();
                List<int> l_compareList = new List<int>();

                for (int i = 0; i < userDrawList.Count(); i++)
                {
                    if (userDrawList[i].Highpt == true)
                        h_compareList.Add(userDrawList[i].Semester);
                    if (userDrawList[i].Lowpt == true)
                        l_compareList.Add(userDrawList[i].Semester);
                }

                bool h_1st_semester = false, h_2st_semester = false, h_3st_semester = false, h_4st_semester = false;
                bool l_1st_semester = false, l_2st_semester = false, l_3st_semester = false, l_4st_semester = false;

                int n = period / 4;
                for (int i = 0; i < h_compareList.Count(); i++)
                {
                    switch (h_compareList[i])
                    {
                        case 1:
                            for (int j = n * 0; j < n * 1; j++)
                                if (tempStockList[j].Highpt == true)
                                    h_1st_semester = true;
                            break;
                        case 2:
                            for (int j = n * 1; j < n * 2; j++)
                                if (tempStockList[j].Highpt == true)
                                    h_2st_semester = true;
                            break;
                        case 3:
                            for (int j = n * 2; j < n * 3; j++)
                                if (tempStockList[j].Highpt == true)
                                    h_3st_semester = true;
                            break;
                        case 4:
                            for (int j = n * 3; j < n * 4; j++)
                                if (tempStockList[j].Highpt == true)
                                    h_4st_semester = true;
                            break;
                    }
                }

                for (int i = 0; i < l_compareList.Count(); i++)
                {
                    switch (l_compareList[i])
                    {
                        case 1:
                            for (int j = n * 0; j < n * 1; j++)
                                if (tempStockList[j].Lowpt == true)
                                    l_1st_semester = true;
                            break;
                        case 2:
                            for (int j = n * 1; j < n * 2; j++)
                                if (tempStockList[j].Lowpt == true)
                                    l_2st_semester = true;
                            break;
                        case 3:
                            for (int j = n * 2; j < n * 3; j++)
                                if (tempStockList[j].Lowpt == true)
                                    l_3st_semester = true;
                            break;
                        case 4:
                            for (int j = n * 3; j < n * 4; j++)
                                if (tempStockList[j].Lowpt == true)
                                    l_4st_semester = true;
                            break;
                    }
                }

                int h_matchcount = 0;
                for (int i = 0; i < h_compareList.Count(); i++)
                {
                    switch (h_compareList[i])
                    {
                        case 1:
                            if (h_1st_semester == true)
                                h_matchcount++;
                            break;
                        case 2:
                            if (h_2st_semester == true)
                                h_matchcount++;
                            break;
                        case 3:
                            if (h_3st_semester == true)
                                h_matchcount++;
                            break;
                        case 4:
                            if (h_4st_semester == true)
                                h_matchcount++;
                            break;
                    }
                }

                int l_matchcount = 0;
                for (int i = 0; i < l_compareList.Count(); i++)
                {
                    switch (l_compareList[i])
                    {
                        case 1:
                            if (l_1st_semester == true)
                                l_matchcount++;
                            break;
                        case 2:
                            if (l_2st_semester == true)
                                l_matchcount++;
                            break;
                        case 3:
                            if (l_3st_semester == true)
                                l_matchcount++;
                            break;
                        case 4:
                            if (l_4st_semester == true)
                                l_matchcount++;
                            break;
                    }
                }

                if (h_compareList.Count() == h_matchcount && l_compareList.Count() == l_matchcount)
                    matchIdxList.Add(scanIdx);

                for (int i = 0; i < tempStockList.Count(); i++)
                {
                    tempStockList[i].Highpt = false;
                    tempStockList[i].Lowpt = false;
                }
                tempStockList.Clear();
            }
        }

        void GetMatchRate_Algorithm(string code)
        {
            double highlow_MatchRate = 0, deviation_MatchRate = 0, slope_MatchRate = 0;

            for (int i = 0; i < userDrawList.Count() - 1; i++)
            {
                if (userDrawList[i].Price > userDrawList[i + 1].Price)
                    userDrawList[i + 1].Up_down = "down";
                else if (userDrawList[i].Price < userDrawList[i + 1].Price)
                    userDrawList[i + 1].Up_down = "up";
                else
                    userDrawList[i + 1].Up_down = "same";
            }

            for (int i = 0; i < stockList.Count() - 1; i++)
            {
                if (stockList[i].StPrice > stockList[i + 1].StPrice)
                    stockList[i + 1].Up_down = "down";
                else if (stockList[i].StPrice < stockList[i + 1].StPrice)
                    stockList[i + 1].Up_down = "up";
                else
                    stockList[i + 1].Up_down = "same";
            }

            for (int initIdx = 0; initIdx < matchIdxList.Count(); initIdx++)
            {
                int startidx = matchIdxList[initIdx];
                for (int i = startidx; i < startidx + period; i++)
                    tempStockList.Add(stockList[i]);

                int headValue = 0;
                headValue = (userDrawList[0].Price + userDrawList[1].Price + userDrawList[2].Price + userDrawList[3].Price + userDrawList[4].Price) / userDrawList.Count();

                for (int i = 0; i < userDrawList.Count(); i++)
                    userDrawList[i].Deviation = userDrawList[i].Price - headValue;

                double tempHeadValue = 0;
                for (int i = 0; i < tempStockList.Count(); i++)
                {
                    tempStockList[i].PricePercent = (int)(tempStockList[i].StPrice / tempStockList[0].StPrice * 100);
                    tempHeadValue += tempStockList[i].PricePercent;
                }
                tempHeadValue = tempHeadValue / tempStockList.Count();

                for (int i = 0; i < tempStockList.Count(); i++)
                {
                    tempStockList[i].Deviation = (int)(tempStockList[i].PricePercent - tempHeadValue);
                    tempStockList[i].Day = i;
                }

                List<UserStock> semesterList = new List<UserStock>();
                for (int i = 0; i < userDrawList.Count(); i++)
                {
                    if (userDrawList[i].Highpt == true || userDrawList[i].Lowpt == true)
                    {
                        semesterList.Add(userDrawList[i]);
                    }
                }

                int n = period / 4;

                int firstSemesterIdx = 0, secondSemesterIdx = 0, thirdSemesterIdx = 0, forthSemesterIdx = 0;

                for (int i = 0; i < semesterList.Count(); i++)
                {
                    if (semesterList[i].Highpt == true)
                    {
                        int semester = semesterList[i].Semester;
                        switch (semester)
                        {
                            case 1:
                                for (int j = n * 0; j < n * 1 - 1; j++)
                                    if (tempStockList[j].StPrice > tempStockList[j + 1].StPrice)
                                    {
                                        if (tempStockList[j].StPrice >= tempStockList[firstSemesterIdx].StPrice)
                                            firstSemesterIdx = j;
                                    }
                                    else if (tempStockList[j].StPrice < tempStockList[j + 1].StPrice)
                                    {
                                        if (tempStockList[firstSemesterIdx].StPrice <= tempStockList[j + 1].StPrice)
                                            firstSemesterIdx = j + 1;
                                    }
                                tempStockList[firstSemesterIdx].Highpt = true;
                                break;
                            case 2:
                                for (int j = n * 1; j < n * 2 - 1; j++)
                                    if (tempStockList[j].StPrice > tempStockList[j + 1].StPrice)
                                    {
                                        if (tempStockList[j].StPrice >= tempStockList[secondSemesterIdx].StPrice)
                                            secondSemesterIdx = j;
                                    }
                                    else if (tempStockList[j].StPrice < tempStockList[j + 1].StPrice)
                                    {
                                        if (tempStockList[secondSemesterIdx].StPrice <= tempStockList[j + 1].StPrice)
                                            secondSemesterIdx = j + 1;
                                    }
                                tempStockList[secondSemesterIdx].Highpt = true;
                                break;
                            case 3:
                                for (int j = n * 2; j < n * 3 - 1; j++)
                                    if (tempStockList[j].StPrice > tempStockList[j + 1].StPrice)
                                    {
                                        if (tempStockList[j].StPrice >= tempStockList[thirdSemesterIdx].StPrice)
                                            thirdSemesterIdx = j;
                                    }
                                    else if (tempStockList[j].StPrice < tempStockList[j + 1].StPrice)
                                    {
                                        if (tempStockList[thirdSemesterIdx].StPrice <= tempStockList[j + 1].StPrice)
                                            thirdSemesterIdx = j + 1;
                                    }
                                tempStockList[thirdSemesterIdx].Highpt = true;
                                break;
                            case 4:
                                for (int j = n * 3; j < n * 4 - 1; j++)
                                    if (tempStockList[j].StPrice > tempStockList[j + 1].StPrice)
                                    {
                                        if (tempStockList[j].StPrice >= tempStockList[forthSemesterIdx].StPrice)
                                            forthSemesterIdx = j;
                                    }
                                    else if (tempStockList[j].StPrice < tempStockList[j + 1].StPrice)
                                    {
                                        if (tempStockList[forthSemesterIdx].StPrice <= tempStockList[j + 1].StPrice)
                                            forthSemesterIdx = j + 1;
                                    }
                                tempStockList[forthSemesterIdx].Highpt = true;
                                break;
                        }
                    }
                    if (semesterList[i].Lowpt == true)
                    {
                        int semester = semesterList[i].Semester;
                        switch (semester)
                        {
                            case 1:
                                for (int j = n * 0; j < n * 1 - 1; j++)
                                    if (tempStockList[j].StPrice < tempStockList[j + 1].StPrice)
                                    {
                                        if (tempStockList[j].StPrice <= tempStockList[firstSemesterIdx].StPrice)
                                            firstSemesterIdx = j;
                                    }
                                    else if (tempStockList[j].StPrice > tempStockList[j + 1].StPrice)
                                    {
                                        if (tempStockList[firstSemesterIdx].StPrice >= tempStockList[j + 1].StPrice)
                                            firstSemesterIdx = j + 1;
                                    }
                                tempStockList[firstSemesterIdx].Lowpt = true;
                                break;
                            case 2:
                                for (int j = n * 1; j < n * 2 - 1; j++)
                                    if (tempStockList[j].StPrice < tempStockList[j + 1].StPrice)
                                    {
                                        if (tempStockList[j].StPrice <= tempStockList[secondSemesterIdx].StPrice)
                                            secondSemesterIdx = j;
                                    }
                                    else if (tempStockList[j].StPrice > tempStockList[j + 1].StPrice)
                                    {
                                        if (tempStockList[secondSemesterIdx].StPrice >= tempStockList[j + 1].StPrice)
                                            secondSemesterIdx = j + 1;
                                    }
                                tempStockList[secondSemesterIdx].Lowpt = true;
                                break;
                            case 3:
                                for (int j = n * 2; j < n * 3 - 1; j++)
                                    if (tempStockList[j].StPrice < tempStockList[j + 1].StPrice)
                                    {
                                        if (tempStockList[j].StPrice <= tempStockList[thirdSemesterIdx].StPrice)
                                            thirdSemesterIdx = j;
                                    }
                                    else if (tempStockList[j].StPrice > tempStockList[j + 1].StPrice)
                                    {
                                        if (tempStockList[thirdSemesterIdx].StPrice >= tempStockList[j + 1].StPrice)
                                            thirdSemesterIdx = j + 1;
                                    }
                                tempStockList[thirdSemesterIdx].Lowpt = true;
                                break;
                            case 4:
                                for (int j = n * 3; j < n * 4 - 1; j++)
                                    if (tempStockList[j].StPrice < tempStockList[j + 1].StPrice)
                                    {
                                        if (tempStockList[j].StPrice <= tempStockList[forthSemesterIdx].StPrice)
                                            forthSemesterIdx = j;
                                    }
                                    else if (tempStockList[j].StPrice > tempStockList[j + 1].StPrice)
                                    {
                                        if (tempStockList[forthSemesterIdx].StPrice >= tempStockList[j + 1].StPrice)
                                            forthSemesterIdx = j + 1;
                                    }
                                tempStockList[forthSemesterIdx].Lowpt = true;
                                break;
                        }
                    }
                }

                for (int i = 0; i < tempStockList.Count(); i++)
                {
                    if (tempStockList[i].Highpt == true)
                    {
                        int maxIdx = 0;
                        if (n * 0 <= i && i < n * 1)
                        {
                            for (int j = n * 0; j < n * 1 - 1; j++)
                            {
                                if (tempStockList[j].StPrice > tempStockList[j + 1].StPrice)
                                {
                                    if (tempStockList[j].StPrice >= tempStockList[maxIdx].StPrice)
                                        maxIdx = j;
                                }
                                else if (tempStockList[j].StPrice < tempStockList[j + 1].StPrice)
                                {
                                    if (tempStockList[maxIdx].StPrice <= tempStockList[j + 1].StPrice)
                                        maxIdx = j + 1;
                                }
                            }
                            tempStockList[maxIdx].Charactor = true;
                        }
                        else if (n * 1 <= i && i < n * 2)
                        {
                            for (int j = n * 1; j < n * 2 - 1; j++)
                            {
                                if (tempStockList[j].StPrice > tempStockList[j + 1].StPrice)
                                {
                                    if (tempStockList[j].StPrice >= tempStockList[maxIdx].StPrice)
                                        maxIdx = j;
                                }
                                else if (tempStockList[j].StPrice < tempStockList[j + 1].StPrice)
                                {
                                    if (tempStockList[maxIdx].StPrice <= tempStockList[j + 1].StPrice)
                                        maxIdx = j + 1;
                                }
                            }
                            tempStockList[maxIdx].Charactor = true;
                        }
                        else if (n * 2 <= i && i < n * 3)
                        {
                            for (int j = n * 2; j < n * 3 - 1; j++)
                            {
                                if (tempStockList[j].StPrice > tempStockList[j + 1].StPrice)
                                {
                                    if (tempStockList[j].StPrice >= tempStockList[maxIdx].StPrice)
                                        maxIdx = j;
                                }
                                else if (tempStockList[j].StPrice < tempStockList[j + 1].StPrice)
                                {
                                    if (tempStockList[maxIdx].StPrice <= tempStockList[j + 1].StPrice)
                                        maxIdx = j + 1;
                                }
                            }
                            tempStockList[maxIdx].Charactor = true;
                        }
                        else if (n * 3 <= i && i < n * 4)
                        {
                            for (int j = n * 3; j < n * 4 - 1; j++)
                            {
                                if (tempStockList[j].StPrice > tempStockList[j + 1].StPrice)
                                {
                                    if (tempStockList[j].StPrice >= tempStockList[maxIdx].StPrice)
                                        maxIdx = j;
                                }
                                else if (tempStockList[j].StPrice < tempStockList[j + 1].StPrice)
                                {
                                    if (tempStockList[maxIdx].StPrice <= tempStockList[j + 1].StPrice)
                                        maxIdx = j + 1;
                                }
                            }
                            tempStockList[maxIdx].Charactor = true;
                        }
                    }

                    else if (tempStockList[i].Lowpt == true)
                    {
                        int lowIdx = 0;
                        if (n * 0 <= i && i < n * 1)
                        {
                            for (int j = n * 0; j < n * 1 - 1; j++)
                            {
                                if (tempStockList[j].StPrice < tempStockList[j + 1].StPrice)
                                {
                                    if (tempStockList[j].StPrice <= tempStockList[lowIdx].StPrice)
                                        lowIdx = j;
                                }
                                else if (tempStockList[j].StPrice > tempStockList[j + 1].StPrice)
                                {
                                    if (tempStockList[lowIdx].StPrice >= tempStockList[j + 1].StPrice)
                                        lowIdx = j + 1;
                                }
                            }
                            tempStockList[lowIdx].Charactor = true;
                        }
                        else if (n * 1 <= i && i < n * 2)
                        {
                            for (int j = n * 1; j < n * 2 - 1; j++)
                            {
                                if (tempStockList[j].StPrice < tempStockList[j + 1].StPrice)
                                {
                                    if (tempStockList[j].StPrice <= tempStockList[lowIdx].StPrice)
                                        lowIdx = j;
                                }
                                else if (tempStockList[j].StPrice > tempStockList[j + 1].StPrice)
                                {
                                    if (tempStockList[lowIdx].StPrice >= tempStockList[j + 1].StPrice)
                                        lowIdx = j + 1;
                                }
                            }
                            tempStockList[lowIdx].Charactor = true;
                        }
                        else if (n * 2 <= i && i < n * 3)
                        {
                            for (int j = n * 2; j < n * 3 - 1; j++)
                            {
                                if (tempStockList[j].StPrice < tempStockList[j + 1].StPrice)
                                {
                                    if (tempStockList[j].StPrice <= tempStockList[lowIdx].StPrice)
                                        lowIdx = j;
                                }
                                else if (tempStockList[j].StPrice > tempStockList[j + 1].StPrice)
                                {
                                    if (tempStockList[lowIdx].StPrice >= tempStockList[j + 1].StPrice)
                                        lowIdx = j + 1;
                                }
                            }
                            tempStockList[lowIdx].Charactor = true;
                        }
                        else if (n * 3 <= i && i < n * 4)
                        {
                            for (int j = n * 3; j < n * 4 - 1; j++)
                            {
                                if (tempStockList[j].StPrice < tempStockList[j + 1].StPrice)
                                {
                                    if (tempStockList[j].StPrice <= tempStockList[lowIdx].StPrice)
                                        lowIdx = j;
                                }
                                else if (tempStockList[j].StPrice > tempStockList[j + 1].StPrice)
                                {
                                    if (tempStockList[lowIdx].StPrice >= tempStockList[j + 1].StPrice)
                                        lowIdx = j + 1;
                                }
                            }
                            tempStockList[lowIdx].Charactor = true;
                        }
                    }
                }

                double slope = 0, hypotenuse = 0;
                for (int i = 0; i < userDrawList.Count() - 1; i++)
                {
                    slope = (userDrawList[i + 1].Price - userDrawList[i].Price) / (userDrawList[i + 1].Day - userDrawList[i].Day);
                    userDrawList[i].Slope = slope;

                    hypotenuse = Math.Sqrt(Math.Pow(userDrawList[i + 1].Day - userDrawList[i].Day, 2) + Math.Pow(userDrawList[i + 1].Price - userDrawList[i].Price, 2));
                    userDrawList[i].Hypotenuse = hypotenuse;
                }



                int slope_MatchIdx = 0, slope_MatchCount = 0;

                for (int i = 0; i < userDrawList.Count() - 1; i++)
                {
                    for (int j = slope_MatchIdx; j < tempStockList.Count(); j++)
                    {
                        if (tempStockList[j].Charactor == true)
                        {
                            if (tempStockList[j].Day - tempStockList[slope_MatchIdx].Day != 0)
                                slope = (double)(tempStockList[j].PricePercent - tempStockList[slope_MatchIdx].PricePercent) / (double)(tempStockList[j].Day - tempStockList[slope_MatchIdx].Day);
                            tempStockList[slope_MatchIdx].Slope = slope;

                            hypotenuse = Math.Sqrt(Math.Pow(tempStockList[j].Day - tempStockList[slope_MatchIdx].Day, 2) + Math.Pow(tempStockList[j].PricePercent - tempStockList[slope_MatchIdx].PricePercent, 2));
                            tempStockList[slope_MatchIdx].Hypotenuse = hypotenuse;

                            if (userDrawList[i].Slope + 0.5 >= tempStockList[slope_MatchIdx].Slope && userDrawList[i].Slope - 0.5 <= tempStockList[slope_MatchIdx].Slope)
                                if (userDrawList[i].Hypotenuse + 2 >= tempStockList[slope_MatchIdx].Hypotenuse && userDrawList[i].Hypotenuse - 2 <= tempStockList[slope_MatchIdx].Hypotenuse)
                                {
                                    slope_MatchCount++;
                                }
                        }
                    }
                }

                int highlow_Matchcount = 0;

                for (int j = 0; j < tempStockList.Count(); j++)
                {
                    if (j >= n * 0 && j < n * 1)
                        if (tempStockList[j].Up_down == userDrawList[1].Up_down)
                            highlow_Matchcount++;
                    if (j >= n * 1 && j < n * 2)
                        if (tempStockList[j].Up_down == userDrawList[2].Up_down)
                            highlow_Matchcount++;
                    if (j >= n * 2 && j < n * 3)
                        if (tempStockList[j].Up_down == userDrawList[3].Up_down)
                            highlow_Matchcount++;
                    if (j >= n * 3 && j < n * 4)
                        if (tempStockList[j].Up_down == userDrawList[4].Up_down)
                            highlow_Matchcount++;
                }

                int highlow_DivCount = tempStockList.Count();
                highlow_MatchRate = highlow_Matchcount * defaultHighLowRate / highlow_DivCount;


                double userDeviation = 0;
                double stockDeviation = 0;

                for (int i = 0; i < userDrawList.Count(); i++)
                    userDeviation += Math.Abs(userDrawList[i].Deviation);

                for (int i = 0; i < tempStockList.Count(); i++)
                    if (tempStockList[i].Charactor == true)
                        stockDeviation += Math.Abs(tempStockList[i].Deviation);

                deviation_MatchRate = defaultDeviRate - (Math.Abs(stockDeviation - userDeviation) * defaultDeviRate / userDeviation);


                int SlopeDivCount = userDrawList.Count() - 1;
                slope_MatchRate = slope_MatchCount * defaultslopeRate / SlopeDivCount;



                if (period == 20)
                {
                    if (highlow_MatchRate + deviation_MatchRate + slope_MatchRate > 30)
                    {
                        string[] resultS1 = new string[] { code, stockList[initIdx].Date, highlow_MatchRate.ToString(), deviation_MatchRate.ToString(), slope_MatchRate.ToString(), string.Format("{0}%", highlow_MatchRate + deviation_MatchRate + slope_MatchRate), matchIdxList[initIdx].ToString() };
                        ListViewItem resultviewItem = new ListViewItem(resultS1);
                        ResultView.Items.Add(resultviewItem);
                    }
                }
                else if (period == 60)
                {
                    if (highlow_MatchRate + deviation_MatchRate + slope_MatchRate > 20)
                    {
                        string[] resultS1 = new string[] { code, stockList[initIdx].Date, highlow_MatchRate.ToString(), deviation_MatchRate.ToString(), slope_MatchRate.ToString(), string.Format("{0}%", highlow_MatchRate + deviation_MatchRate + slope_MatchRate), matchIdxList[initIdx].ToString() };
                        ListViewItem resultviewItem = new ListViewItem(resultS1);
                        ResultView.Items.Add(resultviewItem);
                    }
                }
                else if (period == 120)
                {
                    if (highlow_MatchRate + deviation_MatchRate + slope_MatchRate > 10)
                    {
                        string[] resultS1 = new string[] { code, stockList[initIdx].Date, highlow_MatchRate.ToString(), deviation_MatchRate.ToString(), slope_MatchRate.ToString(), string.Format("{0}%", highlow_MatchRate + deviation_MatchRate + slope_MatchRate), matchIdxList[initIdx].ToString() };
                        ListViewItem resultviewItem = new ListViewItem(resultS1);
                        ResultView.Items.Add(resultviewItem);
                    }
                }

                tempStockList.Clear();
            }
        }


        class Stock
        {
            private string code;
            private string date;
            int day;
            private double stprice;
            private string up_down;
            bool highpt;
            bool lowpt;
            bool charactor;
            int deviation;
            int pricePercent;
            double slope;
            double hypotenuse;
            public Stock() { }
            public Stock(string d, int sp) { date = d; stprice = sp; }
            public string Date { get { return date; } set { date = value; } }
            public int Day { get { return day; } set { day = value; } }

            public string Code { get { return code; } set { code = value; } }
            public double StPrice { get { return stprice; } set { stprice = value; } }
            public string Up_down { get { return up_down; } set { up_down = value; } }
            public bool Highpt { get { return highpt; } set { highpt = value; } }
            public bool Lowpt { get { return lowpt; } set { lowpt = value; } }
            public bool Charactor { get { return charactor; } set { charactor = value; } }
            public int Deviation { get { return deviation; } set { deviation = value; } }
            public int PricePercent { get { return pricePercent; } set { pricePercent = value; } }
            public double Slope { get { return slope; } set { slope = value; } }
            public double Hypotenuse { get { return hypotenuse; } set { hypotenuse = value; } }
        }
        class UserStock
        {
            private int price;
            int day;
            private string up_down;
            bool highpt;
            bool lowpt;
            int semester;
            int deviation;
            double slope;
            double hypotenuse;
            public UserStock() { }
            public UserStock(int p) { price = p; }
            public int Price { get { return price; } set { price = value; } }
            public int Day { get { return day; } set { day = value; } }
            public string Up_down { get { return up_down; } set { up_down = value; } }
            public bool Highpt { get { return highpt; } set { highpt = value; } }
            public bool Lowpt { get { return lowpt; } set { lowpt = value; } }
            public int Semester { get { return semester; } set { semester = value; } }
            public int Deviation { get { return deviation; } set { deviation = value; } }
            public double Slope { get { return slope; } set { slope = value; } }
            public double Hypotenuse { get { return hypotenuse; } set { hypotenuse = value; } }
        }

        private void ResultView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ResultView.SelectedItems.Count < 1) return;

            string code = ResultView.SelectedItems[0].SubItems[0].Text;
            string idx = ResultView.SelectedItems[0].SubItems[6].Text;

            stockchart.Series.Clear();

            stockList.Clear();
            matchIdxList.Clear();

            string connectionString =
               "server = 127.0.0.1,21433; uid = sa; pwd = sj12sj12; database = StockInformation1;";
            SqlConnection scon = null;
            SqlCommand scom = null;
            SqlDataReader sdr = null;

            try
            {
                scon = new SqlConnection(connectionString);
                scon.Open();

                scom = new SqlCommand();
                scom.Connection = scon;
                string findDB = string.Format("select 일자, 시가 from Day_C_Data where 종목코드 in ({0}) order by 일자 asc", code);
                scom.CommandText = findDB;


                sdr = scom.ExecuteReader();

                while (sdr.Read())
                {
                    string d = sdr["일자"].ToString();
                    int sp = int.Parse(sdr["시가"].ToString());

                    stockList.Add(new Stock(d, sp));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                if (scon != null)
                {
                    scon.Close();
                }
            }

            Series data = new Series();

            data.ChartType = SeriesChartType.Line;
            data.Name = "Price";

            int num = int.Parse(idx);
            for (int i = num; i < num + period; i++)
                data.Points.Add(stockList[i].StPrice);

            stockchart.Series.Add(data);

            if (period == 20)
            {
                stockchart.ChartAreas[0].AxisY.Maximum = stockList[num].StPrice * 1.1;
                stockchart.ChartAreas[0].AxisY.Minimum = stockList[num].StPrice * 0.9;
            }
            else if (period == 60)
            {
                stockchart.ChartAreas[0].AxisY.Maximum = stockList[num].StPrice * 1.2;
                stockchart.ChartAreas[0].AxisY.Minimum = stockList[num].StPrice * 0.8;
            }
            else if (period == 120)
            {
                stockchart.ChartAreas[0].AxisY.Maximum = stockList[num].StPrice * 1.3;
                stockchart.ChartAreas[0].AxisY.Minimum = stockList[num].StPrice * 0.7;
            }
        }

        private void Default_MRate_btn_Click(object sender, EventArgs e)
        {
            hlRate_textbox.Text = "30";
            devRate_textbox.Text = "30";
            slopeRate_textbox.Text = "40";
        }

        private void Clear_UserBoard_btn_Click(object sender, EventArgs e)
        {
            userDrawList.Clear();
            drawptList.Clear();

            drawptList.Add(new Point(0, DrawPannel.Height / 2));
            drawptList.Add(new Point(DrawPannel.Width, DrawPannel.Height / 2));

            DrawPannel_Paint(sender, null);
        }

        private void Period_20btn_Click(object sender, EventArgs e)
        {
            period = 20;
            label12.Text = "20 일";
        }

        private void Period_60btn_Click(object sender, EventArgs e)
        {
            period = 60;
            label12.Text = "60 일";
        }

        private void Period_120btn_Click(object sender, EventArgs e)
        {
            period = 120;
            label12.Text = "120 일";
        }
    }
}
