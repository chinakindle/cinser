

//wanghl
//2013.7.2
//进度条信息


using System;
using System.Windows.Controls;
using System.Windows;


namespace Cinser.Silverlight.Controls
{
    public partial class ProgressUI : UserControl
    {
        public ProgressUI()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 进度条的宽度
        /// </summary>
        public double BarWidth
        {
            get { return this.Border1.Width; }
            set { this.Border1.Width = value; }
        }


        /// <summary>
        /// 进度条的高度
        /// </summary>
        public double BarHeight
        {
            get { return this.Border1.Height; }
            set { this.Border1.Height = value; }
        }


        /// <summary>
        /// 进度条显示的信息
        /// </summary>
        public string BarMessage
        {
            get { return this.label1.Content.ToString(); }
            set { this.label1.Content = value; }
        }



        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="pGrid"></param>
        /// <param name="pMessage"></param>
        public static void Show(Grid pGrid, string pMessage)
        {
            ProgressUI myProgressUI = null;
            foreach (UIElement myUIElement in pGrid.Children)
            {
                if (myUIElement is ProgressUI)
                {
                    myProgressUI = myUIElement as ProgressUI;
                    break ;
                }
            }
            if (myProgressUI == null)
            {
                myProgressUI = new ProgressUI();
                pGrid.Children.Add(myProgressUI);
            }
            myProgressUI.BarMessage = pMessage;
        }


        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="pGrid"></param>
        public static void Close(Grid pGrid)
        {
            foreach (UIElement myUIElement in pGrid.Children)
            {
                if (myUIElement is ProgressUI)
                {
                    pGrid.Children.Remove(myUIElement);
                    break;
                }
            }
        }



    }
}
