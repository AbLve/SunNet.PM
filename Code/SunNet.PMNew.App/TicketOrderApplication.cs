using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Core.TicketModule;
using StructureMap;
using SunNet.PMNew.Entity.TicketModel;

namespace SunNet.PMNew.App
{
/*
    public class TicketOrderApplication : BaseApp
    {
        private TicketsOrderManager mgr;
        private ITicketsOrderRespository repository;

        public TicketOrderApplication()
        {
            mgr = new TicketsOrderManager(
                                ObjectFactory.GetInstance<ITicketsOrderRespository>()
                                );

            repository = ObjectFactory.GetInstance<ITicketsOrderRespository>();
        }

        public List<TicketsOrderEntity> GetListByProjectId(string pidList)
        {
            if (pidList.Length <= 0) return null;

            return repository.GetListByProjectId(pidList);
        }
    }
*/
}
