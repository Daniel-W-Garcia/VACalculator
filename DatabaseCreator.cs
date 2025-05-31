/*
using VACalculatorApp.Models;
using SQLite;

namespace VACalculatorApp;

public static class DatabaseCreator
{
    public static async Task CreateDatabase()
    {
        try
        {
            // Create database in a known location
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "compensation_rates.db3");
            
            System.Diagnostics.Debug.WriteLine($"Creating database at: {dbPath}");
            
            var connection = new SQLiteAsyncConnection(dbPath);
            
            // Create tables
            await connection.CreateTableAsync<CompensationRate>();
            await connection.CreateTableAsync<AdditionalRate>();
            await connection.CreateTableAsync<RateMetadata>();

            System.Diagnostics.Debug.WriteLine("Tables created successfully");

            // Get data from dictionary
            var rates = CompensationRateDictionary.GetRateFromDictionary();
            var compensationRates = new List<CompensationRate>();

            foreach (var kvp in rates)
            {
                var parts = kvp.Key.Split('-');
                if (parts.Length == 4)
                {
                    compensationRates.Add(new CompensationRate
                    {
                        DisabilityPercentage = int.Parse(parts[0]),
                        IsMarried = bool.Parse(parts[1]),
                        Parents = int.Parse(parts[2]),
                        Children = int.Parse(parts[3]),
                        Rate = kvp.Value
                    });
                }
            }

            await connection.InsertAllAsync(compensationRates);
            System.Diagnostics.Debug.WriteLine($"Inserted {compensationRates.Count} compensation rates");

            // Add additional rates
            var additionalRates = new List<AdditionalRate>();

            foreach (var percentage in new[] {30, 40, 50, 60, 70, 80, 90, 100})
            {
                var childUnder18Rate = CompensationRateDictionary.GetChildUnder18Rate(percentage);
                if (childUnder18Rate > 0)
                    additionalRates.Add(new AdditionalRate
                    {
                        DisabilityPercentage = percentage,
                        RateType = "ChildUnder18",
                        Rate = childUnder18Rate
                    });

                var childOver18Rate = CompensationRateDictionary.GetChildOver18SchoolRate(percentage);
                if (childOver18Rate > 0)
                    additionalRates.Add(new AdditionalRate
                    {
                        DisabilityPercentage = percentage,
                        RateType = "ChildOver18School",
                        Rate = childOver18Rate
                    });

                if (percentage >= 70)
                {
                    var spouseAidRate = CompensationRateDictionary.GetSpouseAidAttendanceRate(percentage);
                    if (spouseAidRate > 0)
                        additionalRates.Add(new AdditionalRate
                        {
                            DisabilityPercentage = percentage,
                            RateType = "SpouseAidAttendance",
                            Rate = spouseAidRate
                        });
                }
            }

            await connection.InsertAllAsync(additionalRates);
            System.Diagnostics.Debug.WriteLine($"Inserted {additionalRates.Count} additional rates");

            // Add metadata
            await connection.InsertAsync(new RateMetadata
            {
                LastUpdated = DateTime.Now,
                EffectiveDate = "2025"
            });

            System.Diagnostics.Debug.WriteLine($"✅ Database created successfully at: {dbPath}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"❌ Error creating database: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }
}
*/
