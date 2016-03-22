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
            Assert.IsTrue(new BusinessLogic().VerifyPurchase(new PurchaseRecord()
            {
                INAPP_PURCHASE_DATA = @"{'orderId':'GPA.1320-0782-4663-65250','packageName':'com.mattandmikeandscott.richpersonleaderboard','productId':'com.mattandmikeandscott.richpersonleaderboard.purchasetwo','purchaseTime':1458653994873,'purchaseState':0,'developerPayload':'1de62b5d-730b-4a35-9735-10feb8f08fe5','purchaseToken':'bilgohingaimhdgbbegodpln.AO-J1OyjNljyStDY4A7qUX042bgXvCcDx8CNkEapVLorK1XEuC9uaHpOBkee5pphaN1Nlw8atitLbXZdttssUzs39eK7gzOMdO22QQbNBHk5cmpqGwRkmYvHQVP3BuPakLly2jT1Ui4ZYs2PwnzbhouzshxjJzqGPuufwhKOVgiSg2-twJXiHkwxeTbzKNK5JJu7QCafIBM9VBuEhoZmlq2A_uHhut_5ng'}".Replace('\'', '"'),
                INAPP_DATA_SIGNATURE = "TUD0v4A7e0uy7dYwKbr0iG1tM818m8k34lTDHfpTmykiR5k2ECFmrU9Eu8r6o9PTAxfh0BTKVwlV6VJbVU2JzifVIi+K1sGhTD2h0kbhMCIUQpjCO3geVZLE9tF17RYQEK/62NHO9czj4FzCX6mEkujC+czv6u8jpAYe4D8y4GeaKzF6aDI4ZsDR6qMBE84Ah3CZ0Zm20vbSgDKmG4G5SaN0vAjN7dOrjx5L1j8SZjkFs8JdOej0GAwdNAunQG5RjYefsnxJwyLwcDD5hC/7Z6+YIoujOiqfvfnQ1pxkgx0zNwTDBau2whRLzYEWoM0pUFWG+a+kK15bEUiFtdk25w==",
                RESPONSE_CODE = "200"
            }) != null);
        }
    }
}