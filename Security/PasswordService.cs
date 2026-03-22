using BCrypt.Net;

public class PasswordService
{
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string password, string hashed)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashed);
    }
}