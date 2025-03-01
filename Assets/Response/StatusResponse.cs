namespace Porkbun {

    /// <summary>
    /// Base response for status-only responses.
    /// </summary>
    public class StatusResponse {

        /// <summary>
        /// The status of the API response, expected to be "SUCCESS" on success.
        /// </summary>
        public string status { get; set; } = null;

        public string message { get; set; } = null;

        public override string ToString() =>
            $"{this.GetType().Name} {{ status: \"{status ?? "null"}\", message: \"{message ?? "null"}\" }}";

    }

}