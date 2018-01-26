using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Framework.Utils.Helpers
{
    public class MessageInfo
    {

        public string NameFieldIsNullOrEmpty()
        {
            return "Please, supply a name!";
        }

        public string NameFieldIsNullOrEmpty(string filed, bool IsVowel)//a e i o u is vowel
        {
            return string.Format("Please, supply {0} {1}!", IsVowel == true ? "a" : "an", filed);
        }

        public string EmailFieldIsInvalid()
        {
            return "Please, provide a valid email!";
            // 	Please, enter valid e-mail address.
        }

        public string AgeBetweenMinAndMax(int min, int max)
        {
            return string.Format("Age should between {0} and {1}!", min, max);
        }

    }
}
