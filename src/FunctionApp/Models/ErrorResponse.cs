using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

namespace FunctionApp.Models
{
    /// <summary>
    /// This represents the response model entity for error.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
        /// </summary>
        public ErrorResponse()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
        /// </summary>
        /// <param name="ex"><see cref="Exception"/> instance.</param>
        public ErrorResponse(Exception ex)
        {
            this.Message = ex.Message;
            this.StackTrace = ex.StackTrace;
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        [JsonProperty("message")]
        public virtual string Message { get; set; }

        /// <summary>
        /// Gets the stack trace.
        /// </summary>
        [JsonProperty("stackTrace")]
        public virtual List<string> StackTraces
        {
            get
            {
                return this.StackTrace
                           .Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
        }

        /// <summary>
        /// Set the stack trace.
        /// </summary>
        [JsonIgnore]
        public virtual string StackTrace { private get; set; }
    }
}
