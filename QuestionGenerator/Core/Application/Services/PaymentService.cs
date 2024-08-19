using Newtonsoft.Json;
using PayStack.Net;
using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Models;

namespace QuestionGenerator.Core.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly PayStackApi _payStackApi;
        private readonly HttpClient _httpClient;

        public PaymentService(PayStackApi payStackApi, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("PaystackApi");
            _payStackApi = payStackApi;
        }

        public async Task<BaseResponse> VerifyPayment(string reference)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/verify/{reference}");

                if (!response.IsSuccessStatusCode)
                {
                    return new BaseResponse
                    {
                        Message = "Payment verification failed",
                        Status = false
                    };
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                dynamic result;
                try
                {
                    result = JsonConvert.DeserializeObject<dynamic>(responseContent);
                }
                catch (JsonException)
                {
                    return new BaseResponse
                    {
                        Message = "Error processing response",
                        Status = false
                    };
                }

                if (result?.status == true)
                {
                    return new BaseResponse
                    {
                        Message = result.message,
                        Status = true
                    };
                }

                return new BaseResponse
                {
                    Message = result?.message ?? "Payment verification failed with no message",
                    Status = false
                };
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
