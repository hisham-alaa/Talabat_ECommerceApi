namespace Talabat.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetMessageForStatusCode(statusCode);
        }

        private string? GetMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                401 => "Un authorized",
                403 => "Forbidden",
                404 => "Not Found",
                405 => "Not Supported",
                409 => "Conflict Occured",
                500 => "Server error",
                _ => null
            };
        }
    }
}
