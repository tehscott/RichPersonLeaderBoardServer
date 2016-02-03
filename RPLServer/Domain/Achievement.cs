using System;

namespace Domain
{
    public class Achievement
    {
        public AchievementType AchievementType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime InsertDate { get; set; }
    }
}