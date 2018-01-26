using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.UserModel.UserModel
{
    public class SelectUserModel
    {
        public int UserID { get; set; }
        public RolesEnum Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
