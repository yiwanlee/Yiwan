using System;

namespace Yiwan.Helpers.Exceptions
{
    [Serializable]
    public class RedisCacheException : Exception
    {
        public RedisCacheException() { }

        public RedisCacheException(string message) : base(message) { }

        public RedisCacheException(string message, Exception inner) : base(message, inner) { }

        protected RedisCacheException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
            throw new NotImplementedException();
        }
    }
}
