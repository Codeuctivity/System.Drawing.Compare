using System;
using System.Runtime.Serialization;

namespace Codeuctivity.BitmapCompare
{
    /// <summary>
    /// Codeuctivity.CompareException gets thrown if comparing of images failed
    /// </summary>
    [Serializable]
    public class CompareException : Exception
    {
        /// <summary>
        /// CompareException gets thrown if comparing of images failed
        /// </summary>
        public CompareException()
        {
        }

        /// <summary>
        /// CompareException gets thrown if comparing of images failed
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public CompareException(string message) : base(message)
        {
        }

        /// <summary>
        /// CompareException gets thrown if comparing of images failed
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <returns></returns>
        public CompareException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// CompareException gets thrown if comparing of images failed
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected CompareException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}