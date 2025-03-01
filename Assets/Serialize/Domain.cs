using System.Collections.Generic;

namespace Porkbun {

    /// <summary>
    /// Represents a single domain associated with the account.
    /// </summary>
    public class Domain {
        /// <summary>
        /// The domain name (e.g., example.com).
        /// </summary>
        public string domain { get; set; }

        /// <summary>
        /// The status of the domain (e.g., "ACTIVE").
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// The top-level domain (TLD) of the domain (e.g., "com").
        /// </summary>
        public string tld { get; set; }

        /// <summary>
        /// The creation date of the domain.
        /// </summary>
        public string createDate { get; set; }

        /// <summary>
        /// The expiration date of the domain.
        /// </summary>
        public string expireDate { get; set; }

        /// <summary>
        /// Whether security lock is enabled for the domain (1 = enabled, 0 = disabled).
        /// </summary>
        public string securityLock { get; set; }

        /// <summary>
        /// Whether WHOIS privacy is enabled for the domain (1 = enabled, 0 = disabled).
        /// </summary>
        public string whoisPrivacy { get; set; }

        /// <summary>
        /// Whether auto-renew is enabled for the domain (0 = disabled, 1 = enabled).
        /// </summary>
        public int autoRenew { get; set; }

        /// <summary>
        /// Indicates if the domain is local to the account (0 = not local, 1 = local).
        /// </summary>
        public int notLocal { get; set; }

        /// <summary>
        /// A list of labels associated with the domain.
        /// </summary>
        public List<DomainLabel> labels { get; set; }

        public override string ToString() =>
            $"Domain {{ domain: {domain ?? "null"}, status: {status ?? "null"}, tld: {tld ?? "null"}, createDate: {createDate ?? "null"}, expireDate: {expireDate ?? "null"}, securityLock: {securityLock ?? "null"}, whoisPrivacy: {whoisPrivacy ?? "null"}, autoRenew: {autoRenew}, notLocal: {notLocal}, labels: [{(labels != null ? string.Join(", ", labels) : "null")}] }}";
    }

}