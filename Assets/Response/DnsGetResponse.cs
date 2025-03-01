using System.Collections.Generic;

namespace Porkbun {

    /// <summary>
    /// Represents the response object containing DNS records.
    /// </summary>
    public class DnsGetResponse : StatusResponse {
        /// <summary>
        /// A list of DNS records associated with the domain.
        /// </summary>
        public List<DnsRecord> records { get; set; }

        public override string ToString() =>
            $"DnsRetrieveResponse {{ status: \"{status ?? "null"}\", records: [{(records != null ? string.Join(", ", records) : "null")}] }}";
    }

}