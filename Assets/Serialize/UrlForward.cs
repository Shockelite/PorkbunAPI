namespace Porkbun {

    /// <summary>
    /// Represents a single URL forwarding record.
    /// </summary>
    public class UrlForward {
        /// <summary>
        /// The unique ID of the URL forwarding record.
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// The subdomain being forwarded (empty if root domain).
        /// </summary>
        public string subdomain { get; set; }

        /// <summary>
        /// The destination URL for the forward.
        /// </summary>
        public string location { get; set; }

        /// <summary>
        /// The type of forwarding (e.g., "temporary" or "permanent").
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Whether to include the path in the forwarding (e.g., "yes" or "no").
        /// </summary>
        public string includePath { get; set; }

        /// <summary>
        /// Whether the forwarding is a wildcard (e.g., "yes" or "no").
        /// </summary>
        public string wildcard { get; set; }

        public override string ToString() =>
            $"UrlForward {{ id: \"{id ?? "null"}\", subdomain: \"{subdomain ?? "null"}\", location: \"{location ?? "null"}\", type: \"{type ?? "null"}\", includePath: \"{includePath ?? "null"}\", wildcard: \"{wildcard ?? "null"}\" }}";
    }

}