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

        /// <summary>
        /// TODO: fill this out!
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        private decimal GetAmount(string productId)
        {
            switch (productId)
            {
                case "1":
                    return 1;
                case "2":
                    return 2;
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
            PurchaseData purchaseData = null;
            var correct = GooglePlayVerification.Verify(record.INAPP_PURCHASE_DATA, record.INAPP_DATA_SIGNATURE);
            if (correct)
            {
                purchaseData = JsonConvert.DeserializeObject<PurchaseData>(record.INAPP_PURCHASE_DATA);
            }

            //TODO: Go to google and verify the purchse
            var apikey = "AIzaSyCDZ0NsC1dEfUVNmVr80kXe9_hDiVgXVTI";
            string serviceAccountEmail = "306227090509-compute@developer.gserviceaccount.com";//"your-mail-in-developer-console@developer.gserviceaccount.com";
            var applicationName = "Rich Person Leaderboard";
            var packageName = "com.mattandmikeandscott.richpersonleaderboard";
            var productId = purchaseData.productId;//"your-inapp-item-id";
            var token = purchaseData.purchaseToken;//"purchase-token";

            ServiceAccountCredential credential = new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer(serviceAccountEmail)
                {
                    Scopes = new[] { "https://www.googleapis.com/auth/androidpublisher" }
                }.FromPrivateKey(apikey));
            
            var service = new AndroidPublisherService(
                new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = applicationName //"GooglePlay API Sample",
                });

            var request = service.Purchases.Products.Get(packageName, productId, token);
            var purchaseState = request.Execute();

            Console.WriteLine(JsonConvert.SerializeObject(purchaseState));


            //PurchasesResource.ProductsResource.GetRequest request = new PurchasesResource.ProductsResource.GetRequest(clientService, packageName, productId, token);
            //var certificate = new X509Certificate2(@"physical-path-to-your-key\key.p12", "notasecret", X509KeyStorageFlags.Exportable);
            // try catch this function because if you input wrong params ( wrong token) google will return error.
            //var request = service.Inappproducts.List(packageName);
            //var purchaseState = request.Execute();
            //var request = service.Purchases.Products.Get(packageName, itemId, purchaseToken);
            //var auth = new OAuth2Authenticator<WebServerClient>(provider, GetAuthorization);
            //var service = new AndroidPublisherService(
            //    new BaseClientService.Initializer()
            //    {
            //        Authenticator = auth,
            //        ApplicationName = applicationName
            //    });

            //TODO: check to make sure that the OrderId is a unique value that I have not yet seen


            return purchaseData;
        }
    }
}
