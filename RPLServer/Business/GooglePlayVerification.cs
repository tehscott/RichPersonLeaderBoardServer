using System;
using System.Security.Cryptography;
using System.Text;

namespace Business
{
    public class GooglePlayVerification
    {
        private const string PublicKey = "<RSAKeyValue><Modulus>l33UVxeoM2D0rjCUQGip7iCHwJrFT8XUiv+y+17eLVdWzC2zUgEJs+f8dGn2KEC7/y5yoJ45cBxg2uMrh6w0e1soKMDP5WuaUkjUFJdYmWRn6HkIIEoS77XUyRYzTDT4QidYj15gYYP0xz9rrE69CSifIcEfWNZRSAq1cK0A5fMHchIenMO2plwLHL0V/BJ8Kx5321uAvhzhjiatS2E3lJ+MZmZV8jJtoMDwpcxVEJyEbvBs1dGkWy776VUVirgeNEWM3jhp9gIPJMSCrF+MUrkCh/HCKKrhSI1dVT2FDd0eEQboFgrdtToqrhwY2qmLNyF3uJbZg+NG4mnrfCdhIw==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        public static bool Verify(string message, string base64Signature)
        {
            // By default the result is false
            bool result = false;
            try
            {
                // Create the provider and load the KEY
                RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
                provider.FromXmlString(PublicKey);

                // The signature is supposed to be encoded in base64 and the SHA1 checksum
                // Of the message is computed against the UTF-8 representation of the 
                // message
                byte[] signature = Convert.FromBase64String(base64Signature);
                SHA1Managed sha = new SHA1Managed();
                byte[] data = Encoding.UTF8.GetBytes(message);

                result = provider.VerifyData(data, sha, signature);
            }
            catch (Exception /* e */) { /* TODO: add some kind of logging here */}
            return result;
        }
    }
}