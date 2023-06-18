using System;
using System.Collections.Generic;
using System.Text;

namespace MikroFly_Reports.Models
{
    public class SalesRecords
    {
        public string Country { get; set; }
        public string Customer { get; set; }
        public string LotNu { get; set; }
        public string SterNu { get; set; }
        public string Product { get; set; }
        public float Quantity { get; set; }
        public float EuroValue { get; set; }
        public DateTime Date { get; set; }
        public string Group { get; set; }
    }
}
