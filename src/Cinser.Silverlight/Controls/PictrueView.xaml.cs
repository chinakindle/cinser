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
using System.Windows.Navigation;

using System.Windows.Media.Imaging;
using DevExpress.Xpf.Core;
using System.Windows.Browser;
using System.IO;

namespace Cinser.Silverlight.Controls
{
    public partial class PictrueView : DXWindow
    {
        List<BitmapImage> pictrues;
        int index = -1;
        private bool m_IsMouseLeftButtonDown;
        private Point m_PreviousMousePoint;

        public int Index
        {
            get { return index; }
            set
            {
                if (pictrues != null && value >= 0 && value < pictrues.Count)
                {
                    index = value;

                    var group = LayoutRoot.Resources["ImageTransformResource"] as TransformGroup;
                    var transform = group.Children[0] as ScaleTransform;
                    transform.ScaleX = 1;
                    transform.ScaleY = 1;
                    var transform1 = group.Children[1] as TranslateTransform;
                    transform1.X = 0;
                    transform1.Y = 0;

                    SetCurImg(pictrues[index]);
                    lbInfo.Text = (index + 1).ToString() + "/" + pictrues.Count;
                }
            }
        }

        private void SetCurImg(BitmapImage currImg)
        {
            //<Image   Name="img" Stretch="Fill"  Source="/SilverlightApplicationWCF;component/0824wallpaper-6_1600.jpg" RenderTransform="{StaticResource ImageTransformResource}" />
            //img = new Image()
            //{
            //    Stretch = Stretch.Fill,
            //    Name = "img",
            //    RenderTransform = LayoutRoot.Resources["ImageTransformResource"] as TransformGroup  
            //};
            img.Source = currImg;
            //double imgHeight = currImg.PixelHeight;
            //double imgWidth = currImg.PixelWidth;
            //m_PreviousMousePoint = new Point();
            //if (imgHeight > this.LayoutRoot.Height - 50 || imgWidth > this.LayoutRoot.Width)
            //{
            //    double percentOfHeight = imgHeight / (this.LayoutRoot.Height - 50);
            //    double percentOfWidth = imgWidth / this.LayoutRoot.Width;
            //    if (percentOfHeight > percentOfWidth)
            //    {
            //        img.Height = this.LayoutRoot.Height - 50;
            //        img.Width = imgWidth * img.Height / imgHeight;
            //    }
            //    else
            //    {
            //        img.Width = this.LayoutRoot.Width;
            //        img.Height = imgHeight * img.Width / imgWidth;
            //    }
            //}
            //else
            //{
            //    img.Height = imgHeight;
            //    img.Width = imgWidth;
            //}
        }

        void img_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            MessageBox.Show(e.Error.ToString());
        }

        /// <summary>
        /// 根据传入图片List路径，显示图片
        /// </summary>
        /// <param name="imgUris">Uri:   ../Images/pic.png</param>
        public PictrueView(List<string> imgUris)
        {
            InitializeComponent();
            pictrues = new List<BitmapImage>();
            try
            {
                for (int i = 0; i < imgUris.Count; i++)
                {
                    pictrues.Add(new BitmapImage(new Uri(imgUris[i], UriKind.RelativeOrAbsolute)));
                }
                if (imgUris.Count > 0)
                {
                    img.Source = pictrues[0];
                    this.index = 0;
                    lbInfo.Text = "1/" + pictrues.Count;
                }
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
        public PictrueView(List<BitmapImage> imgs)
        {
            InitializeComponent();
            pictrues = imgs;
            try
            {
                if (imgs.Count > 0)
                {
                    img.Source = pictrues[0];
                    this.index = 0;
                    lbInfo.Text = "1/" + pictrues.Count;
                }
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

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            this.Index = this.Index - 1;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            this.Index = this.Index + 1;
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            //string filePath = "http://localhost:34270/ClientBin/test.rar";

            //WebRequest request = WebRequest.Create(filePath);

            //request.BeginGetResponse((responseAsyncCallBack) =>
            //{
                

            //}, null);
            //byte[] thisFile = pictrues[index];

            //SaveFileDialog sfd = new SaveFileDialog();

            //string extension = System.IO.Path.GetExtension(filePath);

            //sfd.Filter = string.Format("*{0}| *{0}", extension);

            //if (sfd.ShowDialog() == true)
            //{

            //    Stream openFileStream = sfd.OpenFile();

            //    #region 获取response bytes

            //    WebResponse response = request.EndGetResponse(responseAsyncCallBack);
            //    Stream responseStream = response.GetResponseStream();

            //    Byte[] bufferBytes = new Byte[responseStream.Length];

            //    responseStream.Read(bufferBytes, 0, bufferBytes.Length);

            //    #endregion

            //    openFileStream.Write(bufferBytes, 0, bufferBytes.Length);
            //    openFileStream.Flush();

            //}
        }

    }
}
