using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SF.Framework.Core
{
    public enum UserType
    {
        Administrator = 1,
        Trainer = 3,
        CenterDirector = 2,
        Practitioner = 4
    }
    public interface ILogin
    {
        int ID { get; set; }
        UserType UserType { get; }
    }

    public interface ILoginSession
    {
        int CompanyID { get; set; }
        string CompanyName { get; set; }
        string Email { get; set; }
        int SubCompanyID { get; set; }
        bool IsSubCompany { get; set; }
        int UserID { get; set; }
        string LoginName { get; set; }
        int LoginID { get; set; }
        string UserEmail { get; set; }
        string UserName { get; set; }
        string Logo { get; set; }
    }
}
