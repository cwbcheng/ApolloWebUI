using ApolloWebUI.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApolloWebUI.Data
{
    public class ApolloService
    {
        private static HttpClient httpClient = new HttpClient();
        private IConfiguration Configuration;

        public ApolloService(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public async Task<NamespaceModel> GetNamespanceAsync(string name)
        {
            var url = Configuration.GetValue<string>("GetApplicationUrl");
            var key = Configuration.GetValue<string>("ApolloKey");
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(key);
            var response = await httpClient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            jsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
            return JsonSerializer.Deserialize<NamespaceModel>(result, jsonSerializerOptions);
        }
    }
}
