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
    public partial class DXChartBaseControl : UserControl
    {
        public Grid _chartGrid
        {
            get { return this.gridChart; }
        }
        public Grid _tableGrid
        {
            get { return this.gridTable; }
        }


        public DXChartBaseControl()
        {
            InitializeComponent();
        }

        public DXChartBaseControl( Control chartCtl,Grid tableCtl)
        {
            InitializeComponent();
            this.gridTable.Children.Clear();
            this.gridTable.Children.Add(tableCtl);

            this.gridChart.Children.Clear();
            this.gridChart.Children.Add(chartCtl);
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        
    }
}
