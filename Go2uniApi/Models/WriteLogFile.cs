using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Go2uniApi.Models
{
    public static class WriteLogFile
    {
        public static bool WriteLog(string strFileName, string strMessage)
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("~\\Logs\\") + strFileName;
                if (!File.Exists(path))
                {
                    using (var tw = new StreamWriter(path, true))
                    {
                        tw.WriteLine("##STARTED##" + DateTime.Now.ToLongTimeString().ToString());
                    }
                }

                string sLogFormat;

                sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";


                FileStream objFilestream = new FileStream(string.Format("{0}\\{1}", HttpContext.Current.Server.MapPath("~\\Logs"), strFileName), FileMode.Append, FileAccess.Write);
                StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
                objStreamWriter.WriteLine(sLogFormat + strMessage);
                objStreamWriter.Close();
                objFilestream.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool WriteErrorLog(string strFileName, string strMessage)
        {
            try
            {
                string sErrorTime;

                string sYear = DateTime.Now.Year.ToString();
                string sMonth = DateTime.Now.Month.ToString();
                string sDay = DateTime.Now.Day.ToString();
                sErrorTime = sYear + sMonth + sDay;

                string path = HttpContext.Current.Server.MapPath("~\\Logs\\") + strFileName;
                if (!File.Exists(path))
                {
                    using (var tw = new StreamWriter(path, true))
                    {
                        tw.WriteLine("##STARTED##" + sErrorTime);
                    }
                }

                FileStream objFilestream = new FileStream(string.Format("{0}\\{1}", HttpContext.Current.Server.MapPath("~\\Logs"), strFileName), FileMode.Append, FileAccess.Write);
                StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
                objStreamWriter.WriteLine(sErrorTime + strMessage);
                objStreamWriter.Close();
                objFilestream.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}