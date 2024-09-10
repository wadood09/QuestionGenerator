using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace QuestionGenerator.Infrastructure.Services
{
    public class CohereService
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://api.cohere.ai/v1/generate";

        public CohereService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CohereApi");
        }

        public async Task<string> GenerateAsync(string prompt)
        {
            try
            {
                var request = new
                {
                    model = "command-r-plus",
                    prompt = prompt,
                    max_tokens = 1000, 
                    temperature = 0.5
                };

                var requestContent = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync(ApiUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JObject.Parse(responseContent);
                    var generatedText = jsonResponse["generations"]?[0]?["text"]?.ToString();
                    return generatedText;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return null;
                }

            }
            catch(Exception)
            {
                throw new Exception();
            }
        }
    }
}
