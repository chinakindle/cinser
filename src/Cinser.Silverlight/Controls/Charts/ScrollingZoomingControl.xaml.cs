using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using DevExpress.Xpf.Charts;
using System.Xml;

namespace Cinser.Silverlight.Controls.Charts
{
    public partial class ScrollingZoomingControl : UserControl, IXYChart
    {
        public ScrollingZoomingControl()
        {
            InitializeComponent();
        }

        public ScrollingZoomingControl(List<SeriesItem> products)
        {
            InitializeComponent();
            this.products = products;
        }

        public class SeriesItem
        {
            string title;
            ScaleType argumentScaleType = ScaleType.Qualitative;
            bool seriesLabelVisible = true;

            public bool SeriesLabelVisible
            {
                get { return seriesLabelVisible; }
                set { seriesLabelVisible = value; }
            }
 
            public ScaleType ArgumentScaleType
            {
                get { return argumentScaleType; }
                set { argumentScaleType = value; }
            }

            public string Title
            {
                get { return title; }
                set { title = value; }
            }
            List<SeriesPoint> seriesPoints;

            public List<SeriesPoint> SeriesPoints
            {
                get { return seriesPoints; }
                set { seriesPoints = value; }
            }

            public SeriesItem()
            {
                this.Title = title;
                this.SeriesPoints = seriesPoints;
            }

            public SeriesItem(string title, List<SeriesPoint> seriesPoints)
            {
                this.Title = title;
                this.SeriesPoints = seriesPoints;
            }
        }


        public string XTitle
        {
            get
            {
                return this.xTitle.Content.ToString();
            }
            set
            {
                this.xTitle.Content = value;
            }
        }

        public string YTitle
        {
            get
            {
                return this.yTitle.Content.ToString();
            }
            set
            {
                this.yTitle.Content = value;
            }
        }

        public ScaleType ScaleType
        {
            get;
            set;
        }

        public string ChartTitle
        {
            get
            {
                return this.Title.Content.ToString();
            }
            set
            {
                this.Title.Content = value;
            }
        }

        public string ChartRemark
        {
            get
            {
                return this.cTitle.Content.ToString();
            }
            set
            {
                this.cTitle.Content = value;
            }
        }

        public ChartControl ActualChart
        {
            get
            {
                return this.chart;
            }
        }

        List<SeriesItem> products;

        public List<SeriesItem> Products
        {
            get { return products; }
            set { products = value; }
        }

        public void InitUI()
        {
            if (Products != null)
            {
                for (int i = 0; i < Products.Count; i++)
                {
                    LineSeries2D series = new LineSeries2D();
                    series.DataSource = Products[i].SeriesPoints;
                    series.ArgumentDataMember = "Argument";
                    series.ValueDataMember = "Value";
                    series.ArgumentScaleType = this.ScaleType;
                    series.DisplayName = Products[i].Title;
                    if (series.Label == null)
                        series.Label = new SeriesLabel();
                    series.Label.Visible = products[i].SeriesLabelVisible;
                    this.ActualChart.Diagram.Series.Add(series);
                }
            }
        }

        void chart_MouseMove(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(chart);
            ChartHitInfo hitInfo = chart.CalcHitInfo(position);
            if (hitInfo != null && hitInfo.SeriesPoint != null)
            {
                string xStr = "X";
                string yStr = "Y";
                if (string.IsNullOrEmpty(XTitle) == false)
                    xStr = XTitle;

                if (string.IsNullOrEmpty(YTitle) == false)
                    yStr = YTitle;

                string xValue = hitInfo.SeriesPoint.Argument;
                if (ScaleType == DevExpress.Xpf.Charts.ScaleType.DateTime)
                    xValue = DateTime.Parse(xValue).ToString("yyyy-MM-dd hh:mm");

                ttContent.Text = string.Format(xStr + " = {0}\n" + yStr + " = {1}", xValue, Math.Round(hitInfo.SeriesPoint.NonAnimatedValue, 2));
                pointTooltip.HorizontalOffset = 1;
                pointTooltip.VerticalOffset = 1;
                pointTooltip.IsOpen = true;
                Cursor = Cursors.Hand;
            }
            else
            {
                pointTooltip.IsOpen = false;
                Cursor = Cursors.Arrow;
            }
        }
        void chart_MouseLeave(object sender, MouseEventArgs e)
        {
            pointTooltip.IsOpen = false;
        }
    }
}
