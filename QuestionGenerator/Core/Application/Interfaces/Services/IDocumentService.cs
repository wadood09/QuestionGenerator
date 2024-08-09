using QuestionGenerator.Models;
using QuestionGenerator.Models.DocumentModel;

namespace QuestionGenerator.Core.Application.Interfaces.Services
{
    public interface IDocumentService
    {
        Task<BaseResponse> CreateDocument(DocumentRequest request);
        Task<BaseResponse<DocumentResponse>> GetDocument(int id);
        Task<BaseResponse<ICollection<DocumentsResponse>>> GetDocumentsByUser(int userId);
        Task<BaseResponse> DeleteDocument(int id);
    }
}
