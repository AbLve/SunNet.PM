using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using StructureMap;
using SunNet.PMNew.App;
using SunNet.PMNew.Core.TicketModule;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Entity.InvoiceModel;
using SunNet.PMNew.Entity.InvoiceModel.Enums;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.PM2014.Codes;
using SunNet.PMNew.Web.Do;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunNet.PMNew.PM2014.Service
{
    /// <summary>
    /// Invoice 的摘要说明
    /// </summary>
    public class Invoice : DoBase, IHttpHandler
    {
        CompanyApplication companyApp = new CompanyApplication();
        InvoicesApplication _invoiceApp = new InvoicesApplication();
        private IEmailSender emailSender;
        private EmailManager mgr;

        public Invoice()
        {
            mgr = new EmailManager(
                  ObjectFactory.GetInstance<IEmailSender>()
                );
            emailSender = ObjectFactory.GetInstance<IEmailSender>();
        }

        public void ProcessRequest(HttpContext context)
        {
            var Request = context.Request;
            context.Response.ContentType = "application/json";
            if (UserID < 1 || UserInfo == null || UserInfo.Role == Entity.UserModel.RolesEnum.CLIENT)
            {
                context.Response.Write("[]");
                context.Response.End();
            }
            string action = context.Request.Params["action"].ToLower();
            string projectName = context.Request.Params["projectName"];
            int proposalId;
            string msg = "";
            int deleteID;
            switch (action)
            {
                case "getcompanies":
                    int companyId = int.Parse(context.Request.Params["companyId"]);
                    int projectId = int.Parse(context.Request.Params["projectId"]);
                    string data1 = GetCompanyModels(companyId, projectId);
                    context.Response.Write(data1);
                    break;
                case "getproposalinvoices":
                    int proposalid = int.Parse(context.Request.Params["proposalid"]);
                    string data2 = GetInvoiceByProposalId(proposalid);
                    context.Response.Write(data2);
                    break;
                case "editinvoice":
                    InvoiceEntity entityUpdate = GetEntity(context, out msg);
                    if (entityUpdate == null)
                        context.Response.Write(ResponseMessage.GetResponse(false, msg, 0));
                    else
                    {
                        if (entityUpdate.DueOn >= entityUpdate.SendOn || entityUpdate.DueOn == null || entityUpdate.SendOn == null)
                        {
                            bool boolResult = UpdateInvoice(entityUpdate, projectName, out msg);
                            context.Response.Write(boolResult
                                ? ResponseMessage.GetResponse(true, "", entityUpdate.ID)
                                : ResponseMessage.GetResponse(false, msg, entityUpdate.ID));
                            EditStatus(entityUpdate.ProposalId);
                        }
                        else { context.Response.Write(ResponseMessage.GetResponse(false, "The due date must be later than send date.", entityUpdate.ID)); }
                    }
                    break;
                case "addinvoice":
                    InvoiceEntity entityAdd = GetEntity(context, out msg);
                    if (entityAdd == null)
                        context.Response.Write(ResponseMessage.GetResponse(false, msg, 0));
                    else
                    {
                        if (entityAdd.DueOn >= entityAdd.SendOn || entityAdd.DueOn == null || entityAdd.SendOn == null)
                        {
                            entityAdd.ID = AddInvoice(entityAdd,projectName, out msg);
                            context.Response.Write(entityAdd.ID > 0
                                ? ResponseMessage.GetResponse(true, "", entityAdd.ID)
                                : ResponseMessage.GetResponse(false, msg, entityAdd.ID));
                            EditStatus(entityAdd.ProposalId);
                        }
                        else { context.Response.Write(ResponseMessage.GetResponse(false, "The due date must be later than send date.", entityAdd.ID)); }
                    }
                    break;
                case "delinvoice":
                    InvoiceEntity entitydel = GetEntity(context, out msg);
                    if (int.TryParse(Request["invoiceId"], out deleteID) && deleteID > 0)
                    {
                        if (DeleteInvoice(deleteID))
                            msg = ResponseMessage.GetResponse(true, "", deleteID);
                        else
                            msg = ResponseMessage.GetResponse(false, "", deleteID);
                    }
                    int.TryParse(Request["proposalId"], out proposalId);
                    EditStatus(proposalId);
                    context.Response.Write(msg);
                    break;
                case "sendemail":
                    //string projectNameItem = context.Request.Params["projectNameItem"];
                    //string milestoneItem = context.Request.Params["milestone"];
                    //string invoiceNoItem = context.Request.Params["invoiceNo"];
                    //string receiveOnItem = context.Request.Params["receiveOn"];
                    //string proposalTitle = context.Request.Params["proposalTitle"];
                    //if (receiveOnItem != null && receiveOnItem != "")
                    //{ 
                    //    string body = UtilFactory.Helpers.FileHelper.GetTemplateFileContent("SendEmailToPayment.txt");
                    //    body = body.Replace("[ProposalName]", proposalTitle);
                    //    body = body.Replace("[Milestone]", milestoneItem);
                    //    body = body.Replace("[InvoiceNo]", invoiceNoItem);
                    //    emailSender.SendMail("payment.pm@sunnet.us", Config.DefaultSendEmail, "Project Payment", body);
                    //}
                    InvoiceEntity entityUpdate1 = GetEntity(context, out msg);
                    if (entityUpdate1 == null)
                        context.Response.Write(ResponseMessage.GetResponse(false, msg, 0));
                    else
                    {
                        if (entityUpdate1.DueOn >= entityUpdate1.SendOn || entityUpdate1.DueOn == null || entityUpdate1.SendOn == null)
                        {
                            bool boolResult = UpdateInvoice(entityUpdate1, projectName, out msg);
                            context.Response.Write(boolResult
                                ? ResponseMessage.GetResponse(true, "", entityUpdate1.ID)
                                : ResponseMessage.GetResponse(false, msg, entityUpdate1.ID));
                            EditStatus(entityUpdate1.ProposalId);
                        }
                        else { context.Response.Write(ResponseMessage.GetResponse(false, "The due date must be later than send date.", entityUpdate1.ID)); }
                    }
                    //context.Response.Write(msg);
                    break;
                case "confirmpayment":
                    int invoiceId = 0;
                    int.TryParse(Request["invoiceId"], out invoiceId);
                    if(invoiceId>0)
                    {
                        if(UpdateConfirmPayment(invoiceId))
                        {
                            msg = ResponseMessage.GetResponse(true, "", invoiceId);
                        }
                        else
                        {
                            msg = ResponseMessage.GetResponse(false, "", invoiceId);
                        }
                    }
                    context.Response.Write("[]");
                    break;
                default:
                    context.Response.Write("[]");
                    break;
            }
        }

        private string GetCompanyModels(int companyId, int projectId)
        {
            
            var companys = companyApp.GetCompanyProjectModels(companyId, projectId);
            List<CompanyProjectModel> models = new List<CompanyProjectModel>();
            foreach (var company in companys)
            {
                models.Add(company.Value);
            }
            return JsonConvert.SerializeObject(models);
        }

        private string GetInvoiceByProposalId(int proposalId)
        {
            var invoices = _invoiceApp.GetInvoiceByProposalId(proposalId);
            if (invoices == null)
                return "";
            else
                return JsonConvert.SerializeObject(invoices);
        }

        private InvoiceEntity GetEntity(HttpContext context, out string msg)
        {
            msg = string.Empty;
            HttpRequest request = context.Request;
            try
            {
                InvoiceEntity entity = new InvoiceEntity();
                entity.ID = int.Parse(request.Params["id"]);
                if (entity.ID > 0)
                {
                    entity = _invoiceApp.GetInvoice(entity.ID);
                }

                entity.ProposalId = int.Parse(request.Params["proposalId"]);
                entity.Milestone = request.Params["milestone"];
                entity.InvoiceNo = request.Params["invoiceNo"];
                DateTime sendOn;
                if (DateTime.TryParse(request.Params["sendOn"], out sendOn))
                {
                    entity.SendOn = sendOn;
                }
                else
                    entity.SendOn = null;

                DateTime receiveOn;
                if (DateTime.TryParse(request.Params["receiveOn"], out receiveOn))
                {
                    entity.ReceiveOn = receiveOn;
                }
                else
                    entity.ReceiveOn = null;

                DateTime dueOn;
                if (DateTime.TryParse(request.Params["dueOn"], out dueOn))
                {
                    entity.DueOn = dueOn;
                }
                else
                    entity.DueOn = null;
                entity.Approved = request.Params["approved"] == "1";
                DateTime etaDate;
                if (DateTime.TryParse(request.Params["etaDate"], out etaDate))
                {
                    entity.ETADate = etaDate;
                }
                else
                    entity.ETADate = null;
                entity.ModifiedBy = UserInfo.UserID;
                entity.ModifiedOn = DateTime.Now;
                if (entity.InvoiceNo == "")
                {
                    entity.Color = "yellow";
                    entity.ColorFor = "InvoiceNo";
                }
                else
                {
                    entity.Color = "";
                    entity.ColorFor = "";
                }
                if(entity.InvoiceNo=="")
                {
                    entity.Status = InvoiceStatus.Missing_Invoice;
                }
                else if (entity.ReceiveOn != null && entity.ReceiveOn != null)
                {
                    entity.Status = InvoiceStatus.Payment_Received;
                }
                else if (entity.SendOn != null)
                {
                    entity.Status = InvoiceStatus.Awaiting_Payment;
                }
                else if (entity.Approved)
                {
                    entity.Status = InvoiceStatus.Awaiting_Send;
                }
                else
                {
                    entity.Status = InvoiceStatus.Invoice_Created;
                }
                return entity;
            }
            catch (Exception ex)
            {
                msg = string.Format("Input Error:{0}", ex.Message);
                return null;
            }
        }

        private int AddInvoice(InvoiceEntity entity, string projectName, out string msg)
        {
            msg = string.Empty;
            if (string.IsNullOrEmpty(entity.InvoiceNo))
                entity.Status = InvoiceStatus.Missing_Invoice;
            else
                entity.Status = InvoiceStatus.Invoice_Created;
            entity.CreatedBy = UserInfo.UserID;
            entity.CreatedOn = DateTime.Now;
            int id = _invoiceApp.AddInvoice(entity);
            ProposalInvoiceModel model = _invoiceApp.GetInvoiceModelById(id);
            if (entity.ReceiveOn != null)
            {
                string body = UtilFactory.Helpers.FileHelper.GetTemplateFileContent("SendEmailToPayment.txt");
                body = body.Replace("[ProposalName]", model.ProposalTitle);
                body = body.Replace("[Milestone]", entity.Milestone);
                body = body.Replace("[InvoiceNo]", entity.InvoiceNo);
                emailSender.SendMail("payment.pm@sunnet.us", Config.DefaultSendEmail, "Project Payment", body);
            }
            if (_invoiceApp.BrokenRuleMessages.Count > 0 || id <= 0)
            {
                msg = _invoiceApp.BrokenRuleMessages[0].Message;
            }
            return id;
        }

        private bool UpdateInvoice(InvoiceEntity entity, string projectName, out string msg)
        {
            msg = string.Empty;
            entity.ModifiedBy = UserInfo.UserID;
            entity.ModifiedOn = DateTime.Now;
            bool result = _invoiceApp.UpdateInvoice(entity);
            ProposalInvoiceModel model = _invoiceApp.GetInvoiceModelById(entity.ID);
            if (entity.ReceiveOn != null)
            {
                string body = UtilFactory.Helpers.FileHelper.GetTemplateFileContent("SendEmailToPayment.txt");
                body = body.Replace("[ProposalName]", model.ProposalTitle);
                body = body.Replace("[Milestone]", entity.Milestone);
                body = body.Replace("[InvoiceNo]", entity.InvoiceNo);
                result = emailSender.SendMail("payment.pm@sunnet.us", Config.DefaultSendEmail, "Project Payment", body);
            }
            if (_invoiceApp.BrokenRuleMessages.Count > 0)
            {
                msg = _invoiceApp.BrokenRuleMessages[0].Message;
            }
            return result;
        }

        private bool DeleteInvoice(int id)
        {
            return _invoiceApp.DeleteInvoice(id);
        }

        private void EditStatus(int id)
        {
            List<InvoiceEntity> invoices = _invoiceApp.GetInvoiceByProposalId(id);
            ProposalTrackerEntity entity = new App.ProposalTrackerApplication().Get(id);
            entity.ModifyOn = DateTime.Now;
            var newStatus = 7;//Paid/Completed
            foreach (var invoice in invoices)
            {
                if (!invoice.Approved)
                {
                    newStatus = 4;//Awaiting Development
                    break;
                }
                else if (invoice.SendOn == null || invoice.DueOn == null)
                {
                    newStatus = 5;//Awaiting Sending Invoice
                    break;
                }
                else if (invoice.ReceiveOn == null)
                {
                    newStatus = 6;//Awaiting Payment
                    break;
                }
            }
            if (entity.Status != newStatus)
            {
                entity.Status = newStatus;
                new App.ProposalTrackerApplication().UpdateProposalTracker(entity);
            }
        }

        private bool UpdateConfirmPayment(int invoiceId)
        {
            InvoiceEntity invoices = _invoiceApp.GetInvoice(invoiceId);
            invoices.Status = InvoiceStatus.Payment_Confirmed;
            return _invoiceApp.UpdateInvoice(invoices);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}