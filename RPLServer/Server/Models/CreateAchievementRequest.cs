using Domain;

namespace Server.Models
{
    public class CreateAchievementRequest
    {
        public int PersonId { get; set; }
        public AchievementType AchievementType { get; set; }
    }
}