using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Framework.Core;
using System.Data;

namespace SunNet.PMNew.Entity.SealModel
{
    public class SealsEntity : BaseEntity
    {
        public SealsEntity()
        {
        }

        public SealsEntity(IDataReader Reader, bool isList)
        {
            ID = (int)Reader["ID"];
            SealName = (string)Reader["SealName"];
            Owner = (int)Reader["Owner"];
            Approver = (int)Reader["Approver"];
            Description = (string)Reader["Description"];
            Status = (Status)(int)Reader["Status"];

            if (isList)
            {
                OwnerFirstName = (string)Reader["OwnerFirstName"];
                OwnerLastName = (string)Reader["OwnerLastName"];
                ApproverFirstName = (string)Reader["ApproverFirstName"];
                ApproverLastName = (string)Reader["ApproverLastName"];
            }
        }

        public int ID { get; set; }

        public string SealName { get; set; }

        public int Owner { get; set; }

        public int Approver { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Active = 0 ,Inactive = 1
        /// </summary>
        public Status Status { get; set; }

        #region 扩展属性

        public string OwnerFirstName { get; set; }

        public string OwnerLastName { get; set; }

        public string ApproverFirstName { get; set; }

        public string ApproverLastName { get; set; }

        /// <summary>
        /// 给 Seal Request 页面所用的
        /// </summary>
        public bool Checked { get; set; }
        #endregion
    }
}
