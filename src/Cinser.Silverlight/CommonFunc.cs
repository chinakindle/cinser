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
using System.Windows.Media.Imaging;

namespace Cinser.Silverlight
{
    public class CommonFunc
    {
        /// <summary>
        /// 根据所给Argb颜色字符串，返回该颜色值。
        /// </summary>
        /// <param name="colorString">8位或9位Argb颜色字符串（如：DD000000/#DD000000）</param>
        /// <returns>Color</returns>
        public static Color GetColorFromArgbColorString(string colorString)
        {
            if (colorString.StartsWith("#"))
                colorString = colorString.Substring(1);

            string alpha = colorString.Substring(0, 2);
            string red = colorString.Substring(2, 2);
            string green = colorString.Substring(4, 2);
            string blue = colorString.Substring(6, 2);

            byte alphaByte = Convert.ToByte(alpha, 16);
            byte redByte = Convert.ToByte(red, 16);
            byte greenByte = Convert.ToByte(green, 16);
            byte blueByte = Convert.ToByte(blue, 16);
            return Color.FromArgb(alphaByte, redByte, greenByte, blueByte);
        }

        /// <summary>
        /// 根据所给rgb颜色字符串，返回该颜色值。
        /// </summary>
        /// <param name="colorString">（#）+ 1、2、3、6位rgb颜色字符串（如：#0/0/#00/000/#000000）</param>
        /// <returns>Color</returns>
        public static Color GetColorFromRgbColorString(string colorString)
        {
            if (colorString.Length != 6)
            {
                if (colorString.StartsWith("#"))
                    colorString = colorString.Substring(1);
                string colorModelString = "000000";

                if (colorString.Length == 1)
                    colorString = colorModelString.Replace('0', colorString[0]);
                else if (colorString.Length == 2)
                    colorString = colorModelString.Replace("00", colorString);
                else if (colorString.Length == 3)
                    colorString = colorModelString.Replace("000", colorString);
            }
            colorString = "FF" + colorString;

            string alpha = colorString.Substring(0, 2);
            string red = colorString.Substring(2, 2);
            string green = colorString.Substring(4, 2);
            string blue = colorString.Substring(6, 2);

            byte alphaByte = Convert.ToByte(alpha, 16);
            byte redByte = Convert.ToByte(red, 16);
            byte greenByte = Convert.ToByte(green, 16);
            byte blueByte = Convert.ToByte(blue, 16);

            return Color.FromArgb(alphaByte, redByte, greenByte, blueByte);
        }

        public static void DownLoad(byte[] file,string fileName)
        {
            string SaveFileName = string.Empty;//文件名称
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string extension = GetFileExtension(fileName);
            saveFileDialog.DefaultExt = extension;//默认保存文件的扩展名称
            saveFileDialog.Filter = extension + "文件 |*" + extension; //文件的多种格式设置

            try
            {
                if (saveFileDialog.ShowDialog().Value) //方法只能从用户启动的代码，如果是用户启动，则返回true
                    SaveFileName = saveFileDialog.SafeFileName;//添加保存文件的文件名
                if (SaveFileName == "")
                {
                    MessageBox.Show("保存路径不能为空！");
                }
                //using 定义一个范围，将在此范围之外释放一个或多个对象。
                using (var sw = saveFileDialog.OpenFile()) //sw :参数指定的文件的读写流
                {
                    sw.Write(file, 0, file.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败！" + ex.ToString());
            }
        }

        public static string GetFileExtension(string fileName)
        {
            string extension = string.Empty;
            if (string.IsNullOrEmpty(fileName) == false)
            {
                extension = fileName.Substring(fileName.LastIndexOf(".")).ToLower();
            }
            return extension;
        }
    }
}
