using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.TicketModule;
using StructureMap;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Impl.SqlDataProvider.Ticket;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Core;

namespace SunNet.PMNew.App
{
    public class CateGoryApplication : BaseApp
    {
        CateGoryManager mgr;
        public CateGoryApplication()
        {
            mgr = new CateGoryManager(ObjectFactory.GetInstance<IEmailSender>(),
                                    ObjectFactory.GetInstance<ICache<CateGoryManager>>(),
                                    ObjectFactory.GetInstance<ICateGoryRepository>(),
                                    ObjectFactory.GetInstance<ICateGoryTicketRepository>(),
                                    ObjectFactory.GetInstance<ITicketsRepository>());
        }

        public int AddCateGory(CateGoryEntity entity)
        {
            this.ClearBrokenRuleMessages();
            int id = mgr.AddCateGory(entity);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return id;
        }

        public bool DeleteCateGroy(int id)
        {
            this.ClearBrokenRuleMessages();
            bool result = mgr.DeleteCateGroy(id);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return result;
        }

        public CateGoryEntity GetCateGory(int id)
        {
            this.ClearBrokenRuleMessages();
            CateGoryEntity entity = mgr.GetCateGory(id);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return entity;
        }

        public List<CateGoryEntity> GetCateGroyListByUserID(int id)
        {
            this.ClearBrokenRuleMessages();
            List<CateGoryEntity> list = mgr.GetCateGroyListByUserID(id);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return list;
        }

        public int AssignTicketToCateGory(CateGoryTicketEntity model)
        {
            this.ClearBrokenRuleMessages();
            int id = mgr.AssignTicketToCateGory(model);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return id;
        }

        /// <summary>
        /// 从category中移除Ticket.
        /// </summary>
        /// <param name="ticketID">需要移除的TicketId，0表示移除全部.</param>
        /// <param name="categoryID">The category identifier.</param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/7 14:50
        public bool RemoveTicketFromCateGory(int ticketID, int categoryID)
        {
            this.ClearBrokenRuleMessages();
            bool result = mgr.RemoveTicketFromCateGory(ticketID, categoryID);
            this.AddBrokenRuleMessages(mgr.BrokenRuleMessages);
            return result;
        }
    }
}
