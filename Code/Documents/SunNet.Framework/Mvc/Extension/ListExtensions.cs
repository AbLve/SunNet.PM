using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SF.Framework.Mvc.Extension
{
    /// <summary>
    /// Convert to Vertical direction
    /// </summary>
    public static class ListExtensions
    {
        public static List<T> ToList<T>(this List<T> list, int column) where T : new()
        {
            List<T> lAll = new List<T>();
            int columnCount = list.Count / column;
            int shortStart = list.Count % column;
            if (shortStart > 0)
            {
                columnCount++;
                for (int i = shortStart; i < column; i++)
                {
                    list.Add(new T());
                }
            }
            List<List<T>> lists = new List<List<T>>();
            for (int i = 0; i < column; i++)
            {
                List<T> l = new List<T>();
                l.AddRange(list.Skip(i * columnCount).Take(columnCount));
                lists.Add(l);
            }
            for (int i = 0; i < columnCount; i++)
            {
                foreach (var item in lists)
                {
                    if (item.Count > i)
                    {
                        lAll.Add(item[i]);
                    }
                }
            }
            return lAll;
        }
    }
}
