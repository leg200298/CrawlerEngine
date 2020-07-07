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

        public void Debug(string msg = null, Exception ex = null)
        {
            Factory("Debug", msg, ex);
        }
        #endregion

        #region Info，資訊

        public void Info(string msg = null, Exception ex = null)
        {
            Factory("Info", msg, ex);
        }
        #endregion


        #region Trace，追蹤
        public void Trace(string msg = null, Exception ex = null)
        {
            Factory("Trace", msg, ex);
        }
        #endregion

        #region Warn，警告
        public void Warn(Exception ex = null, string msg = null)
        {
            Factory("Warn", msg, ex);
        }
        #endregion
        #region exor，錯誤
        public void Error(Exception ex = null, string msg = null)
        {
            Factory("Error", msg, ex);
        }
        #endregion

        #region Fatal,致命錯誤

        public void Fatal(Exception ex = null, string msg = null)
        {
            Factory("Fatal", msg, ex);
        }
        #endregion

        private void Factory(string type, string msg = null, Exception ex = null)
        {
            var detailInfo = GetParentInfo() + " | " + msg;
            if (type == "Debug") _logger.Debug(ex, detailInfo);
            if (type == "Info") _logger.Info(ex, detailInfo);
            if (type == "Warn") _logger.Warn(ex, detailInfo);
            if (type == "Trace") _logger.Trace(ex, detailInfo);
            if (type == "Error") _logger.Error(ex, detailInfo);
            if (type == "Fatal") _logger.Fatal(ex, detailInfo);
        }
    }
}
