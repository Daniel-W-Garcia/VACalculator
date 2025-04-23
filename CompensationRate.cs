namespace VACalculatorApp;

public class CompensationRate
{
    public int DisabilityPercentage { get; set; }
    public bool Married { get; set; }
    public int Parents { get; set; }
    public int Children { get; set; }
    public float Amount { get; set; }
    
    public string FormattedRate => Amount.ToString("C2");
    
    public string DependentStatus => 
        $"{(Married ? "Married" : "Single")}, {Parents} parent{(Parents != 1 ? "s" : "")}, {Children} child{(Children != 1 ? "ren" : "")}";
}

public static class CompensationRateDictionary
{
    // Dictionary to store all rates with keys in format: "percentage-married-parents-children"
    private static readonly Dictionary<string, float> _rates = new()
    {
        // 10% ratings (updated 4/23/2025)
        {"10-False-0-0", 175.51f},
        {"10-False-0-1", 175.51f},
        {"10-False-1-0", 175.51f},
        {"10-False-1-1", 175.51f},
        {"10-False-2-0", 175.51f},
        {"10-False-2-1", 175.51f},
        {"10-True-0-0", 175.51f},
        {"10-True-0-1", 175.51f},
        {"10-True-1-0", 175.51f},
        {"10-True-1-1", 175.51f},
        {"10-True-2-0", 175.51f},
        {"10-True-2-1", 175.51f},
        
        // 20% ratings (updated 4/23/2025)
        {"20-False-0-0", 346.95f},
        {"20-False-0-1", 346.95f},
        {"20-False-1-0", 346.95f},
        {"20-False-1-1", 346.95f},
        {"20-False-2-0", 346.95f},
        {"20-False-2-1", 346.95f},
        {"20-True-0-0", 346.95f},
        {"20-True-0-1", 346.95f},
        {"20-True-1-0", 346.95f},
        {"20-True-1-1", 346.95f},
        {"20-True-2-0", 346.95f},
        {"20-True-2-1", 346.95f},
        
        // 30% ratings (updated 4/23/2025)
        {"30-False-0-0", 537.42f},
        {"30-False-0-1", 579.42f},
        {"30-False-1-0", 588.42f},
        {"30-False-1-1", 630.42f},
        {"30-False-2-0", 639.42f},
        {"30-False-2-1", 681.42f},
        {"30-True-0-0", 601.42f},
        {"30-True-0-1", 648.42f},
        {"30-True-1-0", 652.42f},
        {"30-True-1-1", 699.42f},
        {"30-True-2-0", 703.42f},
        {"30-True-2-1", 750.42f},
        
        // 40% ratings (updated 4/23/2025)
        {"40-False-0-0", 774.16f},
        {"40-False-0-1", 831.16f},
        {"40-False-1-0", 842.16f},
        {"40-False-1-1", 899.16f},
        {"40-False-2-0", 910.16f},
        {"40-False-2-1", 967.16f},
        {"40-True-0-0", 859.16f},
        {"40-True-0-1", 922.16f},
        {"40-True-1-0", 927.16f},
        {"40-True-1-1", 990.16f},
        {"40-True-2-0", 995.16f},
        {"40-True-2-1", 1058.16f},
        
        // 50% ratings (updated 4/23/2025)
        {"50-False-0-0", 1102.04f},
        {"50-False-0-1", 1173.04f},
        {"50-False-1-0", 1187.04f},
        {"50-False-1-1", 1258.04f},
        {"50-False-2-0", 1272.04f},
        {"50-False-2-1", 1343.04f},
        {"50-True-0-0", 1208.04f},
        {"50-True-0-1", 1287.04f},
        {"50-True-1-0", 1293.04f},
        {"50-True-1-1", 1372.04f},
        {"50-True-2-0", 1378.04f},
        {"50-True-2-1", 1457.04f},
        
        // 60% ratings (updated 4/23/2025)
        {"60-False-0-0", 1395.93f},
        {"60-False-0-1", 1480.93f},
        {"60-False-1-0", 1497.93f},
        {"60-False-1-1", 1582.93f},
        {"60-False-2-0", 1599.93f},
        {"60-False-2-1", 1684.93f},
        {"60-True-0-0", 1523.93f},
        {"60-True-0-1", 1617.93f},
        {"60-True-1-0", 1625.93f},
        {"60-True-1-1", 1719.93f},
        {"60-True-2-0", 1727.93f},
        {"60-True-2-1", 1821.93f},
        
        // 70% ratings (updated 4/23/2025)
        {"70-False-0-0", 1759.19f},
        {"70-False-0-1", 1858.19f},
        {"70-False-1-0", 1879.19f},
        {"70-False-1-1", 1978.19f},
        {"70-False-2-0", 1999.19f},
        {"70-False-2-1", 2098.19f},
        {"70-True-0-0", 1908.19f},
        {"70-True-0-1", 2018.19f},
        {"70-True-1-0", 2028.19f},
        {"70-True-1-1", 2138.19f},
        {"70-True-2-0", 2148.19f},
        {"70-True-2-1", 2258.19f},
        
        // 80% ratings (updated 4/23/2025)
        {"80-False-0-0", 2044.89f},
        {"80-False-0-1", 2158.89f},
        {"80-False-1-0", 2181.89f},
        {"80-False-1-1", 2295.89f},
        {"80-False-2-0", 2318.89f},
        {"80-False-2-1", 2432.89f},
        {"80-True-0-0", 2214.89f},
        {"80-True-0-1", 2340.89f},
        {"80-True-1-0", 2351.89f},
        {"80-True-1-1", 2477.89f},
        {"80-True-2-0", 2488.89f},
        {"80-True-2-1", 2614.89f},
        
        // 90% ratings (updated 4/23/2025)
        {"90-False-0-0", 2297.96f},
        {"90-False-0-1", 2425.96f},
        {"90-False-1-0", 2451.96f},
        {"90-False-1-1", 2579.96f},
        {"90-False-2-0", 2605.96f},
        {"90-False-2-1", 2733.96f},
        {"90-True-0-0", 2489.96f},
        {"90-True-0-1", 2630.96f},
        {"90-True-1-0", 2643.96f},
        {"90-True-1-1", 2784.96f},
        {"90-True-2-0", 2797.96f},
        {"90-True-2-1", 2938.96f},
        
        // 100% ratings (updated 4/23/2025)
        {"100-False-0-0", 3831.30f},
        {"100-False-0-1", 3757.00f},
        {"100-False-1-0", 4002.74f},
        {"100-False-1-1", 3919.07f},
        {"100-False-2-0", 4174.18f},
        {"100-False-2-1", 4081.14f},
        {"100-True-0-0", 4044.91f},
        {"100-True-0-1", 3971.78f},
        {"100-True-1-0", 4216.35f},
        {"100-True-1-1", 4133.85f},
        {"100-True-2-0", 4387.79f},
        {"100-True-2-1", 4295.92f}
    };
    
    // Cache for combined ratings
    private static readonly Dictionary<string, float> _combinedRateCache = new();
    
    private static string GetRateKey(int disabilityPercentage, bool married, int parents, int children)
    {
        // Normalize inputs
        parents = Math.Clamp(parents, 0, 2);
        children = children > 0 ? 1 : 0; // Data only distinguishes between 0 or 1+
        
        return $"{disabilityPercentage}-{married}-{parents}-{children}";
    }
    
    /// <summary>
    /// Gets the compensation rate for a specific disability percentage and dependent status
    /// </summary>
    public static float GetRate(int disabilityPercentage, bool married, int parents, int children)
    {
        string key = GetRateKey(disabilityPercentage, married, parents, children);
        
        if (_rates.TryGetValue(key, out float rate))
            return rate;
            
        return -1; // Not found
    }
    
    /// <summary>
    /// Gets the formatted compensation rate (as currency string)
    /// </summary>
    public static string GetFormattedRate(int disabilityPercentage, bool married, int parents, int children)
    {
        float rate = GetRate(disabilityPercentage, married, parents, children);
        return rate >= 0 ? rate.ToString("C") : "Rate not available";
    }
    
    /// <summary>
    /// Gets all compensation rates as a list
    /// </summary>
    public static List<CompensationRate> GetAllRates()
    {
        var result = new List<CompensationRate>();
        
        foreach (var entry in _rates)
        {
            // Parse the key: "percentage-married-parents-children"
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
    
    /// <summary>
    /// Calculates the combined rate based on multiple disability percentages
    /// </summary>
    public static float CalculateCombinedRate(bool married, int parents, int children, List<int> disabilityPercentages)
    {
        if (disabilityPercentages == null || disabilityPercentages.Count == 0)
            return 0;
            
        // Create a cache key for this specific combination
        string cacheKey = $"{string.Join(",", disabilityPercentages.OrderBy(p => p))}-{married}-{parents}-{children}";
        
        // Check if we've calculated this before
        if (_combinedRateCache.TryGetValue(cacheKey, out float cachedRate))
            return cachedRate;
        
        // Calculate combined percentage using VA's formula
        int combinedPercentage = CombineDisabilityRatings(disabilityPercentages);
        
        // Get rate for the combined percentage
        float rate = GetRate(combinedPercentage, married, parents, children);
        
        // Cache the result
        _combinedRateCache[cacheKey] = rate;
        
        return rate;
    }
    
    /// <summary>
    /// Calculates the combined disability rating percentage using VA's formula
    /// </summary>
    public static int CombineDisabilityRatings(List<int> ratings)
    {
        if (ratings == null || ratings.Count == 0)
            return 0;
            
        if (ratings.Count == 1)
            return ratings[0];
            
        // Sort percentages in descending order (highest first)
        var sortedRatings = ratings.OrderByDescending(r => r).ToList();
        
        // Start with the highest rating
        double efficiency = 100.0;
        double combined = 0.0;
        
        foreach (var rating in sortedRatings)
        {
            // Calculate the additional percentage (VA formula)
            combined += (efficiency * rating) / 100.0;
            
            // Reduce efficiency for the next rating
            efficiency = (100.0 - combined) / 100.0 * 100.0;
        }
        
        // Round to the nearest 10%
        int roundedCombined = (int)Math.Round(combined / 10.0) * 10;
        
        // Ensure it doesn't exceed 100%
        return Math.Min(roundedCombined, 100);
    }
}