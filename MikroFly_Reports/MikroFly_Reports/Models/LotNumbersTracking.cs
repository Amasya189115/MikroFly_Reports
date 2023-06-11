using System;
using System.Collections.Generic;
using System.Text;

namespace MikroFly_Reports.Models
{
    public class LotNumbersTracking
    {
        public string Code { get; set; }
        public string Desc { get; set; }    
        public string LotNumber { get; set; }
        public double Module1 { get; set; } 
        public double Module2 { get; set; }
        public double Module3 { get; set; }
        public double Module4 { get; set; }
        public double Module5 { get; set; }
        public double Packed { get; set; }
        public double Sterilized { get; set; }
        public double Released { get; set; }
        public double Sold { get; set; }
    }
}
