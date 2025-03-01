using System;
namespace Porkbun {
    public class InvalidAPIKeyException : Exception {
        public InvalidAPIKeyException(string message) : base(message) { }
    }
}