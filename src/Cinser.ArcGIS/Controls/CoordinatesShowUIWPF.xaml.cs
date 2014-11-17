



using System;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace Cinser.ArcGIS.Controls
{
    public partial class CoordinatesShowUIWPF : UserControl
    {

        //默认坐标系：web墨卡托/经纬度
        public enum CoordinateSystemType
        {
            WebMercator,
            WGS84
        }

        public CoordinateSystemType CoordinateSystem { get; set; }

        public CoordinatesShowUIWPF()
        {
            InitializeComponent();
            this.Border1.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 114, 159, 219));
            this.Border1.Background = new SolidColorBrush(Color.FromArgb(200, 202, 224, 254));
        }

        private Map _map = null;


        public Map Map
        {
            get { return this._map; }
            set
            {
                this._map = value;
                if (this._map != null)
                {
                    this._map.MouseMove += new MouseEventHandler(_map_MouseMove);
                }
            }
        }


        void _map_MouseMove(object sender, MouseEventArgs e)
        {
            MapPoint myMapPoint = this._map.ScreenToMap(e.GetPosition(this._map));
            if (myMapPoint == null)
            {
                this.lblC.Content = "";
                return;
            }
            if (CoordinateSystem != CoordinateSystemType.WGS84)
                myMapPoint = this.Mercator2lonLat(myMapPoint);
            this.lblC.Content = "经度：" + myMapPoint.X.ToString("0.000") + ",纬度：" + myMapPoint.Y.ToString("0.000");
        }


        public MapPoint lonLat2Mercator(MapPoint lonLat)
        {
            MapPoint mercator = new MapPoint();
            double x = lonLat.X * 20037508.3427892 / 180;
            double y = Math.Log(Math.Tan((90 + lonLat.Y) * Math.PI / 360)) / (Math.PI / 180);
            y = y * 20037508.3427892 / 180;
            mercator.X = x;
            mercator.Y = y;
            return mercator;
        }

        //墨卡托转经纬度
        public MapPoint Mercator2lonLat(MapPoint mercator)
        {
            MapPoint lonLat = new MapPoint();
            double x = mercator.X / 20037508.3427892 * 180;
            double y = mercator.Y / 20037508.3427892 * 180;
            y = 180 / Math.PI * (2 * Math.Atan(Math.Exp(y * Math.PI / 180)) - Math.PI / 2);
            lonLat.X = x;
            lonLat.Y = y;
            return lonLat;
        }
    }
}
