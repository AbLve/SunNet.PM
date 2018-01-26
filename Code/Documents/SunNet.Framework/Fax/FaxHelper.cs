using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SF.Framework.ConfigLibray;
using System.Threading;

namespace SF.Framework.Fax
{
    public class FaxHelper
    {
        public class FileEntity
        {
            public FileEntity()
            { }

            public FileEntity(string fileName, string filePath)
            {
                FileName = fileName;
                FilePath = filePath;
            }

            public string FileName { get; set; }
            public string FilePath { get; set; }
        }

        public bool BeginSend(string[] sendTos, params FileEntity[] fileEntitys)
        {
            SF.Framework.Log.Providers.TextFileLogger log = new SF.Framework.Log.Providers.TextFileLogger();

            string testToFaxNumber = SFConfig.TestToFaxNumber;
            if (testToFaxNumber.Trim().ToLower().Equals("off"))
            {
                log.Log("Fax Off");
                return true;
            }

            string sendTo = "";
            if (sendTos != null && sendTos.Length > 0)
            {
                sendTos = sendTos.Distinct().ToArray();
            }
            for (int i = 0; i < 9 && i < sendTos.Length; i++)
            {
                if (!string.IsNullOrEmpty(sendTos[i]))
                {
                    sendTo += "1" + sendTos[i].Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "") + "|";
                }
            }

            string toFaxNumber = sendTo.TrimEnd('|');
            if (!string.IsNullOrEmpty(testToFaxNumber.Trim()))
            {
                toFaxNumber = testToFaxNumber;
            }
            else if (string.IsNullOrEmpty(toFaxNumber))
            {
                return false;
            }

            com.srfax.www.SRFaxWebServices srFaxWebServices = new com.srfax.www.SRFaxWebServices();

            string accessID = SFConfig.AccessID;
            string accessPwd = SFConfig.AccessPwd;
            string sCallerID = SFConfig.SCallerID;
            string senderEmail = SFConfig.SenderEmail;
            string eFaxType = SFConfig.EFaxType;

            string retries = ConfigManager.ReadValueByKey("Retries");

            string AccountCode = "";
            string result;
            string status = srFaxWebServices.Queue_Fax_WR
                (accessID, accessPwd, sCallerID, senderEmail, eFaxType, toFaxNumber, AccountCode, retries,
                fileEntitys.Length == 1 ? fileEntitys[0].FileName : "", fileEntitys.Length == 1 ? EncodeBase64(fileEntitys[0].FilePath) : "",
                fileEntitys.Length == 2 ? fileEntitys[1].FileName : "", fileEntitys.Length == 2 ? EncodeBase64(fileEntitys[1].FilePath) : "",
                fileEntitys.Length == 3 ? fileEntitys[2].FileName : "", fileEntitys.Length == 3 ? EncodeBase64(fileEntitys[2].FilePath) : "",
                fileEntitys.Length == 4 ? fileEntitys[3].FileName : "", fileEntitys.Length == 4 ? EncodeBase64(fileEntitys[3].FilePath) : "",
                fileEntitys.Length == 5 ? fileEntitys[4].FileName : "", fileEntitys.Length == 5 ? EncodeBase64(fileEntitys[4].FilePath) : "",
                out result);
            log.Log(status + "|" + result);
            return status.ToLower().Equals("success");
        }

        private string EncodeBase64(string filename)
        {
            string encode = "";
            byte[] data;
            FileStream inFile = new FileStream(filename, FileMode.Open, FileAccess.Read);
            data = new Byte[inFile.Length];
            long bytesRead = inFile.Read(data, 0, (int)inFile.Length);
            inFile.Close();

            encode = Convert.ToBase64String(data, 0, data.Length);

            return encode;
        }

        /// <summary>
        /// Send
        /// </summary>
        /// <param name="mail"></param>
        public void ExecuteInBackground(string[] sendTos, params FileEntity[] fileEntitys)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("SendTo", sendTos);
            dic.Add("Files", fileEntitys);
            Thread t = new Thread(new ParameterizedThreadStart(ExecuteSend));
            t.Start(dic);
        }

        private void ExecuteSend(object m)
        {
            Dictionary<string, object> dic = (Dictionary<string, object>)m;
            if (dic != null)
            {
                string[] sendTos = new string[0];
                if (dic.Keys.Contains("SendTo"))
                {
                    sendTos = (string[])dic["SendTo"];
                }
                FileEntity[] fileEntitys = new FileEntity[0];
                if (dic.Keys.Contains("Files"))
                {
                    fileEntitys = (FileEntity[])dic["Files"];
                }
                if (sendTos != null && sendTos.Length > 0)
                {
                    new FaxHelper().BeginSend(sendTos, fileEntitys);
                }
            }
        }

    }
}
