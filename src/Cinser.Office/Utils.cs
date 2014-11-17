using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace Cinser.Office
{
    /// <summary>
    /// Cinser.Office公共程序集内部所需要用到的一些公共方法，不对外开放。因为这些功能在其它公共类中开放如Cinser.BaseLib，为减少耦合性copy至此类库
    /// </summary>
    internal class Utils
    {     
        /// <summary>
        /// 将文件流存入内存中返回
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static MemoryStream ReadFileMemoryStream(string filePath)
        {
            MemoryStream ms = null;
            byte[] bt = ReadFileByteArray(filePath);
            ms = new MemoryStream(bt, 0, bt.Length, true);
            return ms;
        }

        public static byte[] ReadFileByteArray(string filePath)
        {
            FileStream pFileStream = null;
            byte[] pReadByte = new byte[0];
            try
            {
                pFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                BinaryReader r = new BinaryReader(pFileStream);
                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
                pReadByte = r.ReadBytes((int)r.BaseStream.Length);
                return pReadByte;
            }
            catch
            {
                return pReadByte;
            }

            finally
            {
                if (pFileStream != null)
                    pFileStream.Close();
            }
        }

    }
}
