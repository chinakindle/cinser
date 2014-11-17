using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cinser.BaseLib.CommonModel
{
    public class SdeConnection
    {
        string ip, name, userName, password;
        string serverName = "5151", version = "sde.DEFAULT";

        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        public string ServerName
        {
            get { return serverName; }
            set { serverName = value; }
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
        public SdeConnection() { }
        public SdeConnection(string dbName, string userName, string password, string serverIp = "127.0.0.1", string serverName = "5151", string version = "sde.DEFAULT")
        {
            this.Name = dbName;
            this.UserName = userName;
            this.Password = password;
            this.Ip = serverIp;
            this.ServerName = serverName;
            this.Version = version;
        }
    }
}
