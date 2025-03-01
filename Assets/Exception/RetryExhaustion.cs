using System;
namespace Porkbun {
    public class RetryExhaustion : Exception {
        public RetryExhaustion(string message) : base(message) { }
    }
}