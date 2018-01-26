using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SunNet.PMNew.Entity.TimeSheetModel
{
    public class ExpandTimeSheetsEntity : TimeSheetsEntity
    {
        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>
        public static ExpandTimeSheetsEntity ReaderBind(IDataReader dataReader)
        {
            TimeSheetsEntity model = TimeSheetsEntity.ReaderBind(dataReader);
            ExpandTimeSheetsEntity expandModel = (ExpandTimeSheetsEntity)model;
            expandModel.TicketDescription = dataReader["TicketDescription"].ToString();
            return expandModel;
        }
        public string TicketDescription { get; set; }
    }
}
