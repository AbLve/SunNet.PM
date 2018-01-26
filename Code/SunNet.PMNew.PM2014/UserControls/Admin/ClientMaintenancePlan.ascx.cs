using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.UserControls.Admin
{
    public partial class ClientMaintenancePlan : BaseAscx
    {
        float totalHours;
        float remainHours;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public float RemainHours
        {
            get
            {
                if (float.TryParse(this.txtRemainHours.Text, out remainHours))
                {
                    return remainHours;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                txtRemainHours.Text = value.ToString("f2");
                hdRemainHours.Value = txtRemainHours.Text;
            }

        }

        public float TotalHours
        {
            get
            {
                if (float.TryParse(this.txtTotalHours.Text, out totalHours))
                {
                    return totalHours;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                txtTotalHours.Text = value.ToString("f2");
                hdTotalHours.Value = txtTotalHours.Text;
            }
        }

        public UserMaintenancePlanOption SelectedMaintenancePlan
        {
            get
            {
                if (rbtnHasPlan.Checked)
                {
                    return UserMaintenancePlanOption.HAS;
                }
                else if (rbtnHasNoPlan.Checked)
                {
                    if (rbtnALLOWME.Checked)
                        return UserMaintenancePlanOption.ALLOWME;
                    else if (rbtnDONTNEEDAPPROVAL.Checked)
                        return UserMaintenancePlanOption.DONTNEEDAPPROVAL;
                    else if (rbtnNEEDAPPROVAL.Checked)
                        return UserMaintenancePlanOption.NEEDAPPROVAL;
                    else
                    {
                        return UserMaintenancePlanOption.NO;
                    }
                }
                else
                {
                    return UserMaintenancePlanOption.NONE;
                }
            }
            set
            {
                switch (value)
                {
                    case UserMaintenancePlanOption.HAS:
                        rbtnHasPlan.Checked = true;
                        rbtnHasNoPlan.Checked = false;
                        rbtnALLOWME.Checked = false;
                        rbtnDONTNEEDAPPROVAL.Checked = false;
                        rbtnNEEDAPPROVAL.Checked = false;
                        break;
                    case UserMaintenancePlanOption.NO:
                        rbtnHasPlan.Checked = false;
                        rbtnHasNoPlan.Checked = true;
                        rbtnALLOWME.Checked = false;
                        rbtnDONTNEEDAPPROVAL.Checked = false;
                        rbtnNEEDAPPROVAL.Checked = false;
                        break;
                    case UserMaintenancePlanOption.ALLOWME:
                        rbtnHasPlan.Checked = false;
                        rbtnHasNoPlan.Checked = true;
                        rbtnALLOWME.Checked = true;
                        rbtnDONTNEEDAPPROVAL.Checked = false;
                        rbtnNEEDAPPROVAL.Checked = false;
                        break;
                    case UserMaintenancePlanOption.DONTNEEDAPPROVAL:
                        rbtnHasPlan.Checked = false;
                        rbtnHasNoPlan.Checked = true;
                        rbtnALLOWME.Checked = false;
                        rbtnDONTNEEDAPPROVAL.Checked = true;
                        rbtnNEEDAPPROVAL.Checked = false;
                        break;
                    case UserMaintenancePlanOption.NEEDAPPROVAL:
                        rbtnHasPlan.Checked = false;
                        rbtnHasNoPlan.Checked = true;
                        rbtnALLOWME.Checked = false;
                        rbtnDONTNEEDAPPROVAL.Checked = false;
                        rbtnNEEDAPPROVAL.Checked = true;
                        break;
                    default: break;
                }
            }
        }
    }
}