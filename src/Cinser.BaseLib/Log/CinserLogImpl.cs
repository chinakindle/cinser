using System;

using log4net.Core;

namespace Cinser.BaseLib.Log
{
    public class CinserLogImpl : LogImpl, ICinserLog
    {
        /// <summary>
        /// The fully qualified name of this declaring type not the type of any subclass.
        /// </summary>
        private readonly static Type ThisDeclaringType = typeof(CinserLogImpl);

        public CinserLogImpl(ILogger logger)
            : base(logger)
        {
        }

        public void Info(string id, string userName, string application, string remark, object message)
        {
            Info(id, userName, application, remark, message, null);
        }

        public void Info(string id, string userName, string application, string remark, object message, Exception t)
        {
            if (this.IsInfoEnabled)
            {
                LoggingEvent loggingEvent = new LoggingEvent(ThisDeclaringType, Logger.Repository, Logger.Name, Level.Info, message, t);
                loggingEvent.Properties["ID"] = id;
                loggingEvent.Properties["UserName"] = userName;
                loggingEvent.Properties["Application"] = application;
                loggingEvent.Properties["Remark"] = remark;
                Logger.Log(loggingEvent);
            }
        }

        public void Warn(string id, string userName, string application, string remark, object message)
        {
            Warn(id, userName, application, remark, message, null);
        }

        public void Warn(string id, string userName, string application, string remark, object message, Exception t)
        {
            if (this.IsInfoEnabled)
            {
                LoggingEvent loggingEvent = new LoggingEvent(ThisDeclaringType, Logger.Repository, Logger.Name, Level.Warn, message, t);
                loggingEvent.Properties["ID"] = id;
                loggingEvent.Properties["UserName"] = userName;
                loggingEvent.Properties["Application"] = application;
                loggingEvent.Properties["Remark"] = remark;
                Logger.Log(loggingEvent);
            }
        }

        public void Error(string id, string userName, string application, string remark, object message)
        {
            Error(id, userName, application, remark, message, null);
        }

        public void Error(string id, string userName, string application, string remark, object message, Exception t)
        {
            if (this.IsInfoEnabled)
            {
                LoggingEvent loggingEvent = new LoggingEvent(ThisDeclaringType, Logger.Repository, Logger.Name, Level.Error, message, t);
                loggingEvent.Properties["ID"] = id;
                loggingEvent.Properties["UserName"] = userName;
                loggingEvent.Properties["Application"] = application;
                loggingEvent.Properties["Remark"] = remark;
                Logger.Log(loggingEvent);
            }
        }

        public void Fatal(string id, string userName, string application, string remark, object message)
        {
            Fatal(id, userName, application, remark, message, null);
        }

        public void Fatal(string id, string userName, string application, string remark, object message, Exception t)
        {
            if (this.IsInfoEnabled)
            {
                LoggingEvent loggingEvent = new LoggingEvent(ThisDeclaringType, Logger.Repository, Logger.Name, Level.Fatal, message, t);
                loggingEvent.Properties["ID"] = id;
                loggingEvent.Properties["UserName"] = userName;
                loggingEvent.Properties["Application"] = application;
                loggingEvent.Properties["Remark"] = remark;
                Logger.Log(loggingEvent);
            }
        }

        public void Debug(string id, string userName, string application, string remark, object message)
        {
            Debug(id, userName, application, remark, message, null);
        }

        public void Debug(string id, string userName, string application, string remark, object message, Exception t)
        {
            if (this.IsInfoEnabled)
            {
                LoggingEvent loggingEvent = new LoggingEvent(ThisDeclaringType, Logger.Repository, Logger.Name, Level.Debug, message, t);
                loggingEvent.Properties["ID"] = id;
                loggingEvent.Properties["UserName"] = userName;
                loggingEvent.Properties["Application"] = application;
                loggingEvent.Properties["Remark"] = remark;
                Logger.Log(loggingEvent);
            }
        }
    }
}
