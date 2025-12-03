namespace HRSystem.Application.DTOs.Common
{
    public class ErrorResponse
    {
        public bool Success => false;
        public string Message { get; set; }

        public ErrorResponse(string message)
        {
            Message = message;
        }
    }
}
