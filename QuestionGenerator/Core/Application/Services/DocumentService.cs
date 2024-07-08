using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Models;
using QuestionGenerator.Models.DocumentModel;

namespace QuestionGenerator.Core.Application.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public Task<BaseResponse> CreateDocument(DocumentRequest request)
        {
            throw new NotImplementedException();
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
