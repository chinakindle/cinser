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
using System.IO;

namespace Cinser.BaseLib
{
    public partial class HttpHelper
    {
        public void DownHttpFile(string filePath, UserControl owner)
        {
            WebRequest request = WebRequest.Create(filePath);
            //判断是否需要下载
            bool needDownload = false;
            SaveFileDialog sfd = new SaveFileDialog();
            string extension = System.IO.Path.GetExtension(filePath);
            sfd.Filter = string.Format("*{0}| *{0}", extension);
            if (sfd.ShowDialog() == true)
            {
                needDownload = true;
            }
            if (needDownload)
            {
                request.BeginGetResponse((responseAsyncCallBack) =>
                {
                    owner.Dispatcher.BeginInvoke(() =>
                    {
                        using (Stream openFileStream = sfd.OpenFile())
                        {
                            #region 获取response bytes
                            WebResponse response = request.EndGetResponse(responseAsyncCallBack);
                            Stream responseStream = response.GetResponseStream();
                            Byte[] bufferBytes = new Byte[responseStream.Length];
                            responseStream.Read(bufferBytes, 0, bufferBytes.Length);
                            #endregion
                            openFileStream.Write(bufferBytes, 0, bufferBytes.Length);
                            openFileStream.Flush();
                            System.Windows.MessageBox.Show("下载成功！");
                        }
                    });
                }, null);
            }
        }
    }
}
