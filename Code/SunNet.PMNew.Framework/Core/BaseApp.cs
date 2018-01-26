using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Framework.Core.Validator.UserControls;

namespace SunNet.PMNew.Framework.Core
{
    public class BaseApp: BrokenMessageEnabled
    {
        public int ContextUserID
        {
            get
            {
                return IdentityContext.UserID;
            }
        }

        #region DisplayMessages
        //Web form usage
        public void BindBrokenMessages2WebPageBindableFieldValidators(Page page)
        {
            BindBrokenMessages2WebPageBindableFieldValidators(page.Controls);
        }
        private void BindBrokenMessages2WebPageBindableFieldValidators(ControlCollection controls)
        {
            foreach (BrokenRuleMessage rule in this.BrokenRuleMessages)
            {
                for(int i=0;i<controls.Count;i++)
                {
                    if (controls[i] is BindableFieldValidator)
                    {
                        BindableFieldValidator cv = (BindableFieldValidator)controls[i];
                        if (cv.FieldName == null)
                            continue;
                        if (cv.FieldName != rule.Key)
                            continue;

                        cv.ErrorMessage = rule.Message;
                        cv.IsValid = false;
                    }
                    else if (controls[i].Controls.Count > 0)
                    {
                        BindBrokenMessages2WebPageBindableFieldValidators(controls[i].Controls);
                    }
                }
            }
        }


        //Winform usage
        public void BindBrokenMessages2WinformErrorProvider(System.Windows.Forms.ErrorProvider errorProvider, System.Windows.Forms.Control.ControlCollection controls)
        {
            BindBrokenMessages2WinformErrorProvider(errorProvider, controls, true);
        }
        private void BindBrokenMessages2WinformErrorProvider(System.Windows.Forms.ErrorProvider errorProvider, System.Windows.Forms.Control.ControlCollection controls, bool clearMessages)
        {
            if (clearMessages)
                errorProvider.Clear();

            foreach (BrokenRuleMessage rule in this.BrokenRuleMessages)
            {
                for (int i = 0; i < controls.Count; i++)
                {
                    if (controls[i] is System.Windows.Forms.Control)
                    {
                        System.Windows.Forms.Control cv = (System.Windows.Forms.Control)controls[i];
                        if (cv.Tag == null)
                            continue;
                        if (Convert.ToString(cv.Tag) != rule.Key)
                            continue;

                        errorProvider.SetError(cv, rule.Message);
                    }
                    else if (controls[i].Controls.Count > 0)
                    {
                        BindBrokenMessages2WinformErrorProvider(errorProvider, controls[i].Controls, false);
                    }
                }
            }
        }


        //javascript usage
        public void BindBrokenMessages2JQuery(Page page)
        {
            System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonJS = json.Serialize(this.BrokenRuleMessages);
            string js = string.Format(@"
                                            window.SNF.ValidateFieldName="""";
                                            window.SNF.ServerResponse={0};
                                            window.SNF.DisplayMessage();
                                           ", jsonJS);
            string key = string.Format("DisplayErrorMessage{0}", Guid.NewGuid());
            page.ClientScript.RegisterStartupScript(this.GetType(), key, js, true);
        }
        #endregion
    }
}
