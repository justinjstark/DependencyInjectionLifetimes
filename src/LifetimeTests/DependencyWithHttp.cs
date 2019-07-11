using System.Net.Http;

namespace LifetimeTests
{
    public class DependencyWithHttp
    {
        public HttpClient HttpClient;

        public DependencyWithHttp(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }
    }
}
