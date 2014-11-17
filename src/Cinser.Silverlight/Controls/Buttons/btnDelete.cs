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
using System.Windows.Media.Imaging;

namespace Cinser.Silverlight.Controls.Buttons
{
    public class btnDelete : Button
    {
        public btnDelete()
        {
            BitmapImage bitmapImage =  new BitmapImage(new Uri("/Cinser.Silverlight;component/Controls/Icons/delete_16.png", UriKind.RelativeOrAbsolute));            
            Image img = new Image();
            img.Height = 16;
            img.Width = 16;
            img.Source = bitmapImage;
            this.Content = img;
            ToolTipService.SetToolTip(this, "删除");
        }
    }
}
