using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Domain;

namespace Business
{
    public class BusinessLogic
    {
        private IDao Dao { get; }

        public BusinessLogic()
        {
            Dao = new Dao();
        }

        public List<Person> GetPersons(RankType rankType, int offset = 0, int perPage = 100)
        {
            return Dao.GetPersons(offset, perPage, rankType);
        }

        public void ResetWealth(RankType rankType = RankType.Day)
        {
            Dao.ResetWealth(rankType);
        }

        public Person GetPerson(string googleId)
        {
            return Dao.GetPerson(googleId);
        }

        public Person CreatePerson(string name, string googleId)
        {
            return Dao.CreatePerson(name, googleId);
        }

        public List<Payment> GetPayments(string googleId)
        {
            return Dao.GetPayments(googleId);
        }

        public Person GetPersonByName(string name)
        {
            return Dao.GetPerson(name);
        }

        public void CreatePayment(string googleId, Decimal amount)
        {
            Dao.CreatePayment(googleId, amount);

            var person = Dao.GetPerson(googleId);
            if (person.Rank <= 1 && person.Rank >= 1)
            {
                CreateAchievement(person, AchievementType.Richest);
            }
            if (person.Rank <= 2 && person.Rank >= 1)
            {
                CreateAchievement(person, AchievementType.SecondRichest);
            }
            if (person.Rank <= 3 && person.Rank >= 1)
            {
                CreateAchievement(person, AchievementType.ThirdRichest);
            }
            if (person.Rank <= 5 && person.Rank >= 1)
            {
                CreateAchievement(person, AchievementType.FifthRichest);
            }
            if (person.Rank <= 10 && person.Rank >= 1)
            {
                CreateAchievement(person, AchievementType.TenthRichest);
            }
            if (person.Rank <= 100 && person.Rank >= 1)
            {
                CreateAchievement(person, AchievementType.OneHundrethRichest);
            }
            if (person.Payments.Count(payment => payment.InsertDate > DateTime.Today) >= 5)
            {
                CreateAchievement(person, AchievementType.FiveADay);
            }
            if (person.Payments.Count(payment => payment.InsertDate > DateTime.Today) >= 10)
            {
                CreateAchievement(person, AchievementType.TenADay);
            }
            if (person.Payments.Count(payment => payment.InsertDate > DateTime.Today) >= 100)
            {
                CreateAchievement(person, AchievementType.OneHundredADay);
            }
            if (amount >= 10)
            {
                CreateAchievement(person, AchievementType.TenSpender);
            }
            if (amount >= 100)
            {
                CreateAchievement(person, AchievementType.OneHundredSpender);
            }
            if (amount >= 1000)
            {
                CreateAchievement(person, AchievementType.OneThousandSpender);
            }
        }

        public List<Achievement> GetAchievements(string googleId)
        {
            return Dao.GetAchievements(googleId);
        }

        public void CreateAchievement(string googleId, AchievementType achievementType)
        {
            CreateAchievement(Dao.GetPerson(googleId), achievementType);
        }

        public void CreateAchievement(Person person, AchievementType achievementType)
        {
            if (person.Achievements.All(achievement => achievement.AchievementType != achievementType))
            {
                Dao.CreateAchievement(person.GoogleId, achievementType);
            }
        }

        public List<Person> GetPersonAndSurroundingPeople(string googleId, int range, RankType rankType)
        {
            return Dao.GetPersonAndSurroundingPeople(googleId, range, rankType);
        }

        public DateTime GetLastResetDate()
        {
            return Dao.GetLastResetDate();
        }
    }
}
