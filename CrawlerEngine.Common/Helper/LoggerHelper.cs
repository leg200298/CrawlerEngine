using NLog;
using System;
using System.Diagnostics;
using System.Reflection;

namespace CrawlerEngine.Common.Helper
{
    /// <summary>
    /// nLog
    /// </summary>
    public class LoggerHelper
    {
        private readonly Logger _logger = LogManager.GetLogger("");

        private static LoggerHelper _obj;

        public static LoggerHelper _
        {
            get => _obj ?? (new LoggerHelper());
            set => _obj = value;
        }

        /// <summary>
        /// 取得父類別的相關資訊
        /// </summary>
        /// <returns></returns>
        private String GetParentInfo()
        {
            MethodBase mb = new StackTrace(true).GetFrame(3).GetMethod();
            //取得呼叫當前方法之上一層類別(父方)的function 所屬class Name
            return mb.DeclaringType.FullName + "." + mb.Name;

        }
        #region Debug，除錯

        public void Debug(string msg, Exception err = null)
        {
            Factory(msg, "Debug", err);
        }
        #endregion

        #region Info，資訊

        public void Info(string msg, Exception err = null)
        {
            Factory(msg, "Info", err);
        }
        #endregion

        #region Warn，警告
        public void Warn(string msg, Exception err = null)
        {
            Factory(msg, "Warn", err);
        }
        #endregion

        #region Trace，追蹤
        public void Trace(string msg, Exception err = null)
        {
            Factory(msg, "Trace", err);
        }
        #endregion

        #region Error，錯誤
        public void Error(string msg, Exception err = null)
        {
            Factory(msg, "Error", err);
        }
        #endregion

        #region Fatal,致命錯誤

        public void Fatal(string msg, Exception err = null)
        {
            Factory(msg, "Fatal", err);
        }
        #endregion

        private void Factory(string msg, string type, Exception err = null)
        {
            var detailInfo = GetParentInfo() + " | " + msg;
            if (type == "Debug") _logger.Debug(err, detailInfo);
            if (type == "Info") _logger.Info(err, detailInfo);
            if (type == "Warn") _logger.Warn(err, detailInfo);
            if (type == "Trace") _logger.Trace(err, detailInfo);
            if (type == "Error") _logger.Error(err, detailInfo);
            if (type == "Fatal") _logger.Fatal(err, detailInfo);
        }
    }
}
