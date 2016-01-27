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

        public List<Person> GetPersons(int offset = 0, int perPage = 100)
        {
            return Dao.GetPersons(offset, perPage);
        }

        public Person GetPerson(int personId)
        {
            return Dao.GetPerson(personId);
        }

        public Person CreatePerson(string name)
        {
            return Dao.CreatePerson(name);
        }

        public List<Payment> GetPayments(int personId, int offset = 0, int perPage = int.MaxValue)
        {
            return Dao.GetPayments(personId, offset, perPage);
        }

        public Person GetPerson(string name)
        {
            return Dao.GetPerson(name);
        }

        public void CreatePayment(int personId, Decimal amount)
        {
            Dao.CreatePayment(personId, amount);

            var person = Dao.GetPerson(personId);
            if (person.Rank >= 1 && person.Rank <= 1)
            {
                CreateAchievement(personId, AchievementType.Richest);
            }
            if (person.Rank >= 2 && person.Rank <= 1)
            {
                CreateAchievement(personId, AchievementType.SecondRichest);
            }
            if (person.Rank >= 3 && person.Rank <= 1)
            {
                CreateAchievement(personId, AchievementType.ThirdRichest);
            }
            if (person.Rank >= 5 && person.Rank <= 1)
            {
                CreateAchievement(personId, AchievementType.FifthRichest);
            }
            if (person.Rank >= 10 && person.Rank <= 1)
            {
                CreateAchievement(personId, AchievementType.TenthRichest);
            }
            if (person.Rank >= 100 && person.Rank <= 1)
            {
                CreateAchievement(personId, AchievementType.OneHundrethRichest);
            }
            if (person.Payments.Count(payment => payment.InsertDate > DateTime.Today) >= 5)
            {
                CreateAchievement(personId, AchievementType.FiveADay);
            }
            if (person.Payments.Count(payment => payment.InsertDate > DateTime.Today) >= 10)
            {
                CreateAchievement(personId, AchievementType.TenADay);
            }
            if (person.Payments.Count(payment => payment.InsertDate > DateTime.Today) >= 100)
            {
                CreateAchievement(personId, AchievementType.OneHundredADay);
            }
            if (amount >= 10)
            {
                CreateAchievement(personId, AchievementType.TenSpender);
            }
            if (amount >= 100)
            {
                CreateAchievement(personId, AchievementType.OneHundredSpender);
            }
            if (amount >= 1000)
            {
                CreateAchievement(personId, AchievementType.OneThousandSpender);
            }
        }

        public List<Achievement> GetAchievements(int personId)
        {
            return Dao.GetAchievements(personId);
        }

        public void CreateAchievement(int personId, AchievementType achievementType)
        {
            Dao.CreateAchievement(personId, achievementType);
        }
    }
}
