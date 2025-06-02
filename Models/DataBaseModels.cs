using SQLite;

namespace VACalculatorApp.Models;

[Table("CompensationRates")]
public class CompensationRate
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    public int DisabilityPercentage { get; set; }
    public bool IsMarried { get; set; }
    public int Parents { get; set; }
    public int Children { get; set; }
    public float Rate { get; set; }
    
    // Create composite key for lookups
    [Ignore]
    public string Key => $"{DisabilityPercentage}-{IsMarried}-{Parents}-{Children}";
}

[Table("AdditionalRates")]
public class AdditionalRate
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    public int DisabilityPercentage { get; set; }
    public string RateType { get; set; } // "ChildUnder18", "ChildOver18School", "SpouseAidAttendance"
    public float Rate { get; set; }
}

[Table("RateMetadata")]
public class RateMetadata
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    public DateTime LastUpdated { get; set; }
    public string EffectiveDate { get; set; }
}