using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

#region Version Info
/* ========================================================================
*
* Author: lynnm Date: 2013/11/26 16:21:04
* File Name: DocManagementEntity
* Version: 4.0.30319.1008
*
* ========================================================================
*/
#endregion

using FamilyBook.Entity.Common;

namespace FamilyBook.Entity.DocManagements
{
    public class DocManagementEntity : IShowUserName
    {
        public DocManagementEntity()
        {
            this.FileName = "";
            this.DisplayFileName = "";
            this.FileContentType = "";
            this.FileUrl = "";
            this.CreatedOn = DateTime.Now;
            this.UpdatedOn = DateTime.Now;
            this.Extenstions = "";
            this.FileSize = 0;
            this.IsDeleted = false;
        }

        public DocManagementEntity(IDataReader dataReader)
        {
            object ojb = new object();
            this.ID = Convert.ToInt32(dataReader["ID"]);
            this.ProjectID = Convert.ToInt32(dataReader["ProjectID"]);
            this.CompanyID = Convert.ToInt32(dataReader["CompanyID"]);
            this.UserID = Convert.ToInt32(dataReader["UserID"]);
            this.ParentID = Convert.ToInt32(dataReader["ParentID"]);
            this.Type = (DocType)Convert.ToInt16(dataReader["Type"]);
            this.FileName = dataReader["FileName"].ToString();
            this.DisplayFileName = dataReader["DisplayFileName"].ToString();
            this.FileContentType = dataReader["FileContentType"].ToString();
            this.FileUrl = dataReader["FileUrl"].ToString();
            this.UpdatedOn = Convert.ToDateTime(dataReader["UpdatedOn"]);
            this.CreatedOn = Convert.ToDateTime(dataReader["CreatedOn"]);
            this.Extenstions = dataReader["Extenstions"].ToString();
            this.FileSize = Convert.ToInt32(dataReader["FileSize"]);
            this.IsDeleted = Convert.ToBoolean(dataReader["IsDeleted"]);

            this.ProjectName = readerExists(dataReader, "ProjectName") ? dataReader["ProjectName"].ToString() : "";
            this.FirstName = readerExists(dataReader, "FirstName") ? dataReader["FirstName"].ToString() : "";
            this.LastName = readerExists(dataReader, "LastName") ? dataReader["LastName"].ToString() : "";
        }

        public int ID { get; set; }

        public int ProjectID { get; set; }

        public int CompanyID { get; set; }

        public int UserID { get; set; }

        public int ParentID { get; set; }

        /// <summary>
        /// 1: folder 2:file
        /// </summary>
        public DocType Type { get; set; }

        public string FileName { get; set; }

        public string DisplayFileName { get; set; }

        public string FileContentType { get; set; }

        public string FileUrl { get; set; }

        public DateTime UpdatedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Extenstions { get; set; }

        public int FileSize { get; set; }

        public bool IsDeleted { get; set; }

        //ext
        public string ProjectName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        bool readerExists(IDataReader dr, string columnName)
        {

            dr.GetSchemaTable().DefaultView.RowFilter = "ColumnName= '" +

            columnName + "'";

            return (dr.GetSchemaTable().DefaultView.Count > 0);

        }


        public string UserName
        {
            get { return FirstAndLastName; }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string FirstAndLastName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        public string LastNameAndFirst
        {
            get
            {
                return string.Format("{0}, {1}", LastName, FirstName);
            }
        }
    }
}
