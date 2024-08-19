using QuestionGenerator.Models;
using QuestionGenerator.Models.PaymentModel;

namespace QuestionGenerator.Core.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<BaseResponse> VerifyPayment(string reference);
    }
}
