
namespace TalabatAPIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int statusCode,string? message=null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMassageForStatuseCode(StatusCode);
        }

        private string? GetDefaultMassageForStatuseCode(int? statusCode)
        {
            return StatusCode switch
            {
                400 => "Bad REquest",
                401 => "You Are Not Authorized",
                404 => "Resource Not Found",
                500 => "Internal Server Error",
                _ => null
            };
        }
    }
}
