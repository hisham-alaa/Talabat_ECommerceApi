namespace Talabat.APIs.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public string[] Errors { get; set; }

        public ApiValidationErrorResponse() : base(400, "Validation Error")
        {
        }
        public ApiValidationErrorResponse(string[] errors) : base(400, "Validation Error")
        {
            Errors = errors;
        }

    }
}
