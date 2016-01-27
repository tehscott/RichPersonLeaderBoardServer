using System;
using System.Collections.Generic;

namespace Domain
{
    public interface IDao
    {
        List<Person> GetPersons(int offset = 0, int perPage = 100);
        Person GetPerson(int personId);
        Person GetPerson(string name);
        Person CreatePerson(string name);
        List<Payment> GetPayments(int personId);
        void CreatePayment(int personId, Decimal amount);
        List<Achievement> GetAchievements(int personId);
        void CreateAchievement(int personId, AchievementType achievementType);
        List<Person> GetPersonAndSurroundingPeople(int personId, int range);
    }
}