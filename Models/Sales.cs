using Microsoft.VisualBasic;
using System;

namespace PMS.Models
{
    public class Sales
    {
        public int sale_id { get; set; }
        public string sale_date { get; set; }
        public string sale_name { get; set; }
        public string sale_type { get; set; }
        public int sale_value { get; set; }
        public string sale_description { get; set; }
    }
}
