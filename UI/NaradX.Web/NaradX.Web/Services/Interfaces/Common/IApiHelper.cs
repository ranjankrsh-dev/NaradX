namespace NaradX.Web.Services.Interfaces.Common
{
    public interface IApiHelper
    {
        Task<TResponse> PostData<TRequest, TResponse>(string endpoint, TRequest data, string token = null);
        Task<TResponse> PutData<TRequest, TResponse>(string endpoint, TRequest data, string token = null);
        Task<TResponse> GetData<TResponse>(string endpoint, string token = null);
        Task<HttpResponseMessage> GetFileAsync(string endpoint, string token = null);
        Task<HttpResponseMessage> PostData(string endpoint, object data, string token = null);
        Task<TResponse> PostMultipartData<TRequest, TResponse>(string endpoint, TRequest data, string token = null);
    }
}
