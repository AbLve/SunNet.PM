using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FamilyBook.Entity.Common
{
    public class CountryEntity
    {
        public CountryEntity()
        { }

        public CountryEntity(IDataReader reader)
        {
            ID = Convert.ToInt32(reader["ID"]);
            Country = Convert.ToString(reader["Country"]);
            Language = Convert.ToInt32(reader["Language"]);
        }

        public int ID { get; set; }

        public string Country { get; set; }

        public int Language { get; set; }
    }
}
