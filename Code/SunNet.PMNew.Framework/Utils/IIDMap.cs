using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Framework.Utils
{
    public interface IIDMap<T>
    {
        int? this[Guid externalId] { get; set; }
    }
}
