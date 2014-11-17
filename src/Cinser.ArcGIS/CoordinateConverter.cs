using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Cinser.ArcGIS
{
    public class CoordinateConverter
    {
        /// <summary>
        /// 经纬度转墨卡托
        /// </summary>
        /// <param name="lonLat"></param>
        /// <returns></returns>
        public static Point lonLat2Mercator(Point lonLat)
        {
            Point mercator = new Point();
            double x = lonLat.X * 20037508.3427892 / 180;
            double y = Math.Log(Math.Tan((90 + lonLat.Y) * Math.PI / 360)) / (Math.PI / 180);
            y = y * 20037508.3427892 / 180;
            mercator.X = x;
            mercator.Y = y;
            return mercator;
        }

        /// <summary>
        /// 墨卡托转经纬度
        /// </summary>
        /// <param name="mercator"></param>
        /// <returns></returns>
        public static Point Mercator2lonLat(Point mercator)
        {
            Point lonLat = new Point();
            double x = mercator.X / 20037508.3427892 * 180;
            double y = mercator.Y / 20037508.3427892 * 180;
            y = 180 / Math.PI * (2 * Math.Atan(Math.Exp(y * Math.PI / 180)) - Math.PI / 2);
            lonLat.X = x;
            lonLat.Y = y;
            return lonLat;
        }
    }
}
