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
using DevExpress.Xpf.Charts;

namespace Cinser.Silverlight.Controls.Charts
{
     interface IXYChart:IChart
    {
        /// <summary>
        /// x轴标题
        /// </summary>
         string XTitle { get; set; }
        /// <summary>
        /// y轴标题
        /// </summary>
         string YTitle { get; set; }

        /// <summary>
        /// x轴标注类型
        /// </summary>
         ScaleType ScaleType { get; set; }
    }
}
