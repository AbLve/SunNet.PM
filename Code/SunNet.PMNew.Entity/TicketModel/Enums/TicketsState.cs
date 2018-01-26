using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.TicketModel
{
    public enum TicketsState
    {
        Draft = 0,
        Submitted = 1,
        Cancelled = 2,
        #region Inprocess
        PM_Reviewed = 3,
        #region Estimation
        
        /// <summary>
        /// 等待估时
        /// </summary>
        Waiting_For_Estimation = 4,

        /// <summary>
        /// PM 等待确认估时
        /// </summary>
        PM_Verify_Estimation = 5,

        /// <summary>
        /// Waiting Sales Confirm 变更为 Waiting Confirm
        /// PM 分配 最终确认估时的人
        /// </summary>
        Waiting_Confirm = 6,

        /// <summary>
        /// Estimation_Fail 变更为 denied
        /// 估时被否定
        /// </summary>
        Denied = 7,

        /// <summary>
        /// 估时通过
        /// </summary>
        Estimation_Approved = 8,
        #endregion
        Developing = 9,
        Testing_On_Local = 10,
        Tested_Fail_On_Local = 11,
        Tested_Success_On_Local = 12,
        Testing_On_Client = 13, //RTesting :Remote Testing 
        Tested_Fail_On_Client = 14,
        Tested_Success_On_Client = 15,
        /// <summary>
        /// PM 否认测试达到要求
        /// </summary>
        PM_Deny = 16,
        #endregion
        Ready_For_Review = 17,
        Not_Approved = 18, //ClientDeny
        Completed = 19,// ClientApp
        Internal_Cancel = 20, //Internal ticket Cancel

        #region AdditionalStates
        [Description("Waiting_for_PM's_feedback")]
        Wait_PM_Feedback = 30,
        [Description("Waiting_for_your_feedback")]
        Wait_Client_Feedback = 31,
        [Description("Waiting_for_SunNet's_feedback")]
        Wait_Sunnet_Feedback = 32

        #endregion
    }

    public enum TicketIsRead 
    {
        Read = 1,
        Unread = 0
    }

    public enum AdditionalStates
    {
        Normal = -1,
        Wait_PM_Feedback = 30,
        Wait_Client_Feedback = 31,
        Wait_Sunnet_Feedback = 32
    }


    public class TicketsStateHelper
    {
        /// <summary>
        /// 转化ClientTicketState 到 TicketsState.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>List&lt;TicketsState&gt;.</returns>
        public static List<TicketsState> Map(ClientTicketState state)
        {
            var target = new List<TicketsState>();
            switch (state)
            {
                case ClientTicketState.Cancelled:
                    target.Add(TicketsState.Cancelled);
                    target.Add(TicketsState.Internal_Cancel);
                    break;
                case ClientTicketState.Completed:
                    target.Add(TicketsState.Completed);
                    break;
                case ClientTicketState.Draft:
                    target.Add(TicketsState.Draft);
                    break;
                case ClientTicketState.Estimating:
                    target.Add(TicketsState.Waiting_For_Estimation);
                    target.Add(TicketsState.Waiting_Confirm);
                    target.Add(TicketsState.PM_Verify_Estimation);
                    break;
                case ClientTicketState.Denied:
                    target.Add(TicketsState.Denied);
                    break;
                case ClientTicketState.In_Progress:
                    target.Add(TicketsState.Estimation_Approved);
                    target.Add(TicketsState.Developing);
                    target.Add(TicketsState.Testing_On_Local);
                    target.Add(TicketsState.Tested_Fail_On_Local);
                    target.Add(TicketsState.Tested_Success_On_Local);
                    target.Add(TicketsState.Testing_On_Client);
                    target.Add(TicketsState.Tested_Success_On_Client);
                    target.Add(TicketsState.Tested_Fail_On_Client);
                    target.Add(TicketsState.PM_Deny);
                    break;
                case ClientTicketState.None:
                    target.Add(TicketsState.Submitted);
                    target.Add(TicketsState.PM_Reviewed);
                    target.Add(TicketsState.Waiting_For_Estimation);
                    target.Add(TicketsState.PM_Verify_Estimation);
                    target.Add(TicketsState.Waiting_Confirm);
                    target.Add(TicketsState.Denied);
                    target.Add(TicketsState.Estimation_Approved);
                    target.Add(TicketsState.Developing);
                    target.Add(TicketsState.Testing_On_Local);
                    target.Add(TicketsState.Tested_Fail_On_Local);
                    target.Add(TicketsState.Tested_Success_On_Local);
                    target.Add(TicketsState.Testing_On_Client);
                    target.Add(TicketsState.Tested_Fail_On_Client);
                    target.Add(TicketsState.Tested_Success_On_Client);
                    target.Add(TicketsState.PM_Deny);
                    target.Add(TicketsState.Ready_For_Review);
                    target.Add(TicketsState.Not_Approved);
                    target.Add(TicketsState.Wait_PM_Feedback);
                    target.Add(TicketsState.Wait_Sunnet_Feedback);
                    target.Add(TicketsState.Wait_Client_Feedback);
                    break;
                case ClientTicketState.Not_Approved:
                    target.Add(TicketsState.Not_Approved);
                    break;
                case ClientTicketState.Ready_For_Review:
                    target.Add(TicketsState.Ready_For_Review);
                    break;
                case ClientTicketState.Submitted:
                    target.Add(TicketsState.Submitted);
                    break;
                case ClientTicketState.Waiting_Client_Feedback:
                    target.Add(TicketsState.Wait_Client_Feedback);
                    break;
                case ClientTicketState.Waiting_Sunnet_Feedback:
                    target.Add(TicketsState.Wait_Sunnet_Feedback);
                    break;
                default:
                    break;
            }
            return target;
        }

        /// <summary>
        /// all the states will show in timesheet
        /// </summary>
        public static List<TicketsState> TimeSheetStates
        {
            get
            {
                List<TicketsState> listStatus = new List<TicketsState>();
                listStatus.Add(TicketsState.Submitted);
                listStatus.Add(TicketsState.PM_Reviewed);
                listStatus.Add(TicketsState.Waiting_For_Estimation);
                listStatus.Add(TicketsState.PM_Verify_Estimation);
                listStatus.Add(TicketsState.Waiting_Confirm);
                listStatus.Add(TicketsState.Developing);
                listStatus.Add(TicketsState.Testing_On_Local);
                listStatus.Add(TicketsState.Tested_Fail_On_Local);
                listStatus.Add(TicketsState.Tested_Success_On_Local);
                listStatus.Add(TicketsState.Testing_On_Client);
                listStatus.Add(TicketsState.Tested_Fail_On_Client);
                listStatus.Add(TicketsState.Tested_Success_On_Client);
                listStatus.Add(TicketsState.PM_Deny);
                listStatus.Add(TicketsState.Ready_For_Review);
                listStatus.Add(TicketsState.Not_Approved);
                listStatus.Add(TicketsState.Wait_Client_Feedback);
                listStatus.Add(TicketsState.Wait_PM_Feedback);
                return listStatus;
            }
        }

        public static List<TicketsState> ScheduleStates
        {
            get
            {
                List<TicketsState> listStatus = new List<TicketsState>();
                listStatus.Add(TicketsState.Submitted);
                listStatus.Add(TicketsState.PM_Reviewed);
                listStatus.Add(TicketsState.Waiting_For_Estimation);
                listStatus.Add(TicketsState.PM_Verify_Estimation);
                listStatus.Add(TicketsState.Waiting_Confirm);
                listStatus.Add(TicketsState.Denied);
                listStatus.Add(TicketsState.Developing);
                listStatus.Add(TicketsState.Testing_On_Local);
                listStatus.Add(TicketsState.Tested_Fail_On_Local);
                listStatus.Add(TicketsState.Tested_Success_On_Local);
                listStatus.Add(TicketsState.Testing_On_Client);
                listStatus.Add(TicketsState.Tested_Fail_On_Client);
                listStatus.Add(TicketsState.Tested_Success_On_Client);
                listStatus.Add(TicketsState.PM_Deny);
                listStatus.Add(TicketsState.Ready_For_Review);
                listStatus.Add(TicketsState.Not_Approved);
                listStatus.Add(TicketsState.Completed);
                return listStatus;
            }
        }

        public static List<TicketsState> SunnetSHAllowShowStatus
        {
            get
            {
                //allow status
                var statuses = new List<TicketsState>()
                {
                    TicketsState.Submitted,
                    TicketsState.Waiting_For_Estimation,
                    TicketsState.Developing,
                    TicketsState.PM_Reviewed,
                    TicketsState.PM_Verify_Estimation,
                    TicketsState.Denied,
                    TicketsState.Testing_On_Local,
                    TicketsState.Testing_On_Client,
                    TicketsState.Ready_For_Review,
                    TicketsState.Completed,
                    TicketsState.Waiting_Confirm,
                    TicketsState.Tested_Success_On_Local,
                    TicketsState.Tested_Success_On_Client,
                    TicketsState.Tested_Fail_On_Local,
                    TicketsState.Tested_Fail_On_Client,
                    TicketsState.Estimation_Approved,
                    TicketsState.Not_Approved,
                    TicketsState.PM_Deny,
                    TicketsState.Cancelled,
                    TicketsState.Internal_Cancel
                };
                return statuses;
            }
        }

        public static List<TicketsState> SunnetUSAllowShowStatus
        {
            get
            {
                //allow status
                var statuses = new List<TicketsState>()
                {
                    TicketsState.Submitted,
                    TicketsState.Waiting_For_Estimation,
                    TicketsState.Developing,
                    TicketsState.PM_Reviewed,
                    TicketsState.PM_Verify_Estimation,
                    TicketsState.Denied,
                    TicketsState.Testing_On_Local,
                    TicketsState.Testing_On_Client,
                    TicketsState.Ready_For_Review,
                    TicketsState.Completed,
                    TicketsState.Waiting_Confirm,
                    TicketsState.Tested_Success_On_Local,
                    TicketsState.Tested_Success_On_Client,
                    TicketsState.Tested_Fail_On_Local,
                    TicketsState.Tested_Fail_On_Client,
                    TicketsState.Estimation_Approved,
                    TicketsState.Not_Approved,
                    TicketsState.PM_Deny,
                    TicketsState.Cancelled,
                    TicketsState.Internal_Cancel,
                    TicketsState.Wait_Client_Feedback,
                    TicketsState.Wait_Sunnet_Feedback
                };
                return statuses;
            }
        }

        public static List<TicketsState> ClientAllowShowStatus
        {
            get
            {
                //allow status
                var statuses = new List<TicketsState>()
                {
                    TicketsState.Submitted,
                    TicketsState.PM_Reviewed,
                    TicketsState.Denied,
                    TicketsState.Estimation_Approved,
                    TicketsState.Developing,
                    TicketsState.Waiting_For_Estimation,
                    TicketsState.Testing_On_Client,
                    TicketsState.Testing_On_Local,
                    TicketsState.Tested_Fail_On_Client,
                    TicketsState.Tested_Fail_On_Local,
                    TicketsState.Tested_Success_On_Client,
                    TicketsState.Tested_Success_On_Local,
                    TicketsState.PM_Deny,
                    TicketsState.PM_Verify_Estimation,
                    TicketsState.Not_Approved,
                    TicketsState.Ready_For_Review,
                    TicketsState.Waiting_Confirm,
                    TicketsState.Completed,
                    TicketsState.Wait_Client_Feedback,
                    TicketsState.Wait_Sunnet_Feedback
                };
                return statuses;
            }
        }

        public static List<TicketsState> UnderDevelopingStatus
        {
            get
            {
                var status = new List<TicketsState>()
                {
                    TicketsState.Testing_On_Client,
                    TicketsState.Tested_Success_On_Client,
                    TicketsState.Tested_Fail_On_Client,
                    TicketsState.Tested_Fail_On_Local,
                    TicketsState.Testing_On_Local,
                    TicketsState.Tested_Success_On_Local,
                    TicketsState.Developing,
                    TicketsState.PM_Deny,
                    TicketsState.Estimation_Approved
                };
                return status;
            }
        }

        public static List<TicketsState> UnderEstimationStatus
        {
            get
            {
                var status = new List<TicketsState>()
                {
                    TicketsState.PM_Verify_Estimation,
                    TicketsState.Waiting_Confirm,
                    TicketsState.Waiting_For_Estimation
                };
                return status;

            }
        }

        public static List<TicketsState> NoneFailStates
        {
            get
            {
                var statuses = new List<TicketsState>()
                {
                    TicketsState.Submitted,
                    TicketsState.Waiting_For_Estimation,
                    TicketsState.Developing,
                    TicketsState.PM_Reviewed,
                    TicketsState.PM_Verify_Estimation,
                    TicketsState.Testing_On_Local,
                    TicketsState.Testing_On_Client,
                    TicketsState.Ready_For_Review,
                    TicketsState.Completed,
                    TicketsState.Waiting_Confirm,
                    TicketsState.Tested_Success_On_Local,
                    TicketsState.Tested_Success_On_Client,
                    TicketsState.Tested_Fail_On_Local,
                    TicketsState.Tested_Fail_On_Client,
                    TicketsState.Estimation_Approved,
                    TicketsState.Not_Approved,
                    TicketsState.PM_Deny,
                    TicketsState.Ready_For_Review,
                    TicketsState.Not_Approved,
                    TicketsState.Completed,
                    TicketsState.Wait_PM_Feedback,
                    TicketsState.Wait_Client_Feedback,
                    TicketsState.Wait_Sunnet_Feedback
                };
                return statuses;
            }
        }
    }
}
