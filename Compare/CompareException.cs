using System;
using System.Runtime.Serialization;

namespace Codeuctivity
{
    /// <summary>
    /// System.Drawing.CompareException gets thrown if comparing of images failes
    /// </summary>
    [Serializable]
    public class CompareException : Exception
    {
        /// <summary>
        /// System.Drawing.CompareException gets thrown if comparing of images failes
        /// </summary>
        public CompareException()
        {
        }

        /// <summary>
        /// System.Drawing.CompareException gets thrown if comparing of images failes
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public CompareException(string message) : base(message)
        {
        }

        /// <summary>
        /// System.Drawing.CompareException gets thrown if comparing of images failes
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <returns></returns>
        public CompareException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// System.Drawing.CompareException gets thrown if comparing of images failes
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected CompareException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}