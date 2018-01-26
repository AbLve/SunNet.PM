using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SF.Framework.IDMap.Providers
{
    public class DictionaryIDMap<T> : IIDMap<T>
    {
        private Dictionary<Guid, int> map = new Dictionary<Guid, int>();

        public int? this[Guid externalId]
        {
            get
            {
                if (map.ContainsKey(externalId))
                    return map[externalId];
                return new Nullable<int>();
            }
            set
            {
                if (value == null || !value.HasValue)
                {
                    if (map.ContainsKey(externalId))
                        map.Remove(externalId);
                    return;
                }

                map[externalId] = value.Value;
            }
        }
    }
}
