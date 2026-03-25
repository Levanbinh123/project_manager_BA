using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/users")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService=userService;
    }
    [HttpGet("profile")]
    public async Task<ActionResult<User>>GetUserprofile([FromHeader(Name ="Authorization")]string Authorization)
    {
        var user=await _userService.FindUserProfileByJwt(Authorization);
        return Ok(user);
    }
    [HttpGet("admin")]
    [Authorize(Roles ="ROLE_ADMIN")]
    public async Task<ActionResult<List<User>>>GetAllUsers()
    {
        var users=await _userService.FindAllUsers();
        return Ok(users);
    }
    [HttpPatch("admin/{userId}")]
    [Authorize(Roles ="ROLE_ADMIN")]
    public async Task<ActionResult<User>>UpdateUser(long userId,[FromBody]User user)
    {
        var updatedUser=await _userService.UpdateUser(userId,user);
        return Ok(updatedUser);
    }
    [HttpDelete("admin/{userId}")]
    [Authorize(Roles ="ROLE_ADMIN")]
    public async Task<ActionResult>DeleteUser(long userId)
    {
        await _userService.DeleteUser(userId);
        return Ok(new {Message="User deleted  successfully"});
    }

}