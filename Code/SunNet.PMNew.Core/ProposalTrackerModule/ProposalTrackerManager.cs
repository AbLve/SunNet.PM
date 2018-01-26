using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Framework.Core.Notify;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Framework.Core.Validator;

using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Framework;
using System.Web;
using System.Text.RegularExpressions;
using System.Transactions;
using SunNet.PMNew.Entity.ProposalTrackerModel;
namespace SunNet.PMNew.Core.ProposalTrackerModule
{
    public class ProposalTrackerManager : BaseMgr
    {
        private IProposalTrackerRepository proposalTrackerRepository;

        public ProposalTrackerManager(IProposalTrackerRepository proposalTrackerRepository)
        {
            this.proposalTrackerRepository = proposalTrackerRepository;
        }

        public ProposalTrackerEntity Get(int id)
        {
            return proposalTrackerRepository.Get(id);
        }

        public int Add(ProposalTrackerEntity entity)
        {
            BaseValidator<ProposalTrackerEntity> validator = new AddProposalTrackerValidator();
            if (entity.ProjectID == 0)
            {
                this.AddBrokenRuleMessage("Project", "Please select Project");
                return 0;
            }

            if (string.IsNullOrEmpty(entity.Title))
            {
                this.AddBrokenRuleMessage("Title", "Please input Title");
                return 0;
            }
            //if (string.IsNullOrEmpty(entity.WorkScope))
            //{
            //    this.AddBrokenRuleMessage("WorkScope", "Please select WorkScope");
            //    return 0;
            //}
            if (entity.Description.Length > 500)
            {
                this.AddBrokenRuleMessage("Description", "Please input Description in 500 words");
                return 0;
            }
            //if (!validator.Validate(entity))
            //{
            //    this.AddBrokenRuleMessages(validator.BrokenRuleMessages);
            //    return 0;
            //}
            int id = proposalTrackerRepository.Insert(entity);
            if (id <= 0)
            {
                this.AddBrokenRuleMessage();
                return 0;
            }
            return id;
        }


        public bool Update(ProposalTrackerEntity entity)
        {
            BaseValidator<ProposalTrackerEntity> validator = new AddProposalTrackerValidator();
            if (entity.ProjectID == 0)
            {
                this.AddBrokenRuleMessage("Project", "Please select Project");
                return false;
            }
            if (string.IsNullOrEmpty(entity.Title))
            {
                this.AddBrokenRuleMessage("Title", "Please input Title");
                return false;
            }
            if (entity.Description.Length > 500)
            {
                this.AddBrokenRuleMessage("Description", "Please input Description in 500 words");
                return false;
            }
            bool result = proposalTrackerRepository.Update(entity);
            if (!result)
            {
                this.AddBrokenRuleMessage();
                return false;
            }
            return result;
        }

        public bool UpdateProposalTrackerForPayment(ProposalTrackerEntity entity, string connStr)
        {
            BaseValidator<ProposalTrackerEntity> validator = new AddProposalTrackerValidator();
            return proposalTrackerRepository.UpdateProposalTrackerForPayment(entity, connStr);
        }


        public int GetProposalTrackerNo(DateTime beginDate, DateTime endDate)
        {
            return proposalTrackerRepository.GetProposalTrackers("", 0, 0, 0, 0, "RequestNo", "desc").Where(r => r.CreatedOn >= beginDate.Date && r.CreatedOn < endDate).Count();
        }

        public List<ProposalTrackerEntity> GetProposalTrackerByPid(int projectId)
        {
            return proposalTrackerRepository.GetProposalTrackerByPid(projectId);
        }
        public ProposalTrackerRelationEntity GetProposalTrackerByTid(int Tid)
        {
            return proposalTrackerRepository.GetProposalTrackerByTid(Tid);
        }
        public bool UpdateProposalByProposal(ProposalTrackerRelationEntity model)
        {
            return proposalTrackerRepository.UpdateProposalByProposal(model);
        }

        public bool DelProposalTrackerRelationByID(int ID)
        {
            return proposalTrackerRepository.DelProposalTrackerRelationByID(ID);
        }

        public SearchProposalTrackerRequest GetSearchProposalTrackers(string keyword, int projectId, int status, int companyId,
            int payment, int userId,DateTime? beginTime,DateTime? endTime, string orders, string dir, int pageCount, int pageIndex)
        {
            return proposalTrackerRepository.GetProposalTrackers(keyword, projectId, status, companyId, payment,
                userId, beginTime,endTime, orders, dir, pageCount, pageIndex);
        }

        public decimal GetProposalTrackerHours(int ID)
        {
            return proposalTrackerRepository.GetProposalTrackerHours(ID);
        }

        public List<ProposalTrackerEntity> GetEntitiesForPaymentEmail(string condition, string connStr)
        {
            return proposalTrackerRepository.GetEntitiesForPaymentEmail(condition, connStr);
        }

        public ProposalViewModel GetProposalViewModel(int proposalId)
        {
            return proposalTrackerRepository.GetProposalViewModel(proposalId);
        }
    }
}
