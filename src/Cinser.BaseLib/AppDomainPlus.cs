using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cinser.BaseLib
{
    public class AppDomainPlus
    {
        static string debugPath;
        public static string DebugPath
        {
            get
            {
                if (string.IsNullOrEmpty(debugPath))
                    debugPath = System.AppDomain.CurrentDomain.BaseDirectory;

                if (debugPath.EndsWith("\\") == false)
                {
                    debugPath += "\\";
                }
                return debugPath;
            }
        }
    }
}
