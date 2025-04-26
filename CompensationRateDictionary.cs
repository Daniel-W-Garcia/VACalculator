namespace VACalculatorApp;


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
        {"100-False-0-1", 3974.15f},
        {"100-False-1-0", 4002.74f},
        {"100-False-1-1", 4145.59f},
        {"100-False-2-0", 4174.18f},
        {"100-False-2-1", 4317.03f},
        {"100-True-0-0", 4044.91f},
        {"100-True-0-1", 4201.35f},
        {"100-True-1-0", 4216.35f},
        {"100-True-1-1", 4372.79f},
        {"100-True-2-0", 4387.79f},
        {"100-True-2-1", 4544.23f}
    };
    
    // Dictionary to store all rates with keys in format: "percentage-amount per additional dependent"
    private static readonly Dictionary<int, float> _childUnder18Rates = new()
    {
        {30, 31.00f},
        {40, 42.00f},
        {50, 53.00f},
        {60, 63.00f},
        {70, 74.00f},
        {80, 84.00f},
        {90, 95.00f},
        {100, 106.14f}
    };
    private static readonly Dictionary<int, float> _childOver18SchoolRates = new()
    {
        {30, 102.00f},
        {40, 137.00f},
        {50, 171.00f},
        {60, 205.00f},
        {70, 239.00f},
        {80, 274.00f},
        {90, 308.00f},
        {100, 342.85f}
    };
    
    private static readonly Dictionary<int, float> _spouseAidAttendanceRates = new()
    {
        {70, 137.00f},
        {80, 157.00f},
        {90, 176.00f},
        {100, 195.92f}
    };
    public static Dictionary<string, float> GetRateFromDictionary()
    {
        return new Dictionary<string, float>(_rates);
    }
    
    public static int GetRateKey(int percentage)
    {
        // Don't go above 100%
        if (percentage >= 100) return 100;
        
        // Round down to the nearest valid percentage (10% increments)
        return (percentage / 10) * 10;
    }
    
    // get a rate for a specific percentage and dependent type
    public static float GetChildUnder18Rate(int disabilityPercentage)
    {
        int key = GetRateKey(disabilityPercentage);
        return _childUnder18Rates.TryGetValue(key, out float rate) ? rate : 0;
    }
    
    public static float GetChildOver18SchoolRate(int disabilityPercentage)
    {
        int key = GetRateKey(disabilityPercentage);
        return _childOver18SchoolRates.TryGetValue(key, out float rate) ? rate : 0;
    }
    
    public static float GetSpouseAidAttendanceRate(int disabilityPercentage) //not in use currently in app
    {
        int key = GetRateKey(disabilityPercentage);
        return _spouseAidAttendanceRates.TryGetValue(key, out float rate) ? rate : 0;
    }
}