using System;
namespace Porkbun {
    public class InvalidDomainException : Exception {
        public InvalidDomainException(string message) : base(message) { }
    }
}