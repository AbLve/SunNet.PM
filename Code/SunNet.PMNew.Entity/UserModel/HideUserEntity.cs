using SunNet.PMNew.Framework.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SunNet.PMNew.Entity.UserModel
{
    public class HideUserEntity : BaseEntity
    {
        public static HideUserEntity ReaderBind(IDataReader dataReader)
        {
            HideUserEntity model = new HideUserEntity();
            object ojb;
            ojb = dataReader["ID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.ID = (int)ojb;
            }
            ojb = dataReader["UserID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.UserID = (int)ojb;
            }
            model.HideUserIds = dataReader["HideUserIds"].ToString();
            return model;
        }
        public int UserID { get; set; }
        public string HideUserIds { get; set; }
    }
}
