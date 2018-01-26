using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.App;
using Microsoft.Security.Application;

namespace SunNet.PMNew.Web.Sunnet.Tickets
{
    public partial class WeekPlanEdit : BaseWebsitePage
    {
        WeekPlanApplication planApp;
        protected bool IsEdit = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            planApp = new WeekPlanApplication();
            if (!IsPostBack)
            {
                BindEstimate();
                int id = QS("id", 0); //修改　plan
                int oldplan = QS("oldplan", 0); //追加本周的计划
                WeekPlanEntity weekPlanEntity;

                DateTime weekDay = DateTime.Now;
                if (weekDay.DayOfWeek != DayOfWeek.Sunday)
                    weekDay = weekDay.AddDays(-(int)DateTime.Now.DayOfWeek).AddDays(7);

                if (id == 0 && oldplan == 0) //添加下周计划
                {
                    weekPlanEntity = planApp.GetInfo(UserInfo.UserID, weekDay);
                    if (weekPlanEntity == null)
                    {
                        lblTitle.InnerText = "Add Week Plan";
                        lblStart.InnerText = weekDay.ToString("MM/dd/yyyy");
                        lblEnd.InnerText = weekDay.AddDays(6).ToString("MM/dd/yyyy");
                        IsEdit = false;

                        WeekPlanEntity old_weekPlanEntity = planApp.GetInfo(UserInfo.UserID, weekDay.AddDays(-7));
                        if (old_weekPlanEntity == null) //本周计划没有写
                            lblAddPlan.InnerHtml = string.Format("<a href='weekplanEdit.aspx?oldplan=1'>{0} ------ {1}</a>"
                            , weekDay.AddDays(-7).ToString("MM/dd/yyyy"), weekDay.AddDays(-1).ToString("MM/dd/yyyy"));
                        else
                        {//有写本周计划
                            lblAddPlan.InnerHtml = string.Format("<a href='weekplanEdit.aspx?id={2}'>{0} ------ {1}</a>"
                            , weekDay.AddDays(-7).ToString("MM/dd/yyyy"), weekDay.AddDays(-1).ToString("MM/dd/yyyy"), old_weekPlanEntity.ID);
                        }
                        BindImport();

                    }
                    else
                    {
                        BindData(weekPlanEntity);
                        SetSelectedEstimate(weekPlanEntity);
                    }
                }
                else if (id == 0 && oldplan == 1) //追加本周计划
                {
                    weekDay = weekDay.AddDays(-7);
                    lblTitle.InnerText = "Add Week Plan";
                    lblStart.InnerText = weekDay.ToString("MM/dd/yyyy");
                    lblEnd.InnerText = weekDay.AddDays(6).ToString("MM/dd/yyyy");
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
                        lblStart.InnerText = weekDay.ToString("MM/dd/yyyy");
                        lblEnd.InnerText = weekDay.AddDays(6).ToString("MM/dd/yyyy");
                        IsEdit = false;
                        BindImport();
                    }
                    else
                    {
                        BindData(weekPlanEntity);
                        SetSelectedEstimate(weekPlanEntity);
                    }
                }
                else
                {
                    lblAddPlan.InnerHtml = "<span style='color: Red;'>unauthorized access</span>";
                    btnOK.Visible = false;
                    btnSave.Visible = false;
                }

            }
        }

        private void BindImport()
        {
            foreach (WeekPlanEntity entity in planApp.GetWeekDay(UserInfo.UserID))
            {
                ddlImport.Items.Add(new ListItem()
                {
                    Value = entity.ID.ToString()
                    ,
                    Text = string.Format("{0} --- {1} ", entity.WeekDay.ToString("MM/dd/yyyy"), entity.WeekDayEnd.ToString("MM/dd/yyyy"))
                });
            }
            ddlImport.Items.Insert(0, new ListItem() { Value = "0", Text = "Please select plan" });
        }

        private void BindData(WeekPlanEntity entity)
        {
            hfID.Value = entity.ID.ToString();
            lblTitle.InnerText = "Edit Week Plan";
            lblStart.InnerText = entity.WeekDay.ToString("MM/dd/yyyy");
            lblEnd.InnerText = entity.WeekDay.AddDays(6).ToString("MM/dd/yyyy");
            txtSunday.Text = entity.Sunday.Replace("<br>", "\r\n"); ;
            txtMonday.Text = entity.Monday.Replace("<br>", "\r\n"); ;
            txtTuesday.Text = entity.Tuesday.Replace("<br>", "\r\n"); ;
            txtWednesday.Text = entity.Wednesday.Replace("<br>", "\r\n"); ;
            txtThursday.Text = entity.Thursday.Replace("<br>", "\r\n"); ;
            txtFriday.Text = entity.Friday.Replace("<br>", "\r\n"); ;
            txtSaturday.Text = entity.Saturday.Replace("<br>", "\r\n"); ;
        }



        private void BindEstimate()
        {
            List<ListItem> hours = new List<ListItem>();
            for (int i = 0; i < 9; i++)
            {
                hours.Add(new ListItem(i.ToString(), i.ToString()));
            }
            ddlMondayEst.DataSource = hours;
            ddlMondayEst.DataBind();
            ddlMondayEst.SelectedValue = "8";

            ddlTuesdayEst.DataSource = hours;
            ddlTuesdayEst.DataBind();
            ddlTuesdayEst.SelectedValue = "8";

            ddlWednesdayEst.DataSource = hours;
            ddlWednesdayEst.DataBind();
            ddlWednesdayEst.SelectedValue = "8";

            ddlThursdayEst.DataSource = hours;
            ddlThursdayEst.DataBind();
            ddlThursdayEst.SelectedValue = "8";

            ddlFridayEst.DataSource = hours;
            ddlFridayEst.DataBind();
            ddlFridayEst.SelectedValue = "8";

            ddlSaturdayEst.DataSource = hours;
            ddlSaturdayEst.DataBind();
            ddlSaturdayEst.SelectedValue = "0";

            ddlSundayEst.DataSource = hours;
            ddlSundayEst.DataBind();
            ddlSundayEst.SelectedValue = "0";

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
                DateTime weekDay = DateTime.Now;
                if (weekDay.DayOfWeek != DayOfWeek.Sunday)
                    weekDay = weekDay.AddDays(-(int)DateTime.Now.DayOfWeek).AddDays(7);

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
            entity.SundayEstimate = int.Parse(ddlSundayEst.SelectedValue);
            entity.Monday = (txtMonday.Text).Replace("\r\n", "<br>");
            entity.MondayEstimate = int.Parse(ddlMondayEst.SelectedValue);
            entity.Tuesday = (txtTuesday.Text).Replace("\r\n", "<br>");
            entity.TuesdayEstimate = int.Parse(ddlTuesdayEst.SelectedValue);
            entity.Wednesday = (txtWednesday.Text).Replace("\r\n", "<br>");
            entity.WednesdayEstimate = int.Parse(ddlWednesdayEst.SelectedValue);
            entity.Thursday = (txtThursday.Text).Replace("\r\n", "<br>");
            entity.ThursdayEstimate = int.Parse(ddlThursdayEst.SelectedValue);
            entity.Friday = (txtFriday.Text).Replace("\r\n", "<br>");
            entity.FridayEstimate = int.Parse(ddlFridayEst.SelectedValue);
            entity.Saturday = (txtSaturday.Text).Replace("\r\n", "<br>");
            entity.SaturdayEstimate = int.Parse(ddlSaturdayEst.SelectedValue);
            if (id > 0)
            {
                bool result;
                if (UserInfo.Role == RolesEnum.ADMIN || UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.Sales)
                    result = planApp.Update(entity, false);
                else
                    result = planApp.Update(entity, true);

                if (result)
                {
                    ShowSuccessMessageToClient(true, true);
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
                    ShowSuccessMessageToClient();
                }
                else
                {
                    ShowFailMessageToClient(planApp.BrokenRuleMessages);
                }
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(ddlImport.SelectedValue, out id))
            {
                if (id > 0)
                {
                    WeekPlanEntity weekPlanEntity;

                    DateTime weekDay = DateTime.Now;
                    if (weekDay.DayOfWeek != DayOfWeek.Sunday)
                        weekDay = weekDay.AddDays(-(int)DateTime.Now.DayOfWeek).AddDays(7);

                    weekPlanEntity = planApp.GetInfo(id, UserInfo.UserID);
                    if (weekPlanEntity == null)
                    {
                    }
                    else
                    {
                        weekPlanEntity.WeekDay = weekDay;
                        weekPlanEntity.CreateDate = DateTime.Now;
                        weekPlanEntity.ModifiedBy = UserInfo.UserID;
                        weekPlanEntity.ModifiedOn = DateTime.Now;
                        if ((weekPlanEntity.ID = planApp.Add(weekPlanEntity)) > 0)
                        {
                            Response.Redirect("weekplanedit.aspx?id=" + weekPlanEntity.ID);
                        }
                        else
                        {
                            ShowFailMessageToClient(planApp.BrokenRuleMessages);
                        }
                    }
                }
            }
        }
    }
}
