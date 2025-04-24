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