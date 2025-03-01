namespace Porkbun {

    /// <summary>
    /// Represents the details of the domain's availability status.
    /// </summary>
    public class DomainAvailability {
        /// <summary>
        /// Indicates if the domain is available ("yes" or "no").
        /// </summary>
        public string avail { get; set; }

        /// <summary>
        /// The type of domain check, e.g., "registration".
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// The price for registering the domain.
        /// </summary>
        public string price { get; set; }

        /// <summary>
        /// Indicates if there is a first-year promo ("yes" or "no").
        /// </summary>
        public string firstYearPromo { get; set; }

        /// <summary>
        /// The regular price of the domain after the first year.
        /// </summary>
        public string regularPrice { get; set; }

        public override string ToString() =>
            $"DomainAvailabilityResponse {{ avail: \"{avail ?? "null"}\", type: \"{type ?? "null"}\", price: \"{price ?? "null"}\", firstYearPromo: \"{firstYearPromo ?? "null"}\", regularPrice: \"{regularPrice ?? "null"}\" }}";
    }

}