using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Porkbun {

    [TestFixture]
    public class InvalidAPISecretTests {

        private readonly TestConfiguration t;
        private readonly API _api;

        public InvalidAPISecretTests() {
            t = TestConfiguration.LoadConfig();
            _api = new API(t.ApiKey, "sk1_invalid");
        }

        private async Task AssertException(Func<Task> apiCall) {
            while (true) {
                try { await apiCall(); }
                catch (Exception e) {
                    if (e is InvalidAPISecretException) {
                        Assert.IsTrue(true, "Caught expected InvalidAPISecretException.");
                        break;
                    }
                    else if (e is RatelimitException) {
                        Console.WriteLine("Warning: Rate limit reached. Retry in 5 seconds.");
                        await Task.Delay(5000);
                    }
                    else {
                        Assert.Fail("Unexpected exception: " + e);
                    }
                }
            }
        }

        [Test]
        public Task DNS_Get() =>
            AssertException(() => _api.DnsGet(t.Domain));

        [Test]
        public Task DNS_Add() =>
            AssertException(() => _api.DnsAdd(t.Domain, DnsRecordType.A, t.SubDomain, "1.1.1.1"));

        [Test]
        public Task DNS_Remove() =>
            AssertException(() => _api.DnsRemove(t.Domain, DnsRecordType.A, t.SubDomain));

        [Test]
        public Task DNS_AddOrEdit() =>
            AssertException(() => _api.DnsAddOrEdit(t.Domain, DnsRecordType.A, t.SubDomain, "1.1.1.1"));

        [Test]
        public Task DNS_Edit() =>
            AssertException(() => _api.DnsEdit(t.Domain, DnsRecordType.A, t.SubDomain, "1.1.1.1"));

        [Test]
        public Task URLForward_Add() =>
            AssertException(() => _api.UrlForwardAdd(t.Domain, t.SubDomain, "somewhereelse.xyz", DomainForwardingType.Temporary, false, false));

        [Test]
        public Task URLForward_Get() =>
            AssertException(() => _api.UrlForwardGet(t.Domain));

        [Test]
        public Task URLForward_Remove() =>
            AssertException(() => _api.UrlForwardRemove(t.Domain, "1234"));

    }
}
