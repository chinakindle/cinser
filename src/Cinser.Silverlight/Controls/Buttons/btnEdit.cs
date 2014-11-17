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
    public class btnEdit : Button
    {
        public btnEdit()
        {
            BitmapImage bitmapImage =  new BitmapImage(new Uri("/Cinser.Silverlight;component/Controls/Icons/edit_16.png", UriKind.RelativeOrAbsolute));            
            Image img = new Image();
            img.Height = 16;
            img.Width = 16;
            img.Source = bitmapImage;
            this.Content = img;
            ToolTipService.SetToolTip(this, "修改");
        }
    }
}
