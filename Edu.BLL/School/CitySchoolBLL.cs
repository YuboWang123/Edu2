using Edu.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using Wyb.DbUtility;
using Edu.Entity.CitySchool;
using System.Data;
using Edu.Entity.School;

namespace Edu.BLL.School
{
    public class CitySchoolBLL
    {
        public CitySchoolBLL()
        {
            _citySchoolDAL = new DAL.CitySchool();
        }

        private Edu.DAL.CitySchool _citySchoolDAL;

        #region City array
        private DataTable GetCity(string prvid)
        {
            return _citySchoolDAL.GetCity(prvid);
        }
         
        private DataTable GetProvince()
        {
            return _citySchoolDAL.GetProvince();
        }


        private DataTable GetDownCountries(string parentId)
        {
            return _citySchoolDAL.GetDownCountries(parentId);
        }
        /// <summary>
        /// provinces
        /// </summary>
        /// <returns></returns>
        public List<City> GetModelList()
        {
            return TableToModel<City>.FillModel(GetProvince());
        }

        /// <summary>
        /// cities in the province.
        /// </summary>
        /// <param name="provinceid"></param>
        /// <returns></returns>
        public List<City> GetCitiesMdl(string provinceid)
        {
            return TableToModel<City>.FillModel(GetCity(provinceid));
        }

        public List<City> GetDownCountriesMdl(string parentId)
        {
            return TableToModel<City>.FillModel(GetDownCountries(parentId));
        }

        #endregion

        #region School Type

        public int GetSchoolType(string schoolId)
        {
            return _citySchoolDAL.GetSchoolType(schoolId);
        }

        /// <summary>
        /// get all base type of schools
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CitySchoolType> GetSchoolType()
        {
            return TableToModel<CitySchoolType>.FillModel(_citySchoolDAL.GetSchoolType());

        }

        public IEnumerable<SchoolEntity> GetCitySchools(string cityid)
        {
            throw new NotImplementedException();
        }

        #endregion



    }
}
