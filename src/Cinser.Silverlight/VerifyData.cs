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
using System.Text.RegularExpressions;

namespace Cinser.Silverlight
{

    public class VerifyData
    {
        public static bool IsEmail(string s)
        {
            return Regex.IsMatch(s, @"^(.+)@(.+)$");
        }

        public static bool IsIP(string s)
        {
            return Regex.IsMatch(s, @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$");
        }

        public static bool IsURL(string s)
        {
            return Regex.IsMatch(s, @"^[a-zA-z]+://(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?$");
        }

        public static bool IsID(string s)
        {
            return Regex.IsMatch(s, @"^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$");
        }

        public static bool IsPhone(string s)
        {
            return Regex.IsMatch(s, @"^(\d{2,4}[-_－—]?)?\d{3,8}([-_－—]?\d{3,8})?([-_－—]?\d{1,7})?$)|(^0?1[35]\d{9}$");
        }

        public static bool IsDate(string s)
        {
            return Regex.IsMatch(s, @"^(\d{4})(-)(\d{2})(-)(\d{2})$");
        }

        public static bool IsChinese(string s)
        {
            return Regex.IsMatch(s, @"^[\u4e00-\u9fa5]+$");
        }

        public static bool IsCharNumber(string s)
        {
            return Regex.IsMatch(s, @"^\w+$");
        }

        public static bool IsName(string s)
        {
            return Regex.IsMatch(s, @"^[a-zA-Z0-9_]+$");
        }

        public static bool IsCharNum(string s)
        {
            return Regex.IsMatch(s, @"^[A-Za-z0-9]+$");
        }
    }
}
