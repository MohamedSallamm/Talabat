
using Microsoft.AspNetCore.Http;

namespace Talabat.API.Errors
{
    public class APIResponse 
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public APIResponse(int statuscode, string? message = null)
        {
            StatusCode = statuscode;
            Message = message ?? GetDefaultMessageForStatusCode(StatusCode);  
        }



        //private string GetDefaultMessageForStatusCode(int? statusCode) =>
        //    // 500 >> Internal Server Error
        //    // 400 >> Bad Request
        //    // 401 >> UnAuthorized
        //    // 404 >> Not Found
        //    StatusCode switch
        //    {
        //        400 => "Bad Request",
        //        401 => "UnAuthorized",
        //        404 => "Not Found",
        //        500 => "Internal Server Error",

        //    };
        private static string GetDefaultMessageForStatusCode(int statusCode) => statusCode switch
        {
            200 => "OK",
            201 => "Created",
            400 => "Bad Request",
            401 => "Unauthorized",
            404 => "Not Found",
            405 => "Method Not Allowed",
            500 => "Internal Server Error",
            _ => "Unknown Status Code"
        };

    }
}
