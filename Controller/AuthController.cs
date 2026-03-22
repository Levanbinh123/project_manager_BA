using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwtService;
    private readonly PasswordService _passwordService;
    private readonly IUserService _userService;

    public AuthController(
        AppDbContext context,
        JwtService jwtService,
        PasswordService passwordService,
        IUserService userService)
    {
        _context = context;
        _jwtService = jwtService;
        _passwordService = passwordService;
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(User user)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == user.Email);

        if (existingUser != null)
            return BadRequest("Email already exists");

        user.Password = _passwordService.Hash(user.Password);
        user.Role = Role.ROLE_USER;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var jwt = _jwtService.GenerateToken(user.Email,user.Role.ToString());

        return StatusCode(201, new AuthResponse(jwt,"Register success   "));
    }

    [HttpPost("signup")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var user = await _userService.FindUserByEmail(request.Email);

        if (!_passwordService.Verify(request.Password, user.Password))
            return Unauthorized("Invalid password");

        var jwt = _jwtService.GenerateToken(user.Email,user.Role.ToString());

      return Ok(new AuthResponse(jwt,"singupsucces"));
       
    }
}