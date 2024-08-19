using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Core.Domain.Entities;
using QuestionGenerator.Models;
using QuestionGenerator.Models.UserModel;

namespace QuestionGenerator.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ITokenRepository _tokenRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher, IRoleRepository roleRepository, IMapper mapper, IFileRepository fileRepository, ITokenRepository tokenRepository)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _fileRepository = fileRepository;
            _tokenRepository = tokenRepository;
        }

        public async Task<BaseResponse> CreateUser(UserRequest request)
        {
            var exists = await _userRepository.ExistsAsync(request.Email);
            if (exists)
            {
                return new BaseResponse
                {
                    Message = "Email already exists",
                    Status = false
                };
            }

            if (request.Password != request.ConfirmPassword)
            {
                return new BaseResponse
                {
                    Message = "Passwords do not match",
                    Status = false
                };
            }

            var role = _roleRepository.GetAsync(x => x.Name.Equals("Basic User"));
            if (role == null)
            {
                return new BaseResponse
                {
                    Message = "Role not found",
                    Status = false
                };
            }

            var user = new User
            {
                CreatedBy = "0",
                DateCreated = DateTime.UtcNow,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                RoleId = role.Id
            };
            var hashedPassword = _passwordHasher.HashPassword(user, request.Password);
            user.PasswordHash = hashedPassword;

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveAsync();

            return new BaseResponse
            {
                Message = "Registration successfull",
                Status = true
            };
        }

        public async Task<BaseResponse<UserResponse>> GetUser(int id)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                return new BaseResponse<UserResponse>
                {
                    Message = "User not found",
                    Status = false
                };
            }

            var response = _mapper.Map<UserResponse>(user);
            return new BaseResponse<UserResponse>
            {
                Message = "User successfully found",
                Status = true,
                Value = response
            };
        }

        public async Task<BaseResponse<UserResponse>> GetUserByToken(string token)
        {
            var tokenObject = await _tokenRepository.GetAsync(x => x.TokenValue == token);
            if (tokenObject == null)
            {
                return new BaseResponse<UserResponse>
                {
                    Message = "Token is invalid or expired",
                    Status = false
                };
            }

            var response = _mapper.Map<UserResponse>(tokenObject.User);
            return new BaseResponse<UserResponse>
            {
                Message = "User successfully found",
                Status = true,
                Value = response
            };
        }

        public async Task<BaseResponse<UserResponse>> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetAsync(x => x.Email == email);
            if (user == null)
            {
                return new BaseResponse<UserResponse>
                {
                    Message = "User not found",
                    Status = false
                };
            }

            var response = _mapper.Map<UserResponse>(user);
            return new BaseResponse<UserResponse>
            {
                Message = "User successfully found",
                Status = true,
                Value = response
            };
        }

        public async Task<BaseResponse> RemoveUser(int id)
        {
            var user = await _userRepository.GetAsync(id);
            if(user == null)
            {
                return new BaseResponse
                {
                    Message = "User not found",
                    Status = false
                };
            }

            _userRepository.Remove(user);
            return new BaseResponse
            {
                Message = "Account successfully deleted",
                Status = true
            };
        }

        public async Task<BaseResponse> UpdateUser(int id, UpdateUserRequest request)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                return new BaseResponse
                {
                    Message = "User not found",
                    Status = false
                };
            }

            if(request.Email != null)
            {
                var emailExists = await _userRepository.ExistsAsync(x => x.Email == request.Email && x.Id != id);
                if(emailExists)
                {
                    return new BaseResponse
                    {
                        Message = "Email already exists",
                        Status = false
                    };
                }

                user.Email = request.Email;
            }
            user.FirstName = request.FirstName ?? user.FirstName;
            user.LastName = request.LastName ?? user.LastName;
            user.ProfilePictureUrl = await _fileRepository.UploadAsync(request.ImageUrl) ?? user.ProfilePictureUrl;
            user.ModifiedBy = "0";
            user.DateModified = DateTime.UtcNow;
            
            _userRepository.Update(user);
            return new BaseResponse
            {
                Message = "Update successfull",
                Status = true
            };
        }

        public async Task<BaseResponse> ResetPassword(string token, string newPassword)
        {
            var tokenObject = await _tokenRepository.GetAsync(x => x.TokenValue.Equals(token));
            if (tokenObject == null)
            {
                return new BaseResponse
                {
                    Message = "Invalid token",
                    Status = false
                };
            }

            var user = await _userRepository.GetAsync(tokenObject.UserId);
            if (user == null)
            {
                return new BaseResponse
                {
                    Message = "User not found",
                    Status = false
                };
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);
            _userRepository.Update(user);
            await _unitOfWork.SaveAsync();

            return new BaseResponse
            {
                Message = "Password reset succesfull",
                Status = true
            };
        }
    }
}
