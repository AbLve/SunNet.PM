using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SF.Framework.Core.Repository;
using FamilyBook.Entity.Common;

namespace FamilyBook.Core.Common
{
    public interface ICountryDAO : IRepository<CountryEntity>
    {
        List<CountryEntity> GetList();

        CountryEntity GetCountryByCountry(string country);
    }
}
