using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Interfaces;
using UrlShortener.Domain.Entities;
using UrlShortener.Portal.Api.Models;
using Microsoft.AspNetCore.Authorization;


[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepo;

    private readonly IJwtTokenService _jwt;

    public AuthController(IUserRepository userRepo,IJwtTokenService jwt)
    {
        _userRepo = userRepo;
        _jwt = jwt;
    }
    [Authorize]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var existing =await _userRepo.GetByEmailAsync(request.Email,CancellationToken.None);

        if (existing is not null)
            return BadRequest("Email already exists");

        var user = new User(request.Username,request.Email,BCrypt.Net.BCrypt.HashPassword(request.Password));

        await _userRepo.AddAsync(user,CancellationToken.None);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user =await _userRepo.GetByEmailAsync(request.Email,CancellationToken.None);

        if (user is null)
            return Unauthorized();

        var valid =BCrypt.Net.BCrypt.Verify( request.Password, user.PasswordHash);

        if (!valid)
            return Unauthorized();

        var token =_jwt.GenerateToken(user.Id,user.Email);

        return Ok(new {token});
    }
}