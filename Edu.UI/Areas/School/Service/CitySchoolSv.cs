using System;
using System.Collections.Generic;
using Edu.BLL.School;
using Edu.Entity.CitySchool;
using Edu.UI.Models;

namespace Edu.UI.Areas.School.Service
{

    /// <summary>
    /// city and school service.
    /// </summary>
    public class CitySchoolSv
    {


        private CitySchoolBLL citySchoolBLL;
        public CitySchoolSv()
        {
            citySchoolBLL = new CitySchoolBLL();
        }


        public List<City> GetProvince()
        {

            return citySchoolBLL.GetModelList();
       
        }


        public List<City> GetCity(string prvId)
        {
            return citySchoolBLL.GetCitiesMdl(prvId);
        }

        /// <summary>
        /// get countries of a city.
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<City> GetDownCountriesMdl(string parentId)
        {
            return citySchoolBLL.GetDownCountriesMdl(parentId);
        }


        public int GetSchoolType(string schoolId)
        {
            return citySchoolBLL.GetSchoolType(schoolId);
        }

        public IEnumerable<CitySchoolType> GetSchoolType()
        {
            return citySchoolBLL.GetSchoolType();
        }

        #region school crud


        /// <summary>
        /// get base_school table,raw entity ,this entity is related to School enity in  1 to 1 .
        /// </summary>
        /// <param name="schoolId"></param>
        /// <returns></returns>
        public CitySchool GetBaseSchoolById(string schoolId)
        {
            using (var _db = new ApplicationDbContext())
            {
                if (string.IsNullOrEmpty(schoolId))
                    return null;
                //return _db.Base_Schools.Find(schoolId);
            }
            throw new NotImplementedException();
        }



        #endregion
    }
}