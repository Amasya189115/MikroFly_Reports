using System;
using System.Collections.Generic;
using System.Text;

namespace MikroFly_Reports.Models
{
    public class PersonelInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string EntryDate { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string Group { get; set; }
        public string Title { get; set; }
        public string Education { get; set; }
        public string EducationLevel { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public string MilitaryService { get; set; } 
        public double Absentee { get; set; }  
        public string RemainedLeaves { get; set; }
        public string Contact { get; set; } 
    }
}
