using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FamilyBook.Common
{
    /// <summary>
    /// Author: Dave
    /// Date: 2013.12.09
    /// Description:
    ///     Sign up status.
    /// </summary>
    public enum SignUpStatus
    {
        [Description("")]
        NotSignIn = 0,
        [Description("")]
        SignIn = 1,
        [Description("")]
        ConfirmPassValid = 2,
        [Description("")]
        EmailExist = 3
    }

    /// <summary>
    /// Type of user account, eg: admin/user
    /// </summary>
    public enum AccountType
    {
        ADMIN = 0,
        USER = 1
    }

    /// <summary>
    /// The account is useable or not.
    /// </summary>
    public enum AccountStatus
    {
        Deleted = 0,
        Useable = 1,
        Disable = 2
    }

    public enum Gender
    {
        None = -1,
        Male = 1,
        Female = 0
    }

    /// <summary>
    /// Author: Magus
    /// Date: 2013.12.15
    /// </summary>
    public enum FamilyMemberStatus
    {
        Active = 1,
        Inactive = 0,
    }

    /// <summary>
    /// Author: Magus
    /// Date: 2013.12.15
    /// Date: 2013.12.21
    /// </summary>
    public enum FamilyMemberRelationType
    {
        [Description("None")]
        None = -1,
        [Description("Husband")]
        Husband = 0,
        [Description("Wife")]
        Wife = 1,
        [Description("Father")]
        Father = 2,
        [Description("Mother")]
        Mother = 3,
        [Description("Child")]
        Child = 4,
        [Description("ChildByFather")]
        ChildByFather = 5,
        [Description("ChildByMother")]
        ChildByMother = 6,
        [Description("Brother")]
        Brother = 7
    }

    /// <summary>
    /// Author: Magus
    /// Date: 2013.12.18
    /// </summary>
    public enum GroupType
    {
        Family = 0,
        Friend = 1,
        Custom = 2
    }

    /// <summary>
    /// FamilyTree 用户状态
    /// </summary>
    public enum FamilyTreeUserStatus
    {
        /// <summary>
        /// 虚拟账户
        /// </summary>
        VirtualAccount = 0,
        /// <summary>
        /// 正常用户
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 正在发送请求
        /// </summary>
        Requesting = 2,
    }
}
