using System.Collections.Generic;

namespace Porkbun {

    /// <summary>
    /// Represents the response object for retrieving the nameservers of a domain.
    /// </summary>
    public class NameServerGetResponse : StatusResponse {
        /// <summary>
        /// A list of nameservers associated with the domain.
        /// </summary>
        public List<string> ns { get; set; }

        public override string ToString() =>
            $"DomainNameserverGetResponse {{ status: \"{status ?? "null"}\", ns: [{(ns != null ? string.Join(", ", ns) : "null")}] }}";
    }

}