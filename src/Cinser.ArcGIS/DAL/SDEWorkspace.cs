using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cinser.ArcGIS.DAL
{

    public class SDEWorkspace
    {
        public static SDEWorkspace Workspace { get; set; }
        IFeatureWorkspace mySDEWorkspace = null;
        public SDEWorkspace(SdeSettings p)
        {
            IPropertySet mySDEPropertySet = new PropertySetClass();
            mySDEPropertySet.SetProperty("SERVER", p.ServerIp);
            mySDEPropertySet.SetProperty("Instance", p.ServerName);
            mySDEPropertySet.SetProperty("DATABASE", p.DbName);
            mySDEPropertySet.SetProperty("USER", p.UserName);
            mySDEPropertySet.SetProperty("PASSWORD", p.Password);
            mySDEPropertySet.SetProperty("VERSION", p.Version);
            IWorkspaceFactory mySDEWorkspaceFactory = new SdeWorkspaceFactoryClass();
            mySDEWorkspace = mySDEWorkspaceFactory.Open(mySDEPropertySet, 0) as IFeatureWorkspace;
        }

        public IFeatureClass GetFeatureClassByName(string name)
        {
            IFeatureClass mySDEFeatureClass = mySDEWorkspace.OpenFeatureClass(name);
            return mySDEFeatureClass;
        }
    }


    public class SdeSettings
    {
        string serverIp;

        public string ServerIp
        {
            get { return serverIp; }
            set { serverIp = value; }
        }
        string serverName;

        public string ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }
        string dbName;

        public string DbName
        {
            get { return dbName; }
            set { dbName = value; }
        }
        string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        string version;

        public string Version
        {
            get { return version; }
            set { version = value; }
        }


    }
}
