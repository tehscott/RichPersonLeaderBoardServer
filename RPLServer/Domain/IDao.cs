using System;
using System.Collections.Generic;

namespace Domain
{
    public interface IDao
    {
        List<Person> GetPersons(int offset = 0, int perPage = 100, RankType rankType = RankType.AllTime);
        List<Person> GetPersonAndSurroundingPeople(string googleId, int range, RankType rankType = RankType.AllTime);
        void ResetWealth(RankType rankType = RankType.Day);
        Person GetPerson(string googleId);
        Person GetPersonByName(string name);
        Person CreatePerson(string name, string googleId);
        List<Payment> GetPayments(string googleId);
        void CreatePayment(string googleId, Decimal amount);
        List<Achievement> GetAchievements(string googleId);
        void CreateAchievement(string googleId, AchievementType achievementType);
        DateTime GetLastResetDate();
        void RecordPurchase(string googleId, PurchaseData purchaseData);
        PurchaseData GetPurchase(string orderId);
    }
}