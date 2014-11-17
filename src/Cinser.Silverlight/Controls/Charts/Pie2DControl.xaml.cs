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
    public partial class Pie2DControl : UserControl,IChart
    {
        public string ChartTitle
        {
            get { return this.chartTitle.Content.ToString(); }
            set { this.chartTitle.Content = value; }
        }

        public ChartControl ActualChart { get { return chart; } }
        List<SeriesPoint> dataSource;

        public Pie2DControl()
        {
            InitializeComponent();
        }

        public Pie2DControl(List<SeriesPoint> dataSource)
        {
            InitializeComponent();
            this.dataSource = dataSource;
        }

        void chart_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {
            e.LabelText = e.LabelText;                
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

        public void InitUI()
        {
            chart.Diagram.Series[0].DataSource = dataSource;
            chart.Animate();
        }
    }
}

