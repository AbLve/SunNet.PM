using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SF.Framework.Cache
{
    public interface ICache
    {
        string CachePrefix { get; set; }
        object this[string key] { get; set; }
    }
}
