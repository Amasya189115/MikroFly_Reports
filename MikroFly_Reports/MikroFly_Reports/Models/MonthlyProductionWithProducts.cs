using System;
using System.Collections.Generic;
using System.Text;

namespace MikroFly_Reports.Models
{
    public class MonthlyProductionWithProducts
    {
        public string Month { get; set; }
        public string Desc { get; set; }
        public string Lot { get; set; }
        public float Value { get; set; }
    }
}
