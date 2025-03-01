using System;
namespace Porkbun {
    public class InvalidAPISecretException : Exception {
        public InvalidAPISecretException(string message) : base(message) { }
    }
}