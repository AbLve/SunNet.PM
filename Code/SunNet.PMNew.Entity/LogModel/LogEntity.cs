using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunNet.PMNew.Entity.LogModel;
using System.ComponentModel.DataAnnotations;
using SunNet.PMNew.Framework.Core;
using System.Data;

namespace SunNet.PMNew.Entity.LogModel
{
    public class LogEntity : BaseEntity
    {
        /// <summary>
        /// Bind IDataReader to Entity
        /// </summary>

        public Int64 Id
        {
            get;
            set;
        }

        [Required]
        public int currentUserId
        {
            get;
            set;
        }

        [Required]
        public LogType logType
        {
            get;
            set;
        }

        public DateTime operatingTime
        {
            get;
            set;
        }
        [Required]
        [StringLength(200)]
        public string iPAddress
        {
            get;
            set;
        }

        [Required]
        [StringLength(200)]
        public string referrer
        {
            get;
            set;
        }

        [Required]
        [StringLength(500)]
        public string Description
        {
            get;
            set;
        }

        [Required]
        public bool IsSuccess
        {
            get;
            set;
        }
    }
}
