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
        /// <remarks>
        /// File extensions:
        /// - Certificate Chain: ".crt" or ".pem"
        /// - Private Key: ".key" or ".pem"
        /// - Public Key: ".pub"
        /// </remarks>
        public void SaveToFiles(string directory, string certFileName, string privateKeyFileName, string publicKeyFileName) {
            if (status != "SUCCESS")
                throw new InvalidOperationException("Invalid SSL certificate response.");

            string certPath = Path.Combine(directory, certFileName);
            string privateKeyPath = Path.Combine(directory, privateKeyFileName);
            string publicKeyPath = Path.Combine(directory, publicKeyFileName);

            string tempCertPath = certPath + ".tmp";
            string tempPrivateKeyPath = privateKeyPath + ".tmp";
            string tempPublicKeyPath = publicKeyPath + ".tmp";

            try {
                MoveFileIfExists(certPath, tempCertPath);
                MoveFileIfExists(privateKeyPath, tempPrivateKeyPath);
                MoveFileIfExists(publicKeyPath, tempPublicKeyPath);
                File.WriteAllText(certPath, certificatechain);
                File.WriteAllText(privateKeyPath, privatekey);
                File.WriteAllText(publicKeyPath, publickey);
                DeleteIfExists(tempCertPath);
                DeleteIfExists(tempPrivateKeyPath);
                DeleteIfExists(tempPublicKeyPath);
            }
            catch (Exception) {
                RollbackTempFile(tempCertPath, certPath);
                RollbackTempFile(tempPrivateKeyPath, privateKeyPath);
                RollbackTempFile(tempPublicKeyPath, publicKeyPath);
                throw;
            }
        }

        /// <summary>
        /// Being lazy, added to be sure files aren't in-use instead of just replacing the files.
        /// </summary>
        private void DeleteIfExists(string filePath) {
            if (File.Exists(filePath)) {
                File.Delete(filePath);
            }
        }

        private void MoveFileIfExists(string sourcePath, string destinationPath) {
            if (File.Exists(sourcePath)) {
                DeleteIfExists(destinationPath);
                File.Move(sourcePath, destinationPath);
            }
        }

        private void RollbackTempFile(string tempPath, string originalPath) {
            if (File.Exists(tempPath)) {
                DeleteIfExists(originalPath);
                File.Move(tempPath, originalPath);
            }
        }

        public override string ToString() =>
            $"SslRetrieveResponse {{ status: \"{status ?? "null"}\", certificatechain: \"{certificatechain?.Substring(0, 30) ?? "null"}...\", privatekey: \"{privatekey?.Substring(0, 30) ?? "null"}...\", publickey: \"{publickey?.Substring(0, 30) ?? "null"}...\" }}";
    }

}