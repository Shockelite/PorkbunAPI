namespace Porkbun {

    /// <summary>
    /// Represents the response object for creating a DNS record.
    /// </summary>
    public class DnsAddResponse : StatusResponse {
        /// <summary>
        /// The ID of the newly created DNS record.
        /// </summary>
        public long id { get; set; }

        public override string ToString() =>
            $"DnsCreateResponse {{ status: \"{status ?? "null"}\", id: {id} }}";
    }

}