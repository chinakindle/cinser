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
using System.Windows.Media.Imaging;
using System.Windows.Browser;

namespace Cinser.Silverlight.Controls
{
    public partial class SinglePictrueView : UserControl
    {
        public SinglePictrueView()
        {
            InitializeComponent();
        }
        
        BitmapImage pictrue;
        private bool m_IsMouseLeftButtonDown;
        private Point m_PreviousMousePoint;

        private void SetCurImg(BitmapImage currImg)
        {
            img.Source = currImg;
            var group = LayoutRoot.Resources["ImageTransformResource"] as TransformGroup;
            var transform = group.Children[0] as ScaleTransform;
            transform.ScaleX = 1;
            transform.ScaleY = 1;
            var transform1 = group.Children[1] as TranslateTransform;
            transform1.X = 0;
            transform1.Y = 0;
        }

        void img_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            MessageBox.Show(e.Error.ToString());
        }

        /// <summary>
        /// 根据传入图片List路径，显示图片
        /// </summary>
        /// <param name="imgUris">Uri:   ../Images/pic.png</param>
        public SinglePictrueView(string imgUri)
        {
            InitializeComponent();

            try
            {
                pictrue = new BitmapImage(new Uri(imgUri, UriKind.RelativeOrAbsolute));
                img.Source = pictrue;
            }
            catch (Exception ex)
            {
                HtmlPage.Window.Alert("所给图片路径不正确或者非图片文件，错误信息：" + ex.Message);
            }
        }


        /// <summary>
        /// 根据传入图片List路径，显示图片
        /// </summary>
        /// <param name="imgUris">Uri:   ../Images/pic.png</param>
        public SinglePictrueView(BitmapImage pic)
        {
            InitializeComponent();
            try
            {
                img.Source = pic;
                pictrue = pic;
            }
            catch (Exception ex)
            {
                HtmlPage.Window.Alert("所给图片路径不正确或者非图片文件，错误信息：" + ex.Message);
            }
        }
        
        private void contentControl1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var rectangle = sender as ContentControl;
            if (rectangle == null) return;
            rectangle.CaptureMouse();
            m_IsMouseLeftButtonDown = true;
            m_PreviousMousePoint = e.GetPosition(rectangle);
        }

        private void contentControl1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var rectangle = sender as ContentControl;
            if (rectangle == null) return;
            rectangle.ReleaseMouseCapture();
            m_IsMouseLeftButtonDown = false;
        }

        private void contentControl1_MouseMove(object sender, MouseEventArgs e)
        {
            var rectangle = sender as ContentControl;
            if (rectangle == null) return;
            if (m_IsMouseLeftButtonDown)
                DoImageMove(rectangle, e);
        }
        private void DoImageMove(ContentControl rectangle, MouseEventArgs e)
        {
            var group = LayoutRoot.Resources["ImageTransformResource"] as TransformGroup;
            var transform = group.Children[1] as TranslateTransform;
            var position = e.GetPosition(rectangle);
            transform.X += position.X - m_PreviousMousePoint.X;
            transform.Y += position.Y - m_PreviousMousePoint.Y;
            m_PreviousMousePoint = position;
        }
        private void contentControl1_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var image = sender as ContentControl;
            if (image == null) return;
            var group = LayoutRoot.Resources["ImageTransformResource"] as TransformGroup;
            var point = e.GetPosition(image);
            var scale = e.Delta * 0.005;
            ZoomImage(group, point, scale);
        }
        private void ZoomImage(TransformGroup group, Point point, double scale)
        {
            var pointToContent = group.Inverse.Transform(point);
            var transform = group.Children[0] as ScaleTransform;
            if (transform.ScaleX + scale < 1) return;
            transform.ScaleX += scale;
            transform.ScaleY += scale;
            var transform1 = group.Children[1] as TranslateTransform;
            transform1.X = -1 * ((pointToContent.X * transform.ScaleX) - point.X);
            transform1.Y = -1 * ((pointToContent.Y * transform.ScaleY) - point.Y);
        }
    }
}
