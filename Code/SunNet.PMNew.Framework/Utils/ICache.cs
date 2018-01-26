using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Framework.Utils
{
    public interface ICache<T>
    {
        object this[string key] { get; set; }
    }
}
