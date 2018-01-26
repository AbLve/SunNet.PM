using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Core.TicketModule;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Core.Validator;

namespace Pm2012TEST.Fakes
{
    class FakeTicketsRepository : ITicketsRepository
    {

        #region ITicketsRepository Members

        //public List<TicketsEntity> GetTicketListBySearchCondition(TicketsSearchCondition ticketSC(GetTicketsListByConditionRequest request,  out int recordCount, int page, int pageCount)
        //{
        //    List<TicketsEntity> list = new List<TicketsEntity>();
        //    if (null != ticketSC)
        //    {

        //        TicketsEntity info = null;
        //        for (int i = 0; i < 6; i++)
        //        {
        //            info = new TicketsEntity();
        //            list.Add(info);
        //        }
        //    }
        //    return list;

        //}
        //FakeTicketsRepository repository = new FakeTicketsRepository();

        private bool ValdateChangeTicketStateToBug(int tid, int type, string descr)
        {
            bool pass = true;
            if (tid <= 0 || type <= 0)
            {
                pass = false;
            }
            else if (type == 2)
            {
                pass = descr.Trim().Length == 0 ? false : true;
            }
            return pass;
        }

        public bool ChangeTicketStatesBugToRequest(int tid, int type, string descr, TicketsEntity te)
        {
            return true;

            //if (!ValdateChangeTicketStateToBug(tid, type, descr)) return false;

            //BaseValidator<TicketsEntity> validator = new UpdateTicketValidator();

            //if (!validator.Validate(te))
            //{
            //    return false;
            //}
            //bool RecordResult = true;
            ////add new request ticket to db 
            //TicketsEntity newEntity = new TicketsEntity();
            //newEntity.TicketType = TicketsType.Request;
            //RecordResult = Convert.ToBoolean(repository.Insert(newEntity));

            //if (!RecordResult)
            //{
            //    return false;
            //}
            ////update ticket state 
            //switch (type)
            //{
            //    case 1:
            //        te.ConvertDelete = CovertDeleteState.ConvertToHistory;
            //        RecordResult = repository.Update(te);
            //        break;
            //    case 2:
            //        te.Description = descr;
            //        RecordResult = repository.Update(te);
            //        break;
            //    case 3:
            //        te.ConvertDelete = CovertDeleteState.ForeverDelete;
            //        RecordResult = repository.Update(te);
            //        break;
            //    default:
            //        RecordResult = false;
            //        break;
            //}
            //if (!RecordResult)
            //{
            //    return false;
            //}
            //return RecordResult;
        }

        public int GetPmIdByTicketsProjectId(int projectId)
        {
            return 1;
        }

        #endregion

        #region IRepository<TicketsEntity> Members

        public int Insert(TicketsEntity entity)
        {
            return 100;
        }

        public int UpdateCode { get; set; }
        public bool Update(TicketsEntity entity)
        {
            bool Flag = false;
            if (null != entity)
            {
                if (entity.ConvertDelete == CovertDeleteState.ConvertToHistory)
                {
                    UpdateCode = 100;
                    Flag = true;
                }
                else if (!string.IsNullOrEmpty(entity.Description) && entity.ConvertDelete == CovertDeleteState.Normal)
                {
                    UpdateCode = 200;
                    Flag = true;
                }
                else if (entity.ConvertDelete == CovertDeleteState.ForeverDelete)
                {
                    UpdateCode = 300;
                    Flag = true;
                }

            }

            return Flag;

        }

        public bool Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public TicketsEntity Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public bool SendSuc { get; set; }
        FakeEmailSender email = new FakeEmailSender();
        public bool UpdateTicketStateAndSendEmail(int ticketId, string uName, TicketsState ts)
        {
            bool flag = true;
            if (ticketId <= 0 && string.IsNullOrEmpty(uName)) return false;
            flag = true;
            if (!flag) return false;
            switch (ts)
            {
                case TicketsState.PMReviewed:
                    if (uName == "pm")
                    {
                        SendSuc = email.SendMail(uName + "@sunnet.us", "", "", "");
                    }
                    break;

                case TicketsState.Developing:
                    if (uName == "user")
                    {
                        SendSuc = email.SendMail("user", "", "", "");
                    }
                    break;
                case TicketsState.TestOnLocalFail:
                    if (uName == "user")
                    {
                        SendSuc = email.SendMail("user", "", "", "");
                    }
                    break;
                case TicketsState.TestOnLocalSuc:
                    if (uName == "user")
                    {
                        SendSuc = email.SendMail("user", "", "", "");
                    }
                    break;
                case TicketsState.Completed:
                    if (uName == "user")
                    {
                        SendSuc = email.SendMail("user", "", "", "");
                    }
                    break;
                default:
                    SendSuc = false;
                    break;
            }

            return flag;
        }

        #endregion

        #region ITicketsRepository Members

        public List<TicketsEntity> GetTicketListBySearchCondition(TicketsSearchCondition ticketSC, out int recordCount, int page, int pageCount)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ITicketsRepository Members

        List<TicketsEntity> ITicketsRepository.GetTicketListBySearchCondition(TicketsSearchCondition ticketSC, out int recordCount, int page, int pageCount)
        {
            throw new NotImplementedException();
        }

        bool ITicketsRepository.ChangeTicketStatesBugToRequest(int tid, int type, string descr, TicketsEntity te)
        {
            throw new NotImplementedException();
        }

        int ITicketsRepository.GetPmIdByTicketsProjectId(int projectId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRepository<TicketsEntity> Members

        int SunNet.PMNew.Framework.Core.Repository.IRepository<TicketsEntity>.Insert(TicketsEntity entity)
        {
            throw new NotImplementedException();
        }

        bool SunNet.PMNew.Framework.Core.Repository.IRepository<TicketsEntity>.Update(TicketsEntity entity)
        {
            throw new NotImplementedException();
        }

        bool SunNet.PMNew.Framework.Core.Repository.IRepository<TicketsEntity>.Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        TicketsEntity SunNet.PMNew.Framework.Core.Repository.IRepository<TicketsEntity>.Get(int entityId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ITicketsRepository Members


        public int GetMaxTicketId()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ITicketsRepository Members


        public List<TicketsEntity> GetTicketsForEditPriority(int projectID)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ITicketsRepository Members



        //List<TicketsEntity> ITicketsRepository.GetTicketsForEditPriority(int projectID)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region ITicketsRepository Members


        public List<TicketsEntity> GetRelationTicketListByTid(int tid)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ITicketsRepository Members


        public SearchTicketsResponse SearchTickets(SearchTicketsRequest request)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ITicketsRepository Members



        SearchTicketsResponse ITicketsRepository.SearchTickets(SearchTicketsRequest request)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ITicketsRepository Members


        public GetProjectIdAndUserIDResponse GetProjectIdAndUserID(int ticketId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ITicketsRepository Members


           #endregion

        #region ITicketsRepository Members


        public GetTicketCreateByAndStatusResponse GetTicketCreateByAndStatus(int ticketID)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ITicketsRepository Members


        public System.Data.DataTable SearchScheduleTickets(SearchTicketsRequest request)
        {
            throw new NotImplementedException();
        }

        public int SearchScheduleTicketsCount(SearchTicketsRequest request)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ITicketsRepository Members


        System.Data.IDataReader ITicketsRepository.SearchScheduleTickets(SearchTicketsRequest request)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ITicketsRepository Members


        public int GetTotalCountByStatus(TotalCountByStatus Type, int UserID)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
