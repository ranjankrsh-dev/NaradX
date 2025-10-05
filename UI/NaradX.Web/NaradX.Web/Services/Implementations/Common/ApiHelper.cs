using NaradX.Web.Services.Interfaces.Common;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace NaradX.Web.Services.Implementations.Common
{
    public class ApiHelper : IApiHelper
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiHelper(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_configuration["ApiSettings:BaseUrl"]);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.Timeout = TimeSpan.FromMinutes(5);
        }

        public async Task<TResponse> PostData<TRequest, TResponse>(string endpoint, TRequest data, string token = null)
        {
            await AddAuthorizationHeader(token);

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Authentication required");
            }

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(responseContent);
        }

        public async Task<TResponse> PutData<TRequest, TResponse>(string endpoint, TRequest data, string token = null)
        {
            await AddAuthorizationHeader(token);

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(endpoint, content);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Authentication required");
            }

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(responseContent);
        }

        public async Task<TResponse> GetData<TResponse>(string endpoint, string token = null)
        {
            await AddAuthorizationHeader(token);

            var response = await _httpClient.GetAsync(endpoint);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Authentication required");
            }

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(responseContent);
        }

        public async Task<HttpResponseMessage> GetFileAsync(string endpoint, string token = null)
        {
            await AddAuthorizationHeader(token);
            return await _httpClient.GetAsync(endpoint);
        }

        public async Task<HttpResponseMessage> PostData(string endpoint, object data, string token = null)
        {
            await AddAuthorizationHeader(token);

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await _httpClient.PostAsync(endpoint, content);
        }

        public async Task<TResponse> PostMultipartData<TRequest, TResponse>(string endpoint, TRequest data, string token = null)
        {
            await AddAuthorizationHeader(token);

            using var content = new MultipartFormDataContent();

            var properties = typeof(TRequest).GetProperties();
            foreach (var prop in properties)
            {
                var value = prop.GetValue(data);
                if (value == null) continue;

                if (value is IFormFile file)
                {
                    var streamContent = new StreamContent(file.OpenReadStream());
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                    content.Add(streamContent, prop.Name, file.FileName);
                }
                else
                {
                    content.Add(new StringContent(value.ToString()), prop.Name);
                }
            }

            var response = await _httpClient.PostAsync(endpoint, content);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Authentication required");
            }

            //response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(responseContent);
        }

        private async Task AddAuthorizationHeader(string token = null)
        {
            var authToken = token ?? _httpContextAccessor.HttpContext?.Session.GetString("authToken");

            if (!string.IsNullOrEmpty(authToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", authToken);
            }

            // Clear previous headers to avoid duplication
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            if (!string.IsNullOrEmpty(authToken))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
            }
        }
    }
}
