using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuestionGenerator.Core.Application.Config;
using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Core.Domain.Enums;
using QuestionGenerator.Models;
using QuestionGenerator.Models.UserModel;

namespace QuestionGenerator.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly JwtConfig _jwtConfig;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;
        private readonly IPaymentService _paymentService;

        public AuthenticationController(IOptions<JwtConfig> jwtConfig, IUserService userService, IIdentityService identityService, ITokenService tokenService, IAuthService authService, IPaymentService paymentService)
        {
            _jwtConfig = jwtConfig.Value;
            _userService = userService;
            _identityService = identityService;
            _tokenService = tokenService;
            _authService = authService;
            _paymentService = paymentService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _authService.Login(request);
            if (user.Status)
            {
                if (user.HasSignedInWithGoogle)
                {
                    return Conflict(new
                    {
                        message = "This account was created using Google Sign-In, and no password is set. Please set a password or use Google Sign-In.",
                    });
                }

                var accessToken = _identityService.GenerateAccessToken(_jwtConfig.Key, _jwtConfig.Issuer, _jwtConfig.Audience, user.User);
                var isValid = _identityService.IsTokenValid(_jwtConfig.Key, _jwtConfig.Issuer, _jwtConfig.Audience, accessToken);
                if (request.RememberMe)
                {
                    var refreshToken = await _tokenService.GenerateToken(user.User.Id, TokenType.RefreshToken);
                    if (!refreshToken.Status)
                        return Unauthorized(new { message = "Invalid Login Credentials" });

                    Response.Cookies.Append("refreshToken", refreshToken.Value, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddDays(7)
                    });
                }
                return Ok(new { accessToken, role = user.User.RoleName });
            }
            return Unauthorized(new { message = user.Message });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRequest request)
        {
            var user = await _userService.CreateUser(request);
            if (user.Status)
            {
                return Ok(new { message = user.Message });
            }
            return BadRequest(new { message = user.Message });
        }

        [HttpGet("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var isValid = await _tokenService.ValidateToken(refreshToken ?? "");
            if (!isValid.Status)
                return Unauthorized(new { message = isValid.Message });

            var user = await _userService.GetUserByToken(refreshToken!);
            if (!user.Status)
                return Unauthorized(new { message = user.Message });

            var newAccessToken = _identityService.GenerateAccessToken(_jwtConfig.Key, _jwtConfig.Issuer, _jwtConfig.Audience, user.Value!);
            return Ok(new { accessToken = newAccessToken });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery] string token, [FromBody] PasswordResetRequest request)
        {
            if (token.IsNullOrEmpty())
                return BadRequest(new { message = "Token is missing" });

            var isValid = await _tokenService.ValidateToken(token);
            if (!isValid.Status)
                return BadRequest(new { message = isValid.Message });

            var user = await _userService.ResetPassword(token, request.NewPassword);
            if (!user.Status)
                return BadRequest(new { message = user.Message });

            return Ok(new { message = user.Message });
        }

        [HttpPost("send-resetLink")]
        public async Task<IActionResult> SendResetLink([FromBody] ResetLinkRequest request)
        {
            var user = await _userService.GetUserByEmail(request.Email);
            if (!user.Status)
                return NotFound(new { message = user.Message });

            var resetToken = await _tokenService.GenerateToken(user.Value!.Id, TokenType.PasswordResetToken);
            if (!resetToken.Status)
                return NotFound(new { message = resetToken.Message });

            var resetLink = await _authService.SendPasswordResetEmail(request.Email, resetToken.Value!);
            if (!resetLink.Status)
                return BadRequest(new { message = resetLink.Message });

            return Ok(new { message = resetLink.Message });
        }

        [HttpPost("signin-google")]
        public async Task<IActionResult> GoogleSignin([FromBody] GoogleSigninRequest request)
        {
            var user = await _authService.VerifyGoogleToken(request.IdToken);
            if (!user.Status)
                return BadRequest(new { message = user.Message });

            var accessToken = _identityService.GenerateAccessToken(_jwtConfig.Key, _jwtConfig.Issuer, _jwtConfig.Audience, user.Value!);
            var refreshToken = await _tokenService.GenerateToken(user.Value!.Id, TokenType.RefreshToken);
            if (!refreshToken.Status)
                return Unauthorized(new { message = "User not found. Please register and try again later" });

            Response.Cookies.Append("refreshToken", refreshToken.Value!, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });
            return Ok(new { accessToken });
        }

        [HttpGet("verify/{reference}")]
        [Authorize]
        public async Task<IActionResult> VerifyPayment([FromRoute] string reference)
        {
            var result = await _paymentService.VerifyPayment(reference);

            if (result.Status)
                return Ok(new { message = result.Message });

            if (result.Message.Contains("Payment verification failed"))
                return StatusCode(502, new { message = result.Message });

            else if (result.Message.Contains("Error processing response"))
                return StatusCode(502, new { message = result.Message });

            else if (result.Message.Contains("Payment verification failed with no message"))
                return BadRequest(new { message = result.Message });

            else
                return StatusCode(500, new { message = result.Message });
        }
    }
}
