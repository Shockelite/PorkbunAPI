namespace Porkbun {

    /// <summary>
    /// Represents a DNS record in the Porkbun system.
    /// </summary>
    public class DnsRecord {
        /// <summary>
        /// The unique ID of the DNS record.
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// The name (subdomain or domain) associated with the DNS record.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// The type of DNS record (e.g., A, CNAME, MX).
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// The content of the DNS record, such as IP address or URL.
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// The time-to-live (TTL) for the DNS record, in seconds.
        /// </summary>
        public string ttl { get; set; }

        /// <summary>
        /// The priority of the DNS record (used for certain types like MX).
        /// </summary>
        public string prio { get; set; }

        /// <summary>
        /// Additional notes or comments for the DNS record.
        /// </summary>
        public string notes { get; set; }

        public override string ToString() => $"DnsRecord {{ id: {id}, name: \"{name ?? "null"}\", type: \"{type}\", content: \"{content ?? "null"}\", ttl: {ttl}, prio: {prio}, notes: \"{notes ?? "null"}\" }}";
    }

}