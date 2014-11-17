using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cinser.BaseLib
{
    public static class StringPlus
    {
        public static List<string> GetStrArray(string str, char speater,bool toLower)
        {
            List<string> list = new List<string>();
            string[] ss =  str.Split(speater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) &&s !=speater.ToString())
                {
                    string strVal = s;
                    if (toLower)
                    {
                        strVal = s.ToLower();
                    }
                    list.Add(strVal);
                }
            }
            return list;
        }
        public static string[] GetStrArray(string str)
        {
            return str.Split(new char[',']);
        }
        public static string GetArrayStr(List<string> list,string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }
        
        
        #region 删除最后一个字符之后的字符

        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(string str)
        {
            return str.Substring(0, str.LastIndexOf(","));
        }

        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str,string strchar)
        {
            if (str.LastIndexOf(strchar) != -1)
            {
                return str.Substring(0, str.LastIndexOf(strchar));
            }
            else
                return str;
        }

        #endregion




        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }        

        /// <summary>
        ///  转半角的函数(SBC case)
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        public static List<string> GetSubStringList(string o_str, char sepeater)
        {
            List<string> list = new List<string>();
            string[] ss = o_str.Split(sepeater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != sepeater.ToString())
                {
                    list.Add(s);
                }
            }
            return list;
        }


        #region 将字符串样式转换为纯字符串
        public static string GetCleanStyle(string StrList, string SplitString)
        {
            string RetrunValue = "";
            //如果为空，返回空值
            if (StrList == null)
            {
                RetrunValue = "";
            }
            else
            {
                //返回去掉分隔符
                string NewString = "";
                NewString = StrList.Replace(SplitString, "");
                RetrunValue = NewString;
            }
            return RetrunValue;
        }
        #endregion

        #region 将字符串转换为新样式
        public static string GetNewStyle(string StrList, string NewStyle, string SplitString, out string Error)
        {
            string ReturnValue = "";
            //如果输入空值，返回空，并给出错误提示
            if (StrList == null)
            {
                ReturnValue = "";
                Error = "请输入需要划分格式的字符串";
            }
            else
            {
                //检查传入的字符串长度和样式是否匹配,如果不匹配，则说明使用错误。给出错误信息并返回空值
                int strListLength = StrList.Length;
                int NewStyleLength = GetCleanStyle(NewStyle, SplitString).Length;
                if (strListLength != NewStyleLength)
                {
                    ReturnValue = "";
                    Error = "样式格式的长度与输入的字符长度不符，请重新输入";
                }
                else
                {
                    //检查新样式中分隔符的位置
                    string Lengstr = "";
                    for (int i = 0; i < NewStyle.Length; i++)
                    {
                        if (NewStyle.Substring(i, 1) == SplitString)
                        {
                            Lengstr = Lengstr + "," + i;
                        }
                    }
                    if (Lengstr != "")
                    {
                        Lengstr = Lengstr.Substring(1);
                    }
                    //将分隔符放在新样式中的位置
                    string[] str = Lengstr.Split(',');
                    foreach (string bb in str)
                    {
                        StrList = StrList.Insert(int.Parse(bb), SplitString);
                    }
                    //给出最后的结果
                    ReturnValue = StrList;
                    //因为是正常的输出，没有错误
                    Error = "";
                }
            }
            return ReturnValue;
        }
        #endregion

        /// <summary>
        /// 截取字符串[string扩展]
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="startStr">开始串</param>
        /// <param name="endStr">结束串</param>
        /// <returns>返回源串中第一个在开始串和结束串中的串</returns>
        public static string SubString(this string source, string startStr, string endStr)
        {
            string sReturn = source.Clone().ToString();
            if(source.IndexOf(startStr)>-1)
            {
                sReturn = source.Substring(source.IndexOf(startStr) + startStr.Length);
                if(sReturn .IndexOf(endStr)>-1)
                {
                    sReturn = sReturn.Substring(0, sReturn.IndexOf(endStr));
                }
            }
            return sReturn;
        }

        /// <summary>
        /// 判断一个字符串是否为数字p[string扩展]
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(this string str)
        {
            bool bReturn = false;
            double d;
            if (string.IsNullOrEmpty(str) == false)
            {
                if (double.TryParse(str, out d))
                    bReturn = true;
            }
            return bReturn;
        }

        /// <summary>
        /// 获取指定后缀名的文件名
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string[] GetFileNames(this object obj, params string[] suffix)
        {
            if (!Directory.Exists(obj.ToString())) return null;
            DirectoryInfo _directory = new DirectoryInfo(obj.ToString());
            List<string> list = new List<string>();
            for (int i = 0; i < suffix.Length; i++)
            {
                foreach (FileInfo f in _directory.GetFiles("*." + suffix[i]))
                {
                    list.Add(f.Name);
                }
            }
            return list.ToArray();
        }
        /// <summary>
        /// 获取指定后缀名的文件路径
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string[] GetFiles(this object obj, params string[] suffix)
        {
            if (!Directory.Exists(obj.ToString())) return null;
            DirectoryInfo _directory = new DirectoryInfo(obj.ToString());
            List<string> list = new List<string>();
            for (int i = 0; i < suffix.Length; i++)
            {
                foreach (FileInfo f in _directory.GetFiles("*." + suffix[i]))
                    list.Add(f.FullName);
            }
            return list.ToArray();
        }
        public static bool IsInt(this object obj)
        {
            int res = 0;
            if (obj == null) return false;
            if (obj.ToString().Trim() == "") return true;
            else if (int.TryParse(obj.ToString(), out res))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsDouble(this object obj)
        {
            double res = 0;
            if (obj == null) return false;
            if (obj.ToString().Trim() == "") return true;
            else if (double.TryParse(obj.ToString(), out res))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsDataTime(this object obj)
        {
            DateTime res;
            if (obj == null) return true;
            else if (obj.ToString().Trim() == "") return true;
            else if (obj.ToString().Split(':').Length == 2)
                return false;
            else if (obj.ToString().Split('：').Length == 2)
                return false;
            else if (DateTime.TryParse(obj.ToString(), out res))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsBool(this object obj)
        {
            bool res;
            if (obj == null) return false;
            else if (bool.TryParse(obj.ToString(), out res))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string TrimNew(this object obj)
        {
            if (obj == null) return null;
            else return obj.ToString().Trim();
        }

        public static bool ToBool(this object obj)
        {
            bool res = false;
            if (obj == null) return false;
            if (obj.ToInt() > 0) return true;
            bool.TryParse(obj.ToString(), out res);
            return res;
        }
        public static int ToInt(this object obj)
        {
            int res = 0;
            if (obj == null) return 0;
            if (obj is Enum) return Convert.ToInt32((obj as Enum));
            int.TryParse(obj.ToString(), out res);
            return res;
        }
        public static double ToDouble(this object obj)
        {
            double res = 0;
            if (obj == null) return 0;
            double.TryParse(obj.ToString(), out res);
            return res;
        }
        public static DateTime ToDateTime(this object obj)
        {
            DateTime res = new DateTime(1900, 1, 1);
            if (obj == null) return res;
            DateTime.TryParse(obj.ToString(), out res);
            return res;
        }
        public static string ToStringNew(this object obj)
        {
            if (obj == null) return "";
            else
                return obj.ToString();
        }

        public static bool IsDataType(this object obj, Type type)
        {
            if (obj == null) return false;
            else if (type == typeof(System.String))
            {
                return true;
            }
            else if (type == typeof(System.Double) || type == typeof(System.Decimal))
            {
                return obj.IsDouble();
            }
            else if (type == typeof(System.Int32))
            {
                return obj.IsInt();
            }
            else if (type == typeof(System.DateTime))
            {
                return obj.IsDataTime();
            }
            else if (type == typeof(System.Boolean))
            {
                return obj.IsBool();
            }
            return true;
        }

        public static string ToChinese(this Type type)
        {
            if (type == null) return "";
            else if (type == typeof(System.String))
            {
                return "字符串类型";
            }
            else if (type == typeof(System.Double) || type == typeof(System.Decimal))
            {
                return "数字类型";
            }
            else if (type == typeof(System.Int32))
            {
                return "整数类型";
            }
            else if (type == typeof(System.DateTime))
            {
                return "时间类型";
            }
            else if (type == typeof(System.Boolean))
            {
                return "布尔类型";
            }
            return "";
        }
    }
}
