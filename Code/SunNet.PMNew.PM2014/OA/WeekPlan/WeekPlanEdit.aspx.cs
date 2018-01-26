using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.TimeSheetModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Impl.SqlDataProvider.TimeSheet;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.OA.WeekPlan
{
    public partial class WeekPlanEdit : BasePage
    {
        WeekPlanApplication planApp;
        protected bool IsEdit = true;
        TimeSheetsRepositorySqlDataProvider tsp = new TimeSheetsRepositorySqlDataProvider();
        TicketsApplication tkapp = new TicketsApplication();
        private TimeSheetApplication tsApp = new TimeSheetApplication();

        protected void Page_Load(object sender, EventArgs e)
        {
            planApp = new WeekPlanApplication();
            if (!IsPostBack)
            {
                BindEstimate();
                int id = QS("id", 0); //修改　plan
                int oldplan = QS("oldplan", 0); //追加本周的计划
                int isEdit = QS("isEdit", 0);
                WeekPlanEntity weekPlanEntity;
                DateTime weekDay = DateTime.Now.AddDays(7);
                int weekOfDay = (int)weekDay.DayOfWeek;
                weekOfDay = weekOfDay == 0 ? 7 : weekOfDay;
                weekDay = weekDay.AddDays(-weekOfDay);

                if (id == 0 && oldplan == 0) //添加下周计划
                {
                    weekPlanEntity = planApp.GetInfo(UserInfo.UserID, weekDay.AddDays(7));
                    if (weekPlanEntity == null)
                    {
                        lblTitle.InnerText = "Add Week Plan";
                        IsEdit = false;
                    }
                    else
                    {
                        BindData(weekPlanEntity);
                        SetSelectedEstimate(weekPlanEntity);
                    }
                    lblNextWeek.InnerHtml = weekDay.AddDays(1).ToString("MM/dd/yyyy") + " ------ "
                        + weekDay.AddDays(7).ToString("MM/dd/yyyy");
                    WeekPlanEntity old_weekPlanEntity = planApp.GetInfo(UserInfo.UserID, weekDay);
                    if (old_weekPlanEntity == null) //本周计划没有写
                        lblFrontWeek.InnerHtml = string.Format("<a href='WeekPlanEdit.aspx?oldplan=1'>{0} ------ {1}</a>"
                        , weekDay.AddDays(-6).ToString("MM/dd/yyyy"), weekDay.ToString("MM/dd/yyyy"));
                    else
                    {//有写本周计划
                        lblFrontWeek.InnerHtml = string.Format("<a href='WeekPlanEdit.aspx?id={2}'>{0} ------ {1}</a>"
                        , weekDay.AddDays(-6).ToString("MM/dd/yyyy"), weekDay.ToString("MM/dd/yyyy"), old_weekPlanEntity.ID);
                    }
                    BindImport();
                }
                else if (id == 0 && oldplan == 1) //追加本周计划
                {
                    lblTitle.InnerText = "Add Week Plan";
                    lblFrontWeek.InnerHtml = weekDay.AddDays(-6).ToString("MM/dd/yyyy") + " ------ "
                        + weekDay.ToString("MM/dd/yyyy");
                    lblNextWeek.InnerHtml = string.Format("<a href='WeekPlanEdit.aspx'>{0} ------ {1}</a>"
                        , weekDay.AddDays(1).ToString("MM/dd/yyyy"), weekDay.AddDays(7).ToString("MM/dd/yyyy"));
                    IsEdit = false;
                    BindImport();
                }
                else if (id > 0 && oldplan == 0) //编辑周计划
                {
                    if (UserInfo.Role == RolesEnum.ADMIN || UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.Sales)
                        weekPlanEntity = planApp.GetInfo(id);
                    else
                        weekPlanEntity = planApp.GetInfo(id, UserInfo.UserID);

                    if (weekPlanEntity == null)
                    {
                        lblTitle.InnerText = "Add Week Plan";
                        lblFrontWeek.InnerHtml = weekDay.AddDays(-6).ToString("MM/dd/yyyy") + " ------ "
                        + weekDay.ToString("MM/dd/yyyy");
                        IsEdit = false;
                        BindImport();
                    }
                    else
                    {
                        BindData(weekPlanEntity);
                        SetSelectedEstimate(weekPlanEntity);
                    }
                    if (isEdit == 0)
                    {
                        lblNextWeek.InnerHtml = string.Format("<a href='WeekPlanEdit.aspx'>{0} ------ {1}</a>"
                        , weekDay.AddDays(1).ToString("MM/dd/yyyy"), weekDay.AddDays(7).ToString("MM/dd/yyyy"));
                    }
                }
                else
                {
                    lblNextWeek.InnerHtml = "<span style='color: Red;'>unauthorized access</span>";
                    btnSave.Visible = false;
                }

            }
            int result;
            int.TryParse(ddlImport.SelectedValue, out result);
            if (result > 0)
                IsEdit = false;
        }

        private void BindData(WeekPlanEntity entity)
        {
            hfID.Value = entity.ID.ToString();
            lblTitle.InnerText = "Edit Week Plan";
            lblFrontWeek.InnerHtml = entity.WeekDay.AddDays(-6).ToString("MM/dd/yyyy") + " ------ "
                + entity.WeekDay.ToString("MM/dd/yyyy");
            txtSunday.Text = entity.Sunday.Replace("<br>", "\r\n"); ;
            txtMonday.Text = entity.Monday.Replace("<br>", "\r\n"); ;
            txtTuesday.Text = entity.Tuesday.Replace("<br>", "\r\n"); ;
            txtWednesday.Text = entity.Wednesday.Replace("<br>", "\r\n"); ;
            txtThursday.Text = entity.Thursday.Replace("<br>", "\r\n"); ;
            txtFriday.Text = entity.Friday.Replace("<br>", "\r\n"); ;
            txtSaturday.Text = entity.Saturday.Replace("<br>", "\r\n"); ;

            mondayTicketIds.Value = entity.MondayTickets;
            tuesdayTicketIds.Value = entity.TuesdayTickets;
            wednesdayTicketIds.Value = entity.WednesdayTickets;
            thursdayTicketIds.Value = entity.ThursdayTickets;
            fridayTicketIds.Value = entity.FridayTickets;
            saturdayTicketIds.Value = entity.SaturdayTickets;
            sundayTicketIds.Value = entity.SundayTickets;
        }

        private void BindImport()
        {
            List<WeekPlanEntity> listWeekPlan = planApp.GetWeekDay(UserInfo.UserID);
            listWeekPlan = listWeekPlan.OrderByDescending(e => e.WeekDay).Take(2).ToList();
            foreach (WeekPlanEntity entity in listWeekPlan)
            {
                ddlImport.Items.Add(new ListItem()
                {
                    Value = entity.ID.ToString()
                    ,
                    Text = string.Format("{0} --- {1} ", entity.WeekDay.AddDays(-6).ToString("MM/dd/yyyy"), entity.WeekDay.ToString("MM/dd/yyyy"))
                });
            }
            ddlImport.Items.Insert(0, new ListItem() { Value = "0", Text = "Please select plan" });
        }

        private void BindEstimate()
        {
            List<ListItem> hours = new List<ListItem>();
            for (int i = 0; i < 25; i++)
            {
                hours.Add(new ListItem(i.ToString(), i.ToString()));
            }
            ddlMondayEst.DataSource = hours;
            ddlMondayEst.DataBind();
            ddlMondayEst.SelectedValue = "8";
            txtMondayRemaining.Value = "16";

            ddlTuesdayEst.DataSource = hours;
            ddlTuesdayEst.DataBind();
            ddlTuesdayEst.SelectedValue = "8";
            txtTuesdayRemaining.Value = "16";

            ddlWednesdayEst.DataSource = hours;
            ddlWednesdayEst.DataBind();
            ddlWednesdayEst.SelectedValue = "8";
            txtWednesdayRemaining.Value = "16";

            ddlThursdayEst.DataSource = hours;
            ddlThursdayEst.DataBind();
            ddlThursdayEst.SelectedValue = "8";
            txtThursdayRemaining.Value = "16";

            ddlFridayEst.DataSource = hours;
            ddlFridayEst.DataBind();
            ddlFridayEst.SelectedValue = "8";
            txtFridayRemaining.Value = "16";

            ddlSaturdayEst.DataSource = hours;
            ddlSaturdayEst.DataBind();
            ddlSaturdayEst.SelectedValue = "0";
            txtSaturdayRemaining.Value = "24";

            ddlSundayEst.DataSource = hours;
            ddlSundayEst.DataBind();
            ddlSundayEst.SelectedValue = "0";
            txtSundayRemaining.Value = "24";

        }


        private void SetSelectedEstimate(WeekPlanEntity weekPlanEntity)
        {
            ddlSundayEst.SelectedValue = weekPlanEntity.SundayEstimate.ToString();
            ddlMondayEst.SelectedValue = weekPlanEntity.MondayEstimate.ToString();
            ddlTuesdayEst.SelectedValue = weekPlanEntity.TuesdayEstimate.ToString();
            ddlWednesdayEst.SelectedValue = weekPlanEntity.WednesdayEstimate.ToString();
            ddlThursdayEst.SelectedValue = weekPlanEntity.ThursdayEstimate.ToString();
            ddlFridayEst.SelectedValue = weekPlanEntity.FridayEstimate.ToString();
            ddlSaturdayEst.SelectedValue = weekPlanEntity.SaturdayEstimate.ToString();

            //set remaining hours
            txtMondayRemaining.Value = (24 - weekPlanEntity.MondayEstimate).ToString();
            txtTuesdayRemaining.Value = (24 - weekPlanEntity.TuesdayEstimate).ToString();
            txtWednesdayRemaining.Value = (24 - weekPlanEntity.WednesdayEstimate).ToString();
            txtThursdayRemaining.Value = (24 - weekPlanEntity.ThursdayEstimate).ToString();
            txtFridayRemaining.Value = (24 - weekPlanEntity.FridayEstimate).ToString();
            txtSaturdayRemaining.Value = (24 - weekPlanEntity.SaturdayEstimate).ToString();
            txtSundayRemaining.Value = (24 - weekPlanEntity.SundayEstimate).ToString();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            WeekPlanEntity entity = new WeekPlanEntity();
            int id;
            if (int.TryParse(hfID.Value, out id))
            {
                //编辑
                entity.ID = id;
                entity.UpdateDate = DateTime.Now;
                entity.UpdateUserID = UserInfo.UserID;
            }
            else  //新建
            {
                DateTime weekDay = DateTime.Now.AddDays(7);
                int weekOfDay = (int)weekDay.DayOfWeek;
                weekOfDay = weekOfDay == 0 ? 7 : weekOfDay;
                weekDay = weekDay.AddDays(-weekOfDay).AddDays(7);

                if (QS("id", 0) == 0 && QS("oldplan", 0) == 1) // 追加本周计划
                {
                    weekDay = weekDay.AddDays(-7);
                }

                entity.WeekDay = weekDay.Date;
                entity.UserID = UserInfo.UserID;
                entity.CreateDate = DateTime.Now;
                entity.UpdateDate = DateTime.Now;
                entity.UpdateUserID = UserInfo.UserID;
                entity.IsDeleted = false;
            }

            entity.Sunday = (txtSunday.Text).Replace("\r\n", "<br>");
            entity.SundayEstimate = txtSunday.Text == "" ? 0 : int.Parse(ddlSundayEst.SelectedValue);


            entity.Monday = (txtMonday.Text).Replace("\r\n", "<br>");
            entity.MondayEstimate = txtMonday.Text == "" ? 0 : int.Parse(ddlMondayEst.SelectedValue);


            entity.Tuesday = (txtTuesday.Text).Replace("\r\n", "<br>");
            entity.TuesdayEstimate = txtTuesday.Text == "" ? 0 : int.Parse(ddlTuesdayEst.SelectedValue);

            entity.Wednesday = (txtWednesday.Text).Replace("\r\n", "<br>");
            entity.WednesdayEstimate = txtWednesday.Text == "" ? 0 : int.Parse(ddlWednesdayEst.SelectedValue);

            entity.Thursday = (txtThursday.Text).Replace("\r\n", "<br>");
            entity.ThursdayEstimate = txtThursday.Text == "" ? 0 : int.Parse(ddlThursdayEst.SelectedValue);

            entity.Friday = (txtFriday.Text).Replace("\r\n", "<br>");
            entity.FridayEstimate = txtFriday.Text == "" ? 0 : int.Parse(ddlFridayEst.SelectedValue);

            entity.Saturday = (txtSaturday.Text).Replace("\r\n", "<br>");
            entity.SaturdayEstimate = txtSaturday.Text == "" ? 0 : int.Parse(ddlSaturdayEst.SelectedValue);

            entity.MondayTickets = mondayTicketIds.Value;
            entity.TuesdayTickets = tuesdayTicketIds.Value;
            entity.WednesdayTickets = wednesdayTicketIds.Value;
            entity.ThursdayTickets = thursdayTicketIds.Value;
            entity.FridayTickets = fridayTicketIds.Value;
            entity.SaturdayTickets = saturdayTicketIds.Value;
            entity.SundayTickets = sundayTicketIds.Value;


            if (id > 0)
            {
                bool result;
                if (UserInfo.Role == RolesEnum.ADMIN || UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.Sales)
                    result = planApp.Update(entity, false);
                else
                    result = planApp.Update(entity, true);

                if (result)
                {
                    RedirectBack("Index.aspx");
                }
                else
                {
                    ShowFailMessageToClient(planApp.BrokenRuleMessages);
                }
            }
            else
            {
                if (planApp.Add(entity) > 0)
                {
                    Response.Redirect("Index.aspx?date=" + entity.WeekDay.ToString("MM/dd/yyyy"));
                }
                else
                {
                    ShowFailMessageToClient(planApp.BrokenRuleMessages);
                }
            }
        }

        protected void ddlImport_SelectedIndexChanged(object sender, EventArgs e)
        {
            WeekPlanEntity weekPlan = planApp.GetInfo(int.Parse(ddlImport.SelectedValue));
            if (weekPlan != null)
            {
                txtMonday.Text = weekPlan.Monday.Replace("<br>", "\r\n"); ;
                ddlMondayEst.SelectedValue = weekPlan.MondayEstimate.ToString();
                txtTuesday.Text = weekPlan.Tuesday.Replace("<br>", "\r\n"); ;
                ddlTuesdayEst.SelectedValue = weekPlan.TuesdayEstimate.ToString();
                txtWednesday.Text = weekPlan.Wednesday.Replace("<br>", "\r\n"); ;
                ddlWednesdayEst.SelectedValue = weekPlan.WednesdayEstimate.ToString();
                txtThursday.Text = weekPlan.Thursday.Replace("<br>", "\r\n"); ;
                ddlThursdayEst.SelectedValue = weekPlan.ThursdayEstimate.ToString();
                txtFriday.Text = weekPlan.Friday.Replace("<br>", "\r\n"); ;
                ddlFridayEst.SelectedValue = weekPlan.FridayEstimate.ToString();
                txtSaturday.Text = weekPlan.Saturday.Replace("<br>", "\r\n"); ;
                ddlSaturdayEst.SelectedValue = weekPlan.SaturdayEstimate.ToString();
                txtSunday.Text = weekPlan.Sunday.Replace("<br>", "\r\n"); ;
                ddlSundayEst.SelectedValue = weekPlan.SundayEstimate.ToString();

                //set remaining hours
                txtMondayRemaining.Value = (8 - weekPlan.MondayEstimate).ToString();
                txtTuesdayRemaining.Value = (8 - weekPlan.TuesdayEstimate).ToString();
                txtWednesdayRemaining.Value = (8 - weekPlan.WednesdayEstimate).ToString();
                txtThursdayRemaining.Value = (8 - weekPlan.ThursdayEstimate).ToString();
                txtFridayRemaining.Value = (8 - weekPlan.FridayEstimate).ToString();
                txtSaturdayRemaining.Value = (8 - weekPlan.SaturdayEstimate).ToString();
                txtSundayRemaining.Value = (8 - weekPlan.SundayEstimate).ToString();

                mondayTicketIds.Value = weekPlan.MondayTickets;
                tuesdayTicketIds.Value = weekPlan.TuesdayTickets;
                wednesdayTicketIds.Value = weekPlan.WednesdayTickets;
                thursdayTicketIds.Value = weekPlan.ThursdayTickets;
                fridayTicketIds.Value = weekPlan.FridayTickets;
                saturdayTicketIds.Value = weekPlan.SaturdayTickets;
                sundayTicketIds.Value = weekPlan.SundayTickets;
            }
        }

        private List<int> GetTickets(string ticketValue)
        {
            List<int> ticketIds = new List<int>();
            int ticketId = -1;
            foreach (string item in ticketValue.Split('_'))
            {
                if (!string.IsNullOrEmpty(item))
                {
                    if (int.TryParse(item, out ticketId))
                    {
                        if (ticketId > 0)
                        {
                            ticketIds.Add(ticketId);
                        }
                    }
                }
            }
            return ticketIds;
        }

        private void UpdateTimeSheetsByWeekPlan(DateTime weekDay)
        {
            Dictionary<DateTime, List<int>> dic_Ticket = new Dictionary<DateTime, List<int>>();
            dic_Ticket.Add(weekDay.AddDays(-6), GetTickets(mondayTicketIds.Value));
            dic_Ticket.Add(weekDay.AddDays(-5), GetTickets(tuesdayTicketIds.Value));
            dic_Ticket.Add(weekDay.AddDays(-4), GetTickets(wednesdayTicketIds.Value));
            dic_Ticket.Add(weekDay.AddDays(-3), GetTickets(thursdayTicketIds.Value));
            dic_Ticket.Add(weekDay.AddDays(-2), GetTickets(fridayTicketIds.Value));
            dic_Ticket.Add(weekDay.AddDays(-1), GetTickets(saturdayTicketIds.Value));
            dic_Ticket.Add(weekDay, GetTickets(sundayTicketIds.Value));

            if (dic_Ticket.Count > 0)
            {
                DataTable TimeSheets = tsp.GetAllTimeSheetRecord(UserInfo.UserID, weekDay.AddDays(-6), weekDay);
                List<TimeSheetsEntity> list_TimeSheets = new List<TimeSheetsEntity>();
                foreach (DataRow item in TimeSheets.Rows)
                {
                    TimeSheetsEntity TimeSheet = new TimeSheetsEntity();
                    TimeSheet.ID = (int)item["ID"];
                    TimeSheet.SheetDate = (DateTime)item["SheetDate"];
                    TimeSheet.TicketID = (int)item["TicketID"];
                    list_TimeSheets.Add(TimeSheet);
                }

                List<int> List_allTicketIds = new List<int>();
                foreach (List<int> item in dic_Ticket.Values)
                {
                    foreach (int item2 in item)
                    {
                        List_allTicketIds.Add(item2);
                    }
                }
                List<TicketsEntity> list_Tickets = tkapp.GetTicketsByIds(List_allTicketIds);

                StringBuilder sb = new StringBuilder();
                List<int> List_addIds = new List<int>(); //要添加的TicketID集合
                List<int> List_removeIds = new List<int>(); //要删除的TicketID集合

                foreach (var item in dic_Ticket)
                {
                    //获取要删除的TimeSheet的TickeID集合
                    List_removeIds.AddRange(list_TimeSheets.Where(r => r.SheetDate == item.Key)
                        .Select(r => r.TicketID).Except(item.Value));
                    if (List_removeIds.Count > 0)
                    {
                        foreach (int item_remove in List_removeIds)
                        {
                            sb = sb.AppendFormat("delete TimeSheets where SheetDate={0} and TicketID={1} and UserID={2} and IsSumbitted=0;"
                                  , item.Key, item_remove, UserInfo.UserID);
                        }
                    }

                    //获取要添加的TimeSheet的TickeID集合
                    List_addIds.AddRange(item.Value.Except(list_TimeSheets.Where(r => r.SheetDate == item.Key)
                        .Select(r => r.TicketID)));
                    if (List_addIds.Count > 0)
                    {
                        TimeSheetsEntity moveTimesheet = tsApp.GetByUserId(UserInfo.UserID, item.Key);
                        foreach (int item_add in List_addIds)
                        {
                            if (moveTimesheet == null)
                            {
                                int projectId = list_Tickets.Find(r => r.TicketID == item_add).ProjectID;
                                sb = sb.Append(" insert into TimeSheets([SheetDate],[ProjectID],[TicketID],[UserID],[Hours],[Percentage],[Description],");
                                sb = sb.Append("[IsSubmitted],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy],[IsMeeting],[EventID])");
                                sb = sb.AppendFormat("Values({0},{1},{2},{3},0,0.00,'',0,{4},{3},{4},{3},0,0)"
                                    , item.Key, projectId, item_add, UserInfo.UserID, DateTime.Now);
                            }
                            else
                            {
                                if (moveTimesheet.IsSubmitted == false)
                                {
                                    int projectId = list_Tickets.Find(r => r.TicketID == item_add).ProjectID;
                                    sb = sb.Append(" insert into TimeSheets([SheetDate],[ProjectID],[TicketID],[UserID],[Hours],[Percentage],[Description],");
                                    sb = sb.Append("[IsSubmitted],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy],[IsMeeting],[EventID])");
                                    sb = sb.AppendFormat("Values({0},{1},{2},{3},0,0.00,'',0,{4},{3},{4},{3},0,0)"
                                        , item.Key, projectId, item_add, UserInfo.UserID, DateTime.Now);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }

                if (sb.Length > 0)
                {
                    //Database db = DatabaseFactory.CreateDatabase();
                    //string sql = string.Format("update Tickets set ConfirmEstmateUserId={0} where TicketID={1}"
                    //    , userId, ticketId);
                    //using (DbCommand dbCommand = db.GetSqlStringCommand(sql))
                    //{
                    //    return db.ExecuteNonQuery(dbCommand) > 0;
                    //}
                }
            }
            else //删除当前日期内该用户所有的TimeSheets
            {

            }
        }
    }
}