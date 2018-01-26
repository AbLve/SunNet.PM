using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FamilyBook.Core;

namespace FamilyBook.Business.Global
{
    public static  class GlobalDataBusiness
    {
        public static void SetGolbalData()
        {
            GlobalDataAgent.GetDatas().Init();
        }
    }
}
