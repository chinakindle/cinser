using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cinser.ArcGIS.Controls
{
    /// <summary>
    /// LegendUI.xaml 的交互逻辑
    /// </summary>
    public partial class LegendUI : UserControl
    {
        public LegendUI()
        {
            InitializeComponent();
        }

        public void InitUI(ESRI.ArcGIS.Client.ArcGISDynamicMapServiceLayer pMapLayer)
        {
            this.LayerTransparencySetUI1.SetMapLayer(pMapLayer);
            this.MapLayersTree1.InitUI(pMapLayer);
        }

    }
}
