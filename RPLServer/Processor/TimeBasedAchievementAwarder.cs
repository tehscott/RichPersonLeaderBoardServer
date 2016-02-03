using System;
using System.Linq;
using Business;
using Domain;

namespace Processor
{
    public class TimeBasedAchievementAwarder
    {
        private readonly BusinessLogic _logic;

        public TimeBasedAchievementAwarder(BusinessLogic logic)
        {
            _logic = logic;
        }

        public void AwardTimeBasedAchievements()
        {
            var date = _logic.GetLastResetDate().ToLocalTime();
            var today = DateTime.Today;
            if (date.Date < today)
            {
                if (date.Year != today.Year)
                {
                    AwardAchievement(RankType.Year, AchievementType.RichestFullYear);
                }
                if (date.Month != today.Month)
                {
                    AwardAchievement(RankType.Month, AchievementType.RichestFullMonth);
                }
                if (date.DayOfWeek > today.DayOfWeek)
                {
                    AwardAchievement(RankType.Week, AchievementType.RichestFullWeek);
                }
                
                AwardAchievement(RankType.Day, AchievementType.RichestFullDay);
                AwardAchievement(RankType.AllTime, AchievementType.Richest);
            }
        }

        private void AwardAchievement(RankType rankType, AchievementType richestFullDay)
        {
            var topDay = _logic.GetPersons(rankType, 1).FirstOrDefault();
            if (topDay != null)
            {
                _logic.CreateAchievement(topDay.PersonId, richestFullDay);
            }
        }
    }
}