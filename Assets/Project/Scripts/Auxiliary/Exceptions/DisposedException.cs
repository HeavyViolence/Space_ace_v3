using System;

namespace SpaceAce.Auxiliary.Exceptions
{
    public sealed class DisposedException : Exception
    {
        private const string ErrorMessage = "Object is disposed!";

        public DisposedException() : base(ErrorMessage) { }
    }
}