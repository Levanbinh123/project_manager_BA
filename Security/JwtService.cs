using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class JwtService
{
    private readonly JwtSettings _settings;

    public JwtService(JwtSettings settings)
    {
        _settings = settings;
    }

    public string GenerateToken(string email, string role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Role,role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(_settings.ExpirationDays),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GetEmailFromToken(string token)
    {   
          if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        token = token.Substring("Bearer ".Length).Trim();
        var handler = new JwtSecurityTokenHandler();
        if(!handler.CanReadToken(token))
        throw new SecurityTokenMalformedException("Jwt khong dung dinh dang");
        var jwt = handler.ReadJwtToken(token);

        return jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
    }
}