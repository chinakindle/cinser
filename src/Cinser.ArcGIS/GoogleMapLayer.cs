using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;

namespace Cinser.ArcGIS
{
    public class GoogleMapLayer : TiledMapServiceLayer
    {
        private const double cornerCoordinate = 20037508.3427892;
        private string _URL = "";
        private int _LodCount = 0;


        private GoogleMapTylpe _GoogleMapType = GoogleMapTylpe.Image;


        public GoogleMapLayer()
        {
            this.GoogleMapTylpe = GoogleMapTylpe.Image;
        }


        public GoogleMapTylpe GoogleMapTylpe
        {
            get { return this._GoogleMapType; }
            set
            {
                this._GoogleMapType = value;
                if (this._GoogleMapType == GoogleMapTylpe.Terrain)
                {
                    _URL = "http://mt1.google.cn/vt/lyrs=t@131,r@225000000&hl=zh-CN&gl=CN&src=app&x={0}&y={1}&z={2}&s=Ga";
                    this._LodCount = 16;
                }
                else if (this._GoogleMapType == GoogleMapTylpe.Image)
                {
                    //_URL = "http://mt1.google.cn/vt/lyrs=s@132&hl=zh-CN&gl=CN&src=app&x={0}&y={1}&z={2}&s=G";
                    _URL = "http://mt2.google.cn/vt/lyrs=s@169000000,m@169000000&hl=zh-CN&gl=cn&s=Galil&x={0}&y={1}&z={2}";
                    this._LodCount = 20;
                }
                else if (this._GoogleMapType == GoogleMapTylpe.Street)
                {
                    _URL = "http://mt1.google.cn/vt/lyrs=m@225000000&hl=zh-CN&gl=CN&src=app&x={0}&y={1}&z={2}&s=Ga";
                    this._LodCount = 19;
                }
                this.Refresh();
            }
        }


        public override void Initialize()
        {
            ESRI.ArcGIS.Client.Projection.WebMercator mercator = new ESRI.ArcGIS.Client.Projection.WebMercator();
            this.FullExtent = new ESRI.ArcGIS.Client.Geometry.Envelope(-20037508.3427892, -20037508.3427892, 20037508.3427892, 20037508.3427892)
            {
                SpatialReference = new SpatialReference(102100)
            };
            //图层的空间坐标系
            this.SpatialReference = new SpatialReference(102100);
            // 建立切片信息，每个切片大小256*256px，
            this.TileInfo = new TileInfo()
            {
                Height = 256,
                Width = 256,
                Origin = new MapPoint(-cornerCoordinate, cornerCoordinate) { SpatialReference = new ESRI.ArcGIS.Client.Geometry.SpatialReference(102100) },
                Lods = new Lod[this._LodCount]
            };
            //为每级建立方案，每一级是前一级别的一半.
            double resolution = cornerCoordinate * 2 / 256;
            for (int i = 0; i < TileInfo.Lods.Length; i++)
            {
                TileInfo.Lods[i] = new Lod() { Resolution = resolution };
                resolution /= 2;
            }
            // 调用初始化函数
            base.Initialize();
        }

        public override string GetTileUrl(int level, int row, int col)
        {
            return string.Format(this._URL, col, row, level);
        }
    }


    public enum GoogleMapTylpe
    {
        /// <summary>
        /// 地形
        /// </summary>
        Terrain,
        /// <summary>
        /// 影像
        /// </summary>
        Image,

        /// <summary>
        /// 街道
        /// </summary>
        Street
    }

}
