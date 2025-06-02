using VACalculatorApp.Services;

namespace VACalculatorApp;

public class CalculateRate
{
    private static CompensationDatabaseService _dbService = new CompensationDatabaseService();

    // Update GetTableRate to use database
    private static async Task<float> GetTableRate(int disabilityPercentage, bool married, int parents, int children)
    {
        return await _dbService.GetRate(disabilityPercentage, married, parents, children);
    }

    // Make CalculateTotalCompensation async
    public static async Task<float> CalculateTotalCompensation(Veteran veteran)
    {
        float rate = await GetTableRate(veteran.DisabilityPercentage, veteran.IsMarried,
                                       veteran.ParentCount, veteran.ChildrenUnder18Count);

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
                float childBonusUnder18 = await _dbService.GetAdditionalRate(
                    veteran.DisabilityPercentage, "ChildUnder18");
                rate += veteran.AdditionalChildrenUnder18Count * childBonusUnder18;
            }

            // Add for children over 18 in school
            if (veteran.ChildrenOver18InSchoolCount > 0)
            {
                float childBonusOver18InSchool = await _dbService.GetAdditionalRate(
                    veteran.DisabilityPercentage, "ChildOver18School");
                rate += veteran.ChildrenOver18InSchoolCount * childBonusOver18InSchool;
            }

            // Add for spouse Aid and Attendance
            if (veteran.IsMarried && veteran.SpouseReceivingAidAndAttendance && veteran.DisabilityPercentage >= 70)
            {
                float aidAndAttendanceBonus = await _dbService.GetAdditionalRate(
                    veteran.DisabilityPercentage, "SpouseAidAttendance");
                rate += aidAndAttendanceBonus;
            }
        }
        return rate;
    }
    public static int CombineDisabilityRatings(List<int> ratings)
    {
        if (ratings == null || ratings.Count == 0)
            return 0;

        if (ratings.Count == 1)
            return ratings[0];

        var sortedRatings = ratings.OrderByDescending(r => r).ToList();

        double remainingPercentage = 100.0;
        double combinedPercentage = 0.0;

        foreach (var rating in sortedRatings)
        {
            combinedPercentage += (remainingPercentage * rating) / 100.0;
            remainingPercentage = (100.0 - combinedPercentage);
        }

        int roundedCombined = (int)Math.Round(combinedPercentage / 10.0, MidpointRounding.AwayFromZero) * 10;

        return Math.Min(roundedCombined, 100);
    }

}
