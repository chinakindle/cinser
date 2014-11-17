using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace Cinser.Demo.Wpf
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DAL.ConnectionSettings.IDbConnection = BLL.DemoApplication.CreateDbConnection();
            DAL.ConnectionSettings.DbInstanceType = DBUtility.DbInstanceType.Access;
            DAL.ConnectionSettings.OracleIDbConnection = new System.Data.OracleClient.OracleConnection(Cinser.BaseLib.AppSettingsHelper.GetAppSettingsValue("ConnectionStringOracle"));
        }
    }
}
