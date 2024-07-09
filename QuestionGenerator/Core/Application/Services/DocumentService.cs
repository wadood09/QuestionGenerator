using AutoMapper;
using Microsoft.Extensions.Options;
using QuestionGenerator.Core.Application.Config;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Core.Domain.Entities;
using QuestionGenerator.Models;
using QuestionGenerator.Models.AssessmentModel;
using QuestionGenerator.Models.DocumentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace QuestionGenerator.Core.Application.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly OpenAiConfig _openAiConfig;
        private readonly StorageConfig _storageConfig;
        private readonly IDocumentRepository _documentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DocumentService(IOptions<OpenAiConfig> openAiConfig, IOptions<StorageConfig> storageConfig, IDocumentRepository documentRepository, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper, IFileRepository fileRepository)
        {
            _openAiConfig = openAiConfig.Value;
            _storageConfig = storageConfig.Value;
            _documentRepository = documentRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileRepository = fileRepository;
        }

        public async Task<BaseResponse> CreateDocument(DocumentRequest request)
        {
            var loginUserId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub));
            var user = await _userRepository.GetAsync(loginUserId);
            if (user == null)
            {
                return new BaseResponse
                {
                    Message = "User is not authenticated",
                    Status = false
                };
            }

            var documentExists = await _documentRepository.ExistsAsync(loginUserId, request.Title);
            if (documentExists)
            {
                return new BaseResponse
                {
                    Message = $"{request.Title} document already exists",
                    Status = false
                };
            }

            if (request.Document == null || request.Document.Length == 0)
            {
                return new BaseResponse
                {
                    Message = "No file uploaded",
                    Status = false
                };
            }

            double fileSizeInMB = request.Document.Length / (1024.0 * 1024.0);

            if (user.Role.Name == "Basic User")
            {
                if (fileSizeInMB > 10)
                {
                    return new BaseResponse
                    {
                        Message = "File size must be 10mb or less",
                        Status = false
                    };
                }
            }
            else
            {
                if (fileSizeInMB > 30)
                {
                    return new BaseResponse
                    {
                        Message = "File size must be 30mb or less",
                        Status = false
                    };
                }
            }

            var documentUrl = await _fileRepository.UploadAsync(request.Document);
            var documentContent = File.ReadAllLines($"{_storageConfig.Path}\\{documentUrl}");
            var prompt = "Please generate a JSON list of strings representing the table of contents or chapters or headings of the following document:\r\n\r\n" +
                $"---\r\n{documentContent}\r\n---\r\n\r\n" +
                "The output should be a JSON array, where each string represents a chapter or heading from the document. " +
                "The JSON should be formatted as follows:\r\n\r\n[\r\n    \"Chapter 1: Introduction\",\r\n    \"Chapter 2: Literature Review\",\r\n    \"Chapter 3: Methodology\",\r\n    ...\r\n]\r\n\r\nIf the document does not contain clear chapters or headings, extract the main sections based on the content.\r\n"

            var document = new Document
            {
                Title = request.Title,
                CreatedBy = loginUserId.ToString(),
                DateCreated = DateTime.Now,

            }
        }

        public Task<BaseResponse> DeleteDocument(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<DocumentResponse>> GetDocument(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<ICollection<DocumentResponse>>> GetDocumentsMyUser(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
