using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Quartz.Impl.AdoJobStore;
using Quartz.Xml.JobSchedulingData20;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApolloWebUI.Data
{
    public class HolidaySearchService
    {
        static HttpClient HttpClient { get; set; } = new HttpClient();
        public IConfiguration Configuration { get; }

        public HolidaySearchService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        Dictionary<string, bool> dateDic = new Dictionary<string, bool>();

        public async Task<(bool success, bool result, string message)> CheckIsWorkday(DateTime dateTime)
        {
            try
            {
                string date = dateTime.ToString("yyyy-MM-dd");
                if (dateDic.ContainsKey(date))
                {
                    return (true, dateDic[date], default);
                }
                var url = Configuration.GetSection("HolidaySearch")["url"] + date;
                var response = await HttpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JsonDocument jsonDocument = JsonDocument.Parse(result);
                    var code = jsonDocument.RootElement.GetProperty("code").GetInt32();
                    if (code == 0)
                    {
                        var type = jsonDocument.RootElement.GetProperty("type").GetProperty("type").GetInt32();
                        //enum(0, 1, 2, 3) 节假日类型，分别表示 工作日、周末、节日、调休
                        if (type == 0 || type == 3)
                        {
                            dateDic.Add(date, true);
                            return (true, true, default);
                        }
                        else
                        {
                            dateDic.Add(date, false);
                            return (true, false, default);
                        }
                    }
                    else
                    {
                        return (false, false, $"查询接口出错:{result}");
                    }
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return (false, default, $"{response.StatusCode.ToString()},{content}");
                }
            }
            catch (Exception ex)
            {
                return (false, false, ex.ToString());
            }
        }
    }
}
