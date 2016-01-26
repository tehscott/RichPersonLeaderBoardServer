using System;
using System.Collections.Generic;

namespace Domain
{
    public interface IDao
    {
        List<Person> GetPersons(int offset = 0, int perPage = 100);
        Person GetPerson(int personId);
        Person CreatePerson(string name);
        List<Payment> GetPayments(int personId, int offset = 0, int perPage = int.MaxValue);
        void CreatePayment(int personId, Decimal amount);
        List<Achievement> GetAchievements(int personId);
        void CreateAchievement(int personId, AchievementType achievementType);
    }
}