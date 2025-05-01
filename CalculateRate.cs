namespace VACalculatorApp;

public class CalculateRate
{
    private static readonly Dictionary<string, float> _combinedRateCache = new();
    
    private static string GetRateKey(int disabilityPercentage, bool married, int parents, int children)
    {
        // Normalize inputs
        parents = Math.Clamp(parents, 0, 2); // 2 parents max on VA site
        children = children > 0 ? 1 : 0; // Data only distinguishes between 0 or 1 on the VA table. We account for more children later
        
        return $"{disabilityPercentage}-{married}-{parents}-{children}"; //this is the string we use for the dictionary
    }
    
    // Gets compensation rate from dictionary
    public static float GetTableRate(int disabilityPercentage, bool married, int parents, int children)
    {
        string key = GetRateKey(disabilityPercentage, married, parents, children);
        
        // Use the CompensationRateDictionary.GetRateFromDictionary() method to access the rates
        var rates = CompensationRateDictionary.GetRateFromDictionary();
        if (rates.TryGetValue(key, out float tableRate))
            return tableRate;
            
        return -1; 
    }
    
    public static int CombineDisabilityRatings(List<int> ratings)
    {
        if (ratings == null || ratings.Count == 0)
            return 0;
            
        if (ratings.Count == 1)
            return ratings[0];
            
        var sortedRatings = ratings.OrderByDescending(r => r).ToList(); //sort the ratings highest to lowest to comply with how the VA does it
        
        double remainingPercentage = 100.0; //This is part of the wonky way the VA does math. it's a percentage from the percentage.
        double combinedPercentage = 0.0; //combined percentage starts
        
        foreach (var rating in sortedRatings)
        {
            // Each new rating only applies to the remaining percentage portion

            combinedPercentage += (remainingPercentage * rating) / 100.0;  //As the combined percentage increases, the remaining percentage decreases and future ratings have less of an impact on combined rating
            
            remainingPercentage = (100.0 - combinedPercentage); // We start with 100 and then subtract the highest remaining rating
        }
        
        int roundedCombined = (int)Math.Round(combinedPercentage / 10.0, MidpointRounding.AwayFromZero) * 10; // this was not easy to find. I couldn't figure out why my percentages were wrong! C# uses bankers rounding!
        
        return Math.Min(roundedCombined, 100); //cap at 100
    }
    
    public static float CalculateTotalCompensation(Veteran veteran)
    {
        float rate = GetTableRate(veteran.DisabilityPercentage, veteran.IsMarried, veteran.ParentCount, veteran.ChildrenUnder18Count);
        //start with a base table rate

        if (rate < 0)
        {
            return rate; // Rate not found
        }
        
        // Additional benefits only apply for 30%+ ratings
        if (veteran.DisabilityPercentage >= 30)
        {
            // Add for additional children under 18
            if (veteran.AdditionalChildrenUnder18Count > 0)
            {
                float childBonusUnder18 = CompensationRateDictionary.GetChildUnder18Rate(veteran.DisabilityPercentage); //get the amount per child from the dictionary
                rate += veteran.AdditionalChildrenUnder18Count * childBonusUnder18; //add the amount per child to the total rate
            }
        
            // Add for children over 18 in school
            if (veteran.ChildrenOver18InSchoolCount > 0)
            {
                float childBonusOver18InSchool = CompensationRateDictionary.GetChildOver18SchoolRate(veteran.DisabilityPercentage);
                rate += veteran.ChildrenOver18InSchoolCount * childBonusOver18InSchool;
            }
        
            // Add for spouse Aid and Attendance (only applies to 70%+ ratings) I don't have this in my app right now
            if (veteran.IsMarried && veteran.SpouseReceivingAidAndAttendance && veteran.DisabilityPercentage >= 70)
            {
                float aidAndAttendanceBonus = CompensationRateDictionary.GetSpouseAidAttendanceRate(veteran.DisabilityPercentage);
                rate += aidAndAttendanceBonus;
            }
        }
        return rate;
    }
}