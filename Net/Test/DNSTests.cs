using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Porkbun {

    [TestFixture]
    public class DNSTests {

        private readonly TestConfiguration t;
        private readonly API _api;

        public DNSTests() {
            t = TestConfiguration.LoadConfig();
            _api = new API(t.ApiKey, t.ApiSecret);
        }

        [Test]
        public async Task DNS_Get() {
            DnsGetResponse response = await API.RateLimitRetry(() => _api.DnsGet(t.Domain));
            Assert.IsNotNull(response.records, "DNS records should not be null.");
            Console.WriteLine($"Records Count: {response.records.Count()}");
            Console.WriteLine(response);
        }

        [Test]
        public async Task DNS_Add() {
            StatusResponse addResponse = await API.RateLimitRetry(() => _api.DnsAdd(t.Domain, DnsRecordType.A, t.SubDomain, "1.1.1.1"));
            Assert.AreEqual("SUCCESS", addResponse.status, "DNS_Add did not succeed.");

            StatusResponse removeResponse = await API.RateLimitRetry(() => _api.DnsRemove(t.Domain, DnsRecordType.A, t.SubDomain));
            Assert.AreEqual("SUCCESS", removeResponse.status, "DNS_Remove did not succeed.");
        }

    }
}
