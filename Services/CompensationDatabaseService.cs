using SQLite;
using VACalculatorApp.Models;

namespace VACalculatorApp.Services;

public class CompensationDatabaseService
{
    private SQLiteAsyncConnection _database;
    private readonly string _dbPath;

    public CompensationDatabaseService()
    {
        _dbPath = Path.Combine(FileSystem.AppDataDirectory, "compensation_rates.db3");
    }

    private async Task Init()
    {
        if (_database is not null)
            return;

        // Copy database from app bundle if it doesn't exist
        if (!File.Exists(_dbPath))
        {
            await CopyDatabaseFromBundle();
        }

        _database = new SQLiteAsyncConnection(_dbPath);
    }

    private async Task CopyDatabaseFromBundle()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("compensation_rates.db3");
        using var fileStream = new FileStream(_dbPath, FileMode.Create, FileAccess.Write);
        await stream.CopyToAsync(fileStream);
    }

    public async Task<float> GetRate(int disabilityPercentage, bool isMarried, int parents, int children)
    {
        await Init();

        // Normalize inputs
        parents = Math.Clamp(parents, 0, 2);
        children = children > 0 ? 1 : 0;

        var rate = await _database.Table<CompensationRate>()
            .Where(r => r.DisabilityPercentage == disabilityPercentage
                        && r.IsMarried == isMarried
                        && r.Parents == parents
                        && r.Children == children)
            .FirstOrDefaultAsync();

        return rate?.Rate ?? -1;
    }

    public async Task<float> GetAdditionalRate(int disabilityPercentage, string rateType)
    {
        await Init();

        var rate = await _database.Table<AdditionalRate>()
            .Where(r => r.DisabilityPercentage == disabilityPercentage
                        && r.RateType == rateType)
            .FirstOrDefaultAsync();

        return rate?.Rate ?? 0;
    }
}