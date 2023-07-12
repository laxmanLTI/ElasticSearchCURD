using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchData.Entity
{
    public class Cities
    {
        public string Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string PostalCode { get; set; }        
        public string Region { get; set; }
        public int Population { get; set; }
    }
}
