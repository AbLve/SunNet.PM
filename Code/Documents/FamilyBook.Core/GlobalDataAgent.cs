using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FamilyBook.Core
{
   public class GlobalDataAgent
    {

       public static GlobalDataAgent GetDatas()
        {
            return Nested.instance;
        }

         private GlobalDataAgent() { }

        class Nested
        {
            static Nested() { }
            internal static readonly GlobalDataAgent instance = new GlobalDataAgent();
        }

       /// <summary>
       /// 初始化
       /// </summary>
        public void Init()
        {
        }
    }
}
