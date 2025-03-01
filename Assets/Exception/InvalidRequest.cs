using System;
namespace Porkbun {
    public class InvalidRequest : Exception {
        public InvalidRequest(string message) : base(message) { }
    }
}