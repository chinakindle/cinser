using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cinser.BaseLib.CommonModel
{
    public class OracleConnection
    {
        string ip, name, userName, password;
        int port = 1521;

        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }
        public OracleConnection() { }
        public OracleConnection(string dbName, string userName, string password, string serverIp = "127.0.0.1", int port = 1521)
        {
            this.Name = dbName;
            this.UserName = userName;
            this.Password = password;
            this.Ip = serverIp;
            this.Port = port;
        }

        public string GetConnectionStr()
        {
            return string.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))"
                + "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={2})));User Id={3};Password={4}", this.Ip, this.Port, this.Name, this.UserName, this.Password);
        }
    }
}
