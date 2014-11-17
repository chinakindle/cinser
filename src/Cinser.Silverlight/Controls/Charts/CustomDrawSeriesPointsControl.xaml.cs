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
using System.Xml;
using System.Globalization;
using DevExpress.Xpf.Charts;

namespace Cinser.Silverlight.Controls.Charts
{
    public partial class CustomDrawSeriesPointsControl : UserControl,IXYChart
    {
        List<SeriesPoint> dataSource;
        public CustomDrawSeriesPointsControl()
        {
            InitializeComponent();
        }

        public CustomDrawSeriesPointsControl(List<SeriesPoint> dataSource)
        {
            InitializeComponent();
            this.dataSource = dataSource;
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

        bool seriesLabelShowArgument = false;

        public bool SeriesLabelShowArgument
        {
            get { return seriesLabelShowArgument; }
            set { seriesLabelShowArgument = value; }
        }

        public Axis2D AxisY2D
        {
            get { return _y; }
        }

        public void InitUI()
        {
            chart.Diagram.Series[0].DataSource = dataSource;
            chart.Animate();
            InitBarsColors();
        }
        void chart_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {
            if (SeriesLabelShowArgument)
            {
                e.DrawOptions.Color = GetColor();
                e.LabelText = e.SeriesPoint.Argument + ": " + e.LabelText;
                _x.Visible = false;
            }
        }


        void InitBarsColors()
        {
            colors = new List<Color>();
            
            colors.Add(Colors.Red);//红色
            colors.Add(Colors.Green);//绿色

            colors.Add(Colors.Blue);//蓝色
            colors.Add(Colors.Brown);

            colors.Add(CommonFunc.GetColorFromRgbColorString("8A2BE2"));
            colors.Add(CommonFunc.GetColorFromRgbColorString("A52A2A"));
            colors.Add(CommonFunc.GetColorFromRgbColorString("DEB887"));
            colors.Add(CommonFunc.GetColorFromRgbColorString("5F9EA0"));
            colors.Add(CommonFunc.GetColorFromRgbColorString("D2691E"));
            colors.Add(CommonFunc.GetColorFromRgbColorString("6495ED"));
            colors.Add(CommonFunc.GetColorFromRgbColorString("DC143C"));
            colors.Add(CommonFunc.GetColorFromRgbColorString("00008B"));
            colors.Add(CommonFunc.GetColorFromRgbColorString("008B8B"));
            colors.Add(CommonFunc.GetColorFromRgbColorString("006400"));
            colors.Add(CommonFunc.GetColorFromRgbColorString("8B008B"));

            colors.Add(CommonFunc.GetColorFromRgbColorString("FF8C00"));
            colors.Add(CommonFunc.GetColorFromRgbColorString("8FBC8F"));
            colors.Add(CommonFunc.GetColorFromRgbColorString("483D8B"));
            colors.Add(CommonFunc.GetColorFromRgbColorString("00CED1"));
            colors.Add(CommonFunc.GetColorFromRgbColorString("9400D3"));
            colors.Add(CommonFunc.GetColorFromRgbColorString("FF1493"));
            colors.Add(CommonFunc.GetColorFromRgbColorString("8B008B"));
            colors.Add(CommonFunc.GetColorFromRgbColorString("808000"));


        }
        

        List<Color>colors;
        Color GetColor()
        {
            if (colors == null)
                InitBarsColors();
            Color color = Colors.Gray;
            Random r = new Random();
            int index = r.Next(0, colors.Count - 1);
            color = colors[index];
            colors.Remove(colors[index]);
            if (colors.Count < 2)
                InitBarsColors();
            return color;
        }
    }
}

