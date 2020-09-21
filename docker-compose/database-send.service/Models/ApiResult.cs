using System;
using System.Collections.Generic;

namespace database_send.service.Models
{
    public class ApiResult
    {
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string CityCode { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public int Confirmed { get; set; }
        public int Deaths { get; set; }
        public int Recovered { get; set; }
        public int Active { get; set; }
        public DateTime Date { get; set; }
    }

    public class Root
    {
        public List<ApiResult> CountryList { get; set; }
    }

}
