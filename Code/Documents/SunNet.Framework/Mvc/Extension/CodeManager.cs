using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SF.Framework.Mvc.Extension
{
    public static class CodeManager
    {
        private static CodeDescription[] codes = new CodeDescription[]
        {
           new CodeDescription("Yes","Yes","YesNo"),
           new CodeDescription("No","No","YesNo"),
           new CodeDescription("Yes","Yes","YesNoNA"),
           new CodeDescription("No","No","YesNoNA"),
           new CodeDescription("NA","NA","YesNoNA")
        };
        public static Collection<CodeDescription> GetCodes(string category)
        {
            Collection<CodeDescription> codeCollection = new Collection<CodeDescription>();
            foreach (var code in codes.Where(code => code.Category == category))
            {
                codeCollection.Add(code);
            }
            return codeCollection;
        }
    }
}
