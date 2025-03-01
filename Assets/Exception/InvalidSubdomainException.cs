using System;
namespace Porkbun {
    public class InvalidSubdomainException : Exception {
        public InvalidSubdomainException(string message) : base(message) { }
    }
}