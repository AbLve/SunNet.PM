using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SF.Framework.Core.BrokenMessage;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using SF.Framework.Mvc.Extension;

namespace SF.Framework.Core.Validator
{
    public class CommonValidator
    {
        private BaseService service;
        public CommonValidator(BaseService service)
        {
            this.service = service;
        }

        public void EnsureNotNull(object o, string msg)
        {
            if (o == null)
                throw new Exception(msg);
        }


        public void EnsureIntLessOrEqualThan0(int id, string msg)
        {
            if (id > 0)
                throw new Exception(msg);
        }
        public void EnsureGuidEmpty(Guid id, string msg)
        {
            if (!id.Equals(Guid.Empty))
                throw new Exception(msg);
        }


        public void EnsureIntGreaterThan0(int id, string msg)
        {
            if (id <= 0)
                throw new Exception(msg);
        }
        public void EnsureIntGreaterThan0(int id, string key, string msg)
        {
            if (id <= 0)
                this.service.AddBrokenRuleMessage(key, msg);
        }
        public void EnsureFloatFormat(object obj, string key, string msg)
        {
            float f = new float();
            if (obj != null)
            {
                if (float.TryParse(obj.ToString(), out f))
                {

                }
                else
                {
                    throw new Exception(msg);
                }
            }
            else
            {
                throw new Exception(msg);
            }
        }
        public void EnsureGuidNotEmpty(Guid id, string msg)
        {
            if (id.Equals(Guid.Empty))
                throw new Exception(msg);
        }


        public void ValidateFields<T>(T o, bool throwException, BaseService srv)
        {
            var descriptor = GetTypeDescriptor(typeof(T));
            foreach (PropertyDescriptor propertyDescriptor in descriptor.GetProperties())
            {
                foreach (var validationAttribute in propertyDescriptor.Attributes.OfType<ValidationAttribute>())
                {
                    if (validationAttribute is SFBaseValidationAttribute)
                    {
                        //customized by sunnet framework
                        ValidationResult vr = validationAttribute.GetValidationResult(propertyDescriptor.GetValue(o), new ValidationContext(o, null, null));
                        if (vr != ValidationResult.Success)
                        {
                            if (throwException)
                                throw new Exception(vr.ErrorMessage);
                            BrokenRuleMessage msg = new BrokenRuleMessage(propertyDescriptor.Name, vr.ErrorMessage);
                            var displayAtribute = propertyDescriptor.Attributes.OfType<DisplayAttribute>().FirstOrDefault();
                            if (displayAtribute != null)
                                msg = new BrokenRuleMessage(displayAtribute.Name, vr.ErrorMessage.Replace(propertyDescriptor.Name, displayAtribute.Name));
                            srv.AddBrokenRuleMessage(msg);
                        }
                    }
                    else
                    {
                        //standard
                        if (!validationAttribute.IsValid(propertyDescriptor.GetValue(o)))
                        {
                            string exceptionDescription = validationAttribute.FormatErrorMessage(propertyDescriptor.Name);

                            if (throwException)
                                throw new Exception(exceptionDescription);

                            BrokenRuleMessage msg = new BrokenRuleMessage(propertyDescriptor.Name, exceptionDescription);
                            var displayAtribute = propertyDescriptor.Attributes.OfType<DisplayAttribute>().FirstOrDefault();
                            if (displayAtribute != null)
                                msg = new BrokenRuleMessage(displayAtribute.Name, exceptionDescription.Replace(propertyDescriptor.Name, displayAtribute.Name));
                            srv.AddBrokenRuleMessage(msg);
                        }
                    }
                }
            }
        }
        private static ICustomTypeDescriptor GetTypeDescriptor(Type type)
        {
            return new AssociatedMetadataTypeTypeDescriptionProvider(type).GetTypeDescriptor(type);
        }





        public void ExpectStringNotEmpty(string src, string key, string message)
        {
            ExpectStringNotEmpty(src, key, message, true);
        }
        public void ExpectStringNotEmpty(string src, string key, string message, bool ignoreSpace)
        {
            bool empty = false;
            if (src == null || src == string.Empty)
                empty = true;
            if (empty)
            {
                this.service.AddBrokenRuleMessage(key, message);
                return;
            }

            if (ignoreSpace)
            {
                if (src.Trim().Length == 0)
                    empty = true;
            }
            else
            {
                if (src.Length == 0)
                    empty = true;
            }
            if (empty)
            {
                this.service.AddBrokenRuleMessage(key, message);
                return;
            }
        }

        public void ExpectStringLengthInRange(string src, int minLength, int maxLength, string key, string message)
        {
            ExpectStringLengthInRange(src, minLength, maxLength, key, message, true);
        }

        public void ExpectStringLengthInRange(string src, int minLength, int maxLength, string key, string message, bool ignoreSpace)
        {
            if (src == null || src == string.Empty)
                return;

            if (ignoreSpace)
                src = src.Trim();

            if (src.Length >= minLength && src.Length <= maxLength)
                return;

            this.service.AddBrokenRuleMessage(key, string.Format(message, minLength, maxLength));
        }

        public void ExpectEmailFormat(string src, string key, string message)
        {
            if (src == null || src == string.Empty)
                return;
            if (!StringExtension.IsEmail(src))
                this.service.AddBrokenRuleMessage(key, message);
        }
        public void ExpectDateTimeFormat(string src, string key, string message)
        {
            if (!StringExtension.IsDateTime(src))
                this.service.AddBrokenRuleMessage(key, message);
        }
        public void ExpectSelectedOne(int src, string key, string message)
        {
            if (src <= 0)
                this.service.AddBrokenRuleMessage(key, message);
        }
        public void ExpectSelectedOne(Guid? src, string key, string message)
        {
            if (src == Guid.Empty || src == null)
                this.service.AddBrokenRuleMessage(key, message);
        }
    }
}
