using System;
namespace Porkbun {
    public class RatelimitException : Exception {
        public RatelimitException(string message) : base(message) { }
    }
}