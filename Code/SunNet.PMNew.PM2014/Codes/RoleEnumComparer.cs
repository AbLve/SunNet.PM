using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.Codes
{
    /**************************************************************************
     * Developer: 		Jack Zhang
     * Computer:		JACKZ
     * Domain:			Jackz
     * CreatedOn:		4/16 12:16:00
     * Description:		Please input class summary
     * Version History:	Created,4/16 12:16:00
     * 
     * 
     **************************************************************************/
    public class RoleEnumComparer : IComparer<ListItem>
    {
        public int Compare(ListItem x, ListItem y)
        {
            if (x.Text == y.Text)
            {
                return 0;
            }
            else if (System.String.Compare(x.Text, y.Text, System.StringComparison.CurrentCultureIgnoreCase) > 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
}