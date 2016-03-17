using System;
using System.Security.Cryptography;
using NUnit.Framework;

namespace Business.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void GetXMLFromKey()
        {
            var MY_BASE64_PUBLIC_KEY = "INSERT KEY HERE";
            RSACryptoServiceProvider provider = PemKeyLoader.CryptoServiceProviderFromPublicKeyInfo(MY_BASE64_PUBLIC_KEY);
            var xmlString = provider.ToXmlString(false);
            Console.WriteLine(xmlString);
        }
    }
}