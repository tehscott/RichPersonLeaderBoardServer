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
            var MY_BASE64_PUBLIC_KEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAl33UVxeoM2D0rjCUQGip7iCHwJrFT8XUiv+y+17eLVdWzC2zUgEJs+f8dGn2KEC7/y5yoJ45cBxg2uMrh6w0e1soKMDP5WuaUkjUFJdYmWRn6HkIIEoS77XUyRYzTDT4QidYj15gYYP0xz9rrE69CSifIcEfWNZRSAq1cK0A5fMHchIenMO2plwLHL0V/BJ8Kx5321uAvhzhjiatS2E3lJ+MZmZV8jJtoMDwpcxVEJyEbvBs1dGkWy776VUVirgeNEWM3jhp9gIPJMSCrF+MUrkCh/HCKKrhSI1dVT2FDd0eEQboFgrdtToqrhwY2qmLNyF3uJbZg+NG4mnrfCdhIwIDAQAB";
            RSACryptoServiceProvider provider = PemKeyLoader.CryptoServiceProviderFromPublicKeyInfo(MY_BASE64_PUBLIC_KEY);
            var xmlString = provider.ToXmlString(false);
            Console.WriteLine(xmlString);
        }
    }
}