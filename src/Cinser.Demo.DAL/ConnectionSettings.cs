using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cinser.Demo.DAL
{
    public static class ConnectionSettings
    {
        static DBUtility.DbInstanceType _DbInstanceType;
        static System.Data.IDbConnection _IDbConnection;

        public static System.Data.IDbConnection IDbConnection
        {
            get { return _IDbConnection; }
            set { _IDbConnection = value; }
        }

        public static DBUtility.DbInstanceType DbInstanceType
        {
            get { return _DbInstanceType; }
            set { _DbInstanceType = value; }
        }

        static System.Data.IDbConnection _OracleIDbConnection;
        public static System.Data.IDbConnection OracleIDbConnection
        {
            get { return _OracleIDbConnection; }
            set { _OracleIDbConnection = value; }
        }

    }
}
