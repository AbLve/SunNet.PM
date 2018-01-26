using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace SF.Framework.Core.BrokenMessage
{
    public interface IBrokenMessageBinder
    {
        void BindBrokenMessages2WebPageBindableFieldValidators(Page page);
        void BindBrokenMessages2WinformErrorProvider(System.Windows.Forms.ErrorProvider errorProvider, System.Windows.Forms.Control.ControlCollection controls);
        void BindBrokenMessages2JQuery(Page page);
    }
}
