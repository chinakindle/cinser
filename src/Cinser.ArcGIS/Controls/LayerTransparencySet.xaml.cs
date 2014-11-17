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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ESRI.ArcGIS.Client;

namespace Cinser.ArcGIS.Controls
{
    /// <summary>
    /// LayerTransparencySet.xaml 的交互逻辑
    /// </summary>
    public partial class LayerTransparencySet : UserControl
    {
         private ArcGISDynamicMapServiceLayer _mapLayer = null;


         public LayerTransparencySet()
        {
            InitializeComponent();
            this.TrackBarEdit1.Minimum = 0;
            this.TrackBarEdit1.Maximum = 1;
            this.TrackBarEdit1.SmallStep = 0.01;
        }


        public void SetMapLayer(ESRI.ArcGIS.Client.ArcGISDynamicMapServiceLayer pMapLayer)
        {
            this._mapLayer = pMapLayer;
            this.TrackBarEdit1.Value = pMapLayer.Opacity;
        }

        private void TrackBarEdit1_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            this._mapLayer.Opacity = this.TrackBarEdit1.Value;
        }
    }
}
