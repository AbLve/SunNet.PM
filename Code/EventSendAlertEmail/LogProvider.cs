using System.Configuration;
using System.IO;

namespace EventSendAlertEmail
{
    public static class LogProvider
    {
        private static string logPath = ConfigurationManager.AppSettings["LogPath"];
        public static void WriteLog(string message)
        {
            FileStream fs = new FileStream(logPath, FileMode.OpenOrCreate
                , FileAccess.Write);
            StreamWriter m_streamWriter = new StreamWriter(fs);
            m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            m_streamWriter.WriteLine(message + "\n");
            m_streamWriter.Flush();
            m_streamWriter.Close();
            fs.Close();
        }
    }
}
