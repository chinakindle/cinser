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
using System.ComponentModel;
using DevExpress.Xpf.Charts;

namespace Cinser.Silverlight.Controls.Charts
{
    public interface IChart
    {
        /// <summary>
        /// 统计表标题
        /// </summary>
        string ChartTitle { get; set; }

        /// <summary>
        /// 底部备注
        /// </summary>
        string ChartRemark { get; set; }

        /// <summary>
        /// 当前图表实例
        /// </summary>
        ChartControl ActualChart { get; }

        void InitUI();
    }
}
