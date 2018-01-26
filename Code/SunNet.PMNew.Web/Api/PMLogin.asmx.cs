using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.App;
using System.Text;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core;
using System.Security.Cryptography;

namespace SunNet.PMNew.Web.Api
{
    /// <summary>
    /// Summary description for PMLogin
    /// </summary>
    [WebService(Namespace = "http://pm.sunnet.us/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PMLogin : System.Web.Services.WebService
    {
        private UserApplication userApplication = new UserApplication();
        [WebMethod]
        public string Login(string desUserName, string desPassword, bool rememberMe, out string id)
        {
            id = string.Empty;
            IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
            string userName = encrypt.Decrypt(desUserName);
            string password = encrypt.Decrypt(desPassword);
            if ((string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password)))
            {
                return "failure";
            }
            UsersEntity usersEntity = userApplication.Login(userName, password);
            if (usersEntity == null)
            {
                return FormatMessages(userApplication.BrokenRuleMessages);
            }
            id = usersEntity.UserID.ToString();
            return "successful";
        }

        [WebMethod]
        public string GetVersion(string key)
        {
            string path = this.Server.MapPath("upgrade.xml");
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return "";
        }

        [WebMethod]
        public string Update(string file)
        {
            string path = this.Server.MapPath("/update/" + file);
            if (File.Exists(path))
            {
                FileStream fileStream = File.OpenRead(path);
                BinaryReader binaryReader = new BinaryReader(fileStream);
                string content = Convert.ToBase64String(binaryReader.ReadBytes((int)fileStream.Length), 0, (int)fileStream.Length);
                binaryReader.Close();
                fileStream.Close();
                return content;
            }
            return "";
        }

        protected string FormatMessages(List<BrokenRuleMessage> list)
        {
            StringBuilder sbMsgs = new StringBuilder();
            foreach (BrokenRuleMessage msg in list)
            {
                sbMsgs.Append(msg.Message);
                sbMsgs.Append("");
            }
            return sbMsgs.ToString();
        }

        public void LoginSystem(string userName, int userID, int companyID, string password, bool remember)
        {
            IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
            string loginUseridEncrypt = encrypt.Encrypt("LoginUserID");
            string userIdEncrypt = encrypt.Encrypt(userName);

            UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("LoginUserID"), encrypt.Encrypt(userID.ToString()), DateTime.Now.AddMinutes(30));
            UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("Login_UserName_"), encrypt.Encrypt(userName), DateTime.Now.AddDays(7));
            UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("CompanyID"), encrypt.Encrypt(companyID + ""), DateTime.Now.AddMinutes(30));
            if (remember)
            {
                UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("Login_Password_"), encrypt.Encrypt(password), DateTime.Now.AddDays(7));
            }
            else
            {
                UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("Login_Password_"), string.Empty, DateTime.Now.AddSeconds(1));
            }

            IdentityContext.UserID = userID;
            IdentityContext.CompanyID = companyID;

        }

    }
}
