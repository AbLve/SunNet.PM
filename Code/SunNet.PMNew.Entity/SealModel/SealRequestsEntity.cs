using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using SunNet.PMNew.Entity.Common;
using SunNet.PMNew.Framework.Core;


namespace SunNet.PMNew.Entity.SealModel
{
    public class SealRequestsEntity : BaseEntity, IShowUserName
    {

        public SealRequestsEntity() { }

        public SealRequestsEntity(IDataReader Reader, bool isList)
        {
            ID = (int)Reader["ID"];
            Title = (string)Reader["Title"];
            Description = (string)Reader["Description"];
            RequestedBy = (int)Reader["RequestedBy"];
            RequestedDate = (DateTime)Reader["RequestedDate"];
            Status = (RequestStatus)(int)Reader["Status"];
            Type = (int)Reader["Type"];

            if (isList)
            {
                FirstName = (string)Reader["RequestedFirstName"];
                LastName = (string)Reader["RequestedLastName"];
            }
        }


        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        ///// <summary>
        ///// 公章ID
        ///// </summary>
        //public int SealID { get; set; }

        public int RequestedBy { get; set; }

        public DateTime RequestedDate { get; set; }

        //public int ApprovedBy { get; set; }

        //public DateTime ApprovedDate { get; set; }

        public RequestStatus Status { get; set; }

        /// <summary>
        /// 0 for Seal, 1 for other work flows
        /// </summary>
        public int Type { get; set; }

        //public int SealedBy { get; set; }

        //public DateTime SealedDate { get; set; }


        #region 扩展属性

        public List<SealsEntity> SealList { get; set; }

        /// <summary>
        /// 获取列表时，有效
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// 获取列表时，有效
        /// </summary>
        public string LastName { get; set; }

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
        #endregion

    }
}
