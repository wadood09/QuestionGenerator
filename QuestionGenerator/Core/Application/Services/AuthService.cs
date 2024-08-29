using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using QuestionGenerator.Core.Application.Config;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Core.Domain.Entities;
using QuestionGenerator.Models;
using QuestionGenerator.Models.UserModel;
using System.Text;

namespace QuestionGenerator.Core.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly GoogleAuthConfig _googleAuthConfig;
        private readonly BrevoConfig _brevoConfig;
        private readonly HttpClient _httpClient;
        private readonly IUserRepository _userRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;

        public AuthService(IOptions<GoogleAuthConfig> googleAuthConfig, IOptions<BrevoConfig> brevoConfig, IHttpClientFactory httpClientFactory, IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher, IMapper mapper, IFileRepository fileRepository, IRoleRepository roleRepository)
        {
            _googleAuthConfig = googleAuthConfig.Value;
            _httpClient = httpClientFactory.CreateClient("BrevoApi");
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _fileRepository = fileRepository;
            _roleRepository = roleRepository;
            _brevoConfig = brevoConfig.Value;
        }

        public async Task<LoginResponse> Login(LoginRequest model)
        {
            var user = await _userRepository.GetAsync(x => x.Email.Equals(model.Email));
            if (user == null)
            {
                return new LoginResponse
                {
                    Message = "Invalid login credentials",
                    Status = false,
                };
            }

            if (user.PasswordHash.IsNullOrEmpty())
            {
                return new LoginResponse
                {
                    Message = "It looks like you've registered using Google. Would you like to set up a password for email login?",
                    Status = true,
                    HasSignedInWithGoogle = true,
                    User = new UserResponse
                    {
                        Id = user.Id
                    }
                };
            }

            var isPassword = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, model.Password);
            if (isPassword == PasswordVerificationResult.Failed)
            {
                return new LoginResponse
                {
                    Message = "Invalid login credentials",
                    Status = false
                };
            }

            var response = _mapper.Map<UserResponse>(user);
            return new LoginResponse
            {
                Message = "Login successfull",
                Status = true,
                User = response
            };
        }

        public async Task<BaseResponse<UserResponse>> VerifyGoogleToken(string idToken)
        {
            try
            {
                var validPayload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _googleAuthConfig.ClientId }
                });


                var user = await _userRepository.GetAsync(x => x.GoogleId == validPayload.Subject);
                if (user == null)
                {
                    user = await _userRepository.GetAsync(x => x.Email == validPayload.Email);
                    if (user == null)
                    {
                        var role = await _roleRepository.GetAsync(x => x.Name.Equals("Free User"));
                        user = new User
                        {
                            Email = validPayload.Email,
                            CreatedBy = "0",
                            DateCreated = DateTime.UtcNow,
                            FirstName = validPayload.GivenName ?? "",
                            LastName = validPayload.FamilyName ?? "",
                            GoogleId = validPayload.Subject,
                            ProfilePictureUrl = await _fileRepository.SaveProfilePictureAsync(validPayload.Picture),
                            Role = role,
                            RoleId = role.Id
                        };
                        await _userRepository.AddAsync(user);
                    }
                    else
                    {
                        user.GoogleId = validPayload.Subject;
                        _userRepository.Update(user);
                    }
                    await _unitOfWork.SaveAsync();
                }

                var response = _mapper.Map<UserResponse>(user);
                return new BaseResponse<UserResponse>
                {
                    Message = "Login Successfull",
                    Status = true,
                    Value = response
                };
            }
            catch (InvalidJwtException)
            {
                return new BaseResponse<UserResponse>
                {
                    Message = "Invalid google token",
                    Status = false
                };
            }
        }


        public async Task<BaseResponse> SendPasswordResetEmail(string email, string resetToken)
        {
            try
            {
                var resetLink = $"http://localhost:5500/reset-password?token={resetToken}";

                var emailData = new
                {
                    sender = new
                    {
                        name = "Question Generator",
                        email = "wadoodenterprises09@gmail.com"
                    },
                    to = new[]
                    {
                    new { email }
                },
                    subject = "Reset Your Password",
                    htmlContent = $"<p>Click <a href='{resetLink}'>here</a> to reset your password.</p>"
                };

                var jsonContent = JsonConvert.SerializeObject(emailData);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");    

                HttpResponseMessage response = await _httpClient.PostAsync(_brevoConfig.ApiUrl, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    return new BaseResponse
                    {
                        Message = "Password reset link sent!.Check your email for further instructions",
                        Status = true
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new BaseResponse
                    {
                        Message = $"Error sending email: {response.StatusCode} - {errorContent}",
                        Status = false
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Message = $"Exception occurred: {ex.Message}",
                    Status = false
                };
            }
        }
    }
}