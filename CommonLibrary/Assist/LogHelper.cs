using System;
using System.Diagnostics;
using log4net.Config;

namespace CommonLibrary.Assist
{
    public class LogHelper
    {
        //log4net日志专用
        protected static readonly log4net.ILog LogInfo = log4net.LogManager.GetLogger("LogInfo");
        protected static readonly log4net.ILog LogError = log4net.LogManager.GetLogger("LogError");

        static LogHelper()
        {
            try
            {
                XmlConfigurator.Configure();
            }
            catch (Exception)
            {
                Trace.TraceError("load log4net configuration file failed.");
            }
        }
        /// <summary>
        /// 普通的文件记录日志
        /// </summary>
        /// <param name="info"></param>
        public static void WriteInfoLog(string info)
        {
            if (LogInfo.IsInfoEnabled)
            {
                LogInfo.Info(info);
            }
        }

        /// <summary>
        /// 普通的文件记录日志
        /// </summary>
        /// <param name="info"></param>
        public static void WriteInfoLog(string format, params object[] args)
        {
            if (LogInfo.IsInfoEnabled)
            {
                LogInfo.Info(string.Format(format, args));
            }
        }
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ex"></param>
        public static void WriteErrorLog(string info, Exception ex)
        {
            if (LogError.IsErrorEnabled)
            {
                LogError.Error(info, ex);
            }
        }
    }
}
