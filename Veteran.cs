namespace VACalculatorApp
{
    public class Veteran
    {
        public int DisabilityPercentage { get; set; }
        public bool IsMarried { get; set; }
        public int ParentCount { get; set; }
        public int ChildrenUnder18Count { get; set; }
        public int AdditionalChildrenUnder18Count { get; set; }
        public int ChildrenOver18InSchoolCount { get; set; }
        public bool SpouseReceivingAidAndAttendance { get; set; }
        
        public Veteran()
        {
            // Default constructor
        }
        
        public Veteran(int disabilityPercentage, bool isMarried, int parentCount, int childrenUnder18Count, 
            int childrenOver18InSchoolCount, int additionalChildrenUnder18Count = 0, 
            bool spouseReceivingAidAndAttendance = false)
        {
            DisabilityPercentage = disabilityPercentage;
            IsMarried = isMarried;
            ParentCount = parentCount;
            ChildrenUnder18Count = childrenUnder18Count;
            ChildrenOver18InSchoolCount = childrenOver18InSchoolCount;
            AdditionalChildrenUnder18Count = additionalChildrenUnder18Count;
            SpouseReceivingAidAndAttendance = spouseReceivingAidAndAttendance;
        }
    }
}