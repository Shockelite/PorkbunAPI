using System.IO;
using System;

namespace Porkbun {

    /// <summary>
    /// Represents the response object for retrieving an SSL certificate.
    /// </summary>
    public class SslGetResponse : StatusResponse {
        /// <summary>
        /// The full certificate chain, including intermediate and root certificates.
        /// </summary>
        public string certificatechain { get; set; }

        /// <summary>
        /// The private key for the SSL certificate.
        /// </summary>
        public string privatekey { get; set; }

        /// <summary>
        /// The public key for the SSL certificate.
        /// </summary>
        public string publickey { get; set; }

        /// <summary>
        /// Saves the SSL certificate components to files with custom names.
        /// </summary>
        /// <param name="directory">The directory to save the files.</param>
        /// <param name="certFileName">The filename for the certificate chain.</param>
        /// <param name="privateKeyFileName">The filename for the private key.</param>
        /// <param name="publicKeyFileName">The filename for the public key.</param>
        public void SaveToFiles(string directory, string certFileName, string privateKeyFileName, string publicKeyFileName) {
            if (status != "SUCCESS")
                throw new InvalidOperationException("Invalid SSL certificate response.");
            Directory.CreateDirectory(directory);
            DeleteIfExists(Path.Combine(directory, certFileName));
            DeleteIfExists(Path.Combine(directory, privateKeyFileName));
            DeleteIfExists(Path.Combine(directory, publicKeyFileName));
            File.WriteAllText(Path.Combine(directory, certFileName), certificatechain);
            File.WriteAllText(Path.Combine(directory, privateKeyFileName), privatekey);
            File.WriteAllText(Path.Combine(directory, publicKeyFileName), publickey);
        }

        /// <summary>
        /// Being lazy, added to be sure files aren't in-use instead of just replacing the files.
        /// </summary>
        private void DeleteIfExists(string filePath) {
            if (File.Exists(filePath)) {
                File.Delete(filePath);
            }
        }

        public override string ToString() =>
            $"SslRetrieveResponse {{ status: \"{status ?? "null"}\", certificatechain: \"{certificatechain?.Substring(0, 30) ?? "null"}...\", privatekey: \"{privatekey?.Substring(0, 30) ?? "null"}...\", publickey: \"{publickey?.Substring(0, 30) ?? "null"}...\" }}";
    }

}