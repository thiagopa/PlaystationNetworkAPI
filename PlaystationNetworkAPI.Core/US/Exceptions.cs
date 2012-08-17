using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaystationNetworkAPI.Core.US
{
    [Serializable]
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException() { }
        public InvalidCredentialsException(string message) : base(message) { }
        public InvalidCredentialsException(string message, Exception inner) : base(message, inner) { }
        protected InvalidCredentialsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class InvalidServerResponseException : Exception
    {
        public InvalidServerResponseException() { }
        public InvalidServerResponseException(string message) : base(message) { }
        public InvalidServerResponseException(string message, Exception inner) : base(message, inner) { }
        protected InvalidServerResponseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
