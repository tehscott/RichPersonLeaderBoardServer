using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public Person GetPerson(int personId)
        {
            return Dao.GetPerson(personId);
        }

        public Person CreatePerson(string name)
        {
            return Dao.CreatePerson(name);
        }

        public List<Payment> GetPayments(int personId)
        {
            return Dao.GetPayments(personId);
        }

        public Person GetPerson(string name)
        {
            return Dao.GetPerson(name);
        }

        public void CreatePayment(int personId, Decimal amount)
        {
            Dao.CreatePayment(personId, amount);

            var person = Dao.GetPerson(personId);
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

        public List<Achievement> GetAchievements(int personId)
        {
            return Dao.GetAchievements(personId);
        }

        public void CreateAchievement(int personId, AchievementType achievementType)
        {
            CreateAchievement(Dao.GetPerson(personId), achievementType);
        }

        public void CreateAchievement(Person person, AchievementType achievementType)
        {
            if (person.Achievements.All(achievement => achievement.AchievementType != achievementType))
            {
                Dao.CreateAchievement(person.PersonId, achievementType);
            }
        }

        public List<Person> GetPersonAndSurroundingPeople(int personId, int range, RankType rankType)
        {
            return Dao.GetPersonAndSurroundingPeople(personId, range, rankType);
        }
    }
}
