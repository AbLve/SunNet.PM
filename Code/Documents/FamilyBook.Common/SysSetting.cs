using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyBook.Common
{
    public class SysSetting
    {
        public static SysSetting GetSettings()
        {
            return Nested.instance;
        }

        private SysSetting() { }

        class Nested
        {
            static Nested() { }
            internal static readonly SysSetting instance = new SysSetting();
        }

        public void Init()
        {
           
        }
    }
}
