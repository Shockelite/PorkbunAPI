namespace Porkbun {

    /// <summary>
    /// Represents a single DNSSEC record.
    /// </summary>
    public class DnssecRecord {
        /// <summary>
        /// The key tag associated with the DNSSEC record.
        /// </summary>
        public string keyTag { get; set; }

        /// <summary>
        /// The algorithm used for the DNSSEC record.
        /// </summary>
        public string alg { get; set; }

        /// <summary>
        /// The digest type used in the DNSSEC record.
        /// </summary>
        public string digestType { get; set; }

        /// <summary>
        /// The digest value of the DNSSEC record.
        /// </summary>
        public string digest { get; set; }

        public override string ToString() =>
            $"DnssecRecord {{ keyTag: {keyTag ?? "null"}, alg: {alg ?? "null"}, digestType: {digestType ?? "null"}, digest: {digest ?? "null"} }}";
    }

}