using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Edu.Entity.CitySchool
{
    [Serializable]
    public class City
    {
        [Key][StringLength(50)]public string id { get; set; }
        public string name { get; set; }
        public double parentid { get; set; }
        public double level { get; set; }
       // public string first { get; set; }
        public double ismunicipality { get; set; }
        public double hasschool { get; set; }       
    }


    public class CitySchool
    {
        [Key][StringLength(50)]public string id { get; set; }
        public string schoolname { get; set; }
        public int city_id { get; set; }
        public int school_type { get; set; }     
        public string py { get; set; }
        public int country_id { get; set; }
        public int province_id { get; set; }
        public int status { get; set; }
        public int sctype { get; set; }
        public int user_define { get; set; }       

    }

    public class CitySchoolType
    {
        [Key] public string Id { get; set; }
        public string name { get; set; } //type name
    }



   
}
