using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public ApiValidationErrorResponse() : base(400)
        {
        }
        // this property gets loaded at Middleware in Programs.cs
        public IEnumerable<string> Errors { get; set; }
    }
}