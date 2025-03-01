namespace Porkbun {

    /// <summary>
    /// Represents the response object for the ping endpoint, which checks the status and returns the IP address.
    /// </summary>
    public class PingResponse : StatusResponse {
        /// <summary>
        /// The IP address of the server making the request.
        /// </summary>
        public string yourIp { get; set; }

        public override string ToString() =>
            $"PingResponse {{ status: \"{status ?? "null"}\", yourIp: \"{yourIp ?? "null"}\" }}";
    }

}