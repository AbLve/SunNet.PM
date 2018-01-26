using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.Framework.Core.Validator.UserControls
{
    public class BindableFieldValidator:CustomValidator
    {
        public string FieldName { get; set; }
    }
}
