using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.UserModel
{
    public class SearchUserResponse
    {
        public List<UsersEntity> ResultList { get; set; }
        public int ResultCount { get; set; }
    }
}
