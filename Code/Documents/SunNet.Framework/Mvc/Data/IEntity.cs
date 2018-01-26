using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SF.Framework.Mvc.Data
{
    public interface IEntity<TPkType>
    {
        TPkType Id { get; set; }
    }

}
