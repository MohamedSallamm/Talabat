namespace Talabat.API.Errors
{
    public class ApiExceptionResponse : APIResponse
    {
        public string? Details { get; set; }
        public ApiExceptionResponse(int StatusCode, string? Message = null, string? details = null) : base(StatusCode, Message)
        {
            Details = Details;
        }
    }
}
