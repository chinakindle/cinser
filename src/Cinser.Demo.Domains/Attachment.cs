using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cinser.Demo.Domains
{
    [Serializable]
    public class Attachment
    {
        string _ID, _GROUP_ID, _TABEL_NAME, _RELATED_ID, _AT_NAME, _AT_PATH, _AT_EXTENSION, _UPLOADER, _REMARK;
        int _AT_SIZE;
        DateTime _UPLOAD_TIME;

        public string REMARK
        {
            get { return _REMARK; }
            set { _REMARK = value; }
        }

        public string UPLOADER
        {
            get { return _UPLOADER; }
            set { _UPLOADER = value; }
        }

        public string AT_EXTENSION
        {
            get { return _AT_EXTENSION; }
            set { _AT_EXTENSION = value; }
        }

        public string AT_PATH
        {
            get { return _AT_PATH; }
            set { _AT_PATH = value; }
        }

        public string AT_NAME
        {
            get { return _AT_NAME; }
            set { _AT_NAME = value; }
        }

        public string RELATED_ID
        {
            get { return _RELATED_ID; }
            set { _RELATED_ID = value; }
        }

        public string TABEL_NAME
        {
            get { return _TABEL_NAME; }
            set { _TABEL_NAME = value; }
        }

        public string GROUP_ID
        {
            get { return _GROUP_ID; }
            set { _GROUP_ID = value; }
        }

        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public int AT_SIZE
        {
            get { return _AT_SIZE; }
            set { _AT_SIZE = value; }
        }

        public DateTime UPLOAD_TIME
        {
            get { return _UPLOAD_TIME; }
            set { _UPLOAD_TIME = value; }
        }
    }
}
