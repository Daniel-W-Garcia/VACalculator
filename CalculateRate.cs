namespace VACalculatorApp;

public class CalculateRate
{
    private static readonly Dictionary<string, float> _combinedRateCache = new();
    
    private static string GetRateKey(int disabilityPercentage, bool married, int parents, int children)
    {
        // Normalize inputs
        parents = Math.Clamp(parents, 0, 2);
        children = children > 0 ? 1 : 0; // Data only distinguishes between 0 or 1 in the VA table. we account for more children later
        
        return $"{disabilityPercentage}-{married}-{parents}-{children}";
    }
    
    // Gets compensation rate from dictionary
    public static float GetTableRate(int disabilityPercentage, bool married, int parents, int children)
    {
        string key = GetRateKey(disabilityPercentage, married, parents, children);
        
        // Use the CompensationRateDictionary.GetRateFromDictionary() method to access the rates
        var rates = CompensationRateDictionary.GetRateFromDictionary();
        if (rates.TryGetValue(key, out float rate))
            return rate;
            
        return -1; 
    }
    
    public static int CombineDisabilityRatings(List<int> ratings)
    {
        if (ratings == null || ratings.Count == 0)
            return 0;
            
        if (ratings.Count == 1)
            return ratings[0];
            
        var sortedRatings = ratings.OrderByDescending(r => r).ToList();
        
        double efficiency = 100.0;
        double combined = 0.0;
        
        foreach (var rating in sortedRatings)
        {
            combined += (efficiency * rating) / 100.0;
            
            efficiency = (100.0 - combined) / 100.0 * 100.0;
        }
        
        int roundedCombined = (int)Math.Round(combined / 10.0, MidpointRounding.AwayFromZero) * 10; // this was not easy to find. I couldn't figure out why my percentages were wrong! C# uses bankers rounding!
        
        return Math.Min(roundedCombined, 100);
        
    }
    
    public static float CalculateTotalCompensation(int disabilityPercentage, bool married, int parents, int children, // these params are ridiculous. might refactor into puting a method or delegate here later.
        int childrenOver18InSchool, int additionalChildrenUnder18 = 0, bool spouseReceivingAidAndAttendance = false)
    {
        float baseRate = GetTableRate(disabilityPercentage, married, parents, children);

        if (baseRate < 0)
        {
            return baseRate; // Rate not found
        }
        
        // Additional benefits only apply for 30%+ ratings
        if (disabilityPercentage >= 30)
        {
            // Add for additional children under 18
            if (additionalChildrenUnder18 > 0)
            {
                float childBonusUnder18 = CompensationRateDictionary.GetChildUnder18Rate(disabilityPercentage);
                baseRate += additionalChildrenUnder18 * childBonusUnder18;
            }
        
            // Add for children over 18 in school
            if (childrenOver18InSchool > 0)
            {
                float childBonusOver18InSchool = CompensationRateDictionary.GetChildOver18SchoolRate(disabilityPercentage);
                baseRate += childrenOver18InSchool * childBonusOver18InSchool;
            }
        
            // Add for spouse Aid and Attendance (only applies to 70%+ ratings) I don't have this in my app right now
            if (married && spouseReceivingAidAndAttendance && disabilityPercentage >= 70)
            {
                float aidAndAttendanceBonus = CompensationRateDictionary.GetSpouseAidAttendanceRate(disabilityPercentage);
                baseRate += aidAndAttendanceBonus;
            }
        }
        return baseRate;
    }
}