using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cinser.BaseLib.CommonModel
{
    public class Message
    {
        public enum MessageType
        {
            Info,
            Error,
            Warning,
            Successed,
            Exception
        }

        string msgContent;
        MessageType type;
        Exception exceptionInfo;

        public Exception ExceptionInfo
        {
            get { return exceptionInfo; }
            set { exceptionInfo = value; }
        }

        public MessageType Type
        {
            get { return type; }
            set { type = value; }
        }

        public string MsgContent
        {
            get { return msgContent; }
            set { msgContent = value; }
        }

        public Message(string msgContent, MessageType type, Exception ex = null)
        {
            this.MsgContent = msgContent;
            this.Type = type;
            this.ExceptionInfo = ex;
        }

    }
}
