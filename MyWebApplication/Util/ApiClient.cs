using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;

namespace MyWebApplication.Util
{

    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly IConfiguration _configuration;

 

        public ApiClient(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _configuration = configuration;
            _baseUrl = configuration["InternalBaseUrl"]; ;
        }

        public async Task<string> GetAsyncString(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/{endpoint}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP errors
                throw new ApiException($"API request failed: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                // Handle JSON parsing errors
                throw new ApiException($"Failed to parse response: {ex.Message}", ex);
            }
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/{endpoint}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content);
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP errors
                throw new ApiException($"API request failed: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                // Handle JSON parsing errors
                throw new ApiException($"Failed to parse response: {ex.Message}", ex);
            }
        }

        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(data);
                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/{endpoint}", stringContent);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content);
            }
            catch (HttpRequestException ex)
            {
                throw new ApiException($"API request failed: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new ApiException($"Failed to parse response: {ex.Message}", ex);
            }
        }
    }

    // Custom exception class
    public class ApiException : Exception
    {
        public ApiException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
