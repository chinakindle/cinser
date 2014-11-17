using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Cinser.BaseLib
{
    public class AppSettingsHelper
    {
        /// <summary>
        /// 设置AppSettings的值
        /// </summary>
        public static bool SetAppSettingsValue(string key, string value)
        {
            return SetAppSettingsValue(new List<string>() { key }, new List<string>() { value });
        }

        /// <summary>
        /// 批量设置AppSettings的值
        /// </summary>
        public static bool SetAppSettingsValue(List<string> keys, List<string> values)
        {
            bool bReturn = false;
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            try
            {
                for (int i = 0; i < keys.Count; i++)
                {
                    config.AppSettings.Settings[keys[i]].Value = values[i];
                }
                ConfigurationManager.RefreshSection("appSettings");
                config.Save();
                bReturn = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bReturn;
        }

        /// <summary>
        /// 获取AppSettings的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSettingsValue(string key)
        {
            string sReturn = string.Empty;
            sReturn = ConfigurationManager.AppSettings[key];
            return sReturn;
        }
    }
}
