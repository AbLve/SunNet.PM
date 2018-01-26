using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SunNet.PMNew.Entity.UserModel;

using SunNet.PMNew.Framework.Core.Notify;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.Core.UserModule
{
    //public class UserInsertedNotification : INotify
    //{
    //    private IEmailSender emailSender;
    //    private UsersEntity user;
    //    public UserInsertedNotification(IEmailSender emailSender, UsersEntity userInserted)
    //    {
    //        this.emailSender = emailSender;
    //        this.user = userInserted;
    //    }
    //    public void Notify()
    //    {
    //        string from = "sunnet@sunnet.us";
    //        string subject = "Add New User";
    //        string sendTo = string.Empty;
    //        string sendToNames = string.Empty;
    //        switch (user.Role)
    //        {
    //            case RolesEnum.CLIENT:
    //                sendTo = "pm1@sunnet.us;pm2@sunnet.us;admin@sunnet.us";
    //                sendToNames = "Pm1,Pm2,Admin";
    //                break;
    //            default:
    //                sendTo = "pm1@sunnet.us;pm2@sunnet.us;admin@sunnet.us;devLeader@sunnet.us;testLeader@sunnet.us";
    //                sendToNames = "Pm1,Pm2,Admin,Lee,Sam,Susan";
    //                break;
    //        }
    //        string body = string.Format("Hello,{0}:<br/>we got a new user named:{1} {2},Email:{3},Company:{4}.", sendToNames, user.FirstName, user.LastName, user.Email, user.CompanyName);
    //        emailSender.SendMail(sendToNames, from, subject, body);
    //    }
    //}
}
