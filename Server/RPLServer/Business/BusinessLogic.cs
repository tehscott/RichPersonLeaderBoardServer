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

        public void CreatePayment(int personId, Decimal amount)
        {
            //TODO: insert logic to generate achievements here
            Dao.CreatePayment(personId, amount);
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
