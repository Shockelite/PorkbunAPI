namespace Porkbun {

    /// <summary>
    /// Represents the response object for domain availability checking.
    /// </summary>
    public class DomainCheckResponse : StatusResponse {
        /// <summary>
        /// The availability response details for the domain.
        /// </summary>
        public DomainAvailability response { get; set; }

        public override string ToString() =>
            $"DomainCheckResponse {{ status: \"{status ?? "null"}\", response: {response?.ToString() ?? "null"} }}";
    }

}