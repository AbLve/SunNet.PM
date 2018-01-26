using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Web.Codes;

namespace SunNet.PMNew.Web.UserControls
{
    public partial class RelationTicketsList : BaseAscx
    {
        TicketsApplication ticketApp = new TicketsApplication();
        List<TicketsEntity> list = new List<TicketsEntity>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int tid = QS("tid", 0);
                if (tid > 0)
                {
                    GetListRelationByTid(tid);
                }
            }
        }

        private void GetListRelationByTid(int tid)
        {
            list = ticketApp.GetRelationTicketListByTid(tid);

            if (null != list && list.Count > 0)
            {
                this.rptRelationTicketList.DataSource = list;
                this.rptRelationTicketList.DataBind();
            }
            else
            {
                this.trNoTickets.Visible = true;
            }
        }

        #region
        public bool RelationhasPermission { get; set; }
        #endregion


        #region common method

        public string ShowDelteImg(object id)
        {
            if (RelationhasPermission)
            {
                return string.Format(" <a href='#' id='{0}' onclick='deleteRlationTR({1});return false'> <img src='/icons/26.gif' title='Delete' alt='delete'> </a>", id, id);
            }
            return "";
        }

        #endregion
    }
}