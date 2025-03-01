using System.Collections.Generic;

namespace Porkbun {

    /// <summary>
    /// Represents the response object for retrieving DNSSEC records.
    /// </summary>
    public class DnssecGetResponse : StatusResponse {
        /// <summary>
        /// A dictionary of DNSSEC records, where the key is the key tag.
        /// </summary>
        public Dictionary<string, DnssecRecord> records { get; set; }

        public override string ToString() =>
            $"DnssecRecordsResponse {{ status: \"{status ?? "null"}\", records: [{(records != null ? string.Join(", ", records.Values) : "null")}] }}";
    }

}