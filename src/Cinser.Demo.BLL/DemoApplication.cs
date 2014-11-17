using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Cinser.Demo.BLL
{
    public class DemoApplication
    {
        public static IDbConnection CreateDbConnection()
        {
            string debugPath = Cinser.BaseLib.AppDomainPlus.DebugPath;            
            string accessDbConn = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}\\Data\\GERAS_For_Web.accdb", debugPath);            
            return new System.Data.OleDb.OleDbConnection(accessDbConn);
        }
    }
}
