using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Cinser.BaseLib
{
    /// <summary>
    /// 文件下载、上传事件代理
    /// </summary>
    /// <param name="Args"></param>
    public delegate void FileAccessEventHandler(FileAccessProgressArgs Args);

    /// <summary>
    /// 实现FTP客户端功能
    /// <remarks>
    /// /* 使用方法
    ///        /// 读取
    ///        Creatar.Common.FtpHelper ftp = new Creatar.Common.FtpHelper("192.168.1.168", "Administrator", ".");
    ///        if (ftp.CheckFileExist("ftp://192.168.1.168/test/SDE.DBF"))
    ///        {
    ///            ftp.DownLoad(@"test\QQ图片20130829180633.psd", @"F:\Temp\ABC.psd");
    ///        }
    ///     * 
    ///        /// 写入
    ///        Creatar.Common.FtpHelper ftp = new Creatar.Common.FtpHelper("192.168.1.168", "Administrator", ".");
    ///         if (ftp.CheckDirExist("ftp://192.168.1.168/test") == false)
    ///        {
    ///            ftp.CreateDir("test");
    ///        }
    ///        else
    ///        {
    ///            ftp.UploadFile(@"F:\Temp\SDE.DBF", "ftp://192.168.1.168/test/");
    ///        }
    ///     * 
    ///         ///删除
    ///         Creatar.Common.CreatarFtp ftp = new Creatar.Common.CreatarFtp("192.168.1.168", "Administrator", ".");
    ///         if (ftp.CheckFileExist("ftp://192.168.1.168/test/3.png"))
    ///         {
    ///             ftp.DeleteFile("ftp://192.168.1.168/test/3.png");//删除文件
    ///         }
    ///         else
    ///         {
    ///             ftp.DeleteDirAnyWay("ftp://192.168.1.168/test/1");//删除文件夹
    ///         }
    ///      ****************************************************/
    /// </remarks>
    /// </summary>
    public class FtpHelper : IDisposable
    {
        private string _FtpIP;
        private string _FtpUserID;
        private string _FtpPassword;
        private string _FtpServerAddress;
        private FtpWebRequest _FtpRequest;

        /// <summary>
        /// 获取当前Ftp的连接地址主体部分字符串，如：ftp://192.168.1.168:21/
        /// </summary>
        public string FtpServerAddress
        {
            get
            {
                return this._FtpServerAddress;
            }
        }

        /// <summary>
        /// 测试FTP是否有效
        /// </summary>
        /// <returns></returns>
        public bool IsValidFTP()
        {
            try
            {
                this.ListFiles();
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 下载文件事件，监视进度
        /// </summary>
        public event FileAccessEventHandler OnDownLoadingFile;

        /// <summary>
        /// 下载文件事件，监视进度
        /// </summary>
        public event FileAccessEventHandler OnDownLoadFileComplete;
        /// <summary>
        /// 上传文件事件，监视进度
        /// </summary>
        public event FileAccessEventHandler OnUploadingFile;

        /// <summary>
        /// 删除文件事件，监视进度
        /// </summary>
        public event FileAccessEventHandler OnDelFile;

        #region 获取文件和文件夹相关属性
        /// <summary>
        /// 构造FTP访问对象
        /// </summary>
        /// <param name="ftpIP">ftp的纯IP地址</param>
        /// <param name="ftpUserID">登录用户名</param>
        /// <param name="ftpPassword">登录密码</param>
        /// <remarks>
        /// /* 使用方法
        ///        /// 读取
        ///        Creatar.Common.FtpHelper ftp = new Creatar.Common.FtpHelper("192.168.1.168", "Administrator", ".");
        ///        if (ftp.CheckFileExist("ftp://192.168.1.168/test/SDE.DBF"))
        ///        {
        ///            ftp.DownLoad(@"test\QQ图片20130829180633.psd", @"F:\Temp\ABC.psd");
        ///        }
        ///     * 
        ///        /// 写入
        ///        Creatar.Common.FtpHelper ftp = new Creatar.Common.FtpHelper("192.168.1.168", "Administrator", ".");
        ///         if (ftp.CheckDirExist("ftp://192.168.1.168/test") == false)
        ///        {
        ///            ftp.CreateDir("test");
        ///        }
        ///        else
        ///        {
        ///            ftp.UploadFile(@"F:\Temp\SDE.DBF", "ftp://192.168.1.168/test/");
        ///        }
        ///     * 
        ///         ///删除
        ///         Creatar.Common.CreatarFtp ftp = new Creatar.Common.CreatarFtp("192.168.1.168", "Administrator", ".");
        ///         if (ftp.CheckFileExist("ftp://192.168.1.168/test/3.png"))
        ///         {
        ///             ftp.DeleteFile("ftp://192.168.1.168/test/3.png");//删除文件
        ///         }
        ///         else
        ///         {
        ///             ftp.DeleteDirAnyWay("ftp://192.168.1.168/test/1");//删除文件夹
        ///         }
        ///      ****************************************************/
        /// </remarks>
        /// <
        public FtpHelper(string ftpIP, string ftpUserID, string ftpPassword)
            : this(ftpIP, ftpUserID, ftpPassword, 21)
        { }

        /// <summary>
        /// 构造FTP访问对象
        /// </summary>
        /// <param name="ftpIP"></param>
        /// <param name="ftpUserID"></param>
        /// <param name="ftpPassword"></param>
        /// <param name="port"></param>
        /// <remarks>
        /// /* 使用方法
        ///        /// 读取
        ///        Creatar.Common.FtpHelper ftp = new Creatar.Common.FtpHelper("192.168.1.168", "Administrator", ".");
        ///        if (ftp.CheckFileExist("ftp://192.168.1.168/test/SDE.DBF"))
        ///        {
        ///            ftp.DownLoad(@"test\QQ图片20130829180633.psd", @"F:\Temp\ABC.psd");
        ///        }
        ///     * 
        ///        /// 写入
        ///        Creatar.Common.FtpHelper ftp = new Creatar.Common.FtpHelper("192.168.1.168", "Administrator", ".");
        ///         if (ftp.CheckDirExist("ftp://192.168.1.168/test") == false)
        ///        {
        ///            ftp.CreateDir("test");
        ///        }
        ///        else
        ///        {
        ///            ftp.UploadFile(@"F:\Temp\SDE.DBF", "ftp://192.168.1.168/test/");
        ///        }
        ///     * 
        ///         ///删除
        ///         Creatar.Common.CreatarFtp ftp = new Creatar.Common.CreatarFtp("192.168.1.168", "Administrator", ".");
        ///         if (ftp.CheckFileExist("ftp://192.168.1.168/test/3.png"))
        ///         {
        ///             ftp.DeleteFile("ftp://192.168.1.168/test/3.png");//删除文件
        ///         }
        ///         else
        ///         {
        ///             ftp.DeleteDirAnyWay("ftp://192.168.1.168/test/1");//删除文件夹
        ///         }
        ///      ****************************************************/
        /// </remarks>
        public FtpHelper(string ftpIP, string ftpUserID, string ftpPassword, int port)
        {
            this._FtpIP = ftpIP;
            this._FtpUserID = ftpUserID;
            this._FtpPassword = ftpPassword;
            if (port == 21)
            {
                this._FtpServerAddress = "ftp://" + _FtpIP + "/";
            }
            else
            {
                this._FtpServerAddress = "ftp://" + _FtpIP + ":" + port.ToString() + "/";
            }
        }

        /// <summary> 
        /// 列出FTP服务器上面根目录的所有文件 
        /// </summary> 
        public FileStruct[] ListFiles()
        {
            FileStruct[] listAll = ListFilesAndDirectories();
            List<FileStruct> listFile = new List<FileStruct>();
            foreach (FileStruct file in listAll)
            {
                if (!file.IsDirectory)
                {
                    listFile.Add(file);
                }
            }
            return listFile.ToArray();
        }

        /// <summary>
        /// 列出FTP服务器当前目录目录的所有文件 
        /// </summary>
        /// <param name="path">当前目录 Example : a/b</param>
        /// <returns></returns>
        public FileStruct[] ListFiles(string path)
        {
            FileStruct[] listAll = ListFilesAndDirectories(path);
            List<FileStruct> listFile = new List<FileStruct>();
            foreach (FileStruct file in listAll)
            {
                if (!file.IsDirectory)
                {
                    listFile.Add(file);
                }
            }
            return listFile.ToArray();
        }

        /// <summary> 
        /// 列出FTP服务器根目录的所有的目录 
        /// </summary> 
        public FileStruct[] ListDirectories()
        {
            FileStruct[] listAll = ListFilesAndDirectories();
            List<FileStruct> listDirectory = new List<FileStruct>();

            foreach (FileStruct file in listAll)
            {
                if (file.IsDirectory)
                {
                    listDirectory.Add(file);
                }
            }

            return listDirectory.ToArray();
        }

        /// <summary> 
        /// 列出FTP服务器当前目录的所有的目录 
        /// </summary> 
        public FileStruct[] ListDirectories(string path)
        {
            FileStruct[] listAll = ListFilesAndDirectories(path);
            List<FileStruct> listDirectory = new List<FileStruct>();

            foreach (FileStruct file in listAll)
            {
                if (file.IsDirectory)
                {
                    listDirectory.Add(file);
                }
            }

            return listDirectory.ToArray();
        }

        /// <summary> 
        /// 列出FTP服务器根目录的所有文件和目录 
        /// </summary> 
        public FileStruct[] ListFilesAndDirectories()
        {
            return this.ListFilesAndDirectories(this._FtpServerAddress);
        }

        /// <summary>
        /// 列出FTP服务器上面当前目录的所有文件和目录
        /// </summary>
        /// <param name="path">目录路径 Example a/b</param>
        /// <returns></returns>
        public FileStruct[] ListFilesAndDirectories(string path)
        {
            try
            {
                string fullPath = path;
                string tmpServerAdd = this._FtpServerAddress;
                if (this._FtpServerAddress.LastIndexOf('/') == this._FtpServerAddress.Length - 1)
                {
                    tmpServerAdd = this._FtpServerAddress.Substring(0, this._FtpServerAddress.LastIndexOf('/'));
                }
                if (!(fullPath.IndexOf(tmpServerAdd) >= 0))
                {
                    fullPath = this._FtpServerAddress + path;
                }

                //打开ftp连接
                this.FtpConnect(fullPath);
                this._FtpRequest.KeepAlive = false;
                this._FtpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                WebResponse Response = this._FtpRequest.GetResponse();
                //StreamReader stream = new StreamReader(Response.GetResponseStream(), true);
                StreamReader stream = new StreamReader(Response.GetResponseStream(), Encoding.Default);
                string Datastring = stream.ReadToEnd();
                FileStruct[] list = GetList(Datastring, fullPath);

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 文件上传/下载/删除
        /// <summary>
        /// 上传文件，保存到FTP根目录下
        /// </summary>
        /// <param name="fileName">上传文件名称，包含全路径</param>
        /// <returns></returns>
        public bool UploadFile(string fileName)
        {
            return this.UploadFile(fileName, string.Empty);
        }

        /// <summary>
        /// 上传文件，保存到FTP指定目录下
        /// </summary>
        /// <param name="fileName">包含全路径的本地文件Example c:\a.txt</param>
        /// <param name="ftpFullSavePath">FTP地址：Example ftp://192.168.1./a/b/</param>
        /// <returns></returns>
        public bool UploadFile(string fileName, string ftpFullSavePath)
        {
            FileInfo info = new FileInfo(fileName);
            string fullPath = ftpFullSavePath + info.Name;
            //         ftpFullSavePath.Replace("//","\\");
            ftpFullSavePath = ftpFullSavePath.Replace('/', '\\');

            //连接到ftp服务器
            this.FtpConnect(fullPath);

            this._FtpRequest.KeepAlive = false;
            this._FtpRequest.Method = WebRequestMethods.Ftp.UploadFile;
            this._FtpRequest.ContentLength = info.Length;

            // 缓冲大小设置为kb 
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;

            //打开一个文件流
            FileStream fs = info.OpenRead();

            try
            {
                Stream stream = this._FtpRequest.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                long Progress = 0;

                //将文件写入Ftp流
                while (contentLen != 0)
                {
                    stream.Write(buff, 0, contentLen);
                    Progress = Progress + contentLen;

                    contentLen = fs.Read(buff, 0, buffLength);
                    //触发进度事件
                    if (this.OnUploadingFile != null)
                    {
                        FileAccessProgressArgs args = new FileAccessProgressArgs();
                        args.Progress = Progress;
                        this.OnUploadingFile(args);
                    }
                }
                Progress = 0;
                // 关闭两个流
                stream.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return true;
        }
        /// <summary>
        /// 上传文件，保存到FTP指定目录下
        /// </summary>
        /// <param name="fileName">Example c:\a.txt</param>
        /// <param name="ftpSavePath">Example a/b/</param>
        /// <returns></returns>
        public bool UploadFile(string fileName, object ftpSavePath)
        {
            FileInfo info = new FileInfo(fileName);
            string fullPath = this._FtpServerAddress + ftpSavePath + info.Name;

            //连接到ftp服务器
            this.FtpConnect(fullPath);

            this._FtpRequest.KeepAlive = false;
            this._FtpRequest.Method = WebRequestMethods.Ftp.UploadFile;
            this._FtpRequest.ContentLength = info.Length;

            // 缓冲大小设置为kb 
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;

            //打开一个文件流
            FileStream fs = info.OpenRead();

            try
            {
                Stream stream = this._FtpRequest.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                long Progress = 0;

                //将文件写入Ftp流
                while (contentLen != 0)
                {
                    stream.Write(buff, 0, contentLen);
                    Progress = Progress + contentLen;

                    contentLen = fs.Read(buff, 0, buffLength);
                    //触发进度事件
                    if (this.OnUploadingFile != null)
                    {
                        FileAccessProgressArgs args = new FileAccessProgressArgs();
                        args.Progress = Progress;
                        this.OnUploadingFile(args);
                    }
                }
                Progress = 0;
                // 关闭两个流
                stream.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return true;
        }
        /// <summary>
        /// 把指定的文件下载到目标文件
        /// </summary>
        /// <param name="ftpFilePath">下载文件 a/b/c.qar</param>
        /// <param name="newFilePath"></param>
        public void DownLoad(string ftpFilePath, string newFilePath)
        {
            FileStream fs = null;
            System.IO.BinaryWriter writer = null;
            try
            {
                if (null == ftpFilePath || ftpFilePath.Length == 0) throw new Exception("指定的路径无效!");
                byte[] text = DownloadFile(ftpFilePath);
                fs = File.Create(newFilePath);
                writer = new BinaryWriter(fs);
                writer.Write(text);
                writer.Flush();
                writer.Close();

                fs.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                if (writer != null) writer.Close();
                if (fs != null) fs.Dispose();
            }

        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileUrl">下载文件 a/b/c.qar</param>
        public Byte[] DownloadFile(string fileUrl)
        {
            string FileName = fileUrl;
            //纯文件名
            string PureFileName = Path.GetFileName(FileName);

            //ftp服务器完整路径
            string fullPath = string.Empty;
            if (FileName.IndexOf(this._FtpIP) < 0)
            {
                fullPath = this._FtpServerAddress + FileName;
            }
            else
            {
                fullPath = FileName;
            }

            //打开ftp连接
            this.FtpConnect(fullPath);
            this._FtpRequest.KeepAlive = false;

            FtpWebResponse response = (FtpWebResponse)this._FtpRequest.GetResponse();

            Stream stream = response.GetResponseStream();
            long ContentLength = response.ContentLength;

            //定义缓冲区大小
            int bufferSize = 2048;
            int readCount;
            byte[] buffer = new byte[bufferSize];
            readCount = stream.Read(buffer, 0, bufferSize);
            MemoryStream outputStream = new MemoryStream();

            //写入下载数据流到本地
            while (readCount > 0)
            {
                outputStream.Write(buffer, 0, readCount);
                readCount = stream.Read(buffer, 0, bufferSize);
                //触发进度事件
                if (this.OnDownLoadingFile != null)
                {
                    FileAccessProgressArgs args = new FileAccessProgressArgs();
                    args.FilePath = fullPath;
                    args.Progress = outputStream.Length;
                    this.OnDownLoadingFile(args);
                }
            }
            stream.Close();
            stream.Dispose();
            Byte[] RtnValue = outputStream.ToArray();

            //触发进度事件
            if (this.OnDownLoadFileComplete != null)
            {
                FileAccessProgressArgs args = new FileAccessProgressArgs();
                args.FilePath = fullPath;
                args.Progress = outputStream.Length;
                this.OnDownLoadFileComplete(args);
            }
            outputStream.Close();
            outputStream.Dispose();
            response.Close();
            response = null;
            GC.Collect();


            return RtnValue;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName">包含全路径的FTP文件地址</param>
        /// <returns></returns>
        public bool DeleteFile(string fileName)
        {

            string fullPath = fileName;

            //ftp服务器完整路径
            if (fileName.IndexOf(this._FtpIP) < 0)
            {
                fullPath = this._FtpServerAddress + fileName;
            }
            else
            {
                fullPath = fileName;
            }

            this.FtpConnect(fullPath);
            this._FtpRequest.KeepAlive = false;

            // 指定执行什么命令
            this._FtpRequest.Method = WebRequestMethods.Ftp.DeleteFile;

            FtpWebResponse response = (FtpWebResponse)this._FtpRequest.GetResponse();
            response.Close();

            if (this.OnDelFile != null)
            {
                FileAccessProgressArgs args = new FileAccessProgressArgs();
                //args.FilePath = fullPath;
                //args.Progress = outputStream.Length;
                this.OnDelFile(args);
            }
            return true;
        }
        #endregion

        #region 目录 创建\删除
        /// <summary>
        /// 创建文件目录
        /// </summary>
        /// <param name="dirName">FTP目录地址</param>
        /// <returns></returns>
        public bool MakeDir(string dirName)
        {
            FtpWebResponse response = null;
            try
            {
                string fullPath = dirName;

                //连接 
                this.FtpConnect(fullPath);
                this._FtpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;

                response = (FtpWebResponse)this._FtpRequest.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                response.Close();
                throw new Exception(ex.Message);
            }
            return true;
        }

        /// <summary>
        /// 删除文件目录
        /// </summary>
        /// <param name="dirName">FTP文件地址全路径</param>
        /// <returns></returns>
        public bool DeleteDir(string dirName)
        {
            try
            {
                //string fullPath = dirName;

                //ftp服务器完整路径
                string fullPath = string.Empty;
                if (dirName.IndexOf(this._FtpIP) < 0)
                {
                    fullPath = this._FtpServerAddress + dirName;
                }
                else
                {
                    fullPath = dirName;
                }

                this.FtpConnect(fullPath);
                this._FtpRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;



                FtpWebResponse response = (FtpWebResponse)this._FtpRequest.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        /// <summary>
        /// 上传整个目录
        /// </summary>
        /// <param name="pathName">c:\a</param>
        /// <param name="ftpSavePath">//ftp://192.168.1.24/212/a/注意目录格式</param>
        /// <returns></returns>
        public bool UpLoadDir(string pathName, string ftpSavePath)
        {
            //int Progress = 0;
            try
            {
                DirectoryInfo di = new DirectoryInfo(pathName);

                string newMainDir = ftpSavePath + di.Name + "/";
                MakeDir(newMainDir);
                DirectoryInfo[] subDirsInfo = di.GetDirectories();
                foreach (DirectoryInfo singleDirInfo in subDirsInfo)
                {
                    string subDirName = newMainDir + singleDirInfo.Name + "/";
                    UpLoadDir(singleDirInfo.FullName, newMainDir);
                }
                FileInfo[] files = di.GetFiles();
                foreach (FileInfo fi in files)
                {
                    string a = newMainDir.Replace("ftp://" + _FtpIP + "/", "");
                    UploadFile(fi.FullName, newMainDir);

                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_ftpFullPath"></param>
        /// <param name="ftpPath"></param>
        /// <param name="dirPath"></param>
        public void DownDir(string _ftpFullPath, string ftpPath, string dirPath)
        {
            CreatDir(_ftpFullPath, ftpPath, dirPath);
            //_dirList.Add(_ftpFullPath);
            FileStruct[] _fileStruct = ListFiles(_ftpFullPath);
            for (int i = 0; i < _fileStruct.Length; i++)
            {
                CreatFile(_fileStruct[i].FullPath, ftpPath, dirPath);
                // _fileList.Add(_fileStruct[i].FullPath);
            }

            FileStruct[] _dirStruct = ListDirectories(_ftpFullPath);

            for (int i = 0; i < _dirStruct.Length; i++)
            {
                DownDir(_dirStruct[i].FullPath, ftpPath, dirPath);
            }

        }
        /// <summary>
        /// 检查目录是否存在[附加]
        /// </summary>
        /// <param name="dirName">ftp://192.168.1.168/test</param>
        /// <returns></returns>
        public bool CheckDirExist(string dirName)
        {
            FileStruct[] _fileStruct;
            //List<FileStruct> _fileStruct = new List<FileStruct>();
            string parentPath;
            string curentDirName;

            parentPath = dirName.Substring(0, dirName.LastIndexOf("/"));
            curentDirName = dirName.Substring(dirName.LastIndexOf("/") + 1);
            try
            {
                if (parentPath.LastIndexOf("/") <= 5)
                {
                    _fileStruct = ListDirectories();
                }
                else
                {
                    _fileStruct = ListDirectories(parentPath);
                }
                if (_fileStruct.Length > 0)
                {
                    for (int i = 0; i < _fileStruct.Length; i++)
                    {
                        //_fileStruct = ListDirectories(_fileStruct[i].Name);

                        if (_fileStruct[i].Name == curentDirName)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    return false;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// 检查文件是否存在[附加]
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns></returns>
        public bool CheckFileExist(string dirName)
        {
            FileStruct[] _fileStruct;
            //List<FileStruct> _fileStruct = new List<FileStruct>();
            string parentPath;
            string curentDirName;

            parentPath = dirName.Substring(0, dirName.LastIndexOf("/"));
            curentDirName = dirName.Substring(dirName.LastIndexOf("/") + 1);
            try
            {
                if (parentPath.LastIndexOf("/") <= 5)
                {
                    _fileStruct = this.ListFiles();
                }
                else
                {
                    _fileStruct = this.ListFiles(parentPath);
                }
                if (_fileStruct.Length > 0)
                {
                    for (int i = 0; i < _fileStruct.Length; i++)
                    {
                        if (_fileStruct[i].Name.ToUpper() == curentDirName.ToUpper())
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    return false;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 清理资源
        private bool disposed = false;
        /// <summary>
        /// 释放内部资源
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this._FtpIP != null)
                        this._FtpIP = null;
                    if (this._FtpUserID != null)
                        this._FtpUserID = null;
                    if (this._FtpPassword != null)
                        this._FtpPassword = null;
                    if (this._FtpRequest != null)
                        this._FtpRequest = null;
                }
            }
            disposed = true;
        }

        /// <summary>
        /// FTP对象析构
        /// </summary>
        ~FtpHelper()
        {
            Dispose(false);
        }

        /// <summary>
        /// 释放内部资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.Collect();
            GC.SuppressFinalize(this);
        }
        #endregion

        #region 私有方法
        /// <summary> 
        /// 获得文件和目录列表 
        /// </summary> 
        /// <param name="datastring">FTP返回的列表字符信息 </param> 
        /// <param name="path"></param>
        private FileStruct[] GetList(string datastring, string path)
        {
            List<FileStruct> myListArray = new List<FileStruct>();

            string[] dataRecords = datastring.Split('\n');
            FileListStyle _directoryListStyle = GuessFileListStyle(dataRecords);

            foreach (string s in dataRecords)
            {
                if (s == "") continue;
                if (_directoryListStyle == FileListStyle.Unknown) continue;

                FileStruct f = new FileStruct();
                f.Name = "..";

                switch (_directoryListStyle)
                {
                    case FileListStyle.UnixStyle:
                        f = ParseFileStructFromUnixStyleRecord(s, path);
                        break;
                    case FileListStyle.WindowsStyle:
                        f = ParseFileStructFromWindowsStyleRecord(s, path);
                        break;
                }

                //过滤一些无效的，无法识别的信息
                if (!(f.Name == "." || f.Name == ".."
                    || f.Name == "" || f.Name == null))
                {
                    myListArray.Add(f);
                }
            }
            return myListArray.ToArray();
        }

        /// <summary> 
        /// 判断文件列表的方式: Window OR Unix
        /// </summary> 
        /// <param name="recordList">文件信息列表 </param> 
        private FileListStyle GuessFileListStyle(string[] recordList)
        {
            foreach (string s in recordList)
            {
                if (s.Length > 10
                && Regex.IsMatch(s.Substring(0, 10), "(-|d)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)"))
                {
                    return FileListStyle.UnixStyle;
                }
                else if (s.Length > 8
                && Regex.IsMatch(s.Substring(0, 8), "[0-9][0-9]-[0-9][0-9]-[0-9][0-9]"))
                {
                    return FileListStyle.WindowsStyle;
                }
            }
            return FileListStyle.Unknown;
        }

        /// <summary> 
        /// 从Windows格式中返回文件信息 
        /// </summary> 
        /// <param name="record">文件信息 </param> 
        /// <param name="path"></param>
        public FileStruct ParseFileStructFromWindowsStyleRecord(string record, string path)
        {
            FileStruct f = new FileStruct();

            string processstr = record.Trim();

            string dateStr = processstr.Substring(0, 8);
            processstr = (processstr.Substring(8, processstr.Length - 8)).Trim();
            string timeStr = processstr.Substring(0, 7);
            processstr = (processstr.Substring(7, processstr.Length - 7)).Trim();
            DateTimeFormatInfo myDTFI = new CultureInfo("en-US", false).DateTimeFormat;
            myDTFI.ShortTimePattern = "t";

            //文件创建时间
            f.CreateTime = DateTime.Parse(dateStr + " " + timeStr, myDTFI);

            //判断是文件还是文件夹
            if (processstr.Substring(0, 5).Trim() == "<DIR>")
            {
                f.IsDirectory = true;
                processstr = (processstr.Substring(5, processstr.Length - 5)).Trim();
            }
            else
            {
                string[] strs = processstr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);  // true); 
                processstr = strs[1];
                f.IsDirectory = false;
            }

            //文件或文件夹名称
            f.Name = processstr;
            if (path.EndsWith("/")) { f.Path = path + f.Name; }
            else { f.Path = path + "/" + f.Name; }
            f.FullPath = f.Path;
            f.Path = f.Path.Replace(this._FtpServerAddress, "");

            return f;
        }

        /// <summary> 
        /// 从Unix格式中返回文件信息 
        /// </summary> 
        /// <param name="record">文件信息 </param> 
        /// <param name="path"></param>
        public FileStruct ParseFileStructFromUnixStyleRecord(string record, string path)
        {
            FileStruct f = new FileStruct();
            string processstr = record.Trim();
            if (processstr.Length == 0) return f;
            if (processstr.IndexOf("total") == 0) return f;

            f.Flags = processstr.Substring(0, 10);
            f.IsDirectory = (f.Flags[0] == 'd');
            processstr = (processstr.Substring(11)).Trim();

            //跳过一部分 
            this.CutSubstringFromStringWithTrim(ref processstr, ' ', 0);
            f.Owner = CutSubstringFromStringWithTrim(ref processstr, ' ', 0);
            f.Group = CutSubstringFromStringWithTrim(ref processstr, ' ', 0);

            //跳过一部分
            f.FileSize = long.Parse(this.CutSubstringFromStringWithTrim(ref processstr, ' ', 0).Replace(",", ""));
            string yearOrTime = processstr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2];

            //文件创建时间
            if (yearOrTime.IndexOf(":") >= 0)
            {
                processstr = processstr.Replace(yearOrTime, DateTime.Now.Year.ToString());
            }

            f.CreateTime = DateTime.Parse(CutSubstringFromStringWithTrim(ref processstr, ' ', 8));


            //string newname = Convert.ToString(processstr);
            //System.Text.Encoding encoding = System.Text.Encoding.Default;
            //byte[] bcs=encoding.GetBytes(newname.ToCharArray());

            //string nnName = encoding.GetString(bcs);





            //文件或文件夹名称            
            f.Name = processstr;

            if (path.EndsWith("/")) { f.Path = path + f.Name; }
            else { f.Path = path + "/" + f.Name; }
            f.FullPath = f.Path;
            f.Path = f.Path.Replace(this._FtpServerAddress, "");
            return f;
        }

        /// <summary> 
        /// 按照一定的规则进行字符串截取 
        /// </summary> 
        /// <param name="s">截取的字符串 </param> 
        /// <param name="c">查找的字符 </param> 
        /// <param name="startIndex">查找的位置 </param> 
        private string CutSubstringFromStringWithTrim(ref string s, char c, int startIndex)
        {
            int pos = s.IndexOf(c, startIndex);

            string retString = s.Substring(0, pos);
            s = (s.Substring(pos)).Trim();

            return retString;
        }

        /// <summary>
        /// 登录FTP服务器
        /// </summary>
        /// <param name="path"></param>
        private void FtpConnect(string path)
        {
            try
            {

                this._FtpRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));
                this._FtpRequest.UseBinary = true;
                this._FtpRequest.Credentials = new NetworkCredential(this._FtpUserID, this._FtpPassword);
            }
            catch
            {
                throw new Exception("连接FTP出错，请检查FTP配置参数是否正确。");
            }
        }
        #endregion

        #region 本地文件操作
        /// <summary>
        /// 在本地创建文件夹
        /// </summary>
        /// <param name="_path">在Ftp上的FullPath</param>
        /// <param name="ftpPath">Ftp头目录</param>
        /// <param name="dirPath"></param>
        public void CreatDir(string _path, string ftpPath, string dirPath)
        {
            string _tempPath;
            _tempPath = _path.Replace(ftpPath, dirPath);
            _tempPath.Replace("/", @"\");
            if (!Directory.Exists(_tempPath))
            {
                Directory.CreateDirectory(_tempPath);
            }
        }
        /// <summary>
        /// 下载创建文件
        /// </summary>
        /// <param name="_path">ftp文件路径</param>
        /// <param name="ftpPath"></param>
        /// <param name="dirPath">本地目录</param>
        public void CreatFile(string _path, string ftpPath, string dirPath)
        {
            string _tempPath;
            _tempPath = _path.Replace(ftpPath, dirPath);
            _tempPath.Replace("/", @"\");
            if (!File.Exists(_tempPath))
            {
                DownLoad(_path, _tempPath);
            }
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns></returns>
        public bool DeleteDirAnyWay(string dirName)
        {
            string fullPath = string.Empty;
            if (dirName.IndexOf(this._FtpIP) < 0)
            {
                fullPath = this._FtpServerAddress + dirName;
            }
            else
            {
                fullPath = dirName;
            }

            FileStruct[] _fileStruct = ListFiles(fullPath);
            for (int i = 0; i < _fileStruct.Length; i++)
            {
                DeleteFile(fullPath + "/" + _fileStruct[i].Name);
            }

            FileStruct[] _dirStruct = ListDirectories(fullPath);

            for (int i = 0; i < _dirStruct.Length; i++)
            {
                DeleteDirAnyWay(fullPath + "/" + _dirStruct[i].Name);
            }

            DeleteDir(dirName);

            return true;
        }

        //-------------------------------------------------------------

        string curAddress = "";


        /// <summary>
        /// 不做检测！
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns></returns>
        public bool ChangeDir(string dirName)
        {
            //if (dirName.Contains("/"))
            //    return false;

            if (dirName == "..")
            {
                int idx = curAddress.LastIndexOf('/');
                if (idx <= 0)
                    return false;

                curAddress = curAddress.Substring(0, idx);
            }
            else
            {
                if (curAddress != "")
                    curAddress += "/";
                curAddress += dirName;
            }

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns></returns>
        public bool CreateDir(string dirName)
        {
            FtpWebResponse response = null;
            try
            {
                this.FtpConnect(_FtpServerAddress + curAddress + "/" + dirName);
                this._FtpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;

                response = (FtpWebResponse)this._FtpRequest.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                if (response != null)
                    response.Close();
                throw new Exception(ex.Message);
            }

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns></returns>
        public bool DelDir(string dirName)
        {
            try
            {
                this.FtpConnect(_FtpServerAddress + curAddress + "/" + dirName);
                this._FtpRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;

                FtpWebResponse response = (FtpWebResponse)this._FtpRequest.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathName"></param>
        /// <returns></returns>
        public bool UpLoadDirToFTP(string pathName)
        {
            return UpLoadDir(pathName, _FtpServerAddress + curAddress + "/");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FileStruct[] ListDirItems()
        {
            FileStruct[] listAll = ListItems();
            List<FileStruct> listDirectory = new List<FileStruct>();

            foreach (FileStruct file in listAll)
            {
                if (file.IsDirectory)
                {
                    listDirectory.Add(file);
                }
            }

            return listDirectory.ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FileStruct[] ListItems()
        {
            try
            {
                //打开ftp连接
                this.FtpConnect(_FtpServerAddress + curAddress);
                this._FtpRequest.KeepAlive = false;
                this._FtpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                WebResponse Response = this._FtpRequest.GetResponse();
                StreamReader stream = new StreamReader(Response.GetResponseStream(), Encoding.Default);
                string Datastring = stream.ReadToEnd();
                FileStruct[] list = GetList(Datastring, _FtpServerAddress + curAddress);

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="desName"></param>
        /// <returns></returns>
        public bool UploadFileToFTP(string srcPath, string desName)
        {
            FileInfo info = new FileInfo(srcPath);
            string fullPath = "";
            if (desName == null || desName.Trim() == "")
            {
                fullPath = this._FtpServerAddress + curAddress + "/" + info.Name;
            }
            else
            {
                fullPath = this._FtpServerAddress + curAddress + "/" + desName;
            }

            //连接到ftp服务器
            this.FtpConnect(fullPath);

            this._FtpRequest.KeepAlive = false;
            this._FtpRequest.Method = WebRequestMethods.Ftp.UploadFile;
            this._FtpRequest.ContentLength = info.Length;

            // 缓冲大小设置为kb 
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;

            //打开一个文件流
            FileStream fs = info.OpenRead();

            try
            {
                Stream stream = this._FtpRequest.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                long Progress = 0;

                //将文件写入Ftp流
                while (contentLen != 0)
                {
                    stream.Write(buff, 0, contentLen);
                    Progress = Progress + contentLen;

                    contentLen = fs.Read(buff, 0, buffLength);
                    //触发进度事件
                    if (this.OnUploadingFile != null)
                    {
                        FileAccessProgressArgs args = new FileAccessProgressArgs();
                        args.Progress = Progress;
                        this.OnUploadingFile(args);
                    }
                }
                Progress = 0;
                // 关闭两个流
                stream.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return true;
        }
    }

    /// <summary>
    /// FTP中的文件格式命名风格
    /// </summary>
    public enum FileListStyle
    { // pzp: 如果命名为FileSystemStyle呢
        /// <summary>
        /// Unix
        /// </summary>
        UnixStyle,
        /// <summary>
        /// 
        /// </summary>
        WindowsStyle,
        /// <summary>
        /// 
        /// </summary>
        Unknown
    }

    /// <summary>
    /// 
    /// </summary>
    public struct FileStruct
    {
        /// <summary>
        /// 标识
        /// </summary>
        public string Flags;
        /// <summary>
        /// 所有者
        /// </summary>
        public string Owner;
        /// <summary>
        /// 分组
        /// </summary>
        public string Group;
        /// <summary>
        /// 目录
        /// </summary>
        public bool IsDirectory;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 相对路径
        /// </summary>
        public string Path;
        /// <summary>
        /// 全路径
        /// </summary>
        public string FullPath;
        /// <summary>
        /// 大小
        /// </summary>
        public long FileSize;
        /// <summary>
        /// 深度
        /// </summary>
        public int Level
        {
            get
            {
                return this.Path.Split('/').Length;
            }
        }
    }

    /// <summary>
    /// 文件处理进度
    /// </summary>
    public class FileAccessProgressArgs
    {
        private long _Progress;
        private string _FilePath;
        /// <summary>
        /// 当前处理进度
        /// </summary>
        public long Progress
        {
            get { return this._Progress; }
            set { this._Progress = value; }
        }
        private long allCount;

        /// <summary>
        /// 总进度
        /// </summary>
        public long AllCount
        {
            get { return allCount; }
            set { allCount = value; }
        }

        /// <summary>
        /// 正在下载文件路径
        /// </summary>
        public string FilePath
        {
            get { return this._FilePath; }
            set { this._FilePath = value; }
        }
    }

}
