using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.UserModel.UserModel
{
    public class DashboardUserModel
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsHide { get; set; }
    }
}
