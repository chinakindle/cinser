

//wanghl
//2014.10.29
//发布的数据

using System;


namespace Cinser.Demo.Domains
{

    /// <summary>
    /// 发布的数据定义
    /// </summary>
    public class PublishData
    {

        private string _ID = "";
        //GUID 唯一编码
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _Name = "";
        //名称
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private DateTime _PublishDateTime = DateTime.Now;
        //发布时间
        public DateTime PublishDateTime
        {
            get { return _PublishDateTime; }
            set { _PublishDateTime = value; }
        }
        private string _PublishMan = "";
        //发布人
        public string PublishMan
        {
            get { return _PublishMan; }
            set { _PublishMan = value; }
        }
        private string _DataType = "";
        //发布数据类型
        public string DataType
        {
            get { return _DataType; }
            set { _DataType = value.ToString (); }
        }

        private string _OwnerMenuId = "";
        //所属菜单ID
        public string OwnerMenuId
        {
            get { return _OwnerMenuId; }
            set { _OwnerMenuId = value; }
        }

        private string _SourceInfo = "";
        //数据源信息，如果是地图服务，则是地图服务地址
        //如果是图片，word等则存储文件的相对路径
        public string SourceInfo
        {
            get { return _SourceInfo; }
            set { _SourceInfo = value; }
        }

        private string description = "";
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }



        /// <summary>
        /// 发布的数据
        /// </summary>
        public string FilePathOnLocal { get; set; }



        /// <summary>
        /// 数据内容
        /// </summary>
        public byte[] FileBytes { get; set; }


    }
}
