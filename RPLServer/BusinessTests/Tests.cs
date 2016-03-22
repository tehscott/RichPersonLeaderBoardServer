using System;
using System.Security.Cryptography;
using Business;
using Business.Tests;
using Domain;
using NUnit.Framework;

namespace BusinessTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void GetXmlFromKey()
        {
            var MY_BASE64_PUBLIC_KEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAl33UVxeoM2D0rjCUQGip7iCHwJrFT8XUiv+y+17eLVdWzC2zUgEJs+f8dGn2KEC7/y5yoJ45cBxg2uMrh6w0e1soKMDP5WuaUkjUFJdYmWRn6HkIIEoS77XUyRYzTDT4QidYj15gYYP0xz9rrE69CSifIcEfWNZRSAq1cK0A5fMHchIenMO2plwLHL0V/BJ8Kx5321uAvhzhjiatS2E3lJ+MZmZV8jJtoMDwpcxVEJyEbvBs1dGkWy776VUVirgeNEWM3jhp9gIPJMSCrF+MUrkCh/HCKKrhSI1dVT2FDd0eEQboFgrdtToqrhwY2qmLNyF3uJbZg+NG4mnrfCdhIwIDAQAB";
            RSACryptoServiceProvider provider = PemKeyLoader.CryptoServiceProviderFromPublicKeyInfo(MY_BASE64_PUBLIC_KEY);
            var xmlString = provider.ToXmlString(false);
            Console.WriteLine(xmlString);
        }

        [Test]
        public void MakeSureThatVerifyDoesSomething()
        {
            Assert.IsFalse(GooglePlayVerification.Verify("", ""));
        }

        [Test]
        public void MakeSureThatVerifyIsCorrectWithCorrectData()
        {
            //TODO: get the correct data for this
            Assert.IsTrue(new BusinessLogic().VerifyPurchase(new PurchaseRecord()
            {
                INAPP_PURCHASE_DATA =
@"{
  'orderId': 'GPA.1396-2857-6702-22297',
  'packageName': 'com.mattandmikeandscott.richpersonleaderboard',
  'productId': 'com.mattandmikeandscott.richpersonleaderboard.purchaseone',
  'purchaseTime': 1458617266174,
  'purchaseState': 0,
  'developerPayload': '3bd6aaf1-3d8b-4d95-97ab-3fd8012dce24',
  'purchaseToken': 'ljbodpmmimkaaobfbainmbij.AO-J1OyPgMG5sTIaMqgdUTyTG1cc1D62Hv7iIqud7L_paUkVmv2IcAwDRiqWxHn-y7fNDavKhAxYUfGvJUduonuI7LUGNoSJhG7zjwV5z_Cgew2j7asJzQceUvC82EL33Bb67SaDvR4UsK9yVqdySp1Iu4zxOnWwR8EUplrEwGTTIlQHPjph0qL5-ddFzaxVGZ0JRQVC97C_jSjH4Gdl-IRPDyO5NVlQGA'
}",
                INAPP_DATA_SIGNATURE = "DEADBEEF==",
                RESPONSE_CODE = "200"
            }) != null);
        }
    }
}