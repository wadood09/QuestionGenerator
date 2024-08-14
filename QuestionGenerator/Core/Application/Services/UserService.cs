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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher, IRoleRepository roleRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _mapper = mapper;
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
                DateCreated = DateTime.Now,
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

        public async Task<LoginResponse> Login(LoginRequestModel model)
        {
            var user = await _userRepository.GetAsync(x => x.Email.Equals(model.Email));
            if(user == null)
            {
                return new LoginResponse
                {
                    Message = "Invalid login credentials",
                    Status = false,
                };
            }

            if(user.PasswordHash.IsNullOrEmpty())
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

        public Task<BaseResponse> RemoveUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> UpdateUser(int id, UserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
