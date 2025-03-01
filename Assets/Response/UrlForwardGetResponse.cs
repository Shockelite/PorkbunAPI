using System.Collections.Generic;

namespace Porkbun {

    /// <summary>
    /// Represents the response object for retrieving URL forwarding settings.
    /// </summary>
    public class UrlForwardGetResponse : StatusResponse {
        /// <summary>
        /// A list of URL forwarding records.
        /// </summary>
        public List<UrlForward> forwards { get; set; }

        public override string ToString() =>
            $"DomainForwardingGetResponse {{ status: \"{status ?? "null"}\", forwards: [{(forwards != null ? string.Join(", ", forwards) : "null")}] }}";
    }

}