using System.Net;

namespace CaseWork.Exceptions;

[System.Serializable]
public class ErrorResponse : System.Exception
{
    public HttpStatusCode Code { get; set; }
    
    public ErrorResponse(string message) : base(message)
    {
        Code = HttpStatusCode.BadRequest;
    }
    
    public ErrorResponse(string message, HttpStatusCode code) : base(message)
    {
        Code = code;
    }
}