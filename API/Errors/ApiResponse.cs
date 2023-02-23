
namespace API.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public ApiResponse(int statusCode, string message=null)
        {
            StatusCode = statusCode;
            // The null-coalescing operator ?? 
            // returns the value of its left-hand operand if it isn't null 
            // otherwise, it evaluates the right-hand operand and
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "You made a bad request.",
                401 => "You are not authorized.",
                404 => "Page not found",
                500 => "Server side Error",
                _ => null
            };
        }
    }
}