namespace VACalculatorApp;

public class CalculateRate
{
    private static readonly Dictionary<string, float> _combinedRateCache = new();
    
    private static string GetRateKey(int disabilityPercentage, bool married, int parents, int children)
    {
        // Normalize inputs
        parents = Math.Clamp(parents, 0, 2);
        children = children > 0 ? 1 : 0; // Data only distinguishes between 0 or 1+
        
        return $"{disabilityPercentage}-{married}-{parents}-{children}";
    }
    
    // Gets compensation rate for disability percentage and dependents
    public static float GetRate(int disabilityPercentage, bool married, int parents, int children)
    {
        string key = GetRateKey(disabilityPercentage, married, parents, children);
        
        // Use the CompensationRateDictionary.GetRates() method to access the rates
        var rates = CompensationRateDictionary.GetRates();
        if (rates.TryGetValue(key, out float rate))
            return rate;
            
        return -1; 
    }
    
    public static string GetFormattedRate(int disabilityPercentage, bool married, int parents, int children)
    {
        float rate = GetRate(disabilityPercentage, married, parents, children);
        return rate >= 0 ? rate.ToString("C") : "Rate not available";
    }
    
    public static List<CompensationRate> GetAllRates()
    {
        var result = new List<CompensationRate>();
        
        foreach (var entry in CompensationRateDictionary.GetRates())
        {
            string[] parts = entry.Key.Split('-');
            
            result.Add(new CompensationRate
            {
                DisabilityPercentage = int.Parse(parts[0]),
                Married = bool.Parse(parts[1]),
                Parents = int.Parse(parts[2]),
                Children = int.Parse(parts[3]),
                Amount = entry.Value
            });
        }
        return result;
    }
    
    public static float CalculateCombinedRate(bool married, int parents, int children, List<int> disabilityPercentages)
    {
        if (disabilityPercentages == null || disabilityPercentages.Count == 0)
            return 0;
            
        string cacheKey = $"{string.Join(",", disabilityPercentages.OrderBy(p => p))}-{married}-{parents}-{children}";
        
        if (_combinedRateCache.TryGetValue(cacheKey, out float cachedRate))
            return cachedRate;
        
        int combinedPercentage = CombineDisabilityRatings(disabilityPercentages);
        
        float rate = GetRate(combinedPercentage, married, parents, children);
        
        _combinedRateCache[cacheKey] = rate;
        
        return rate;
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
        
        int roundedCombined = (int)Math.Round(combined / 10.0) * 10;
        
        return Math.Min(roundedCombined, 100);
    }
    
    public static float CalculateTotalCompensation(int disabilityPercentage, bool married, int parents, int children, 
        int childrenOver18InSchool, int additionalChildrenUnder18 = 0, bool spouseReceivingAidAndAttendance = false)
    {
        float baseRate = GetRate(disabilityPercentage, married, parents, children);

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
        
            // Add for spouse Aid and Attendance (only applies to 70%+ ratings)
            if (married && spouseReceivingAidAndAttendance && disabilityPercentage >= 70)
            {
                float aidAndAttendanceBonus = CompensationRateDictionary.GetSpouseAidAttendanceRate(disabilityPercentage);
                baseRate += aidAndAttendanceBonus;
            }
        }
        return baseRate;
    }
    
    public static CompensationRate FindClosestRate(int disabilityPercentage, bool married, int parents, int children)
    {
        // Get all rates
        var rates = GetAllRates();
    
        // Find the closest match by disability percentage, then by other factors
        return rates.OrderBy(rate => Math.Abs(rate.DisabilityPercentage - disabilityPercentage))
            .ThenBy(rate => Math.Abs((rate.Married ? 1 : 0) - (married ? 1 : 0)))
            .ThenBy(rate => Math.Abs(rate.Parents - parents))
            .ThenBy(rate => Math.Abs(rate.Children - children))
            .FirstOrDefault();
    }
}