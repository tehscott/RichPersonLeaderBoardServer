using Domain;

namespace Server.Models
{
    public class CreateAchievementRequest
    {
        public string GoogleId { get; set; }
        public AchievementType AchievementType { get; set; }
    }
}