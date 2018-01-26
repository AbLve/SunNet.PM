using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.IO;
using System.Web;
using SunNet.PMNew.Entity.TimeSheetModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils.Providers;
using System.Reflection;


namespace SunNet.PMNew.Web.Codes
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
            rng.Value2 = text;
            if (isDate)
                rng.NumberFormatLocal = "mm/dd/yyyy";
        }

        private object DefaultColor
        {
            get { return ColorTranslator.ToOle(Color.FromArgb(102, 102, 102)); }
        }

        //private delegate List<ClientInfo> AsyncGetDelegate(List<Guid> gIds);

        private string GenerateReport(List<TimeSheetTicket> tss, UsersEntity user)
        {
            tss = tss.OrderBy(ts => ts.SheetDate).ToList<TimeSheetTicket>();

            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Template\\MyTimesheetTemplate.xlsx";
            wb = app.Workbooks.Open(filePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value
                , Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            //wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            ws = (Worksheet)wb.Worksheets[1];

            #region replace username
            Range rng = ws.get_Range(ws.Cells[1, 1], ws.Cells[999, 20]);
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

                //ClientManager cMgr = new ClientManager();
                //AsyncGetDelegate pGetDelegate = new AsyncGetDelegate(cMgr.GetClients);

                //List<ClientInfo> clients = new List<ClientInfo>();
                //IAsyncResult ar = null;
                //if (projectIDs.Count > 0)
                //{
                //begin get projects 
                //ar = pGetDelegate.BeginInvoke(projectIDs, null, null);
                //}

                TimeSheetTicket startTimeSheetTicket = tss.First();
                TimeSheetTicket endTimeSheetTicket = tss.Last();
                int cellRowStart = 8;

                decimal totalHours = decimal.Zero;

                for (DateTime dt = startTimeSheetTicket.SheetDate; dt <= endTimeSheetTicket.SheetDate; dt = dt.AddDays(1))
                {
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
                    if (count > 1)
                    {
                        dateRange = ws.get_Range(ws.Cells[cellRowStart, 2], ws.Cells[cellRowStart + count - 1, 2]);
                        dateRange.Merge(False);
                    }
                    else
                    {
                        dateRange = (Range)ws.Cells[cellRowStart, 2];
                    }

                    int i = 0;
                    foreach (TimeSheetTicket p in distinct)
                    {
                        //set project title
                        projectRange = (Range)ws.Cells[cellRowStart + i, 3];
                        RangeSetting(projectRange, p.ProjectTitle, false, false);
                        projectRange.Font.Color = this.DefaultColor;
                        //get ticket column data from daily data
                        List<TimeSheetTicket> sheetOnTickets = dailySheets.FindAll(ds => ds.ProjectID == p.ProjectID);

                        //get ticket column of the project
                        rngTicket = (Range)ws.Cells[cellRowStart + i, 4];
                        StringBuilder ticketColumntext = new StringBuilder();
                        decimal hoursSpent = decimal.Zero;
                        int j = 1;
                        foreach (TimeSheetTicket ts in sheetOnTickets)
                        {
                            totalHours += ts.Hours;
                            hoursSpent += ts.Hours;
                            ticketColumntext.Append(j.ToString() + (ts.TicketCode.Length == 0 ? "[ ]" : ". [" + ts.TicketCode + "]") +
                                (ts.TicketTitle.Length == 0 ? "[ ]" : "[" + ts.TicketTitle + "]/\t\n"));


                            if (ts.TicketDescription.Length > 0)
                            {
                                ticketColumntext.Append("[" + ts.TicketDescription + "]\t\n");
                            }
                            if (ts.WorkDetail.Length > 0)
                            {
                                ticketColumntext.Append("[" + ts.WorkDetail + "]\t\n");
                            }
                            //ticketColumntext.Append("\t\n");
                            j++;
                        }
                        ticketColumntext.Remove(ticketColumntext.Length - 2, 2);
                        //set ticket column text
                        RangeSetting(rngTicket, ticketColumntext.ToString(), false, false);
                        rngTicket.Font.Color = this.DefaultColor;
                        //set hour column
                        Range rngHours = (Range)ws.Cells[cellRowStart + i, 6];
                        RangeSetting(rngHours, hoursSpent.ToString("#,#0.00"), false, false);
                        rngHours.Font.Color = this.DefaultColor;
                        rngHours.NumberFormat = "#,#0.00";
                        i++;
                    }


                    //set date column

                    RangeSetting(dateRange, dt.ToString("MM/dd/yyyy"), false, true);
                    dateRange.Font.Color = this.DefaultColor;
                    cellRowStart = cellRowStart + i;
                }
                #region end

                for (int k = 8; k < cellRowStart; k++)
                {
                    Range dataRange = (Range)ws.get_Range("B" + k.ToString(), "H" + k.ToString());
                    if (k % 2 == 0)
                    {
                        dataRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(229, 235, 247));
                    }
                    else
                    {
                        dataRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(240, 243, 250));
                    }
                }





                Range wholeRange = (Range)ws.get_Range("B8", "H" + cellRowStart.ToString());
                if (tss.Count > 0)
                {
                    Range totalHoursName = (Range)ws.Cells[cellRowStart, 5];
                    RangeSetting(totalHoursName, "Total Hours:", true, false);
                    totalHoursName.Font.Color = ColorTranslator.ToOle(Color.FromArgb(0, 0, 102));





                    Range totalHoursColumn = (Range)ws.Cells[cellRowStart, 6];
                    //RangeSetting(totalHoursColumn, totalHours.ToString("f1"), true, false);

                    RangeSetting(totalHoursColumn, string.Format("=SUM(F8:F{0})", cellRowStart - 1), true, false);

                    totalHoursColumn.NumberFormat = "#,#0.00";
                    totalHoursColumn.Font.Color = ColorTranslator.ToOle(Color.FromArgb(0, 0, 102));
                    Range hourSameRowRange = (Range)ws.get_Range("B" + cellRowStart.ToString(), "D" + cellRowStart.ToString());
                    hourSameRowRange.Merge(False);

                    hourSameRowRange = (Range)ws.get_Range("G" + cellRowStart.ToString(), "H" + cellRowStart.ToString());
                    hourSameRowRange.Merge(False);

                    hourSameRowRange = (Range)ws.get_Range("B" + cellRowStart.ToString(), "H" + cellRowStart.ToString());


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
                    Range tmpRange = (Range)ws.get_Range(a.ToString() + "8", a.ToString() + (cellRowStart - 1).ToString());
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


                if (projectIDs.Count > 0)
                {
                    Range topRange = (Range)ws.get_Range("F5", "H5");
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




                #endregion
            }
            //Range ticketRange = (Range)ws.get_Range("B8", "B" + cellRowStart.ToString());
            //ticketRange.Interior.Color = 

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

        public void Generate(List<TimeSheetTicket> tss, UsersEntity user, string projectTitle, DateTime eDate)
        {
            string fileName = string.Empty;
            string outputFileName = string.Empty;
            if (user == null)
            {
                if (!string.IsNullOrEmpty(projectTitle) && projectTitle == "All")
                    fileName = GenerateSearchResultReport(tss);
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
                    fileName = GenerateSearchProjectResultReport(tss, projectTitle);
                }
                outputFileName = "TimeSheet.xlsx";
            }
            else
            {

                //if (!string.IsNullOrEmpty(projectTitle) && projectTitle == "All")
                //    fileName = GenerateReport(tss, user);
                //else
                //    fileName = GenerateUserProjectReport(tss, user,projectTitle);
                fileName = GenerateReport(tss, user);

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

        private string GenerateSearchResultReport(List<TimeSheetTicket> tss)
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


                for (DateTime dt = startTimeSheetTicket.SheetDate; dt <= endTimeSheetTicket.SheetDate; dt = dt.AddDays(1))
                {
                    List<TimeSheetTicket> dailySheets = new List<TimeSheetTicket>();
                    dailySheets = tss.FindAll(ts => ts.SheetDate == dt);

                    if (dailySheets == null || dailySheets.Count <= 0)
                        continue;

                    int cellRowStartInner = cellRowStart;
                    int DateRows = cellRowStart;

                    //daily project  
                    List<TimeSheetTicket> projectDistinct = dailySheets.Distinct<TimeSheetTicket>(new Compairint()).ToList<TimeSheetTicket>();

                    foreach (TimeSheetTicket ts in projectDistinct)
                    {
                        List<TimeSheetTicket> dailyUser = new List<TimeSheetTicket>();
                        dailyUser = dailySheets.FindAll(tv => tv.ProjectID == ts.ProjectID).Distinct<TimeSheetTicket>(new CompairintUsers()).ToList<TimeSheetTicket>();

                        int i = 0;
                        int projectBeforeMerge = cellRowStartInner;
                        foreach (TimeSheetTicket tUser in dailyUser)
                        {
                            List<TimeSheetTicket> userSheetsInOneday = dailySheets.FindAll(tx => tx.UserID == tUser.UserID && tx.ProjectID == ts.ProjectID);

                            decimal dailyUserProjectHours = decimal.Zero;
                            Range userTickets = (Range)ws.Cells[cellRowStartInner, 4];
                            string tmp = string.Empty;

                            int j = 1;
                            foreach (TimeSheetTicket tu in userSheetsInOneday)
                            {
                                tmp += j.ToString() + ". [" + tu.TicketCode + "] [" + tu.TicketTitle + "] /\t\n";
                                if (tu.TicketDescription.Length > 0)
                                {
                                    tmp += "[" + tu.TicketDescription + "]";
                                }
                                if (tu.WorkDetail.Length > 0)
                                {
                                    tmp += "\r\n[" + tu.WorkDetail + "]";
                                }
                                tmp += "\r\n";
                                dailyUserProjectHours += tu.Hours;
                                totalHours += tu.Hours;
                                j++;
                            }
                            RangeSetting(userTickets, tmp, false, false);
                            userTickets.Font.Color = this.DefaultColor;

                            Range userNameRange = (Range)ws.Cells[cellRowStartInner, 5];
                            RangeSetting(userNameRange, string.Format("{0} {1}", tUser.FirstName, tUser.LastName), false, false);
                            userNameRange.Font.Color = this.DefaultColor;

                            Range hourTmp = (Range)ws.Cells[cellRowStartInner, 6];
                            RangeSetting(hourTmp, dailyUserProjectHours.ToString("#,#0.00"), false, false);
                            hourTmp.Font.Color = this.DefaultColor;

                            hourTmp.NumberFormat = "#,#0.00";
                            cellRowStartInner++;
                            i++;
                        }


                        Range projectTitleRange;
                        if (i > 1)
                        {
                            DateRows = i;
                            projectTitleRange = (Range)ws.get_Range(ws.Cells[projectBeforeMerge, 3], ws.Cells[cellRowStartInner - 1, 3]);
                            projectTitleRange.Merge(False);
                        }
                        else
                        {
                            projectTitleRange = (Range)ws.Cells[projectBeforeMerge, 3];
                        }

                        RangeSetting(projectTitleRange, ts.ProjectTitle, false, false);
                        projectTitleRange.Font.Color = this.DefaultColor;
                        projectBeforeMerge = cellRowStartInner;

                    }

                    Range dateRange;
                    if (DateRows > 1)
                    {
                        dateRange = (Range)ws.get_Range(ws.Cells[cellRowStart, 2], ws.Cells[cellRowStartInner - 1, 2]);
                        dateRange.Merge(False);
                    }
                    else
                    {
                        dateRange = (Range)ws.Cells[cellRowStart, 2];
                    }
                    RangeSetting(dateRange, dt.ToString("MM/dd/yyyy"), false, true);
                    dateRange.Font.Color = this.DefaultColor;
                    cellRowStart = cellRowStartInner;

                }

                #region end

                Range dataRange = (Range)ws.get_Range("B8", "H" + cellRowStart.ToString());
                dataRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(229, 235, 247));
                //for (int k = 8; k < cellRowStart; k++)
                //{

                //    if (k % 2 == 0)
                //    {
                //        dataRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(229, 235, 247));
                //    }
                //    else
                //    {
                //        dataRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(240, 243, 250));
                //    }
                //}


                Range wholeRange = (Range)ws.get_Range("B8", "H" + cellRowStart.ToString());
                if (tss.Count > 0)
                {
                    Range totalHoursName = (Range)ws.Cells[cellRowStart, 5];
                    RangeSetting(totalHoursName, "Total Hours:", true, false);
                    totalHoursName.Font.Color = ColorTranslator.ToOle(Color.FromArgb(0, 0, 102));
                    //totalHoursName.Borders.Color = ColorTranslator.ToOle(Color.FromArgb(0, 0, 102));


                    Range totalHoursColumn = (Range)ws.Cells[cellRowStart, 6];
                    //RangeSetting(totalHoursColumn, totalHours.ToString("f1"), true, false);
                    RangeSetting(totalHoursColumn, string.Format("=SUM(F8:F{0})", cellRowStart - 1), true, false);

                    totalHoursColumn.Font.Color = ColorTranslator.ToOle(Color.FromArgb(0, 0, 102));
                    totalHoursColumn.NumberFormat = "#,#0.00";

                    Range hourSameRowRange = (Range)ws.get_Range("B" + cellRowStart.ToString(), "D" + cellRowStart.ToString());
                    hourSameRowRange.Merge(False);

                    hourSameRowRange = (Range)ws.get_Range("G" + cellRowStart.ToString(), "H" + cellRowStart.ToString());
                    hourSameRowRange.Merge(False);

                    hourSameRowRange = (Range)ws.get_Range("B" + cellRowStart.ToString(), "H" + cellRowStart.ToString());


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
                for (Char a = 'B'; a <= 'H'; a = Convert.ToChar(Convert.ToInt16(a) + 1))
                {
                    Range tmpRange = (Range)ws.get_Range(a.ToString() + "8", a.ToString() + (cellRowStart - 1).ToString());
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
            }
                #endregion
            return SaveExcel(ws, wb);
        }


        private string GenerateSearchProjectResultReport(List<TimeSheetTicket> tss, string projectName)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Template\\AllTemplate_Project.xlsx";
            wb = app.Workbooks.Open(filePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value
                , Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            ws = (Worksheet)wb.Worksheets[1];


            Range rng = ws.get_Range(ws.Cells[1, 1], ws.Cells[999, 20]);
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
                        strTmp += "[" + entity.TicketDescription + "]\t\n";
                    }
                    if (entity.WorkDetail.Length > 0)
                    {
                        strTmp += "[" + entity.WorkDetail + "]\t\n";
                    }
                    Range userTicketsDesc = (Range)ws.Cells[cellRowStartInner, 4];
                    RangeSetting(userTicketsDesc, strTmp, false, false);
                    userTicketsDesc.Font.Color = this.DefaultColor;

                    Range userNameRange = (Range)ws.Cells[cellRowStartInner, 5];
                    RangeSetting(userNameRange, string.Format("{0} {1}", entity.FirstName, entity.LastName), false, false);
                    userNameRange.Font.Color = this.DefaultColor;

                    Range usrRole = (Range)ws.Cells[cellRowStartInner, 6];
                    RangeSetting(usrRole, entity.RoleName, false, false);
                    usrRole.Font.Color = this.DefaultColor;


                    Range hourTmp = (Range)ws.Cells[cellRowStartInner, 7];
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
                        dateRange = (Range)ws.get_Range(ws.Cells[cellRowStart + i, 2], ws.Cells[cellRowStart + i + (row - 1), 2]);
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

                Range dataRange = (Range)ws.get_Range("B8", "I" + cellRowStart.ToString());
                dataRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(229, 235, 247));

                Range wholeRange = (Range)ws.get_Range("B8", "I" + cellRowStart.ToString());
                if (tss.Count > 0)
                {
                    Range totalHoursName = (Range)ws.Cells[cellRowStart, 6];
                    RangeSetting(totalHoursName, "Total Hours:", true, false);
                    totalHoursName.Font.Color = ColorTranslator.ToOle(Color.FromArgb(0, 0, 102));
                    //totalHoursName.Borders.Color = ColorTranslator.ToOle(Color.FromArgb(0, 0, 102));


                    Range totalHoursColumn = (Range)ws.Cells[cellRowStart, 7];
                    //RangeSetting(totalHoursColumn, totalHours.ToString("f1"), true, false);
                    RangeSetting(totalHoursColumn, string.Format("=SUM(G8:G{0})", cellRowStart - 1), true, false);

                    totalHoursColumn.Font.Color = ColorTranslator.ToOle(Color.FromArgb(0, 0, 102));
                    totalHoursColumn.NumberFormat = "#,#0.00";

                    Range hourSameRowRange = (Range)ws.get_Range("B" + cellRowStart.ToString(), "E" + cellRowStart.ToString());
                    hourSameRowRange.Merge(False);


                    hourSameRowRange = (Range)ws.get_Range("B" + cellRowStart.ToString(), "I" + cellRowStart.ToString());


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
                for (Char a = 'B'; a <= 'I'; a = Convert.ToChar(Convert.ToInt16(a) + 1))
                {
                    Range tmpRange = (Range)ws.get_Range(a.ToString() + "8", a.ToString() + (cellRowStart - 1).ToString());
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
            }
                #endregion
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
            Range rng = ws.get_Range(ws.Cells[1, 1], ws.Cells[999, 20]);
            object tmp = "{UserName}";
            Range userRange = rng.Find(tmp, Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSearchDirection.xlNext, Missing.Value, Missing.Value, Missing.Value);
            if (userRange != null)
            {
                userRange.Value2 = "SunNet - " + user.FirstName;
                userRange.Font.Color = this.DefaultColor;
            }
            #endregion


            Range projectRange = ws.get_Range(ws.Cells[1, 1], ws.Cells[999, 20]);
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
                Range rang = ws.get_Range(ws.Cells[1, 1], ws.Cells[999, 20]);
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
                        dateRange = (Range)ws.get_Range(ws.Cells[cellRowStart + i, 2], ws.Cells[cellRowStart + i + (row - 1), 2]);
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
                    Range dataRange = (Range)ws.get_Range("B" + k.ToString(), "H" + k.ToString());
                    if (k % 2 == 0)
                    {
                        dataRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(229, 235, 247));
                    }
                    else
                    {
                        dataRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(240, 243, 250));
                    }
                }





                Range wholeRange = (Range)ws.get_Range("B8", "H" + cellRowStart.ToString());
                if (tss.Count > 0)
                {
                    Range totalHoursName = (Range)ws.Cells[cellRowStart, 5];
                    RangeSetting(totalHoursName, "Total Hours:", true, false);
                    totalHoursName.Font.Color = ColorTranslator.ToOle(Color.FromArgb(0, 0, 102));





                    Range totalHoursColumn = (Range)ws.Cells[cellRowStart, 6];
                    //RangeSetting(totalHoursColumn, totalHours.ToString("f1"), true, false);

                    RangeSetting(totalHoursColumn, string.Format("=SUM(F8:F{0})", cellRowStart - 1), true, false);

                    totalHoursColumn.NumberFormat = "#,#0.00";
                    totalHoursColumn.Font.Color = ColorTranslator.ToOle(Color.FromArgb(0, 0, 102));
                    Range hourSameRowRange = (Range)ws.get_Range("B" + cellRowStart.ToString(), "D" + cellRowStart.ToString());
                    hourSameRowRange.Merge(False);

                    hourSameRowRange = (Range)ws.get_Range("G" + cellRowStart.ToString(), "H" + cellRowStart.ToString());
                    hourSameRowRange.Merge(False);

                    hourSameRowRange = (Range)ws.get_Range("B" + cellRowStart.ToString(), "H" + cellRowStart.ToString());


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
                    Range tmpRange = (Range)ws.get_Range(a.ToString() + "8", a.ToString() + (cellRowStart - 1).ToString());
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


                Range topRange = (Range)ws.get_Range("F6", "H6");
                Range clientRange = topRange.Find("{Client}", Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSearchDirection.xlNext, Missing.Value, Missing.Value, Missing.Value);

                RangeSetting(clientRange, "", false, false);
                Range rngClientName = topRange.Find("Client:", Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSearchDirection.xlNext, Missing.Value, Missing.Value, Missing.Value);
                topRange.Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlLineStyleNone;
                RangeSetting(rngClientName, "", false, false);

                #endregion
            }

            return SaveExcel(ws, wb);
        }
    }
}
