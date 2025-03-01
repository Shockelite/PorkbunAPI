using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Porkbun;

namespace Porkbun {

    [TestFixture]
    public class InvalidDomainTests {

        private readonly TestConfiguration t;
        private readonly API _api;

        public InvalidDomainTests() {
            t = TestConfiguration.LoadConfig();
            _api = new API(t.ApiKey, t.ApiSecret);
        }

        private async Task AssertException(Func<Task> apiCall) {
            while (true) {
                try { await apiCall(); }
                catch (Exception e) {
                    if (e is InvalidDomainException) {
                        Assert.IsTrue(true, "Caught expected InvalidDomainException.");
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
            AssertException(() => _api.DnsGet(t.BadDomain));

        [Test]
        public Task DNS_Add() =>
            AssertException(() => _api.DnsAdd(t.BadDomain, DnsRecordType.A, t.SubDomain, "1.1.1.1"));

        [Test]
        public Task DNS_Remove() =>
            AssertException(() => _api.DnsRemove(t.BadDomain, DnsRecordType.A, t.SubDomain));

        [Test]
        public Task DNS_AddOrEdit() =>
            AssertException(() => _api.DnsAddOrEdit(t.BadDomain, DnsRecordType.A, t.SubDomain, "1.1.1.1"));

        [Test]
        public Task DNS_Edit() =>
            AssertException(() => _api.DnsEdit(t.BadDomain, DnsRecordType.A, t.SubDomain, "1.1.1.1"));

        [Test]
        public Task URLForward_Add() =>
            AssertException(() => _api.UrlForwardAdd(t.BadDomain, t.SubDomain, "somewhereelse.xyz", DomainForwardingType.Temporary, false, false));

        [Test]
        public Task URLForward_Get() =>
            AssertException(() => _api.UrlForwardGet(t.BadDomain));

        [Test]
        public Task URLForward_Remove() =>
            AssertException(() => _api.UrlForwardRemove(t.BadDomain, "1234"));

    }
}
