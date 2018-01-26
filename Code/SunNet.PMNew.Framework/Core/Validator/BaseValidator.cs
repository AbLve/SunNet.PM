using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SunNet.PMNew.Framework.Core.BrokenMessage;

namespace SunNet.PMNew.Framework.Core.Validator
{
    public abstract class BaseValidator<T> : BrokenMessageEnabled 
        where T : class
    {
        public bool Validate(T o)
        {
            this.ClearBrokenRuleMessages();
            if (o == null)
                throw new Exception("Entity cannot be null");

            ValidateBasicRules(o);
            ValidateExtraRules(o);

            return this.BrokenRuleMessages.Count == 0;
        }
        protected abstract void ValidateExtraRules(T o);

        private void ValidateBasicRules(T o)
        {
            var descriptor = GetTypeDescriptor(typeof(T));
            foreach (PropertyDescriptor propertyDescriptor in descriptor.GetProperties())
            {
                foreach (var validationAttribute in propertyDescriptor.Attributes.OfType<ValidationAttribute>())
                {
                    if (!validationAttribute.IsValid(propertyDescriptor.GetValue(o)))
                    {
                        BrokenRuleMessage msg = new BrokenRuleMessage(propertyDescriptor.Name, validationAttribute.FormatErrorMessage(propertyDescriptor.Name));
                        AddBrokenRuleMessage(msg);
                    }
                }
            }
        }
        private static ICustomTypeDescriptor GetTypeDescriptor(Type type)
        {
            return new AssociatedMetadataTypeTypeDescriptionProvider(type).GetTypeDescriptor(type);
        }
    }
}