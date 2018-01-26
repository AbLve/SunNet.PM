using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Core.ProjectModule
{
    public interface ISearchUsers
    {
        SearchUserResponse GetProjectUsers(SearchUsersRequest request);
    }
}
