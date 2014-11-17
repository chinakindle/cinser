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
    public partial class ConstantLinesControl : UserControl,IXYChart
    {

        public void SetDataSource(List<SeriesPoint> ChartItems)
        {
            chart.Diagram.Series[0].DataSource = ChartItems;
            if (chart.Diagram.Series[0].Label == null)
                chart.Diagram.Series[0].Label = new SeriesLabel();
            chart.Diagram.Series[0].Label.Visible = SeriesLabelVisible;
            chart.Animate();
        }

        ConstantLineCollection ConstantLines { get { return ((XYDiagram2D)chart.Diagram).AxisY.ConstantLinesBehind; } }

        public ConstantLinesControl()
        {
            InitializeComponent();
        }

        public ConstantLinesControl(string chartRemark, string Title, string xTitle, string yTitle, List<SeriesPoint> ChartItems)
        {
            InitializeComponent();
            this.ChartRemark = chartRemark;
            this.ChartTitle = Title;
            this.XTitle = xTitle;
            this.YTitle = yTitle;
            SetDataSource(ChartItems);
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


        private void chart_BoundDataChanged(object sender, RoutedEventArgs e) {
            XYDiagram2D diagram = (XYDiagram2D)chart.Diagram;
            if (diagram.Series[0].Points.Count == 0)
                return;
            double minValue = Double.MaxValue;
            double maxValue = 0;
            double averageValue = 0;
            foreach (SeriesPoint point in diagram.Series[0].Points) {
                double Value = point.Value;
                if (Value < minValue)
                    minValue = Value;
                if (Value > maxValue)
                    maxValue = Value;
                averageValue += Value;
            }
            averageValue /= diagram.Series[0].Points.Count;
            ConstantLine minConstantLine = new ConstantLine(minValue, "最小值:" + minValue.ToString("0.00"));
            minConstantLine.Brush = new SolidColorBrush(Colors.Green);
            minConstantLine.Title.Foreground = new SolidColorBrush(Colors.Green);
            ConstantLine maxConstantLine = new ConstantLine(maxValue, "最大值:" + maxValue.ToString("0.00"));
            maxConstantLine.Brush = new SolidColorBrush(Colors.Red);
            maxConstantLine.Title.Foreground = new SolidColorBrush(Colors.Red);
            ConstantLine averageConstantLine = new ConstantLine(averageValue, "平均值:" + averageValue.ToString("0.00"));
            averageConstantLine.Brush = new SolidColorBrush(Color.FromArgb(0xFF, 0x9A, 0xCD, 0x32));
            averageConstantLine.Title.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x9A, 0xCD, 0x32));
            ConstantLines.AddRange(new ConstantLine[] { minConstantLine, maxConstantLine, averageConstantLine });
            foreach (ConstantLine constantLine in ConstantLines)
                constantLine.Title.Alignment = ConstantLineTitleAlignment.Far;
        }

        bool seriesLabelVisible = true;

        public bool SeriesLabelVisible
        {
            get { return seriesLabelVisible; }
            set { seriesLabelVisible = value; }
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
            get
            {
                return Series.ArgumentScaleType;
            }
            set
            {
                Series.ArgumentScaleType = value;
            }
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

        public void InitUI()
        {
            throw new NotImplementedException();
        }
    }
}
