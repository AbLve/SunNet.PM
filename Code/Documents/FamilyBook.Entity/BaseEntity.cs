using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyBook.Entity
{
    public class BaseEntity
    {

        public int ID { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
    }
}
