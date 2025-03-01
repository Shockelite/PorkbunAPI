using System;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO;

namespace Porkbun {

    [TestFixture]
    public class SSLTests {

        private readonly TestConfiguration t;
        private readonly API _api;

        public SSLTests() {
            t = TestConfiguration.LoadConfig();
            _api = new API(t.ApiKey, t.ApiSecret);
        }

        [Test]
        public async Task SSL_Get() {
            SslGetResponse response = await API.RateLimitRetry(() => _api.SslGet(t.Domain));
            Assert.IsNotNull(response.publickey, "DNS records should not be null.");
            response.SaveToFiles("Certificate/", "cert.crt", "private.key", "public.key");
        }

    }
}
