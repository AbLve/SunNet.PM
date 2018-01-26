using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.InteropServices;
using System.Data;
using System.Windows.Forms;

namespace SunNet.PMNew.WinLogin
{
    public static class Helper
    {
        public static string FormatMessage(string format, params object[] args)
        {
            return string.Format(Properties.Settings.Default.ErrorFormat, string.Format(format, args));
        }

        public static bool Serialize(object obj, string fileName)
        {
            fileName = GetPath(fileName);

            Stream stream = File.Open(Path.Combine(Application.StartupPath, fileName), FileMode.Create);
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            stream.Close();
            return true;
        }
        public static object Deserialize(string fileName)
        {
            fileName = GetPath(fileName);

            object obj = null;
            fileName = Path.Combine(Application.StartupPath, fileName);
            if (!File.Exists(fileName))
                return null;
            Stream stream = File.Open(fileName, FileMode.Open);
            if (stream.Length < 1)
            {
                stream.Close();
                return null;
            }
            var formatter = new BinaryFormatter();
            obj = formatter.Deserialize(stream);
            stream.Close();
            return obj;
        }

        public static string GetPath(string path)
        {
            if (path.IndexOf(":") > 0)
                return path;
            return Path.Combine(Application.StartupPath, path);
        }
    }

    public class CookieHelper
    {
        private string navigateUrl;

        public CookieHelper(string navigateUrl)
        {
            this.navigateUrl = navigateUrl;
        }

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);

        public void SetCookie(string name, string value, DateTime expireTime)
        {
            value = DESEncrypt.Encrypt(value) + ";expires=" + expireTime.ToString("M");
            InternetSetCookie(navigateUrl, DESEncrypt.Encrypt(name), value);
        }

        public void SetWBCookie(WebBrowser wb, string name, string value, DateTime expireTime)
        {
            value = DESEncrypt.Encrypt(value) + ";expires=" + expireTime.ToString("M");
            wb.Document.Cookie += ";" + name + "=" + value;
        }

        public void DeleteCookie(string name, string value)
        {
            value = DESEncrypt.Encrypt(value) + ";expires=" + DateTime.Now.AddSeconds(10).ToString("M");
            InternetSetCookie(navigateUrl, DESEncrypt.Encrypt(name), value);
        }
    }

    public static class DESEncrypt
    {
        public static string Encrypt(string source)
        {
            string key = ".!e@0Na&";
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] bytes = Encoding.UTF8.GetBytes(source);

            des.Key = ASCIIEncoding.ASCII.GetBytes(key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(key);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(bytes, 0, bytes.Length);
            cs.FlushFinalBlock();

            StringBuilder sb = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                sb.AppendFormat("{0:X2}", b);
            }
            return sb.ToString();
        }
    }

    public static class AccountHelper
    {
        private const string key = "#s^un2ye31<cn%|aoXpR,+vh";

        public static string Encrypt(string text)
        {
            try
            {
                TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
                DES.Key = ASCIIEncoding.ASCII.GetBytes(key);
                DES.Mode = CipherMode.ECB;
                ICryptoTransform DESEncrypt = DES.CreateEncryptor();
                byte[] Buffer = ASCIIEncoding.ASCII.GetBytes(text);
                return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
            }
            catch
            {
            }
            return "";
        }

        public static string Decrypt(string text)
        {
            try
            {
                TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

                DES.Key = ASCIIEncoding.ASCII.GetBytes(key);
                DES.Mode = CipherMode.ECB;
                DES.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

                ICryptoTransform DESDecrypt = DES.CreateDecryptor();

                string result = "";
                byte[] Buffer = Convert.FromBase64String(text);
                result = ASCIIEncoding.ASCII.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
                return result;
            }
            catch
            {
            }
            return "";
        }

        public static void SaveData(string dataFile, bool isSave
            , string userName, string password, string userId)
        {
            dataFile = Helper.GetPath(dataFile);
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(Encrypt(userName));
                sb.AppendLine(Encrypt(password));
                sb.AppendLine(Encrypt(isSave ? Boolean.TrueString : Boolean.FalseString));
                sb.AppendLine(Encrypt(userId));
                FileStream fileStream = File.Create(dataFile);
                byte[] b = Encoding.Default.GetBytes(sb.ToString());
                fileStream.Write(b, 0, b.Length);
                fileStream.Close();
            }
            catch
            {
            }
        }

        public static void ReadData(string dataFile, out bool isRememberMe
            , out string userName, out string password, out string userId)
        {
            dataFile = Helper.GetPath(dataFile);
            isRememberMe = false;
            userName = string.Empty;
            password = string.Empty;
            userId = "0";
            try
            {
                if (!File.Exists(dataFile))
                {
                    return;
                }
                string[] context = File.ReadAllLines(dataFile);
                if (context.Length > 0)
                {
                    userName = Decrypt(context[0]);
                    password = Decrypt(context[1]);
                    isRememberMe = bool.Parse(Decrypt(context[2]));
                    userId = Decrypt(context[3]);
                }
            }
            catch
            {
            }
        }
    }

    [Serializable]
    public class FileToUpdate
    {
        public string Name { get; set; }
        public float Version { get; set; }
    }

    [Serializable]
    public class UpdateInfo
    {
        public UpdateInfo()
        {
            Files = new List<FileToUpdate>();
        }
        public float Version { get; set; }
        public float LocalVersion { get; set; }
        public bool Updated { get; set; }


        public List<FileToUpdate> Files { get; set; }

        public bool NeedUpdate
        {
            get { return this.Version > this.LocalVersion && this.Files.Count(x => x.Version > this.LocalVersion) > 0; }
        }
    }
}
