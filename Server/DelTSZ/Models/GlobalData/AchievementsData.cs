namespace DelTSZ.Models.GlobalData;

public class AchievementsData
{
    public int AchievementDuration { get; set; }
    public int AchievementStart { get; set; }
    public AchievementCounter[]? AchievementCounters { get; set; }
}