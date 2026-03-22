public class AuthResponse
{
    public string Message{get;set;}
    public string Jwt{get;set;}
    
    public AuthResponse(string jwt, string message)
    {
        Jwt=jwt;
        Message=message;
    }

}