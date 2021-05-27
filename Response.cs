using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHRS_V1
{
    public class LeaveInfo
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AmPm { get; set; }
        public string LeaveTypeCode { get; set; }
        public string NumberOfDays { get; set; }
        public string LeaveType { get; set; }

    }

    public class Datum
    {
        public List<LeaveInfo> LeaveInfo { get; set; }
        public string Domain { get; set; }
        public string SAPFacCode { get; set; }
        public string StaffNo { get; set; }
        public string SAPDeptCode { get; set; }
        public string SapDept { get; set; }
        public string FacDeptLevelUnitCode { get; set; }
        public string SAPFaculty { get; set; }
        public string UnivLevelUnit { get; set; }
        public List<Object> JobInfo { get; set; }
        public string UnivLevelUnitCode { get; set; }
        public string FacDeptLevelUnit { get; set; }

    }

    public class Root
    {
        public string code { get; set; }
        public string msg { get; set; }
        public List<Datum> data { get; set; }
        public string ts { get; set; }

    }


}
