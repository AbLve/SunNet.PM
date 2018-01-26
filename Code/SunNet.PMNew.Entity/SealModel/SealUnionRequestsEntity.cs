using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SunNet.PMNew.Entity.SealModel
{
    public class SealUnionRequestsEntity
    {
        public SealUnionRequestsEntity()
        {
        }

        public SealUnionRequestsEntity(SealsEntity entity , int sealRequestsID)
        {
            SealRequestsID = sealRequestsID;
            SealID = entity.ID;
            ApprovedBy = entity.Approver;
            ApprovedDate = DateTime.Parse("1753-1-1");
            SealedBy = entity.Owner;
            SealedDate = DateTime.Parse("1753-1-1");
            IsSealed = false;
        }

        public SealUnionRequestsEntity(IDataReader dataReader, bool isList)
        {
            ID = (int)dataReader["ID"];
            SealRequestsID = (int)dataReader["SealRequestsID"];
            SealID = (int)dataReader["SealID"];
            ApprovedBy = (int)dataReader["ApprovedBy"];
            ApprovedDate = (DateTime)dataReader["ApprovedDate"];
            SealedBy = (int)dataReader["SealedBy"];
            SealedDate = (DateTime)dataReader["SealedDate"];
            IsSealed = (bool)dataReader["IsSealed"];
        }

        /// <summary>
        /// 扩展实体
        /// </summary>
        /// <param name="dataReader"></param>
        public SealUnionRequestsEntity(IDataReader dataReader)
        {
            UserID = (int)dataReader["UserID"];
            Email = (string)dataReader["Email"];
            FirstName = (string)dataReader["FirstName"];
            LastName = (string)dataReader["LastName"];
        }

        public int ID { get; set; }

        public int SealRequestsID { get; set; }

        public int SealID { get; set; }

        public int ApprovedBy { get; set; }

        public DateTime ApprovedDate { get; set; }

        public int SealedBy { get; set; }

        public DateTime SealedDate { get; set; }

        /// <summary>
        /// 已盖章
        /// </summary>
        public bool IsSealed { get; set; }


        #region 扩展属性
        public int UserID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        #endregion

    }
}
