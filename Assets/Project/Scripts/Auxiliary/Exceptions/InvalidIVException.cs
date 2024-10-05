using System;

namespace SpaceAce.Auxiliary.Exceptions
{
    public sealed class InvalidIVException : Exception
    {
        private const string ErrorMessage = "Initialization vector is invalid!";

        public InvalidIVException() : base(ErrorMessage) { }
    }
}