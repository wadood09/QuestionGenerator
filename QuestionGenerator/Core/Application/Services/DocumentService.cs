using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenAI_API;
using OpenAI_API.Completions;
using QuestionGenerator.Core.Application.Config;
using QuestionGenerator.Core.Application.Exceptions;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Core.Domain.Entities;
using QuestionGenerator.Core.Domain.Enums;
using QuestionGenerator.Models;
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
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var loginUserId = int.Parse(userId ?? "0");
            var user = await _userRepository.GetAsync(loginUserId) ?? throw new UnAuthenticatedUserException();

            var documentExists = await _documentRepository.ExistsAsync(loginUserId, request.Title);
            if (documentExists)
            {
                throw new DocumentAlreadyExistsException();
            }

            if (request.Document == null || request.Document.Length == 0)
            {
                throw new InvalidDocumentException();
            }

            double fileSizeInMB = request.Document.Length / (1024.0 * 1024.0);

            string[] allowedExtensions = { "txt", "doc", "docx", "pdf", "ppt", "pptx" };
            var currentDocumentExtension = Path.GetExtension(request.Document.FileName)?.TrimStart('.').ToLower();

            if (!allowedExtensions.Contains(currentDocumentExtension) || string.IsNullOrEmpty(currentDocumentExtension))
            {
                throw new UnsupportedDocumentTypeException();
            }

            switch (user.Role.Name)
            {
                case "Basic User":
                    if (fileSizeInMB > 5)
                    {
                        throw new FileTooLargeException(UserType.Basic);
                    }
                    if (currentDocumentExtension != "txt")
                    {
                        throw new FileTypeRestrictedException(UserType.Basic);
                    }
                    break;

                case "Standard User":
                    if (fileSizeInMB > 20)
                    {
                        throw new FileTooLargeException(UserType.Standard);
                    }
                    string[] standardRestrictedExtensions = { "txt", "doc", "docx" };
                    if (!standardRestrictedExtensions.Contains(currentDocumentExtension))
                    {
                        throw new FileTypeRestrictedException(UserType.Standard);
                    }
                    break;

                case "Premium User":
                    if (fileSizeInMB > 50)
                    {
                        throw new FileTooLargeException(UserType.Premium);
                    }
                    break;

                default:
                    throw new InvalidUserRoleException();
            }

            var documentUrl = await _fileRepository.UploadAsync(request.Document);
            var documentContent = File.ReadAllLines($"{_storageConfig.Path}\\Documents\\{documentUrl}");
            var prompt = GetPrompt(documentContent);

            var openApi = new OpenAIAPI(_openAiConfig.ApiKey);
            var completionRequest = new CompletionRequest
            {
                Prompt = prompt,
                MaxTokens = 500
            };

            var result = await openApi.Completions.CreateCompletionAsync(completionRequest);

            var document = new Document
            {
                Title = request.Title,
                CreatedBy = loginUserId.ToString(),
                DateCreated = DateTime.UtcNow,
                DocumentUrl = documentUrl,
                UserId = user.Id,
                TableOfContentsJson = result.Completions[0].Text
            };

            await _documentRepository.AddAsync(document);
            await _unitOfWork.SaveAsync();

            return new BaseResponse
            {
                Message = "Document uploaded successfully",
                Status = true
            };
        }

        public async Task<BaseResponse> DeleteDocument(int id)
        {
            var document = await _documentRepository.GetAsync(id);
            if (document == null)
            {
                return new BaseResponse
                {
                    Message = "Document not found",
                    Status = false
                };
            }

            _documentRepository.Remove(document);
            await _unitOfWork.SaveAsync();

            return new BaseResponse
            {
                Message = "Document has been deleted successfully",
                Status = true
            };

        }

        public async Task<BaseResponse<DocumentResponse>> GetDocument(int id)
        {
            var document = await _documentRepository.GetAsync(id);
            if (document == null)
            {
                return new BaseResponse<DocumentResponse>
                {
                    Message = "Document does not exists",
                    Status = false
                };
            }

            var response = _mapper.Map<DocumentResponse>(document);

            return new BaseResponse<DocumentResponse>
            {
                Status = true,
                Message = "Document successfully found",
                Value = response
            };
        }

        public async Task<BaseResponse<DocumentResponse>> GetDocument(int userId, string title)
        {
            var document = await _documentRepository.GetAsync(x => x.Title == title && x.UserId == userId);
            if (document == null)
            {
                return new BaseResponse<DocumentResponse>
                {
                    Message = "Document does not exists",
                    Status = false
                };
            }

            var response = _mapper.Map<DocumentResponse>(document);

            return new BaseResponse<DocumentResponse>
            {
                Status = true,
                Message = "Document successfully found",
                Value = response
            };
        }

        public async Task<BaseResponse<ICollection<DocumentsResponse>>> GetDocumentsByUser(int userId)
        {
            var documents = await _documentRepository.GetAllAsync(x => x.UserId == userId);
            var response = _mapper.Map<List<DocumentsResponse>>(documents);
            return new BaseResponse<ICollection<DocumentsResponse>>
            {
                Message = "Lists of documents",
                Status = true,
                Value = response
            };
        }

        private static double? GetGrade(AssessmentSubmission assessment)
        {
            if (assessment == null)
                return null;
            var score = 0;
            assessment.Results.ForEach(x =>
            {
                if (x.Question.Answer.Equals(x.UserAnswer))
                    score++;
            });
            var percentage = score / assessment.Results.Count * 100;
            return percentage;
        }

        private static string GetPrompt(string[] documentContent)
        {
            return "Please generate a JSON list of strings representing the table of contents or chapters or headings of the following document:\r\n\r\n" +
                $"---\r\n{documentContent}\r\n---\r\n\r\n" +
                "The output should be a JSON array, where each string represents a chapter or heading from the document. " +
                "The JSON should be formatted as follows:\r\n" +
                @"[ 'Chapter 1: Introduction',
                    'Chapter 2: Literature Review',
                    'Chapter 3: Methodology',
                    ...
                  ]" +
                "\r\n\r\nIf the document does not contain clear chapters or headings, extract the main sections based on the content." +
                "If the document is too short or lacks structure, return an empty array.\r\n";
        }
    }
}
