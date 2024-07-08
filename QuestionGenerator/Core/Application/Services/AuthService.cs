using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Core.Domain.Entities;
using QuestionGenerator.Models;
using QuestionGenerator.Models.UserModel;

namespace QuestionGenerator.Core.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(IUserRepository userRepository, IUnitOfWork unitOfWork, Microsoft.AspNetCore.Identity.IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<BaseResponse<UserResponse>> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetAsync(x => x.Email == request.Email);
            if (user == null)
            {
                return new BaseResponse<UserResponse>
                {
                    Message = "Incorrect Email or Password",
                    Status = false
                };
            }

            var isPassword = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (isPassword == PasswordVerificationResult.Failed)
            {
                return new BaseResponse<UserResponse>
                {
                    Message = "Incorrect Email or Password",
                    Status = false
                };
            }

            return new BaseResponse<UserResponse>
            {
                Message = "Login Successfull",
                Status = true
            };
        }

        public async Task<BaseResponse<UserResponse>> VerifyGoogleTokenAsync(string idToken)
        {
            throw new NotImplementedException();
        }
    }
}
