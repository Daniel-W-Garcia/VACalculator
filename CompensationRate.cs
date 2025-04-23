namespace VACalculatorApp;

public class CompensationRate
{
    public int DisabilityPercentage { get; set; }
    public bool Married { get; set; }
    public int Parents { get; set; }
    public int Children { get; set; }
    public decimal Amount { get; set; }
    
    public string FormattedRate => Amount.ToString("C");
    
    public string DependentStatus => 
        $"{(Married ? "Married" : "Single")}, {Parents} parent{(Parents != 1 ? "s" : "")}, {Children} child{(Children != 1 ? "ren" : "")}";
}

public static class CompensationRateService
{
    // Dictionary to store all rates with keys in format: "percentage-married-parents-children"
    private static readonly Dictionary<string, decimal> _rates = new()
    {
        // 10% ratings
        {"10-False-0-0", 175.51m},
        {"10-False-0-1", 175.51m},
        {"10-False-1-0", 175.51m},
        {"10-False-1-1", 175.51m},
        {"10-False-2-0", 175.51m},
        {"10-False-2-1", 175.51m},
        {"10-True-0-0", 175.51m},
        {"10-True-0-1", 175.51m},
        {"10-True-1-0", 175.51m},
        {"10-True-1-1", 175.51m},
        {"10-True-2-0", 175.51m},
        {"10-True-2-1", 175.51m},
        
        // 20% ratings
        {"20-False-0-0", 346.95m},
        {"20-False-0-1", 346.95m},
        {"20-False-1-0", 346.95m},
        {"20-False-1-1", 346.95m},
        {"20-False-2-0", 346.95m},
        {"20-False-2-1", 346.95m},
        {"20-True-0-0", 346.95m},
        {"20-True-0-1", 346.95m},
        {"20-True-1-0", 346.95m},
        {"20-True-1-1", 346.95m},
        {"20-True-2-0", 346.95m},
        {"20-True-2-1", 346.95m},
        
        // 30% ratings (updated 4/23/2025)
        {"30-False-0-0", 537.42m},
        {"30-False-0-1", 579.42m},
        {"30-False-1-0", 588.42m},
        {"30-False-1-1", 630.42m},
        {"30-False-2-0", 639.42m},
        {"30-False-2-1", 681.42m},
        {"30-True-0-0", 601.42m},
        {"30-True-0-1", 648.42m},
        {"30-True-1-0", 616.05m},
        {"30-True-1-1", 699.42m},
        {"30-True-2-0", 703.42m},
        {"30-True-2-1", 750.42m},
        
        // 40% ratings
        {"40-False-0-0", 731.86m},
        {"40-False-0-1", 785.86m},
        {"40-False-1-0", 795.86m},
        {"40-False-1-1", 849.86m},
        {"40-False-2-0", 859.86m},
        {"40-False-2-1", 913.86m},
        {"40-True-0-0", 811.86m},
        {"40-True-0-1", 870.86m},
        {"40-True-1-0", 875.86m},
        {"40-True-1-1", 934.86m},
        {"40-True-2-0", 939.86m},
        {"40-True-2-1", 998.86m},
        
        // 50% ratings
        {"50-False-0-0", 1041.82m},
        {"50-False-0-1", 1108.82m},
        {"50-False-1-0", 1122.82m},
        {"50-False-1-1", 1189.82m},
        {"50-False-2-0", 1203.82m},
        {"50-False-2-1", 1270.82m},
        {"50-True-0-0", 1141.82m},
        {"50-True-0-1", 1215.82m},
        {"50-True-1-0", 1222.82m},
        {"50-True-1-1", 1296.82m},
        {"50-True-2-0", 1303.82m},
        {"50-True-2-1", 1377.82m},
        
        // 60% ratings
        {"60-False-0-0", 1319.65m},
        {"60-False-0-1", 1400.65m},
        {"60-False-1-0", 1416.65m},
        {"60-False-1-1", 1497.65m},
        {"60-False-2-0", 1513.65m},
        {"60-False-2-1", 1594.65m},
        {"60-True-0-0", 1440.65m},
        {"60-True-0-1", 1528.65m},
        {"60-True-1-0", 1537.65m},
        {"60-True-1-1", 1625.65m},
        {"60-True-2-0", 1634.65m},
        {"60-True-2-1", 1722.65m},
        
        // 70% ratings
        {"70-False-0-0", 1663.06m},
        {"70-False-0-1", 1757.06m},
        {"70-False-1-0", 1776.06m},
        {"70-False-1-1", 1870.06m},
        {"70-False-2-0", 1889.06m},
        {"70-False-2-1", 1983.06m},
        {"70-True-0-0", 1804.06m},
        {"70-True-0-1", 1907.06m},
        {"70-True-1-0", 1917.06m},
        {"70-True-1-1", 2020.06m},
        {"70-True-2-0", 2030.06m},
        {"70-True-2-1", 2133.06m},
        
        // 80% ratings
        {"80-False-0-0", 1933.15m},
        {"80-False-0-1", 2041.15m},
        {"80-False-1-0", 2062.15m},
        {"80-False-1-1", 2170.15m},
        {"80-False-2-0", 2191.15m},
        {"80-False-2-1", 2299.15m},
        {"80-True-0-0", 2094.15m},
        {"80-True-0-1", 2212.15m},
        {"80-True-1-0", 2223.15m},
        {"80-True-1-1", 2341.15m},
        {"80-True-2-0", 2353.15m},
        {"80-True-2-1", 2470.15m},
        
        // 90% ratings
        {"90-False-0-0", 2172.39m},
        {"90-False-0-1", 2293.39m},
        {"90-False-1-0", 2317.39m},
        {"90-False-1-1", 2438.39m},
        {"90-False-2-0", 2462.39m},
        {"90-False-2-1", 2583.39m},
        {"90-True-0-0", 2353.39m},
        {"90-True-0-1", 2486.39m},
        {"90-True-1-0", 2498.39m},
        {"90-True-1-1", 2631.39m},
        {"90-True-2-0", 2643.39m},
        {"90-True-2-1", 2776.39m},
        
        // 100% ratings
        {"100-False-0-0", 3621.95m},
        {"100-False-0-1", 3757.00m},
        {"100-False-1-0", 3784.02m},
        {"100-False-1-1", 3919.07m},
        {"100-False-2-0", 3946.09m},
        {"100-False-2-1", 4081.14m},
        {"100-True-0-0", 3823.89m},
        {"100-True-0-1", 3971.78m},
        {"100-True-1-0", 3985.96m},
        {"100-True-1-1", 4133.85m},
        {"100-True-2-0", 4148.03m},
        {"100-True-2-1", 4295.92m}
    };
    
    // Cache for combined ratings
    private static readonly Dictionary<string, decimal> _combinedRateCache = new();
    
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
    public static decimal GetRate(int disabilityPercentage, bool married, int parents, int children)
    {
        string key = GetRateKey(disabilityPercentage, married, parents, children);
        
        if (_rates.TryGetValue(key, out decimal rate))
            return rate;
            
        return -1; // Not found
    }
    
    /// <summary>
    /// Gets the formatted compensation rate (as currency string)
    /// </summary>
    public static string GetFormattedRate(int disabilityPercentage, bool married, int parents, int children)
    {
        decimal rate = GetRate(disabilityPercentage, married, parents, children);
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
    public static decimal CalculateCombinedRate(bool married, int parents, int children, List<int> disabilityPercentages)
    {
        if (disabilityPercentages == null || disabilityPercentages.Count == 0)
            return 0;
            
        // Create a cache key for this specific combination
        string cacheKey = $"{string.Join(",", disabilityPercentages.OrderBy(p => p))}-{married}-{parents}-{children}";
        
        // Check if we've calculated this before
        if (_combinedRateCache.TryGetValue(cacheKey, out decimal cachedRate))
            return cachedRate;
        
        // Calculate combined percentage using VA's formula
        int combinedPercentage = CombineDisabilityRatings(disabilityPercentages);
        
        // Get rate for the combined percentage
        decimal rate = GetRate(combinedPercentage, married, parents, children);
        
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