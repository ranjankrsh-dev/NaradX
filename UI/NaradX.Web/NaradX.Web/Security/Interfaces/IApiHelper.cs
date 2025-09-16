namespace NaradX.Web.Security.Interfaces
{
    public interface IApiHelper
    {
        Task<TResponse> PostData<TRequest, TResponse>(string endpoint, TRequest data, string token = null);
        Task<TResponse> GetData<TResponse>(string endpoint, string token = null);
        Task<HttpResponseMessage> PostData(string endpoint, object data, string token = null);
    }
}
