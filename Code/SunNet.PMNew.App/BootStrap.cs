using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using SunNet.PMNew.Framework.Utils;

using SunNet.PMNew.Core.UserModule;
using SunNet.PMNew.Core.CompanyModule;

using SunNet.PMNew.Impl.SqlDataProvider.Company;
using SunNet.PMNew.Impl.SqlDataProvider.User;
using SunNet.PMNew.Impl.SqlDataProvider.Ticket;
using SunNet.PMNew.Impl.SqlDataProvider.Project;

using SunNet.PMNew.Core.TicketModule;
using SunNet.PMNew.Core.ProjectModule;
using SunNet.PMNew.Core.UserModule.Adapters;
using SunNet.PMNew.Core.FileModule;
using SunNet.PMNew.Impl.SqlDataProvider.File;
using SunNet.PMNew.Core.CompanyModule.Adapters;
using SunNet.PMNew.Core.TimeSheetModule;
using SunNet.PMNew.Impl.SqlDataProvider.TimeSheet;
using SunNet.PMNew.Core.SealModel;
using SunNet.PMNew.Impl.SqlDataProvider.Seal;
using SunNet.PMNew.Core.SchedulesModule.Interfaces;
using SunNet.PMNew.Impl.SqlDataProvider.Schedules;
using SunNet.PMNew.Core.ProposalTrackerModule;
using SunNet.PMNew.Impl.SqlDataProvider.ProposalTracker;
using SunNet.PMNew.Core.Log;
using SunNet.PMNew.Impl.SqlDataProvider.Log;
using SunNet.PMNew.Core.EventsModule;
using SunNet.PMNew.Impl.SqlDataProvider.Events;
using SunNet.PMNew.Core.ProjectModule.Interfaces;
using SunNet.PMNew.Core.ComplaintModel.Interfaces;
using SunNet.PMNew.Impl.SqlDataProvider.Complaint;
using SunNet.PMNew.Core.SealModel.Interfaces;

using SunNet.PMNew.Core.KPIModule;
using SunNet.PMNew.Impl.SqlDataProvider.KPI;
using SunNet.PMNew.Core.InvoiceModule.Interface;
using SunNet.PMNew.Impl.SqlDataProvider.Invoice;


namespace SunNet.PMNew.App
{
    public static class BootStrap
    {
        public static void Config()
        {
            ObjectFactory.Configure(x =>
                {
                    // basic utils
                    x.For<IEmailSender>().Singleton().Use(SNF.EmailSenderProvider);
                    x.For<ISystemDateTime>().Singleton().Use(SNF.SystemDateTimeProvider);

                    // company
                    x.For<ICompanyRepository>().Singleton().Use(new CompanysRepositorySqlDataProvider());
                    x.For<ICache<CompanyManager>>().Singleton().Use(SNF.GetCacheProvider<CompanyManager>);

                    // file
                    x.For<ICache<FilesManager>>().Singleton().Use(SNF.GetCacheProvider<FilesManager>);
                    x.For<IFilesRepository>().Singleton().Use(new FilesRepositorySqlDataProvider());
                    x.For<IDirectoryRepository>().Singleton().Use(new DirectoryRepositorySqlDataProvider());
                    x.For<IDirectoryObjectRepository>().Singleton().Use(new DirectoryObjectRepositorySqlDataProvider());
                    // project
                    x.For<ICache<ProjectManager>>().Singleton().Use(SNF.GetCacheProvider<ProjectManager>);
                    x.For<IProjectsRepository>().Singleton().Use(new ProjectsRepositorySqlDataProvider());
                    x.For<IProjectUsersRepository>().Singleton().Use(new ProjectUsersRepositorySqlDataProvider());
                    x.For<IProjectPrincipalRepository>().Singleton().Use(new ProjectPrincipalSqlDataProvider());

                    // user module
                    x.For<ICache<UserManager>>().Singleton().Use(SNF.GetCacheProvider<UserManager>);
                    x.For<IUsersRepository>().Singleton().Use(new UsersRepositoryRepositorySqlDataProvider());
                    x.For<IRolesRepository>().Singleton().Use(new RolesRepositorySqlDataProvider());
                    x.For<IModulesRepository>().Singleton().Use(new ModulesRepositorySqlDataProvider());
                    x.For<IRoleModulesRepository>().Singleton().Use(new RoleModulesRepositorySqlDataProvider());
                    x.For<IHideUserRepository>().Singleton().Use(new HideUsersRepositorySqlDataProvider());

                    //ticket user module
                    x.For<ITicketsUserRepository>().Singleton().Use(new TicketUsersRepositorySqlDataProvider());

                    // category
                    x.For<ICateGoryRepository>().Singleton().Use(new CateGoryRepositorySqlDataProvider());
                    x.For<ICateGoryTicketRepository>().Singleton().Use(new CateGoryTicketsRepositorySqlDataProvider());
                    x.For<ICache<CateGoryManager>>().Singleton().Use(SNF.GetCacheProvider<CateGoryManager>);

                    //KPI Module
                    x.For<IKPICategoryRepository>().Singleton().Use(new KPICaterogryRepositorySqlDataProvider());

                    // ticket module
                    x.For<ITicketsRepository>().Singleton().Use(new TicketsRepositorySqlDataProvider());
                    x.For<ICache<TicketsManager>>().Singleton().Use(SNF.GetCacheProvider<TicketsManager>);
                    x.For<IFeedBacksRepository>().Singleton().Use(new FeedBacksRepositorySqlDataProvider());
                    x.For<ICache<FeedBacksManager>>().Singleton().Use(SNF.GetCacheProvider<FeedBacksManager>);
                    x.For<ITicketsOrderRespository>().Singleton().Use(new TicketsOrderRepositorySqlDataProvider());
                    x.For<ITaskRespository>().Singleton().Use(new TasksRepositorySqlDataProvide());
                    x.For<ITicketEsDetailRespository>().Singleton().Use(new TicketEsDetailRepositorySqlDataProvider());

                    //ticket relation
                    x.For<ITicketsRelationRespository>().Singleton().Use(new TicketsRelationRepositorySqlDataProvider());

                    //ticket history
                    x.For<ITicketsHistoryRepository>().Singleton().Use(new TicketHistorysRepositorySqlDataProvider());

                    // timesheet
                    x.For<ICache<TimeSheetManager>>().Singleton().Use(SNF.GetCacheProvider<TimeSheetManager>);
                    x.For<ITimeSheetRepository>().Singleton().Use(new TimeSheetsRepositorySqlDataProvider());

                    x.For<IFeedBackMessagesRepository>().Singleton().Use(new FeedBackMessagesRepositorySqlDataProvider());
                    x.For<ITicketsUserRepository>().Singleton().Use(new TicketUsersRepositorySqlDataProvider());

                    //weekplan
                    x.For<IWeekPlanRepository>().Singleton().Use(new WeekPlanRepositorySqlDataProvider());

                    //Seals
                    x.For<ISealRequestsRepository>().Singleton().Use(new SealRequestsRepositorySqlDataProvider());
                    x.For<ISealsRepository>().Singleton().Use(new SealsRepositorySqlDataProvider());
                    x.For<ISealFileRepository>().Singleton().Use(new SealFilesRepositorySqlDataProvider());
                    x.For<ISealNotesRepository>().Singleton().Use(new SealNotesRepositorySqlDataProvider());
                    x.For<ISealUnionRequestsRepository>().Singleton().Use(new SealUnionRequestsRepositorySqlDataProvider());

                    //Schedules
                    x.For<ISchedulesRepository>().Singleton().Use(new SchedulesRepositorySqlDataProvide());

                    //Proposal Tracker
                    x.For<IProposalTrackerRepository>().Singleton().Use(new ProposalTrackerSqlDataProvider());
                    x.For<IProposalTrackerRelationRepository>().Singleton().Use(new ProposalTrackerRelationSqlDataProvider());
                    x.For<IProposalTrackerNoteRepository>().Singleton().Use(new ProposalTrackerNoteSqlDataProvider());

                    //Log
                    x.For<ILogRepository>().Singleton().Use(new LogRepositorySqlDataProvider());

                    //Events
                    x.For<IEventRepository>().Singleton().Use(new EventsRepositorySqlDataProvider());

                    //Events Comment
                    x.For<IEventCommentsRepository>().Singleton().Use(new EventCommentsSqlDataProvider());

                    // Knowledge share
                    x.For<SunNet.PMNew.Core.ShareModule.Interfaces.IShareRepository>()
                        .Singleton()
                        .Use(new SunNet.PMNew.Impl.SqlDataProvider.Share.ShareRepositorySqlDataProvider());
                    x.For<ICache<SunNet.PMNew.Core.ShareModule.ShareManager>>()
                        .Singleton()
                        .Use(SNF.GetCacheProvider<SunNet.PMNew.Core.ShareModule.ShareManager>);

                    // Complaints
                    x.For<ISystemRepository>().Use<SystemRepositorySqlDataProvider>();
                    x.For<IComplaintRepository>().Use<ComplaintRepositorySqlDataProvider>();
                    x.For<IComplaintHistoryRepository>().Use<ComplaintHistoryRepositorySqlDataProvider>();

                    // Work Flow
                    x.For<IWorkflowHistoryRepository>().Use<WorkflowHistoryRepositorySqlDataProvider>();
                    
                    
                });

            ObjectFactory.Configure(x =>
                {
                    // user
                    x.For<ICompanyCore>().Singleton().Use(new UserCompaniesAdapter());
                    // company
                    x.For<ICompanyUser>().Singleton().Use(new CompanyUserAdapter());
                    // project                 
                    x.For<ISearchUsers>().Use(new SearchUsersAdapter());

                    // ticket
                    x.For<IGetTicketUser>().Singleton().Use(new TicketUsersAdapter());

                    //Invoice
                    x.For<IinvoiceRepository>().Singleton().Use(new InvoiceRepositorySqlDataProvider());

                    //TSInvoiceRelation
                    x.For<ITSInvoiceRelationRpository>().Singleton().Use(new TSInvoiceRelationRepositorySqlDataProvider());
                });
        }
    }
}