namespace Porkbun {

    /// <summary>
    /// Represents the pricing for a specific domain extension (e.g., .com, .design).
    /// </summary>
    public class DomainPrice {
        /// <summary>
        /// The registration price for the domain extension.
        /// </summary>
        public string registration { get; set; }

        /// <summary>
        /// The renewal price for the domain extension.
        /// </summary>
        public string renewal { get; set; }

        /// <summary>
        /// The transfer price for the domain extension.
        /// </summary>
        public string transfer { get; set; }

        public override string ToString() =>
            $"DomainPricing {{ registration: \"{registration ?? "null"}\", renewal: \"{renewal ?? "null"}\", transfer: \"{transfer ?? "null"}\" }}";
    }

}