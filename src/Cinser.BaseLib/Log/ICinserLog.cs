using System;

using log4net;

namespace Cinser.BaseLib.Log
{
    public interface ICinserLog : ILog
    {
        void Info(string id,string userName, string application, string remark, object message);
        void Info(string id, string userName, string application, string remark, object message, Exception t);

        void Warn(string id, string userName, string application, string remark, object message);
        void Warn(string id, string userName, string application, string remark, object message, Exception t);

        void Error(string id, string userName, string application, string remark, object message);
        void Error(string id, string userName, string application, string remark, object message, Exception t);

        void Fatal(string id, string userName, string application, string remark, object message);
        void Fatal(string id, string userName, string application, string remark, object message, Exception t);

        void Debug(string id, string userName, string application, string remark, object message);
        void Debug(string id, string userName, string application, string remark, object message, Exception t);
    }
}