﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Web;
using Microsoft.Office.Interop.Excel;
using NPOI.HSSF.Record.Formula.Functions;
using NPOI.SS.UserModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.TimeSheetModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils.Providers;
using Workbook = Microsoft.Office.Interop.Excel.Workbook;


namespace SunNet.PMNew.PM2014.Codes
{
    public class ExcelReport
    {
        public class Compairint : IEqualityComparer<TimeSheetTicket>
        {

            #region IEqualityComparer<TimeSheetTicket> Members

            public bool Equals(TimeSheetTicket x, TimeSheetTicket y)
            {
                return x.ProjectID == y.ProjectID;
            }

            public int GetHashCode(TimeSheetTicket obj)
            {
                return obj.ProjectID.ToString().GetHashCode();
            }

            #endregion
        }
        public class CompairintTickets : IEqualityComparer<TicketsEntity>
        {

            #region IEqualityComparer<TicketsEntity> Members

            public bool Equals(TicketsEntity x, TicketsEntity y)
            {
                return x.ProjectID == y.ProjectID;
            }

            public int GetHashCode(TicketsEntity obj)
            {
                return obj.ProjectID.ToString().GetHashCode();
            }

            #endregion
        }
        public class CompairintUsers : IEqualityComparer<TimeSheetTicket>
        {

            #region IEqualityComparer<TimeSheetTicket> Members

            public bool Equals(TimeSheetTicket x, TimeSheetTicket y)
            {
                return x.UserID == y.UserID;
            }

            public int GetHashCode(TimeSheetTicket obj)
            {
                return obj.UserID.ToString().GetHashCode();
            }

            #endregion
        }

        Microsoft.Office.Interop.Excel.Application app = new Application();

        Workbook wb;
        Worksheet ws;
        object True = true;
        object False = false;


        private void RangeSetting(Range rng, string fontName, int fontSize, bool isBold, XlHAlign hAlignment, XlVAlign vAlignment, string text, int colorIndex)
        {
            rng.Font.Name = fontName;
            rng.Font.Size = fontSize;
            rng.Font.Bold = isBold ? 1 : 0;

            rng.Interior.ColorIndex = colorIndex;
            rng.HorizontalAlignment = hAlignment;
            rng.VerticalAlignment = vAlignment;
            rng.Value2 = text;
        }

        private void RangeSetting(Range rng, string text, bool isBold, bool isDate)
        {
            rng.Font.Name = "Arial";
            rng.Font.Size = 9;
            rng.Font.Bold = isBold ? 1 : 0;

            rng.Interior.ColorIndex = 2;
            rng.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            rng.VerticalAlignment = XlVAlign.xlVAlignCenter;
            if (text.StartsWith("="))
            {
                text = "'" + text;
            }
            rng.Value2 = text;
            if (isDate)
                rng.NumberFormatLocal = "mm/dd/yyyy";
        }

        private object DefaultColor
        {
            get { return ColorTranslator.ToOle(Color.FromArgb(102, 102, 102)); }
        }

        //private delegate List<ClientInfo> AsyncGetDelegate(List<Guid> gIds);

        private string GenerateReport(List<TimeSheetTicket> tss, List<TicketsEntity> tn, UsersEntity user)
        {
            tss = tss.OrderBy(ts => ts.SheetDate).ToList<TimeSheetTicket>();

            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Template\\MyTimesheetTemplate.xlsx";
            wb = app.Workbooks.Open(filePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value
                , Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            //wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            ws = (Worksheet)wb.Worksheets[1];

            int cellRowStart = 8;

            #region replace username
            Range rng = ws.Range[ws.Cells[1, 1], ws.Cells[999, 20]];
            object tmp = "{UserName}";
            Range userRange = rng.Find(tmp, Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSearchDirection.xlNext, Missing.Value, Missing.Value, Missing.Value);
            if (userRange != null)
            {
                userRange.Value2 = "SunNet - " + user.FirstName;
                userRange.Font.Color = this.DefaultColor;
            }
            #endregion

            if (tss.Count != 0)
            {
                //get what will show on client range
                IEnumerable<TimeSheetTicket> wholeSheets = tss.Distinct<TimeSheetTicket>(new Compairint());
                string clientTmp = string.Empty;
                var q = from w in wholeSheets select w.ProjectID;
                List<Int32> projectIDs = q.Distinct().ToList<Int32>();

                TimeSheetTicket startTimeSheetTicket = tss.First();
                TimeSheetTicket endTimeSheetTicket = tss.Last();
                decimal totalHours = decimal.Zero;
                for (DateTime dt = startTimeSheetTicket.SheetDate; dt <= endTimeSheetTicket.SheetDate; dt = dt.AddDays(1))
                {
                    int totalDataRows = 0; // calculate total rows of all tickets
                    //get sheets in this day
                    List<TimeSheetTicket> dailySheets = new List<TimeSheetTicket>();
                    dailySheets = tss.FindAll(ts => ts.SheetDate == dt);
                    //get distinct count of projects in this sheets
                    IEnumerable<TimeSheetTicket> distinct = dailySheets.Distinct<TimeSheetTicket>(new Compairint());

                    int count = distinct.Count();
                    //merge date column caused by the counts of projects in this day
                    Range dateRange;
                    Range projectRange;
                    Range rngTicket;
                    //when user have multiple project in one day
                    dateRange = (Range)ws.Cells[cellRowStart, 2];
                    if (count > 1)
                    {
                        //dateRange = ws.Range[ws.Cells[cellRowStart, 2], ws.Cells[cellRowStart + count - 1, 2]];
                        // dateRange.Merge(False);
                    }
                    else
                    {
                        dateRange = (Range)ws.Cells[cellRowStart, 2];
                    }

                    int i = 0;
                    int projectRowIndex = cellRowStart;
                    foreach (TimeSheetTicket p in distinct)
                    {
                        //set project title
                        int projectTotalRows = 0;
                        projectRange = (Range)ws.Cells[cellRowStart + i, 3];
                        RangeSetting(projectRange, p.ProjectTitle, false, false);
                        projectRange.Font.Color = this.DefaultColor;
                        //get ticket column data from daily data
                        List<TimeSheetTicket> sheetOnTickets = dailySheets.FindAll(ds => ds.ProjectID == p.ProjectID);

                        //get ticket column of the project
                        // rngTicket = (Range)ws.Cells[cellRowStart + i, 4];
                        StringBuilder ticketColumntext = new StringBuilder();
                        decimal hoursSpent = decimal.Zero;
                        int j = 1;
                        int ticketIndex = 0;
                        foreach (TimeSheetTicket ts in sheetOnTickets)
                        {
                            totalDataRows++;
                            projectTotalRows++;
                            //ticketColumntext.Append("\t\n");
                            j++;
                            Range ticketCode = (Range)ws.Cells[projectRowIndex + ticketIndex, 4];
                            Range ticketTitle = (Range)ws.Cells[projectRowIndex + ticketIndex, 5];
                            Range ticketDescription = (Range)ws.Cells[projectRowIndex + ticketIndex, 6];
                            Range ticketWorkDetail = (Range)ws.Cells[projectRowIndex + ticketIndex, 7];

                            totalHours += ts.Hours;
                            hoursSpent += ts.Hours;

                            RangeSetting(ticketCode, ts.TicketID.ToString(), false, false);
                            ticketCode.Font.Color = this.DefaultColor;

                            RangeSetting(ticketTitle, ts.TicketTitle, false, false);
                            ticketTitle.Font.Color = this.DefaultColor;

                            RangeSetting(ticketDescription, ts.TicketDescription, false, false);
                            ticketDescription.Font.Color = this.DefaultColor;

                            RangeSetting(ticketWorkDetail, ts.WorkDetail, false, false);
                            ticketWorkDetail.Font.Color = this.DefaultColor;
                            i++;
                            ticketIndex++;
                        }
                        if (projectTotalRows > 0)
                        {
                            projectRange = ws.Range[ws.Cells[projectRowIndex, 3], ws.Cells[projectRowIndex + projectTotalRows - 1, 3]];
                            projectRange.Merge(False);

                            for (int index = 8; index <= 11; index++)
                            {
                                projectRange = ws.Range[ws.Cells[projectRowIndex, index], ws.Cells[projectRowIndex + projectTotalRows - 1, index]];
                                projectRange.Merge(False);
                            }

                        }

                        // ticketColumntext.Remove(ticketColumntext.Length - 2, 2);
                        //set ticket column text
                        // RangeSetting(rngTicket, ticketColumntext.ToString(), false, false);
                        //   rngTicket.Font.Color = this.DefaultColor;
                        //set hour column 
                        Range rngHours = (Range)ws.Cells[projectRowIndex, 9];
                        RangeSetting(rngHours, hoursSpent.ToString("#,#0.00"), false, false);
                        rngHours.Font.Color = this.DefaultColor;
                        rngHours.NumberFormat = "#,#0.00";

                        projectRowIndex = projectRowIndex + projectTotalRows;
                    }
                    if (totalDataRows > 0)
                    {
                        dateRange = ws.Range[ws.Cells[cellRowStart, 2], ws.Cells[cellRowStart + totalDataRows - 1, 2]];
                        dateRange.Merge(False);
                    }

                    //set date column

                    RangeSetting(dateRange, dt.ToString("MM/dd/yyyy"), false, true);
                    dateRange.Font.Color = this.DefaultColor;
                    cellRowStart = cellRowStart + i;
                }
                #region end

                for (int k = 8; k < cellRowStart; k++)
                {
                    Range dataRange = (Range)ws.Range["B" + k.ToString(), "K" + k.ToString()];
                    if (k % 2 == 0)
                    {
                        dataRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(229, 235, 247));
                    }
                    else
                    {
                        dataRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(240, 243, 250));
                    }
                }

                Range wholeRange = (Range)ws.Range["B8", "K" + cellRowStart.ToString()];
                if (tss.Count > 0)
                {
                    Range totalHoursName = (Range)ws.Cells[cellRowStart, 8];
                    RangeSetting(totalHoursName, "Total Hours:", true, false);
                    totalHoursName.Font.Color = ColorTranslator.ToOle(Color.FromArgb(0, 0, 102));

                    Range totalHoursColumn = (Range)ws.Cells[cellRowStart, 9];
                    //RangeSetting(totalHoursColumn, totalHours.ToString("f1"), true, false);

                    RangeSetting(totalHoursColumn, string.Format("=SUM(I8:I{0})", cellRowStart - 1), true, false);

                    totalHoursColumn.NumberFormat = "#,#0.00";
                    totalHoursColumn.Font.Color = ColorTranslator.ToOle(Color.FromArgb(0, 0, 102));

                    Range hourSameRowRange = (Range)ws.Range["B" + cellRowStart.ToString(), "D" + cellRowStart.ToString()];
                    hourSameRowRange.Merge(False);

                    hourSameRowRange = (Range)ws.Range["J" + cellRowStart.ToString(), "K" + cellRowStart.ToString()];
                    hourSameRowRange.Merge(False);

                    hourSameRowRange = (Range)ws.Range["B" + cellRowStart.ToString(), "K" + cellRowStart.ToString()];


                    hourSameRowRange.Borders.Color = ColorTranslator.ToOle(Color.FromArgb(127, 127, 127));
                    hourSameRowRange.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                    hourSameRowRange.Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlContinuous;
                    hourSameRowRange.Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                    hourSameRowRange.Borders.get_Item(XlBordersIndex.xlInsideHorizontal).LineStyle = XlLineStyle.xlContinuous;
                    hourSameRowRange.Borders.get_Item(XlBordersIndex.xlInsideVertical).LineStyle = XlLineStyle.xlContinuous;
                    hourSameRowRange.Borders.get_Item(XlBordersIndex.xlEdgeLeft).LineStyle = XlLineStyle.xlContinuous;
                    hourSameRowRange.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    hourSameRowRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(255, 159, 87));


                    wholeRange.EntireRow.AutoFit();
                    //wholeRange.EntireColumn.AutoFit();
                }
                for (Char a = 'B'; a <= 'K'; a = Convert.ToChar(Convert.ToInt16(a) + 1))
                {
                    Range tmpRange = (Range)ws.Range[a.ToString() + "8", a.ToString() + (cellRowStart - 1).ToString()];
                    tmpRange.Borders.Color = ColorTranslator.ToOle(Color.FromArgb(127, 127, 127));
                    tmpRange.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlInsideHorizontal).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlInsideVertical).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlEdgeLeft).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    if (a != 'E' && a != 'F' && a != 'G')
                    {
                        //tmpRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(207, 207, 207));
                        tmpRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    }
                    else
                    {

                        tmpRange.HorizontalAlignment = XlHAlign.xlHAlignLeft;
                    }
                }


                if (projectIDs.Count > 0)
                {
                    Range topRange = (Range)ws.Range["I5", "K5"];
                    //clients = pGetDelegate.EndInvoke(ar);
                    Range clientRange = topRange.Find("{Client}", Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSearchDirection.xlNext, Missing.Value, Missing.Value, Missing.Value);

                    //var q1 = (from cp in clients select cp.ClientName).Distinct();
                    //if (q1.Count() == 1)
                    //{
                    //    if (clientRange != null)
                    //    {
                    //        RangeSetting(clientRange, q1.First(), false, false);
                    //        clientRange.HorizontalAlignment = XlHAlign.xlHAlignLeft;
                    //        clientRange.Font.Color = this.DefaultColor;
                    //    }
                    //}
                    //else
                    //{
                    RangeSetting(clientRange, "", false, false);
                    Range rngClientName = topRange.Find("Client:", Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSearchDirection.xlNext, Missing.Value, Missing.Value, Missing.Value);
                    // clientRange.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlLineStyleNone;
                    topRange.Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlLineStyleNone;
                    RangeSetting(rngClientName, "", false, false);
                    //}
                }
            }

                #endregion

            //插入Ticket报表
            if (tn != null)
            {
                //添加Ticket表头
                int ticket_cellRowStart = cellRowStart + 4;
                Range ticketTitleRange;

                ticketTitleRange = (Range)ws.Cells[ticket_cellRowStart, 2];
                RangeSetting(ticketTitleRange, "Complete Date", false, false);

                ticketTitleRange = (Range)ws.Cells[ticket_cellRowStart, 3];
                RangeSetting(ticketTitleRange, "Project Name", false, false);

                ticketTitleRange = (Range)ws.Cells[ticket_cellRowStart, 4];
                RangeSetting(ticketTitleRange, "Ticket ID", false, false);

                ticketTitleRange = (Range)ws.Cells[ticket_cellRowStart, 5];
                RangeSetting(ticketTitleRange, "Ticket Title", false, false);

                ticketTitleRange = (Range)ws.Cells[ticket_cellRowStart, 6];
                RangeSetting(ticketTitleRange, "Description", false, false);

                ticketTitleRange = (Range)ws.Cells[ticket_cellRowStart, 7];
                RangeSetting(ticketTitleRange, "Estimated Time", false, false);

                BindTitleStyle("B", ticket_cellRowStart.ToString(), "G", ticket_cellRowStart.ToString());

                ticket_cellRowStart++;

                if (tn.Count > 0)
                {
                    tn = tn.OrderBy(t => t.ModifiedOn).ToList();

                    //添加内容
                    TicketsEntity startTicket = tn.First();
                    TicketsEntity endTicket = tn.Last();
                    int DateRows = ticket_cellRowStart;
                    int ticketIndex = ticket_cellRowStart;
                    for (DateTime dt = startTicket.ModifiedOn; dt <= endTicket.ModifiedOn; dt = dt.AddDays(1))
                    {
                        List<TicketsEntity> Tickets = new List<TicketsEntity>();
                        Tickets = tn.FindAll(ts => ts.ModifiedOn.Date == dt.Date);
                        if (Tickets == null || Tickets.Count <= 0)
                            continue;

                        Range dateRange;
                        dateRange = (Range)ws.Cells[DateRows, 2];
                        RangeSetting(dateRange, dt.ToString("MM/dd/yyyy"), false, true);
                        dateRange.Font.Color = this.DefaultColor;

                        //daily project  
                        List<TicketsEntity> projectDistinct = Tickets.Distinct<TicketsEntity>(new CompairintTickets()).ToList<TicketsEntity>();
                        int projectRowIndex = DateRows;
                        foreach (TicketsEntity ts in projectDistinct)
                        {
                            Range projectTitleRange;

                            projectTitleRange = (Range)ws.Cells[projectRowIndex, 3];
                            RangeSetting(projectTitleRange, ts.ProjectTitle, false, false);
                            projectTitleRange.Font.Color = this.DefaultColor;

                            List<TicketsEntity> userSheetsInOneday = Tickets.FindAll(tx => tx.ProjectID == ts.ProjectID);

                            foreach (TicketsEntity tu in userSheetsInOneday)
                            {
                                //ticketColumntext.Append("\t\n"); 
                                Range ticketCode = (Range)ws.Cells[ticketIndex, 4];
                                Range ticketTitle = (Range)ws.Cells[ticketIndex, 5];
                                Range ticketDescription = (Range)ws.Cells[ticketIndex, 6];
                                Range ticketEstimate = (Range)ws.Cells[ticketIndex, 7];

                                RangeSetting(ticketCode, tu.TicketID.ToString(), false, false);
                                ticketCode.Font.Color = this.DefaultColor;

                                RangeSetting(ticketTitle, tu.Title, false, false);
                                ticketTitle.Font.Color = this.DefaultColor;

                                RangeSetting(ticketDescription, tu.FullDescription, false, false);
                                ticketDescription.Font.Color = this.DefaultColor;

                                RangeSetting(ticketEstimate, tu.FinalTime.ToString(), false, false);
                                ticketEstimate.Font.Color = this.DefaultColor;
                                ticketIndex++;
                            }
                            projectTitleRange = (Range)ws.Range[ws.Cells[projectRowIndex, 3], ws.Cells[ticketIndex - 1, 3]];
                            projectTitleRange.Merge(False);
                            projectRowIndex = ticketIndex;
                        }

                        dateRange = (Range)ws.Range[ws.Cells[DateRows, 2], ws.Cells[ticketIndex - 1, 2]];
                        dateRange.Merge(False);
                        DateRows = ticketIndex;
                    }
                    char[] leftItems = new char[] { 'E', 'F' };
                    BindContentStyle('B', ticket_cellRowStart.ToString(), 'G', (DateRows - 1).ToString(), leftItems);
                }
            }

            return SaveExcel(ws, wb);
        }

        private string SaveExcel(Worksheet ws, Workbook wb)
        {//XlFileFormat.xlOpenXMLWorkbook
            string fileSavePath = AppDomain.CurrentDomain.BaseDirectory + "Template\\" + Guid.NewGuid().ToString() + ".xlsx";

            ws.SaveAs(fileSavePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            wb.Close(Missing.Value, Missing.Value, Missing.Value);

            app.Workbooks.Close();
            app.Quit();
            return fileSavePath;
        }

        protected string GetOutputFileName(List<TimeSheetTicket> tss, UsersEntity user, string projectTitle, DateTime eDate)
        {
            if (tss.Count == 0)
                return user.FirstName + "_TimeSheet.xlsx";
            TimeSheetTicket ts = tss[0];
            string pn = projectTitle.Replace(" ", "_");
            string ed = "All";
            string fn = user.FirstName;
            if (eDate <= new DateTime(1800, 1, 1))
                ed = eDate.ToString("MMddyyyy");
            return pn + "_" + ed + "_" + fn + ".xlsx";
        }

        public void Generate(List<TimeSheetTicket> tss, List<TicketsEntity> tn, UsersEntity user, string projectTitle, DateTime eDate)
        {
            string fileName = string.Empty;
            string outputFileName = string.Empty;
            if (user == null)
            {
                if (!string.IsNullOrEmpty(projectTitle) && projectTitle == "All")
                    fileName = GenerateSearchResultReport(tss, tn);
                else
                {
                    foreach (TimeSheetTicket timeSheetTicket in tss)
                    {
                        decimal hours = timeSheetTicket.Hours;
                        decimal integerPart = Decimal.Floor(hours);
                        if (!Decimal.Equals(integerPart, hours))
                        {
                            timeSheetTicket.Hours = integerPart + 1;
                        }
                    }
                    fileName = GenerateSearchProjectResultReport(tss, tn, projectTitle);
                }
                outputFileName = "TimeSheet.xlsx";
            }
            else
            {

                //if (!string.IsNullOrEmpty(projectTitle) && projectTitle == "All")
                //    fileName = GenerateReport(tss, user);
                //elsem
                //    fileName = GenerateUserProjectReport(tss, user,projectTitle);
                fileName = GenerateReport(tss, tn, user);

                outputFileName = GetOutputFileName(tss, user, projectTitle, eDate);//user.FirstName + " " + user.LastName + "'s TimeSheetTicket.xlsx";
            }
            int appInt = GC.GetGeneration(app);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(ws);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
            ws = null;

            wb = null;
            app = null;
            GC.Collect(appInt);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            MemoryStream ms = new MemoryStream(File.ReadAllBytes(fileName));
            try
            {
                File.Delete(fileName);
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(ex);
            }

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.Buffer = false;
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;

            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + outputFileName);
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            byte[] bytes = ms.ToArray();
            HttpContext.Current.Response.AddHeader("Content-Length", bytes.Length.ToString());

            HttpContext.Current.Response.OutputStream.Write(bytes, 0, bytes.Length);
            HttpContext.Current.Response.OutputStream.Flush();
            HttpContext.Current.Response.End();

        }

        private string GenerateSearchResultReport(List<TimeSheetTicket> tss, List<TicketsEntity> tn)
        {
            tss = tss.OrderBy(ts => ts.SheetDate).ToList<TimeSheetTicket>();
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Template\\AllTemplate.xlsx";
            wb = app.Workbooks.Open(filePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value
                , Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            ws = (Worksheet)wb.Worksheets[1];

            int cellRowStart = 8;
            decimal totalHours = decimal.Zero;
            if (tss.Count != 0)
            {
                TimeSheetTicket startTimeSheetTicket = tss.First();
                TimeSheetTicket endTimeSheetTicket = tss.Last();
                int DateRows = cellRowStart;
                int ticketIndex = cellRowStart;
                for (DateTime dt = startTimeSheetTicket.SheetDate; dt <= endTimeSheetTicket.SheetDate; dt = dt.AddDays(1))
                {
                    List<TimeSheetTicket> dailySheets = new List<TimeSheetTicket>();
                    dailySheets = tss.FindAll(ts => ts.SheetDate == dt);
                    if (dailySheets == null || dailySheets.Count <= 0)
                        continue;

                    int cellRowStartInner = cellRowStart;

                    Range dateRange;
                    dateRange = (Range)ws.Cells[DateRows, 2];
                    RangeSetting(dateRange, dt.ToString("MM/dd/yyyy"), false, true);
                    dateRange.Font.Color = this.DefaultColor;
                    // cellRowStart = cellRowStartInner;


                    //daily project  
                    List<TimeSheetTicket> projectDistinct = dailySheets.Distinct<TimeSheetTicket>(new Compairint()).ToList<TimeSheetTicket>();
                    int projectRowIndex = cellRowStart;
                    foreach (TimeSheetTicket ts in projectDistinct)
                    {
                        Range projectTitleRange;
                        List<TimeSheetTicket> dailyUser = new List<TimeSheetTicket>();
                        dailyUser = dailySheets.FindAll(tv => tv.ProjectID == ts.ProjectID).Distinct<TimeSheetTicket>(new CompairintUsers()).ToList<TimeSheetTicket>();

                        int i = cellRowStart;
                        int projectBeforeMerge = cellRowStartInner;

                        projectTitleRange = (Range)ws.Cells[projectRowIndex, 3];
                        RangeSetting(projectTitleRange, ts.ProjectTitle, false, false);
                        projectTitleRange.Font.Color = this.DefaultColor;

                        int userRowIndex = cellRowStart;
                        foreach (TimeSheetTicket tUser in dailyUser)
                        {
                            List<TimeSheetTicket> userSheetsInOneday = dailySheets.FindAll(tx => tx.UserID == tUser.UserID && tx.ProjectID == ts.ProjectID);

                            decimal dailyUserProjectHours = decimal.Zero;

                            Range userNameRange = (Range)ws.Cells[userRowIndex, 8];
                            RangeSetting(userNameRange, string.Format("{0} {1}", tUser.FirstName, tUser.LastName), false, false);
                            userNameRange.Font.Color = this.DefaultColor;
                            // Range userTickets = (Range)ws.Cells[cellRowStartInner, 4];
                            string tmp = string.Empty;

                            int j = 1;

                            foreach (TimeSheetTicket tu in userSheetsInOneday)
                            {
                                dailyUserProjectHours += tu.Hours;
                                totalHours += tu.Hours;
                                j++;
                                //ticketColumntext.Append("\t\n"); 
                                Range ticketCode = (Range)ws.Cells[ticketIndex, 4];
                                Range ticketTitle = (Range)ws.Cells[ticketIndex, 5];
                                Range ticketDescription = (Range)ws.Cells[ticketIndex, 6];
                                Range ticketWorkDetail = (Range)ws.Cells[ticketIndex, 7];

                                RangeSetting(ticketCode, tu.TicketID.ToString(), false, false);
                                ticketCode.Font.Color = this.DefaultColor;

                                RangeSetting(ticketTitle, tu.TicketTitle, false, false);
                                ticketTitle.Font.Color = this.DefaultColor;

                                RangeSetting(ticketDescription, tu.TicketDescription, false, false);
                                ticketDescription.Font.Color = this.DefaultColor;

                                RangeSetting(ticketWorkDetail, tu.WorkDetail, false, false);
                                ticketWorkDetail.Font.Color = this.DefaultColor;
                                i++;
                                ticketIndex++;

                            }
                            cellRowStart = ticketIndex;
                            Range hourTmp = (Range)ws.Cells[userRowIndex, 9];
                            RangeSetting(hourTmp, dailyUserProjectHours.ToString("#,#0.00"), false, false);
                            hourTmp.Font.Color = this.DefaultColor;
                            hourTmp.NumberFormat = "#,#0.00";

                            userNameRange = (Range)ws.Range[ws.Cells[userRowIndex, 8], ws.Cells[ticketIndex - 1, 8]];
                            userNameRange.Merge(False);

                            hourTmp = (Range)ws.Range[ws.Cells[userRowIndex, 9], ws.Cells[ticketIndex - 1, 9]];
                            hourTmp.Merge(False);

                            userNameRange = (Range)ws.Range[ws.Cells[userRowIndex, 10], ws.Cells[ticketIndex - 1, 10]];
                            userNameRange.Merge(False);
                            userNameRange = (Range)ws.Range[ws.Cells[userRowIndex, 11], ws.Cells[ticketIndex - 1, 11]];
                            userNameRange.Merge(False);

                            userRowIndex = ticketIndex;
                        }
                        projectTitleRange = (Range)ws.Range[ws.Cells[projectRowIndex, 3], ws.Cells[ticketIndex - 1, 3]];
                        projectTitleRange.Merge(False);
                        projectRowIndex = ticketIndex;
                    }

                    dateRange = (Range)ws.Range[ws.Cells[DateRows, 2], ws.Cells[ticketIndex - 1, 2]];
                    dateRange.Merge(False);
                    DateRows = ticketIndex;
                }



                #region end

                Range dataRange = (Range)ws.Range["B8", "K" + cellRowStart.ToString()];
                dataRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(229, 235, 247));
                for (int k = 8; k < cellRowStart; k++)
                {

                    if (k % 2 == 0)
                    {
                        dataRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(229, 235, 247));
                    }
                    else
                    {
                        dataRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(240, 243, 250));
                    }
                }


                Range wholeRange = (Range)ws.Range["B8", "K" + cellRowStart.ToString()];
                if (tss.Count > 0)
                {
                    Range totalHoursName = (Range)ws.Cells[cellRowStart, 8];
                    RangeSetting(totalHoursName, "Total Hours:", true, false);
                    totalHoursName.Font.Color = ColorTranslator.ToOle(Color.FromArgb(0, 0, 102));
                    //totalHoursName.Borders.Color = ColorTranslator.ToOle(Color.FromArgb(0, 0, 102));


                    Range totalHoursColumn = (Range)ws.Cells[cellRowStart, 9];
                    //RangeSetting(totalHoursColumn, totalHours.ToString("f1"), true, false);
                    RangeSetting(totalHoursColumn, string.Format("=SUM(I8:I{0})", cellRowStart - 1), true, false);

                    totalHoursColumn.Font.Color = ColorTranslator.ToOle(Color.FromArgb(0, 0, 102));
                    totalHoursColumn.NumberFormat = "#,#0.00";

                    Range hourSameRowRange = (Range)ws.Range["B" + cellRowStart.ToString(), "G" + cellRowStart.ToString()];
                    hourSameRowRange.Merge(False);



                    hourSameRowRange = (Range)ws.Range["B" + cellRowStart.ToString(), "K" + cellRowStart.ToString()];


                    hourSameRowRange.Borders.Color = ColorTranslator.ToOle(Color.FromArgb(127, 127, 127));
                    hourSameRowRange.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                    hourSameRowRange.Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlContinuous;
                    hourSameRowRange.Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                    hourSameRowRange.Borders.get_Item(XlBordersIndex.xlInsideHorizontal).LineStyle = XlLineStyle.xlContinuous;
                    hourSameRowRange.Borders.get_Item(XlBordersIndex.xlInsideVertical).LineStyle = XlLineStyle.xlContinuous;
                    hourSameRowRange.Borders.get_Item(XlBordersIndex.xlEdgeLeft).LineStyle = XlLineStyle.xlContinuous;
                    hourSameRowRange.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    hourSameRowRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(255, 159, 87));


                    wholeRange.EntireRow.AutoFit();
                }
                for (Char a = 'B'; a <= 'K'; a = Convert.ToChar(Convert.ToInt16(a) + 1))
                {
                    Range tmpRange = (Range)ws.Range[a.ToString() + "8", a.ToString() + (cellRowStart - 1).ToString()];
                    tmpRange.Borders.Color = ColorTranslator.ToOle(Color.FromArgb(127, 127, 127));
                    tmpRange.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlInsideHorizontal).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlInsideVertical).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlEdgeLeft).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    if (a != 'E' && a != 'F' && a != 'G')
                    {
                        //tmpRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(207, 207, 207));
                        tmpRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    }
                    else
                    {

                        tmpRange.HorizontalAlignment = XlHAlign.xlHAlignLeft;
                    }
                }
            }
                #endregion

            #region 插入Ticket报表

            if (tn != null)
            {
                //添加Ticket表头
                int ticket_cellRowStart = cellRowStart + 4;

                Range ticketTitleRange;
                ticketTitleRange = (Range)ws.Cells[ticket_cellRowStart, 2];
                RangeSetting(ticketTitleRange, "Complete Date", false, false);

                ticketTitleRange = (Range)ws.Cells[ticket_cellRowStart, 3];
                RangeSetting(ticketTitleRange, "Project Name", false, false);

                ticketTitleRange = (Range)ws.Cells[ticket_cellRowStart, 4];
                RangeSetting(ticketTitleRange, "Ticket ID", false, false);

                ticketTitleRange = (Range)ws.Cells[ticket_cellRowStart, 5];
                RangeSetting(ticketTitleRange, "Ticket Title", false, false);

                ticketTitleRange = (Range)ws.Cells[ticket_cellRowStart, 6];
                RangeSetting(ticketTitleRange, "Description", false, false);

                ticketTitleRange = (Range)ws.Cells[ticket_cellRowStart, 7];
                RangeSetting(ticketTitleRange, "Estimated Time", false, false);

                BindTitleStyle("B", ticket_cellRowStart.ToString(), "G", ticket_cellRowStart.ToString());

                ticket_cellRowStart++;

                if (tn.Count > 0)
                {
                    tn = tn.OrderBy(t => t.ModifiedOn).ToList();

                    //添加内容
                    TicketsEntity startTicket = tn.First();
                    TicketsEntity endTicket = tn.Last();
                    int DateRows = ticket_cellRowStart;
                    int ticketIndex = ticket_cellRowStart;
                    for (DateTime dt = startTicket.ModifiedOn; dt <= endTicket.ModifiedOn; dt = dt.AddDays(1))
                    {
                        List<TicketsEntity> Tickets = new List<TicketsEntity>();
                        Tickets = tn.FindAll(ts => ts.ModifiedOn.Date == dt.Date);
                        if (Tickets == null || Tickets.Count <= 0)
                            continue;

                        Range dateRange;
                        dateRange = (Range)ws.Cells[DateRows, 2];
                        RangeSetting(dateRange, dt.ToString("MM/dd/yyyy"), false, true);
                        dateRange.Font.Color = this.DefaultColor;

                        //daily project  
                        List<TicketsEntity> projectDistinct = Tickets.Distinct<TicketsEntity>(new CompairintTickets()).ToList<TicketsEntity>();
                        int projectRowIndex = DateRows;
                        foreach (TicketsEntity ts in projectDistinct)
                        {
                            Range projectTitleRange;

                            projectTitleRange = (Range)ws.Cells[projectRowIndex, 3];
                            RangeSetting(projectTitleRange, ts.ProjectTitle, false, false);
                            projectTitleRange.Font.Color = this.DefaultColor;

                            List<TicketsEntity> userSheetsInOneday = Tickets.FindAll(tx => tx.ProjectID == ts.ProjectID);

                            foreach (TicketsEntity tu in userSheetsInOneday)
                            {
                                //ticketColumntext.Append("\t\n"); 
                                Range ticketCode = (Range)ws.Cells[ticketIndex, 4];
                                Range ticketTitle = (Range)ws.Cells[ticketIndex, 5];
                                Range ticketDescription = (Range)ws.Cells[ticketIndex, 6];
                                Range ticketEstimate = (Range)ws.Cells[ticketIndex, 7];

                                RangeSetting(ticketCode, tu.TicketID.ToString(), false, false);
                                ticketCode.Font.Color = this.DefaultColor;

                                RangeSetting(ticketTitle, tu.Title, false, false);
                                ticketTitle.Font.Color = this.DefaultColor;

                                RangeSetting(ticketDescription, tu.FullDescription, false, false);
                                ticketDescription.Font.Color = this.DefaultColor;

                                RangeSetting(ticketEstimate, tu.FinalTime.ToString(), false, false);
                                ticketEstimate.Font.Color = this.DefaultColor;
                                ticketIndex++;
                            }
                            projectTitleRange = (Range)ws.Range[ws.Cells[projectRowIndex, 3], ws.Cells[ticketIndex - 1, 3]];
                            projectTitleRange.Merge(False);
                            projectRowIndex = ticketIndex;
                        }

                        dateRange = (Range)ws.Range[ws.Cells[DateRows, 2], ws.Cells[ticketIndex - 1, 2]];
                        dateRange.Merge(False);
                        DateRows = ticketIndex;
                    }

                    char[] leftItems = new char[] { 'E', 'F' };
                    BindContentStyle('B', ticket_cellRowStart.ToString(), 'G', (DateRows - 1).ToString(), leftItems);
                }
            }
            #endregion
            return SaveExcel(ws, wb);
        }


        private string GenerateSearchProjectResultReport(List<TimeSheetTicket> tss, List<TicketsEntity> tn, string projectName)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Template\\AllTemplate_Project.xlsx";
            wb = app.Workbooks.Open(filePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value
                , Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            ws = (Worksheet)wb.Worksheets[1];


            Range rng = ws.Range[ws.Cells[1, 1], ws.Cells[999, 20]];
            object tmp = "{ProjectName}";
            Range userRange = rng.Find(tmp, Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSearchDirection.xlNext, Missing.Value, Missing.Value, Missing.Value);
            if (userRange != null)
            {
                userRange.Value2 = "Project Name: " + projectName;
                userRange.Font.Color = this.DefaultColor;
                userRange.Font.Bold = true;
            }

            int cellRowStart = 8;
            decimal totalHours = decimal.Zero;
            if (tss.Count != 0)
            {
                int cellRowStartInner = cellRowStart;
                List<int> lstTicketid = new List<int>();
                List<string> lstTicketTitle = new List<string>();
                List<string> lstTicketDes = new List<string>();
                foreach (TimeSheetTicket entity in tss)
                {
                    lstTicketid.Add(entity.TicketID);
                    lstTicketTitle.Add(entity.TicketTitle);
                    lstTicketDes.Add(entity.TicketDescription);

                    Range ticketDate = (Range)ws.Cells[cellRowStartInner, 5];
                    RangeSetting(ticketDate, entity.SheetDate.ToString("MM/dd/yyyy"), false, false);
                    ticketDate.Font.Color = this.DefaultColor;



                    Range userTicketsDesc = (Range)ws.Cells[cellRowStartInner, 6];
                    RangeSetting(userTicketsDesc, entity.WorkDetail, false, false);
                    userTicketsDesc.Font.Color = this.DefaultColor;

                    Range userNameRange = (Range)ws.Cells[cellRowStartInner, 7];
                    RangeSetting(userNameRange, string.Format("{0} {1}", entity.FirstName, entity.LastName), false, false);
                    userNameRange.Font.Color = this.DefaultColor;

                    Range usrRole = (Range)ws.Cells[cellRowStartInner, 8];
                    RangeSetting(usrRole, entity.RoleName, false, false);
                    usrRole.Font.Color = this.DefaultColor;


                    Range hourTmp = (Range)ws.Cells[cellRowStartInner, 9];
                    RangeSetting(hourTmp, entity.Hours.ToString("#,#0.00"), false, false);
                    hourTmp.Font.Color = this.DefaultColor;

                    hourTmp.NumberFormat = "#,#0.00";

                    cellRowStartInner++;
                }

                Range dateRange;
                int i = 0;
                while (i < lstTicketid.Count)
                {
                    int row = lstTicketid.Count(tv => tv == lstTicketid[i]);

                    dateRange = (Range)ws.Range[ws.Cells[cellRowStart + i, 2], ws.Cells[cellRowStart + i + (row - 1), 2]];
                    dateRange.Merge(False);

                    RangeSetting(dateRange, lstTicketid[i].ToString(), false, false);
                    dateRange.Font.Color = this.DefaultColor;

                    dateRange = (Range)ws.Range[ws.Cells[cellRowStart + i, 3], ws.Cells[cellRowStart + i + (row - 1), 3]];
                    dateRange.Merge(False);

                    RangeSetting(dateRange, lstTicketTitle[i], false, false);
                    dateRange.Font.Color = this.DefaultColor;

                    dateRange = (Range)ws.Range[ws.Cells[cellRowStart + i, 4], ws.Cells[cellRowStart + i + (row - 1), 4]];
                    dateRange.Merge(False);

                    RangeSetting(dateRange, lstTicketDes[i], false, false);
                    dateRange.Font.Color = this.DefaultColor;
                    i += row;


                }

                cellRowStart = cellRowStartInner;

                #region end

                Range dataRange = (Range)ws.Range["B8", "K" + cellRowStart.ToString()];
                dataRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(229, 235, 247));

                Range wholeRange = (Range)ws.Range["B8", "K" + cellRowStart.ToString()];
                if (tss.Count > 0)
                {
                    Range totalHoursName = (Range)ws.Cells[cellRowStart, 8];
                    RangeSetting(totalHoursName, "Total Hours:", true, false);
                    totalHoursName.Font.Color = ColorTranslator.ToOle(Color.FromArgb(0, 0, 102));
                    //totalHoursName.Borders.Color = ColorTranslator.ToOle(Color.FromArgb(0, 0, 102));


                    Range totalHoursColumn = (Range)ws.Cells[cellRowStart, 9];
                    //RangeSetting(totalHoursColumn, totalHours.ToString("f1"), true, false);
                    RangeSetting(totalHoursColumn, string.Format("=SUM(I8:I{0})", cellRowStart - 1), true, false);

                    totalHoursColumn.Font.Color = ColorTranslator.ToOle(Color.FromArgb(0, 0, 102));
                    totalHoursColumn.NumberFormat = "#,#0.00";

                    Range hourSameRowRange = (Range)ws.Range["B" + cellRowStart.ToString(), "G" + cellRowStart.ToString()];
                    hourSameRowRange.Merge(False);


                    hourSameRowRange = (Range)ws.Range["B" + cellRowStart.ToString(), "K" + cellRowStart.ToString()];


                    hourSameRowRange.Borders.Color = ColorTranslator.ToOle(Color.FromArgb(127, 127, 127));
                    hourSameRowRange.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                    hourSameRowRange.Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlContinuous;
                    hourSameRowRange.Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                    hourSameRowRange.Borders.get_Item(XlBordersIndex.xlInsideHorizontal).LineStyle = XlLineStyle.xlContinuous;
                    hourSameRowRange.Borders.get_Item(XlBordersIndex.xlInsideVertical).LineStyle = XlLineStyle.xlContinuous;
                    hourSameRowRange.Borders.get_Item(XlBordersIndex.xlEdgeLeft).LineStyle = XlLineStyle.xlContinuous;
                    hourSameRowRange.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    hourSameRowRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(255, 159, 87));


                    wholeRange.EntireRow.AutoFit();
                }
                for (Char a = 'B'; a <= 'K'; a = Convert.ToChar(Convert.ToInt16(a) + 1))
                {
                    Range tmpRange = (Range)ws.Range[a.ToString() + "8", a.ToString() + (cellRowStart - 1).ToString()];
                    tmpRange.Borders.Color = ColorTranslator.ToOle(Color.FromArgb(127, 127, 127));
                    tmpRange.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlInsideHorizontal).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlInsideVertical).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlEdgeLeft).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    if (a != 'D' && a != 'F')
                    {
                        //tmpRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(207, 207, 207));
                        tmpRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    }
                    else
                    {

                        tmpRange.HorizontalAlignment = XlHAlign.xlHAlignLeft;
                    }
                }
            }
                #endregion


            //插入Ticket报表
            if (tn != null)
            {
                //添加Ticket表头
                int ticket_cellRowStart = cellRowStart + 4;

                Range ticketTitleRange;
                ticketTitleRange = (Range)ws.Cells[ticket_cellRowStart, 2];
                RangeSetting(ticketTitleRange, "Ticket ID", false, false);

                ticketTitleRange = (Range)ws.Cells[ticket_cellRowStart, 3];
                RangeSetting(ticketTitleRange, "Ticket Title", false, false);

                ticketTitleRange = (Range)ws.Cells[ticket_cellRowStart, 4];
                RangeSetting(ticketTitleRange, "Ticket Description", false, false);

                ticketTitleRange = (Range)ws.Cells[ticket_cellRowStart, 5];
                RangeSetting(ticketTitleRange, "Complete Date", false, false);

                ticketTitleRange = (Range)ws.Cells[ticket_cellRowStart, 6];
                RangeSetting(ticketTitleRange, "Estimated Time", false, false);

                BindTitleStyle("B", ticket_cellRowStart.ToString(), "F", ticket_cellRowStart.ToString());

                ticket_cellRowStart++;

                if (tn.Count > 0)
                {
                    tn = tn.OrderBy(t => t.ModifiedOn).ToList();

                    //添加内容
                    TicketsEntity startTicket = tn.First();
                    TicketsEntity endTicket = tn.Last();
                    int DateRows = ticket_cellRowStart;
                    int ticketIndex = ticket_cellRowStart;
                    for (DateTime dt = startTicket.ModifiedOn; dt <= endTicket.ModifiedOn; dt = dt.AddDays(1))
                    {
                        List<TicketsEntity> Tickets = new List<TicketsEntity>();
                        Tickets = tn.FindAll(ts => ts.ModifiedOn.Date == dt.Date);
                        if (Tickets == null || Tickets.Count <= 0)
                            continue;

                        Range dateRange;
                        dateRange = (Range)ws.Cells[DateRows, 5];
                        RangeSetting(dateRange, dt.ToString("MM/dd/yyyy"), false, true);
                        dateRange.Font.Color = this.DefaultColor;


                        List<TicketsEntity> userSheetsInOneday = Tickets.FindAll(tx => tx.ProjectID == tx.ProjectID);

                        foreach (TicketsEntity tu in userSheetsInOneday)
                        {
                            //ticketColumntext.Append("\t\n"); 
                            Range ticketCode = (Range)ws.Cells[ticketIndex, 2];
                            Range ticketTitle = (Range)ws.Cells[ticketIndex, 3];
                            Range ticketDescription = (Range)ws.Cells[ticketIndex, 4];
                            Range ticketEstimate = (Range)ws.Cells[ticketIndex, 6];

                            RangeSetting(ticketCode, tu.TicketID.ToString(), false, false);
                            ticketCode.Font.Color = this.DefaultColor;

                            RangeSetting(ticketTitle, tu.Title, false, false);
                            ticketTitle.Font.Color = this.DefaultColor;

                            RangeSetting(ticketDescription, tu.FullDescription, false, false);
                            ticketDescription.Font.Color = this.DefaultColor;

                            RangeSetting(ticketEstimate, tu.FinalTime.ToString(""), false, false);
                            ticketEstimate.Font.Color = this.DefaultColor;
                            ticketIndex++;
                        }

                        dateRange = (Range)ws.Range[ws.Cells[DateRows, 5], ws.Cells[ticketIndex - 1, 5]];
                        dateRange.Merge(False);
                        DateRows = ticketIndex;
                    }

                    char[] leftItems = new char[] { 'D' };
                    BindContentStyle('B', ticket_cellRowStart.ToString(), 'F', (DateRows - 1).ToString(), leftItems);
                }
            }
            return SaveExcel(ws, wb);
        }

        private string GenerateUserProjectReport(List<TimeSheetTicket> tss, UsersEntity user, string projectName)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Template\\MyTimesheetTemplate_Project.xlsx";
            wb = app.Workbooks.Open(filePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value
                , Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            //wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            ws = (Worksheet)wb.Worksheets[1];

            #region replace username
            Range rng = ws.Range[ws.Cells[1, 1], ws.Cells[999, 20]];
            object tmp = "{UserName}";
            Range userRange = rng.Find(tmp, Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSearchDirection.xlNext, Missing.Value, Missing.Value, Missing.Value);
            if (userRange != null)
            {
                userRange.Value2 = "SunNet - " + user.FirstName;
                userRange.Font.Color = this.DefaultColor;
            }
            #endregion


            Range projectRange = ws.Range[ws.Cells[1, 1], ws.Cells[999, 20]];
            object proTmp = "{ProjectName}";
            Range proRange = projectRange.Find(proTmp, Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSearchDirection.xlNext, Missing.Value, Missing.Value, Missing.Value);
            if (proRange != null)
            {
                proRange.Value2 = "Project Name: " + projectName;
                proRange.Font.Color = this.DefaultColor;
            }

            int cellRowStart = 8;
            decimal totalHours = decimal.Zero;
            if (tss.Count != 0)
            {

                #region replace rolename
                Range rang = ws.Range[ws.Cells[1, 1], ws.Cells[999, 20]];
                object roleTmp = "{RoleName}";
                Range roleRange = rang.Find(roleTmp, Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSearchDirection.xlNext, Missing.Value, Missing.Value, Missing.Value);
                if (roleRange != null)
                {
                    roleRange.Value2 = tss[0].RoleName;
                    roleRange.Font.Color = this.DefaultColor;
                }
                #endregion

                int cellRowStartInner = cellRowStart;
                List<string> lstTicketcode = new List<string>();
                foreach (TimeSheetTicket entity in tss)
                {
                    lstTicketcode.Add(entity.TicketCode);

                    Range ticketDate = (Range)ws.Cells[cellRowStartInner, 3];
                    RangeSetting(ticketDate, entity.SheetDate.ToString("MM/dd/yyyy"), false, false);
                    ticketDate.Font.Color = this.DefaultColor;

                    string strTmp = string.Empty;
                    if (entity.TicketDescription.Length > 0)
                    {
                        strTmp += "[" + entity.TicketDescription.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(@"\s", "").Replace("  ", " ") + "]";
                    }
                    if (entity.WorkDetail.Length > 0)
                    {
                        strTmp += "\r\n[" + entity.WorkDetail.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(@"\s", "").Replace("  ", " ") + "]";
                    }
                    strTmp += "\r\n";
                    Range userTicketsDesc = (Range)ws.Cells[cellRowStartInner, 4];
                    RangeSetting(userTicketsDesc, strTmp, false, false);
                    userTicketsDesc.Font.Color = this.DefaultColor;

                    //Range userNameRange = (Range)ws.Cells[cellRowStartInner, 5];
                    //RangeSetting(userNameRange, string.Format("{0} {1}", entity.FirstName, entity.LastName), false, false);
                    //userNameRange.Font.Color = this.DefaultColor;

                    //Range usrRole = (Range)ws.Cells[cellRowStartInner, 6];
                    //RangeSetting(usrRole, entity.RoleName, false, false);
                    //usrRole.Font.Color = this.DefaultColor;


                    Range hourTmp = (Range)ws.Cells[cellRowStartInner, 6];
                    RangeSetting(hourTmp, entity.Hours.ToString("#,#0.00"), false, false);
                    hourTmp.Font.Color = this.DefaultColor;

                    hourTmp.NumberFormat = "#,#0.00";

                    cellRowStartInner++;
                }

                Range dateRange;
                int i = 0;
                while (i < lstTicketcode.Count)
                {
                    int row = lstTicketcode.Count(tv => tv == lstTicketcode[i]);
                    if (row > 1)//大于一行则合并
                    {
                        dateRange = (Range)ws.Range[ws.Cells[cellRowStart + i, 2], ws.Cells[cellRowStart + i + (row - 1), 2]];
                        dateRange.Merge(False);

                        RangeSetting(dateRange, lstTicketcode[i], false, false);
                        dateRange.Font.Color = this.DefaultColor;
                        i += row;
                    }
                    else
                    {
                        dateRange = (Range)ws.Cells[cellRowStart + i, 2];
                        RangeSetting(dateRange, lstTicketcode[i], false, false);
                        dateRange.Font.Color = this.DefaultColor;
                        i++;
                    }

                }

                cellRowStart = cellRowStartInner;
                #region end

                for (int k = 8; k < cellRowStart; k++)
                {
                    Range dataRange = (Range)ws.Range["B" + k.ToString(), "K" + k.ToString()];
                    if (k % 2 == 0)
                    {
                        dataRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(229, 235, 247));
                    }
                    else
                    {
                        dataRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(240, 243, 250));
                    }
                }





                Range wholeRange = (Range)ws.Range["B8", "K" + cellRowStart.ToString()];
                if (tss.Count > 0)
                {
                    Range totalHoursName = (Range)ws.Cells[cellRowStart, 5];
                    RangeSetting(totalHoursName, "Total Hours:", true, false);
                    totalHoursName.Font.Color = ColorTranslator.ToOle(Color.FromArgb(0, 0, 102));





                    Range totalHoursColumn = (Range)ws.Cells[cellRowStart, 6];
                    //RangeSetting(totalHoursColumn, totalHours.ToString("f1"), true, false);

                    RangeSetting(totalHoursColumn, string.Format("=SUM(I8:I{0})", cellRowStart - 1), true, false);

                    totalHoursColumn.NumberFormat = "#,#0.00";
                    totalHoursColumn.Font.Color = ColorTranslator.ToOle(Color.FromArgb(0, 0, 102));
                    Range hourSameRowRange = (Range)ws.Range["B" + cellRowStart.ToString(), "D" + cellRowStart.ToString()];
                    hourSameRowRange.Merge(False);

                    hourSameRowRange = (Range)ws.Range["G" + cellRowStart.ToString(), "H" + cellRowStart.ToString()];
                    hourSameRowRange.Merge(False);

                    hourSameRowRange = (Range)ws.Range["B" + cellRowStart.ToString(), "H" + cellRowStart.ToString()];


                    hourSameRowRange.Borders.Color = ColorTranslator.ToOle(Color.FromArgb(127, 127, 127));
                    hourSameRowRange.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                    hourSameRowRange.Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlContinuous;
                    hourSameRowRange.Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                    hourSameRowRange.Borders.get_Item(XlBordersIndex.xlInsideHorizontal).LineStyle = XlLineStyle.xlContinuous;
                    hourSameRowRange.Borders.get_Item(XlBordersIndex.xlInsideVertical).LineStyle = XlLineStyle.xlContinuous;
                    hourSameRowRange.Borders.get_Item(XlBordersIndex.xlEdgeLeft).LineStyle = XlLineStyle.xlContinuous;
                    hourSameRowRange.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    hourSameRowRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(255, 159, 87));


                    wholeRange.EntireRow.AutoFit();
                    //wholeRange.EntireColumn.AutoFit();
                }
                for (Char a = 'B'; a <= 'H'; a = Convert.ToChar(Convert.ToInt16(a) + 1))
                {
                    Range tmpRange = (Range)ws.Range[a.ToString() + "8", a.ToString() + (cellRowStart - 1).ToString()];
                    tmpRange.Borders.Color = ColorTranslator.ToOle(Color.FromArgb(127, 127, 127));
                    tmpRange.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlInsideHorizontal).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlInsideVertical).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlEdgeLeft).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    if (a != 'D')
                    {
                        //tmpRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(207, 207, 207));
                        tmpRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    }
                    else
                    {

                        tmpRange.HorizontalAlignment = XlHAlign.xlHAlignLeft;
                    }
                }


                Range topRange = (Range)ws.Range["I6", "K6"];
                Range clientRange = topRange.Find("{Client}", Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSearchDirection.xlNext, Missing.Value, Missing.Value, Missing.Value);

                RangeSetting(clientRange, "", false, false);
                Range rngClientName = topRange.Find("Client:", Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSearchDirection.xlNext, Missing.Value, Missing.Value, Missing.Value);
                topRange.Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlLineStyleNone;
                RangeSetting(rngClientName, "", false, false);

                #endregion
            }

            return SaveExcel(ws, wb);
        }

        #region 报表

        public void ComparisonExport(List<TimeSheetTicket> tss)
        {
            tss = tss.OrderBy(ts => ts.ProjectID).ToList<TimeSheetTicket>();
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Template\\ComparisonTemplate.xlsx";
            wb = app.Workbooks.Open(filePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value
                , Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            ws = (Worksheet)wb.Worksheets[1];
            if (tss.Count != 0)
            {
                //get what will show on client range
                IEnumerable<TimeSheetTicket> wholeSheets = tss.Distinct<TimeSheetTicket>(new Compairint());
                var q = from w in wholeSheets select w.ProjectID;
                List<Int32> projectIDs = q.Distinct().ToList<Int32>();
                decimal totalHours = decimal.Zero;
                int projectBeginIndex = 8, projectEndIndex = 8,
                    ticketStartindex = 8, ticketEndindex = 8,
                    timesheetStartIndex = 8, timesheetEndIndex = 8;


                Range projectNameRange;
                Range ticketIDRange;
                Range ticketTitleRange;
                Range ticketDesRange;
                Range estimationRange;
                Range userNameRange;
                Range roleRange;
                Range hourRange;
                Range tempRange;
                foreach (Int32 projectId in projectIDs)
                {
                    decimal projectTotalEstimation = 0, projectTotalHours = 0;
                    List<Int32> ticketIds =
                        tss.FindAll(ts => ts.ProjectID == projectId).Select(t => t.TicketID).Distinct().ToList<Int32>();
                    foreach (Int32 ticketId in ticketIds)
                    {
                        decimal ticketTotalHours = 0;
                        List<Int32> userIds = tss.FindAll(ts => ts.ProjectID == projectId && ts.TicketID == ticketId)
                            .Select(t => t.UserID).Distinct().ToList<Int32>();
                        foreach (Int32 userId in userIds)
                        {
                            TimeSheetTicket timesheet = tss.FirstOrDefault(ts => ts.ProjectID == projectId
                                                                                 && ts.TicketID == ticketId &&
                                                                                 ts.UserID == userId);

                            userNameRange = (Range)ws.Cells[timesheetEndIndex, 7];
                            RangeSetting(userNameRange, timesheet.FirstName.ToString(), false, false);
                            userNameRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;

                            roleRange = (Range)ws.Cells[timesheetEndIndex, 8];
                            RangeSetting(roleRange, timesheet.RoleName.ToString(), false, false);

                            hourRange = (Range)ws.Cells[timesheetEndIndex, 9];
                            RangeSetting(hourRange, timesheet.Hours.ToString(), false, false);

                            timesheetEndIndex++;
                            ticketTotalHours = ticketTotalHours + timesheet.Hours;
                        }
                        //获取其中一条sheettime 信息 用来显示ticket 信息
                        TimeSheetTicket ticketDefault = tss.FirstOrDefault(ts => ts.ProjectID == projectId
                                                                                && ts.TicketID == ticketId);
                        #region 显示 ticket 相关信息
                        ticketEndindex = timesheetEndIndex - 1;

                        ticketIDRange = ws.Range[ws.Cells[ticketStartindex, 3], ws.Cells[ticketEndindex, 3]];
                        ticketIDRange.Merge(False);
                        ticketIDRange = (Range)ws.Cells[ticketStartindex, 3];
                        RangeSetting(ticketIDRange, ticketId.ToString(), false, false);
                        ticketIDRange.HorizontalAlignment = XlHAlign.xlHAlignLeft;

                        ticketTitleRange = ws.Range[ws.Cells[ticketStartindex, 4], ws.Cells[ticketEndindex, 4]];
                        ticketTitleRange.Merge(False);
                        ticketTitleRange = (Range)ws.Cells[ticketStartindex, 4];
                        RangeSetting(ticketTitleRange, ticketDefault.TicketTitle.ToString(), false, false);

                        ticketDesRange = ws.Range[ws.Cells[ticketStartindex, 5], ws.Cells[ticketEndindex, 5]];
                        ticketDesRange.Merge(False);
                        ticketDesRange = (Range)ws.Cells[ticketStartindex, 5];
                        RangeSetting(ticketDesRange, ticketDefault.TicketDescription.ToString(), false, false);

                        ticketDesRange = ws.Range[ws.Cells[ticketStartindex, 6], ws.Cells[ticketEndindex, 6]];
                        ticketDesRange.Merge(False);
                        ticketDesRange = (Range)ws.Cells[ticketStartindex, 6];
                        RangeSetting(ticketDesRange, ticketDefault.Estimation.ToString("#0.00"), false, false);
                        #endregion
                        ticketStartindex = ticketEndindex + 1;

                        #region 显示统计信息

                        tempRange = ws.Range[ws.Cells[ticketStartindex, 3], ws.Cells[ticketStartindex, 8]];
                        tempRange.Merge(False);
                        tempRange = (Range)ws.Cells[ticketStartindex, 3];
                        RangeSetting(tempRange, "Sub Total", false, false);

                        tempRange.HorizontalAlignment = XlHAlign.xlHAlignRight;


                        tempRange = (Range)ws.Cells[ticketStartindex, 9];
                        RangeSetting(tempRange, ticketTotalHours.ToString("#0.00"), false, false);
                        if (ticketTotalHours > ticketDefault.Estimation && ticketDefault.Estimation > 0)
                        {
                            tempRange.Interior.Color = ColorTranslator.ToOle(Color.White);
                            tempRange.Font.Color = ColorTranslator.ToOle(Color.Red);
                        }
                        else
                        {
                            tempRange.Interior.Color = ColorTranslator.ToOle(Color.LightGray);
                        }



                        projectTotalEstimation = projectTotalEstimation + ticketDefault.Estimation;
                        projectTotalHours = projectTotalHours + ticketTotalHours;

                        ticketEndindex++;
                        timesheetEndIndex++;
                        ticketStartindex = ticketEndindex + 1;
                        #endregion
                    }

                    //获取其中一条sheettime 信息 用来显示project 信息
                    TimeSheetTicket projectDefault = tss.FirstOrDefault(ts => ts.ProjectID == projectId);

                    #region 显示 project相关信息
                    projectEndIndex = timesheetEndIndex - 1;
                    projectNameRange = ws.Range[ws.Cells[projectBeginIndex, 2], ws.Cells[projectEndIndex, 2]];
                    projectNameRange.Merge(False);
                    projectNameRange = (Range)ws.Cells[projectBeginIndex, 2];
                    RangeSetting(projectNameRange, projectDefault.ProjectTitle, false, false);
                    projectNameRange.HorizontalAlignment = XlHAlign.xlHAlignLeft;
                    projectBeginIndex = projectEndIndex + 1;
                    #endregion

                    #region 显示统计信息

                    tempRange = ws.Range[ws.Cells[projectBeginIndex, 2], ws.Cells[projectBeginIndex, 5]];
                    tempRange.Merge(False);
                    tempRange = (Range)ws.Cells[projectBeginIndex, 2];
                    RangeSetting(tempRange, "Total", false, false);
                    tempRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(255, 159, 87));
                    tempRange.HorizontalAlignment = XlHAlign.xlHAlignRight;

                    tempRange = (Range)ws.Cells[projectBeginIndex, 6];
                    RangeSetting(tempRange, projectTotalEstimation.ToString("#0.00"), false, false);
                    tempRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(255, 159, 87));

                    tempRange = ws.Range[ws.Cells[projectBeginIndex, 7], ws.Cells[projectBeginIndex, 8]];
                    tempRange.Merge(False);
                    tempRange = (Range)ws.Cells[projectBeginIndex, 7];
                    RangeSetting(tempRange, "Total", false, false);
                    tempRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(255, 159, 87));
                    tempRange.HorizontalAlignment = XlHAlign.xlHAlignRight;

                    tempRange = (Range)ws.Cells[projectBeginIndex, 9];
                    RangeSetting(tempRange, projectTotalHours.ToString("#0.00"), false, false);
                    tempRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(255, 159, 87));

                    timesheetEndIndex++;
                    ticketEndindex++;
                    projectEndIndex++;
                    ticketStartindex = ticketEndindex + 1;
                    projectBeginIndex = projectBeginIndex + 1;

                    #endregion
                }

                #region 格式化
                for (Char a = 'B'; a <= 'I'; a = Convert.ToChar(Convert.ToInt16(a) + 1))
                {
                    Range tmpRange = (Range)ws.Range[a.ToString() + "8", a.ToString() + (timesheetEndIndex - 2).ToString()];
                    tmpRange.Borders.Color = ColorTranslator.ToOle(Color.FromArgb(127, 127, 127));
                    tmpRange.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlInsideHorizontal).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlInsideVertical).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlEdgeLeft).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    if (a != 'C' && a != 'H' && a != 'B' && a != 'G')
                    {
                        tmpRange.HorizontalAlignment = XlHAlign.xlHAlignLeft;

                    }
                    if (a == 'I' || a == 'F')
                    {
                        tmpRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    }
                }
                #endregion
            }
            string fileName = SaveExcel(ws, wb);
            ExportFile(fileName, "Ticket Comparison.xlsx");
        }

        public void AnalysisExport(System.Data.DataTable table, string sheetName)
        {

            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Template\\Analysis.xlsx";
            wb = app.Workbooks.Open(filePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value
                , Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            ws = (Worksheet)wb.Worksheets[1];
            ws.Name = sheetName;
            Range cellRange;
            if (table.Rows.Count != 0)
            {

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string priorityStr = "";
                    if (table.Rows[i]["Priority"] != null)
                        priorityStr = table.Rows[i]["Priority"].ToString();

                    string typeStr = "";
                    if (table.Rows[i]["TicketType"] != null)
                        typeStr = table.Rows[i]["TicketType"].ToString();

                    string SourceStr = "";
                    if (table.Rows[i]["Source"] != null)
                        SourceStr = table.Rows[i]["Source"].ToString();

                    int priorityId = 0, sourceId = 0;
                    int.TryParse(priorityStr, out priorityId);
                    int.TryParse(SourceStr, out sourceId);
                    if (priorityId == 0)
                        priorityStr = "";
                    else
                        priorityStr = ((PriorityState)(priorityId)).ToString();

                    if (sourceId == 0)
                        SourceStr = "";
                    else
                        SourceStr = ((RolesEnum)(sourceId)).ToString();

                    cellRange = (Range)ws.Cells[8 + i, 2];
                    RangeSetting(cellRange, table.Rows[i]["ProjectTitle"].ToString(), false, false);
                    cellRange = (Range)ws.Cells[8 + i, 3];
                    RangeSetting(cellRange, table.Rows[i]["TicketID"].ToString(), false, false);
                    cellRange = (Range)ws.Cells[8 + i, 4];
                    RangeSetting(cellRange, table.Rows[i]["Title"].ToString(), false, false);
                    cellRange = (Range)ws.Cells[8 + i, 5];
                    RangeSetting(cellRange, table.Rows[i]["Description"].ToString(), false, false);
                    cellRange = (Range)ws.Cells[8 + i, 6];
                    RangeSetting(cellRange, priorityStr, false, false);
                    cellRange = (Range)ws.Cells[8 + i, 7];
                    RangeSetting(cellRange, typeStr, false, false);
                    cellRange = (Range)ws.Cells[8 + i, 8];
                    RangeSetting(cellRange, SourceStr, false, false);
                    cellRange = (Range)ws.Cells[8 + i, 9];
                    RangeSetting(cellRange, table.Rows[i]["DEV"].ToString().Trim().Trim(','), false, false);
                    cellRange = (Range)ws.Cells[8 + i, 10];
                    RangeSetting(cellRange, table.Rows[i]["QA"].ToString().Trim().Trim(','), false, false);
                }

                #region 格式化
                for (int k = 8; k < table.Rows.Count + 8; k++)
                {
                    Range dataRange = (Range)ws.Range["B" + k.ToString(), "J" + k.ToString()];
                    if (k % 2 == 0)
                    {
                        dataRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(229, 235, 247));
                    }
                    else
                    {
                        dataRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(240, 243, 250));
                    }
                }
                for (Char a = 'B'; a <= 'J'; a = Convert.ToChar(Convert.ToInt16(a) + 1))
                {
                    Range tmpRange = (Range)ws.Range[a.ToString() + "8", a.ToString() + (8 + table.Rows.Count - 1).ToString()];
                    tmpRange.Borders.Color = ColorTranslator.ToOle(Color.FromArgb(127, 127, 127));
                    tmpRange.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlInsideHorizontal).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlInsideVertical).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlEdgeLeft).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    if (a == 'C' || a == 'F' || a == 'G' || a == 'H')
                    {
                        tmpRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    }
                    else
                    {
                        tmpRange.HorizontalAlignment = XlHAlign.xlHAlignLeft;
                    }
                }
                #endregion
            }
            string fileName = SaveExcel(ws, wb);
            ExportFile(fileName, sheetName + ".xlsx");
        }

        public void RatingExport(System.Data.DataTable table, string projectTitle, string DateStr)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Template\\Rating.xlsx";
            wb = app.Workbooks.Open(filePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value
                , Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            ws = (Worksheet)wb.Worksheets[1];
            Range cellRange;
            Range rng = ws.Range[ws.Cells[1, 1], ws.Cells[999, 20]];
            object tmp = "{project}";
            Range tempRange = rng.Find(tmp, Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSearchDirection.xlNext, Missing.Value, Missing.Value, Missing.Value);
            RangeSetting(tempRange, projectTitle, false, false);
            tmp = "{date}";
            tempRange = rng.Find(tmp, Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSearchDirection.xlNext, Missing.Value, Missing.Value, Missing.Value);
            RangeSetting(tempRange, DateStr, false, false);

            if (table.Rows.Count != 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string noneStar = table.Rows[i][4].ToString().Trim() == "" ? " 0" : table.Rows[i][4].ToString();
                    string Star1 = table.Rows[i][4].ToString().Trim() == "" ? " 0" : table.Rows[i][5].ToString();
                    string Star2 = table.Rows[i][4].ToString().Trim() == "" ? " 0" : table.Rows[i][6].ToString();
                    string Star3 = table.Rows[i][4].ToString().Trim() == "" ? " 0" : table.Rows[i][7].ToString();
                    string Star4 = table.Rows[i][4].ToString().Trim() == "" ? " 0" : table.Rows[i][8].ToString();
                    string Star5 = table.Rows[i][4].ToString().Trim() == "" ? " 0" : table.Rows[i][9].ToString();

                    cellRange = (Range)ws.Cells[8 + i, 2];
                    RangeSetting(cellRange, table.Rows[i]["ProjectTitle"].ToString(), false, false);
                    cellRange = (Range)ws.Cells[8 + i, 3];
                    RangeSetting(cellRange, table.Rows[i]["TicketType"].ToString(), false, false);
                    cellRange = (Range)ws.Cells[8 + i, 4];
                    RangeSetting(cellRange, noneStar, false, false);
                    cellRange = (Range)ws.Cells[8 + i, 5];
                    RangeSetting(cellRange, Star1, false, false);
                    cellRange = (Range)ws.Cells[8 + i, 6];
                    RangeSetting(cellRange, Star2, false, false);
                    cellRange = (Range)ws.Cells[8 + i, 7];
                    RangeSetting(cellRange, Star3, false, false);
                    cellRange = (Range)ws.Cells[8 + i, 8];
                    RangeSetting(cellRange, Star4, false, false);
                    cellRange = (Range)ws.Cells[8 + i, 9];
                    RangeSetting(cellRange, Star5, false, false);
                }

                #region 格式化
                for (int k = 8; k < table.Rows.Count + 8; k++)
                {
                    Range dataRange = (Range)ws.Range["B" + k.ToString(), "I" + k.ToString()];
                    if (k % 2 == 0)
                    {
                        dataRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(229, 235, 247));
                    }
                    else
                    {
                        dataRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(240, 243, 250));
                    }
                }
                for (Char a = 'B'; a <= 'I'; a = Convert.ToChar(Convert.ToInt16(a) + 1))
                {
                    Range tmpRange = (Range)ws.Range[a.ToString() + "8", a.ToString() + (8 + table.Rows.Count - 1).ToString()];
                    tmpRange.Borders.Color = ColorTranslator.ToOle(Color.FromArgb(127, 127, 127));
                    tmpRange.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlInsideHorizontal).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlInsideVertical).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.Borders.get_Item(XlBordersIndex.xlEdgeLeft).LineStyle = XlLineStyle.xlContinuous;
                    tmpRange.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    if (a == 'B')
                    {
                        tmpRange.HorizontalAlignment = XlHAlign.xlHAlignLeft;
                    }
                    else
                    {
                        tmpRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    }
                }
                #endregion
            }
            string fileName = SaveExcel(ws, wb);
            ExportFile(fileName, "Ticket Rating.xlsx");
        }



        /// <summary>
        /// 导出生成的报表
        /// </summary>
        /// <param name="fileName">真实文件名</param>
        /// <param name="outputFileName">输出文件名</param>
        /// <returns></returns>

        public void ExportFile(string fileName, string outputFileName)
        {
            int appInt = GC.GetGeneration(app);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(ws);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
            ws = null;
            wb = null;
            app = null;
            GC.Collect(appInt);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            MemoryStream ms = new MemoryStream(File.ReadAllBytes(fileName));
            try
            {
                File.Delete(fileName);
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(ex);
            }

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.Buffer = false;
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;

            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + outputFileName);
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            byte[] bytes = ms.ToArray();
            HttpContext.Current.Response.AddHeader("Content-Length", bytes.Length.ToString());

            HttpContext.Current.Response.OutputStream.Write(bytes, 0, bytes.Length);
            HttpContext.Current.Response.OutputStream.Flush();
            HttpContext.Current.Response.End();
        }

        #endregion



        /// <summary>
        /// 设置Ticket表头样式
        /// </summary>
        /// <param name="startCell"></param>
        /// <param name="startRow"></param>
        /// <param name="endCell"></param>
        /// <param name="endRow"></param>
        private void BindTitleStyle(string startCell, string startRow, string endCell, string endRow)
        {
            Range titleRange = (Range)ws.Range[startCell + startRow.ToString(), endCell + endRow.ToString()];
            titleRange.Font.Name = "Century Gothic";
            titleRange.Font.Size = 9;
            titleRange.Font.Bold = FontStyle.Bold;
            titleRange.Font.Color = Color.White;
            titleRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(39, 64, 139));
            titleRange.Borders.Color = ColorTranslator.ToOle(Color.FromArgb(127, 127, 127));
            titleRange.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            titleRange.Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlContinuous;
            titleRange.Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
            titleRange.Borders.get_Item(XlBordersIndex.xlInsideHorizontal).LineStyle = XlLineStyle.xlContinuous;
            titleRange.Borders.get_Item(XlBordersIndex.xlInsideVertical).LineStyle = XlLineStyle.xlContinuous;
            titleRange.Borders.get_Item(XlBordersIndex.xlEdgeLeft).LineStyle = XlLineStyle.xlContinuous;
            titleRange.VerticalAlignment = XlVAlign.xlVAlignCenter;
            titleRange.RowHeight = 23.25;
        }

        /// <summary>
        /// 设置Ticket内容样式
        /// </summary>
        /// <param name="startCell"></param>
        /// <param name="startRow"></param>
        /// <param name="endCell"></param>
        /// <param name="endRow"></param>
        /// <param name="leftItems">需要左边居中的列名</param>
        private void BindContentStyle(char startCell, string startRow, char endCell, string endRow, char[] leftItems)
        {
            //设置背景色
            Range dataRange = (Range)ws.Range[startCell + startRow, endCell + endRow];
            dataRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(229, 235, 247));
            //设置边框
            for (Char a = startCell; a <= endCell; a = Convert.ToChar(Convert.ToInt16(a) + 1))
            {
                Range tmpRange = (Range)ws.Range[a.ToString() + startRow, a.ToString() + endRow];
                tmpRange.Borders.Color = ColorTranslator.ToOle(Color.FromArgb(127, 127, 127));
                tmpRange.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                tmpRange.Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlContinuous;
                tmpRange.Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                tmpRange.Borders.get_Item(XlBordersIndex.xlInsideHorizontal).LineStyle = XlLineStyle.xlContinuous;
                tmpRange.Borders.get_Item(XlBordersIndex.xlInsideVertical).LineStyle = XlLineStyle.xlContinuous;
                tmpRange.Borders.get_Item(XlBordersIndex.xlEdgeLeft).LineStyle = XlLineStyle.xlContinuous;
                tmpRange.VerticalAlignment = XlVAlign.xlVAlignCenter;
                if (leftItems != null)
                {
                    if (!leftItems.Contains(a))
                    {
                        tmpRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    }
                    else
                    {
                        tmpRange.HorizontalAlignment = XlHAlign.xlHAlignLeft;
                    }
                }
                else
                {
                    tmpRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                }
            }
        }

        #region Invoice  报表
        public void ExportInvoice(List<TimeSheetTicket> tss)
        {
            string fileName = string.Empty;
            string outputFileName = string.Empty;
            fileName = GenerateSearchResultReport(tss, null);
            outputFileName = "TimeSheet.xlsx";
            int appInt = GC.GetGeneration(app);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
            //    System.Runtime.InteropServices.Marshal.ReleaseComObject(ws);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
            ws = null;

            wb = null;
            app = null;
            GC.Collect(appInt);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            MemoryStream ms = new MemoryStream(File.ReadAllBytes(fileName));
            try
            {
                File.Delete(fileName);
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(ex);
            }

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.Buffer = false;
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;

            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + outputFileName);
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            byte[] bytes = ms.ToArray();
            HttpContext.Current.Response.AddHeader("Content-Length", bytes.Length.ToString());

            HttpContext.Current.Response.OutputStream.Write(bytes, 0, bytes.Length);
            HttpContext.Current.Response.OutputStream.Flush();
            HttpContext.Current.Response.End();

        }
        #endregion
    }
}