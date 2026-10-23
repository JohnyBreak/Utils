using System;

namespace Common.BSM.Runtime
{
    public class BSMException : Exception
    {
        public override string Message { get; }

        public BSMException(string message)
        {
            Message = message;
        }
    }
}