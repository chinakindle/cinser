using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cinser.BaseLib.CommonModel
{
    public class FtpConnection
    {
        string ip, userName, password;
        int port = 21;

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
        
        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }

        public string FtpUrl
        {
            get
            {
                return string.Format("ftp://{0}:{1}/", this.Ip, this.Port);
            }
        }

        public FtpConnection() { }
        public FtpConnection(string userName, string password, string serverIp = "127.0.0.1", int port = 21)
        {
            this.UserName = userName;
            this.Password = password;
            this.Ip = serverIp;
            this.Port = port;
        }
    }
}
