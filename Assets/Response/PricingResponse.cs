using System.Collections.Generic;

namespace Porkbun {

    /// <summary>
    /// Represents the pricing details for different domain extensions.
    /// </summary>
    public class PricingResponse : StatusResponse {
        /// <summary>
        /// The pricing details for different domain extensions.
        /// </summary>
        public Dictionary<string, DomainPrice> pricing { get; set; }

        public override string ToString() =>
            $"PricingResponse {{ status: \"{status ?? "null"}\", pricing: [{(pricing != null ? string.Join(", ", pricing) : "null")}] }}";
    }

}