using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;

namespace SF.Framework.Helpers
{
    public static class ExcelExpand
    {
        private static float ExcelWith(string s)
        {
            Form frm = new Form();
            frm.Visible = false;
            Font f = new Font("Calibri", 11);
            Graphics g = frm.CreateGraphics();
            SizeF siF = g.MeasureString(s, f);
            frm.Close();
            return siF.Width * 3 / 4;
        }

        public static string GetDataSetXml(this DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0) return "";
            StringBuilder sb = new StringBuilder();
            sb.Append("<NewDataSet>\r\n");
            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    sb.AppendFormat("<{0}>\r\n", dt.TableName);
                    foreach (DataColumn dc in dt.Columns)
                    {
                        sb.AppendFormat("<{0}>{1}</{0}>\r\n", dc.ColumnName.ColumnNameReplace(), dr[dc.ColumnName].ToString().EntityReference());
                    }
                    sb.AppendFormat("</{0}>\r\n", dt.TableName);
                }
            }
            if (!ds.Tables.Contains("TableFields"))
            {
                sb.Append("<TableFields>" + Environment.NewLine);
                foreach (DataTable dt in ds.Tables)
                {
                    sb.Append("<Fields TableName=\"" + dt.TableName + "\">" + Environment.NewLine);
                    foreach (DataColumn column in dt.Columns)
                    {
                        float len = ExcelWith(column.ColumnName);
                        float tmpLen = len;
                        foreach (DataRow row in dt.Rows)
                        {
                            tmpLen = ExcelWith(row[column].ToString());
                            if (null != row[column] && tmpLen > len)
                            {
                                len = tmpLen;
                            }
                        }
                        sb.AppendFormat("<Field Name=\"{0}\" Width=\"{1}\"></Field>{2}", column.ColumnName, len, Environment.NewLine);
                        // sb.Append("<Field Name=\"" + column.ColumnName + "\" Width=\"" + Convert.ToString(len * 6.75 + 5.25) + "\"></Field>" + Environment.NewLine);
                    }
                    sb.Append("</Fields>" + Environment.NewLine);
                }
                sb.Append("</TableFields>" + Environment.NewLine);
            }
            sb.Append("</NewDataSet>");
            return sb.ToString();
        }

        public static string ColumnNameReplace(this string name)
        {
            char c = name[0];
            if (Char.IsNumber(c))
            {
                name = string.Format("_x003{0}_{1}", c, name.Substring(1));
            }
            name = name.Replace(" ", "_x0020_")
                .Replace("#", "_x0023_")
                .Replace("%", "_x0025_")
                .Replace("&", "_x0026_")
                .Replace("/", "_x002F_")
                .Replace("'", "_x0027_")
                .Replace("(", "_x0028_")
                .Replace(")", "_x0029_")
                .Replace("?", "_x003F_");
            return name;
        }

        public static string EntityReference(this string str)
        {
            return str.Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("&", "&amp;")
                .Replace("'", "&apos;")
                .Replace("\"", "&quot;");
        }
        public static void ExportToCSV<T>(List<T> entityList, List<string> headerColumns, List<string> bodyColumns, string fileName)
        {
            string str = "";
            str = str + GetHeaderString(headerColumns);
            List<PropertyInfo> list = new List<PropertyInfo>();
            foreach (string str2 in bodyColumns)
            {
                PropertyInfo property = typeof(T).GetProperty(str2);
                list.Add(property);
            }
            for (int i = 0; i < entityList.Count; i++)
            {
                foreach (PropertyInfo info in list)
                {
                    if (info.PropertyType.Name.Equals("Boolean"))
                    {
                        str = str + (((bool)info.GetValue(entityList[i], null)) ? "YES" : "NO") + ",";
                    }
                    else if (info.PropertyType.Name.Equals("DateTime"))
                    {
                        str = str + ((DateTime)info.GetValue(entityList[i], null)).ToString("MM/dd/yyyy") + ",";
                    }
                    else
                    {
                        str = str + FormatCSVString(info.GetValue(entityList[i], null).ToString()) + ",";
                    }
                }
                str = str + "\n";
            }
            WriteData(str + "\n", fileName);
        }

        public static void ExportToCSV(DataTable dt, List<string> headerColumns, List<string> bodyColumns, string fileName)
        {
            string str = "";
            str = str + GetHeaderString(headerColumns);
            foreach (DataRow row in dt.Rows)
            {
                foreach (string str2 in bodyColumns)
                {
                    if (dt.Columns[str2].DataType.Name.Equals("Boolean"))
                    {
                        str = str + (((bool)row[str2]) ? "YES" : "NO") + ",";
                    }
                    else if (dt.Columns[str2].DataType.Name.Equals("DateTime"))
                    {
                        str = str + ((DateTime)row[str2]).ToString("MM/dd/yyyy") + ",";
                    }
                    else
                    {
                        str = str + FormatCSVString(row[str2].ToString()) + ",";
                    }
                }
                str = str + "\n";
            }
            WriteData(str + "\n", fileName);
        }

        public static string FormatCSVString(string str)
        {
            if (str.IndexOf(',') != -1)
            {
                str = str.Replace("\"", "\"\"");
                str = "\"" + str + "\"";
            }
            return str;
        }

        public static string GetHeaderString(List<string> headerColumns)
        {
            string str = "";
            foreach (string str2 in headerColumns)
            {
                str = str + FormatCSVString(str2) + ",";
            }
            return (str + "\n");
        }

        public static void WriteData(string data, string fileName)
        {
            string str = string.Format("attachment;filename={0}", fileName);
            HttpResponse response = HttpContext.Current.Response;
            response.ClearHeaders();
            response.Clear();
            response.Charset = "UFT-8";
            response.ContentType = "application/excel";
            response.AppendHeader("Content-disposition", str);
            response.Write(data);
            response.End();
        }
    }
}
