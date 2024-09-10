using Newtonsoft.Json;

namespace QuestionGenerator.Extensions
{
    public static class StringExtensions
    {
        public static T? ExtractJson<T>(this string response)
        {
            // Find the first index of '[' and the last index of ']'
            int startIndex = response.IndexOf('[');
            int endIndex = response.LastIndexOf(']');

            // Validate if indices are found
            if (startIndex != -1 && endIndex != -1 && startIndex < endIndex)
            {
                // Extract the JSON substring
                string json = response.Substring(startIndex, endIndex - startIndex + 1);

                try
                {
                    // Deserialize the JSON into the desired type
                    var result = JsonConvert.DeserializeObject<T>(json);
                    return result;
                }
                catch (JsonException ex)
                {
                    Console.WriteLine("Error parsing JSON: " + ex.Message);
                }
            }

            // Return default if parsing fails
            return default;
        }
    }
}
