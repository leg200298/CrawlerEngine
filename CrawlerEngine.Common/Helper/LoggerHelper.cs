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
        /// 取得父類別的相關資訊(共用的Functiond可用)
        /// </summary>
        /// <returns></returns>
        public static String GetParentInfo()
        {

            //StackTrace st = new StackTrace(1, true);
            //StackFrame[] stFrames = st.GetFrames();

            //foreach (StackFrame sf in stFrames)
            //{
            //    Console.WriteLine("Method: {0}", sf.GetMethod().DeclaringType.FullName);
            //}
            String showString = "";
            StackTrace ss = new StackTrace(true);
            //取得呼叫當前方法之上一層類別(GetFrame(1))的屬性
            MethodBase mb = ss.GetFrame(3).GetMethod();

            ////取得呼叫當前方法之上一層類別(父方)的命名空間名稱
            //showString += mb.DeclaringType.Namespace + "\n";

            //取得呼叫當前方法之上一層類別(父方)的function 所屬class Name
            showString += mb.DeclaringType.Name + "." + mb.DeclaringType.FullName;

            ////取得呼叫當前方法之上一層類別(父方)的Full class Name
            //showString += mb.DeclaringType.FullName + "\n";

            ////取得呼叫當前方法之上一層類別(父方)的Function Name
            //showString += mb.Name + "\n";

            return showString;
        }
        #region Debug，除錯
        public void Debug(string msg)
        {
            _logger.Debug(msg);
        }

        public void Debug(string msg, Exception err)
        {
            _logger.Debug(err, msg);
        }
        #endregion

        #region Info，資訊
        public void Info(string msg)
        {
            Factory(msg, "Info");
        }

        public void Info(string msg, Exception err)
        {
            _logger.Info(err, msg);
        }
        #endregion

        #region Warn，警告
        public void Warn(string msg)
        {
            _logger.Warn(msg);
        }

        public void Warn(string msg, Exception err)
        {
            _logger.Warn(err, msg);
        }
        #endregion

        #region Trace，追蹤
        public void Trace(string msg)
        {
            _logger.Trace(msg);
        }

        public void Trace(string msg, Exception err)
        {
            _logger.Trace(err, msg);
        }
        #endregion

        #region Error，錯誤
        public void Error(string msg)
        {
            _logger.Error(msg);
        }

        public void Error(string msg, Exception err)
        {
            _logger.Error(err, msg);
        }
        #endregion

        #region Fatal,致命錯誤
        public void Fatal(string msg)
        {
            _logger.Fatal(msg);
        }

        public void Fatal(string msg, Exception err)
        {
            _logger.Fatal(err, msg);
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
