using System;
using System.Collections.Generic;
using System.Text;

namespace MikroFly_Reports.Models
{
    public class SalesRecordsFiltered
    {
        public string Country { get; set; }
        public string Customer { get; set; }
        public string Group { get; set; }
        public string Product { get; set; }
        public DateTime ShipmentDate { get; set; }
        public float Amount { get; set; }
        public float EuroValue { get; set; }
        public string Currency { get; set; }
    }
}
