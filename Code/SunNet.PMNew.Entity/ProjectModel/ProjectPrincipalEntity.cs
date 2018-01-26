using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using SunNet.PMNew.Framework.Core;
using System.ComponentModel.DataAnnotations;
using SunNet.PMNew.Entity.UserModel;


namespace SunNet.PMNew.Entity.ProjectModel
{
    public class ProjectPrincipalEntity
    {
        public static ProjectPrincipalEntity ReaderBind(IDataReader dataReader)
        {
            ProjectPrincipalEntity model = new ProjectPrincipalEntity();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ID = (int)ojb;
            }
            ojb = dataReader["ProjectID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ProjectID = (int)ojb;
            }
            model.Module = dataReader["Module"].ToString();
            model.PM = dataReader["PM"].ToString();
            model.DEV = dataReader["DEV"].ToString();
            model.QA = dataReader["QA"].ToString();
            return model;
        }
        public int ID { get; set; }
        public int ProjectID { get; set; }
        public string Module { get; set; }
        public string PM { get; set; }
        public string DEV { get; set; }
        public string QA { get; set; }
    }
}
