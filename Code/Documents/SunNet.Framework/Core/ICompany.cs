using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SF.Framework.Core
{
    public interface ICompany
    {
        int CompanyID { get; set; }
        bool IsDeleted { get; set; }
        string CompanyAccount { get; set; }
    }
}
