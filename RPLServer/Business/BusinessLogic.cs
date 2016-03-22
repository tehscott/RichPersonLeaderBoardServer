using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Data;
using Domain;
using Google.Apis.AndroidPublisher.v2;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Requests;
using Google.Apis.Services;
using Newtonsoft.Json;

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

        public void CreatePayment(string googleId, decimal amount)
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

        public void CreatePayment(PurchaseData purchaseData)
        {
            var data = JsonConvert.DeserializeObject<dynamic>(purchaseData.developerPayload);

            //TODO: consume the item through google
            //If the item was consumed successfully
            //{
            CreatePayment(data.googleId, GetAmount(purchaseData.productId));
            Dao.RecordPurchase(data.googleId, purchaseData);
            //}
        }

        private decimal GetAmount(string productId)
        {
            switch (productId)
            {
                case "com.mattandmikeandscott.richpersonleaderboard.purchaseone":
                    return 1;
                case "com.mattandmikeandscott.richpersonleaderboard.purchasetwo":
                    return 2;
                case "com.mattandmikeandscott.richpersonleaderboard.purchasethree":
                    return 3;
                case "com.mattandmikeandscott.richpersonleaderboard.purchasefour":
                    return 4;
                case "com.mattandmikeandscott.richpersonleaderboard.purchasefive":
                    return 5;
                case "com.mattandmikeandscott.richpersonleaderboard.purchaseten":
                    return 10;
                case "com.mattandmikeandscott.richpersonleaderboard.purchasetwenty":
                    return 20;
                case "com.mattandmikeandscott.richpersonleaderboard.purchasefifty":
                    return 50;
                case "com.mattandmikeandscott.richpersonleaderboard.purchaseonehundred":
                    return 100;
                default:
                    return 0m;
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

        public PurchaseData VerifyPurchase(PurchaseRecord record)
        {
            try
            {
                if (!GooglePlayVerification.Verify(record.INAPP_PURCHASE_DATA, record.INAPP_DATA_SIGNATURE))
                {
                    //TODO: log this - the person sent a bad request. it should be noted
                    return null;
                }

                var apikey = "AIzaSyCDZ0NsC1dEfUVNmVr80kXe9_hDiVgXVTI";
                string serviceAccountEmail =
                    "richpersonleaderboard@api-6003077209940707168-964527.iam.gserviceaccount.com";
                var applicationName = "Rich Person Leaderboard";
                var packageName = "com.mattandmikeandscott.richpersonleaderboard";
                var productId = JsonConvert.DeserializeObject<PurchaseData>(record.INAPP_PURCHASE_DATA).productId;
                var token = JsonConvert.DeserializeObject<PurchaseData>(record.INAPP_PURCHASE_DATA).purchaseToken;
                var certificate = new X509Certificate2(@"key.p12", "notasecret", X509KeyStorageFlags.Exportable);

                ServiceAccountCredential credential = new ServiceAccountCredential(
                    new ServiceAccountCredential.Initializer(serviceAccountEmail)
                    {
                        Scopes = new[] { "https://www.googleapis.com/auth/androidpublisher" }
                    }.FromCertificate(certificate));

                var service = new AndroidPublisherService(
                    new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = applicationName
                    });

                var request = service.Purchases.Products.Get(packageName, productId, token);
                var purchaseState = request.Execute();

                /*
                Console.WriteLine(JsonConvert.SerializeObject(purchaseState));
    {
      "consumptionState":0,
      "developerPayload":"3bd6aaf1-3d8b-4d95-97ab-3fd8012dce24",
      "kind":"androidpublisher#productPurchase",
      "purchaseState":0,
      "purchaseTimeMillis":1458617266174,
      "ETag":"\"o5gAPb6ySV14-48Jv5T-e_Ifp4s/M9QS9m5-BS77yf-C12pzS7Kf0TE\""
    }            */

                var result = JsonConvert.DeserializeObject<PurchaseData>(record.INAPP_PURCHASE_DATA);

                if (purchaseState.PurchaseState == 0 &&
                    purchaseState.ConsumptionState == 0 &&
                    Dao.GetPurchase(result.orderId) == null)
                {
                    return result;
                }
            }
            catch (Exception e)
            {
                //TODO: log this - the person sent a bad request, or google changed their interface. it should be noted
            }

            return null;
        }
    }
}
