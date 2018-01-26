using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.Web.Codes
{
    public class BaseAscx : System.Web.UI.UserControl
    {
        public BaseWebsitePage BaseWebsitePage
        {
            get { return (BaseWebsitePage)this.Page; }
        }
        public UsersEntity UserInfo
        {
            get { return BaseWebsitePage.UserInfo; }
        }
        #region common method

        public void SetDefaultValueForDropdownList<T>(DropDownList ddl, List<T> list)
        {
            if (list.Count > 0)
            {
                ddl.DataSource = list;
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("Please select...", ""));
                ddl.SelectedIndex = list.Count == 1 ? 1 : 0;

            }
        }
        #endregion
        #region QueryString
        protected string QS(string key)
        {
            return Request.QueryString[key] + "";
        }

        protected int QS(string key, int v)
        {
            int result;
            if (int.TryParse(QS(key), out result))
                return result;
            return v;
        }

        protected string QF(string key)
        {
            return Request.Form[key] + "";
        }

        protected int QF(string key, int v)
        {
            int result;
            if (int.TryParse(QF(key), out result))
                return result;
            return v;
        }
        #endregion
        public List<ListItem> ConvertEnumtToListItem(Type type)
        {
            List<ListItem> result = new List<ListItem>();

            string[] names = Enum.GetNames(type);
            Array values = Enum.GetValues(type);
            for (int i = 0; i < names.Length; i++)
            {
                result.Add(new ListItem() { Text = names[i], Value = values.GetValue(i).ToString() });
            }
            result.Sort(new RoleEnumComparer());

            return result;
        }
    }
}
