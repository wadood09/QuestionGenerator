using AutoMapper;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Core.Domain.Entities;
using QuestionGenerator.Core.Domain.Enums;
using QuestionGenerator.Models;
using QuestionGenerator.Models.TokenModel;

namespace QuestionGenerator.Core.Application.Services
{
    public class Tokenservice : ITokenService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Tokenservice(IUserRepository userRepository, IUnitOfWork unitOfWork, ITokenRepository tokenRepository)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _tokenRepository = tokenRepository;
        }

        public async Task<BaseResponse<string>> GenerateToken(int userId, TokenType tokenType)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                return new BaseResponse<string>
                {
                    Message = "User not found. Please register and try again later",
                    Status = false
                };
            }

            var userToken = await _tokenRepository.GetAsync(x => x.UserId == userId && x.TokenType == tokenType);
            if (userToken != null)
                _tokenRepository.Remove(userToken);

            var newToken = new Token
            {
                DateCreated = DateTime.UtcNow,
                CreatedBy = userId.ToString(),
                TokenValue = Guid.NewGuid().ToString(),
                UserId = userId,
                TokenType = tokenType,
                Expires = tokenType == TokenType.PasswordResetToken ? DateTime.UtcNow.AddMinutes(10) : DateTime.UtcNow.AddDays(7)
            };

            await _tokenRepository.AddAsync(newToken);
            await _unitOfWork.SaveAsync();
            return new BaseResponse<string>
            {
                Message = "Token generation successfully",
                Status = true,
                Value = newToken.TokenValue
            };
        }

        public async Task<BaseResponse> ValidateToken(string token)
        {
            var isValid = await _tokenRepository.ValidateTokenAsync(token);
            return new BaseResponse
            {
                Message = isValid ? "Token is valid" : "Token is invalid or expired",
                Status = isValid
            };
        }

        public async Task<BaseResponse> DeactivateToken(string token)
        {
            var tokenObject = await _tokenRepository.GetAsync(x => x.TokenValue.Equals(token));
            if (tokenObject != null)
            {
                _tokenRepository.Remove(tokenObject);
                await _unitOfWork.SaveAsync();
            }

            return new BaseResponse
            {
                Message = "Token has been successfully deactivated",
                Status = true
            };
        }
    }
}
