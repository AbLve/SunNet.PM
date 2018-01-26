using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core.Validator;
using SunNet.PMNew.Core.TicketModule;
using SunNet.PMNew.Entity.TicketModel;


namespace SunNet.PMNew.Core.TicketModule
{
    public class CateGoryManager : BaseMgr
    {
        private const string CACHE_USERCATEGORIES = "CATEGORIES::{0}";

        private IEmailSender emailSender;
        private ICache<CateGoryManager> cache;

        private ICateGoryRepository cgRepo;
        private ICateGoryTicketRepository cgtRepo;
        private ITicketsRepository ticketRepo;
        public CateGoryManager(IEmailSender emailSender,
                                ICache<CateGoryManager> cache,
                                ICateGoryRepository repository,
                                ICateGoryTicketRepository cgtRepo,
                                ITicketsRepository tickRepo
            )
        {
            this.emailSender = emailSender;
            this.cache = cache;

            this.cgRepo = repository;
            this.cgtRepo = cgtRepo;
            this.ticketRepo = tickRepo;
        }
        private bool CheckCategory(string title, int userID)
        {
            int count = cgRepo.CountCateGory(title, userID);
            if (count > 0)
            {
                return false;
            }
            return true;
        }
        public int AddCateGory(CateGoryEntity entity)
        {
            this.ClearBrokenRuleMessages();
            BaseValidator<CateGoryEntity> validator = new AddCateGoryValidator();

            if (!validator.Validate(entity))
            {
                this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
                return 0;
            }
            if (!CheckCategory(entity.Title
                                        .TrimEnd(" ".ToCharArray())
                                        .TrimStart(" ".ToCharArray()),
                                entity.CreatedBy))
            {
                this.AddBrokenRuleMessage("Error",
                    string.Format("You have create a category with the title [{0}].", 
                                    entity.Title));
                return 0;
            }
            int id = cgRepo.Insert(entity);
            if (id <= 0)
            {
                this.AddBrokenRuleMessage();
                return 0;
            }
            ClearCache();
            entity.ID = id;
            return id;
        }

        public bool DeleteCateGroy(int id)
        {
            this.ClearBrokenRuleMessages();
            if (!cgRepo.Delete(id))
            {
                this.AddBrokenRuleMessage();
                return false;
            }
            ClearCache();
            return true;
        }

        public CateGoryEntity GetCateGory(int id)
        {
            this.ClearBrokenRuleMessages();
            if (id <= 0)
                return null;

            CateGoryEntity entity = cgRepo.Get(id);
            return entity;
        }

        public List<CateGoryEntity> GetCateGroyListByUserID(int id)
        {
            if (id <= 0) return null;
            string key = string.Format(CACHE_USERCATEGORIES, id.ToString());
            List<CateGoryEntity> list;
            if (cache[key] != null)
            {
                list = (List<CateGoryEntity>)cache[key];
            }
            else
            {
                list = cgRepo.GetCateGoryListByUserID(id);
                if (list == null || list.Count <= 0) return null;
                cache[key] = list;
            }
            return list;
        }

        public int AssignTicketToCateGory(CateGoryTicketEntity model)
        {
            this.ClearBrokenRuleMessages();
            int id = cgtRepo.Insert(model);
            if (id <= 0)
            {
                this.AddBrokenRuleMessage();
                return 0;
            }
            model.ID = id;
            return id;
        }

        public bool RemoveTicketFromCateGory(int ticketID, int categoryID)
        {
            this.ClearBrokenRuleMessages();
            if (!cgtRepo.Delete(ticketID, categoryID))
            {
                this.AddBrokenRuleMessage();
                return false;
            }
            return true;
        }

        private void ClearCache()
        {
            string key = string.Format(CACHE_USERCATEGORIES, IdentityContext.UserID.ToString());
            cache[key] = null;
        }
    }
}
