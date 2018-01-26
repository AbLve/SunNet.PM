using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FamilyBook.Core.Common;
using FamilyBook.Entity.Common; 

namespace FamilyBook.Core.Common
{
    public class CountryService
    {
        ICountryDAO dao;
        public CountryService(ICountryDAO dao)
        {
            this.dao = dao;
        }

        public List<CountryEntity> GetList()
        {
            return dao.GetList();
        }

        public CountryEntity GetCountryByCountry(string country)
        {
            return dao.GetCountryByCountry(country);
        }
    }
}
