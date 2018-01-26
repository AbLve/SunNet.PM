using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SF.Framework.IDMap
{
    public interface IIDMap<T>
    {
        int? this[Guid externalId] { get; set; }
    }
}
