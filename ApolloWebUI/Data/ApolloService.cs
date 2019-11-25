using ApolloWebUI.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApolloWebUI.Data
{
    public class ApolloService
    {
        private static HttpClient httpClient = new HttpClient();
        private IConfiguration Configuration;
        public ApolloEnvironmentEnum Environment { get; set; } = ApolloEnvironmentEnum.dev;
        JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public ApolloService(IConfiguration configuration)
        {
            this.Configuration = configuration;
            jsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
            var key = Configuration.GetValue<string>("ApolloKey");
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(key);
        }

        public async Task<NamespaceModel> GetNamespanceAsync(string name)
        {
            string apolloDomain = Configuration.GetValue<string>("ApolloDomain");
            string appId = Configuration.GetValue<string>("ApolloAppId");
            var url = $"{apolloDomain}openapi/v1/envs/{Environment.ToString()}/apps/{appId}/clusters/default/namespaces/{name}";
            var response = await httpClient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<NamespaceModel>(result, jsonSerializerOptions);
        }

        public async Task<bool> Update(string key, string value)
        {
            string apolloDomain = Configuration.GetValue<string>("ApolloDomain");
            string appId = Configuration.GetValue<string>("ApolloAppId");
            var url = $"{apolloDomain}openapi/v1/envs/{Environment.ToString()}/apps/{appId}/clusters/default/namespaces/application/items/{key}";
            var content = JsonSerializer.Serialize(new { key, value, comment = "a", dataChangeLastModifiedBy = "apollo" }, typeof(object), jsonSerializerOptions);
            StringContent stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(url, stringContent);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                return false;
            }
        }

        public async Task<bool> Release()
        {
            string apolloDomain = Configuration.GetValue<string>("ApolloDomain");
            string appId = Configuration.GetValue<string>("ApolloAppId");
            var url = $"{apolloDomain}openapi/v1/envs/{Environment.ToString()}/apps/{appId}/clusters/default/namespaces/application/releases";
            var content = JsonSerializer.Serialize(new { releaseTitle = DateTime.Now.ToString("yyyy-MM-dd"), releaseComment = "test", releasedBy = "apollo" }, typeof(object), jsonSerializerOptions);
            StringContent stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, stringContent);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                return false;
            }
        }
    }

    public enum ApolloEnvironmentEnum
    {
        dev,
        pro,
    }
}
