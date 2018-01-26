using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace SF.Framework.Core.Validator
{
    public class SpecialCharChecker : SFBaseValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string src = (string)value;

            string compareString = "`~!@#$%^&*()/.,<>'\"\\|[]{};:-=";
            int count = 0;
            foreach (char c in src.ToCharArray())
                if (compareString.ToCharArray().Contains(c))
                    count++;
            if (count <= 0)
                return new ValidationResult("Not found special char");

            return ValidationResult.Success;
        }
    }
}
