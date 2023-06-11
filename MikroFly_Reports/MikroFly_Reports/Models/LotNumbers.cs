using System;
using System.Collections.Generic;
using System.Text;

namespace MikroFly_Reports.Models
{
    public class LotNumbers
    {
        public string Desc { get; set; }    
        public string Lot { get; set; }    
        public string Cycle { get; set; }
        public DateTime ExpDate { get; set; }
        public double Quantity { get; set; }

    }
}
