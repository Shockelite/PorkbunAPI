using System.Collections.Generic;

namespace Porkbun {

    /// <summary>
    /// Represents the response object for listing all domains associated with an account.
    /// </summary>
    public class DomainListResponse : StatusResponse {
        /// <summary>
        /// A list of domains associated with the account.
        /// </summary>
        public List<Domain> domains { get; set; }

        public override string ToString() =>
            $"DomainsListResponse {{ status: \"{status ?? "null"}\", domains: [{(domains != null ? string.Join(", ", domains) : "null")}] }}";
    }

}