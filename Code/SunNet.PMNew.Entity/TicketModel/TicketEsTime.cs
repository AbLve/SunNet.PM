using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SunNet.PMNew.Entity.TicketModel
{
    [Serializable]
    public class TicketEsTime
    {
        public static TicketEsTime ReaderBind(IDataReader dataReader)
        {
            TicketEsTime model = new TicketEsTime();
            object ojb;
            ojb = dataReader["EsID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.EsID = (int)ojb;
            }
            ojb = dataReader["TicketID"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TicketID = (int)ojb;
            }
            ojb = dataReader["Week"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Week = ojb.ToString();
            }
            ojb = dataReader["QaAdjust"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.QaAdjust = (decimal)ojb;
            }
            ojb = dataReader["DevAdjust"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.DevAdjust = (decimal)ojb;
            }
            ojb = dataReader["GrapTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.GrapTime = (decimal)ojb;
            }
            ojb = dataReader["DocTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.DocTime = (decimal)ojb;
            }
            ojb = dataReader["TrainingTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TrainingTime = (decimal)ojb;
            }
            ojb = dataReader["TotalTimes"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.TotalTimes = (decimal)ojb;
            }
            ojb = dataReader["EsByUserId"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.EsByUserId = (int)ojb;
            }
            ojb = dataReader["CreatedTime"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.CreatedTime = (DateTime)ojb;
            }
            ojb = dataReader["Remark"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.Remark = ojb.ToString();
            }
            ojb = dataReader["IsPM"];
            if (ojb != null && ojb != DBNull.Value)
            {
                model.IsPM = (bool)ojb;
            }
            return model;
        }

        #region
        /// <summary>
        /// EsID
        /// </summary>
        public int EsID { get; set; }

        /// <summary>
        /// TicketID
        /// </summary>
        public int TicketID { get; set; }
        /// <summary>
        /// Week
        /// </summary>
        public string Week { get; set; }
        /// <summary>
        /// QaAdjust
        /// </summary>
        public decimal QaAdjust { get; set; }

        /// <summary>
        /// DevAdjust
        /// </summary>
        public decimal DevAdjust { get; set; }

        /// <summary>
        /// GrapTime
        /// </summary>
        public decimal GrapTime { get; set; }


        /// <summary>
        /// DocTime
        /// </summary>
        public decimal DocTime { get; set; }


        /// <summary>
        /// TrainingTime
        /// </summary>
        public decimal TrainingTime { get; set; }

        /// <summary>
        /// TotalTimes
        /// </summary>
        public decimal TotalTimes { get; set; }

        /// <summary>
        /// EsByUserId
        /// </summary>
        public int EsByUserId { get; set; }

        /// <summary>
        /// CreatedTime
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// IsPM
        /// </summary>
        public bool IsPM { get; set; }

        #endregion Model
    }
}
