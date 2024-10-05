using System;

namespace SpaceAce.Auxiliary.Exceptions
{
    public sealed class InvalidKeyException : Exception
    {
        private const string ErrorMessage = "Key is invalid!";

        public InvalidKeyException() : base(ErrorMessage) { }
    }
}