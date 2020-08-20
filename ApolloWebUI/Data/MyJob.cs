using ApolloWebUI.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ApolloWebUI.Data
{
    internal class MyJob : IJob
    {
        static HttpClient httpClient = new HttpClient();
        private IConfiguration configuration;
        private readonly IServiceProvider serviceProvider;
        private SmtpEmailHelper emailHelper;

        public MyJob(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            this.configuration = configuration;
            this.serviceProvider = serviceProvider;
            string host = configuration.GetSection("EmailHost").Value;
            string emailUserName = configuration.GetSection("EmailUserName").Value;
            string emailPassword = configuration.GetSection("EmailPassword").Value;
            emailHelper = new SmtpEmailHelper(host, emailUserName, emailPassword);
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var now = DateTime.Now.AddMinutes(-2);
            var time = now.ToString("yyyy-MM-ddTHH:mm");
            Console.WriteLine($"{time} 开始检查");
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var onDutyRepository = serviceScope.ServiceProvider.GetService<IOnDutyRepository>();
                try
                {
                    var users = await onDutyRepository.GetAllUsersAsync();
                    var products = await onDutyRepository.GetAllProdcutAsync();
                    var request = BuildRequestBody(time, products);
                    StringContent content = new StringContent(request, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync($"{configuration.GetSection("callChainUrl").Value}error/", content);
                    Console.WriteLine($"{time} 结束检查");
                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            Console.WriteLine($"{time} 开始发送邮件");
                            var result = await response.Content.ReadAsStringAsync();
                            List<AlarmRecordModel> records = GetRecords(result, now);
                            try
                            {
                                using (var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>())
                                {
                                    await dbContext.AlarmRecords.AddRangeAsync(records);
                                    await dbContext.SaveChangesAsync();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                            if (records.Count > 0)
                            {
                                if (users.Any() == false)
                                {
                                    Console.WriteLine("无收件人，结束发送邮件");
                                    return;
                                }
                                string body = GetBody(records, products);

                                foreach (var user in users)
                                {
                                    Console.WriteLine($"向{user.Name}发送邮件");
                                    try
                                    {
                                        await emailHelper.SendEmailAsync($"RPA报警邮件({time})", body, new string[] { user.Email });
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"向{user.Name} 发送邮件出现异常:{ex}");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine($"{time} 无需发送");
                            }
                            Console.WriteLine($"{time} 结束发送邮件");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{time} 异常：{ex}");
                        }
                    }
                    else
                    {
                        Console.WriteLine(await response.Content.ReadAsStringAsync());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{time} 异常：{ex}");
                }
            }
        }

        private string GetBody(List<AlarmRecordModel> records, IEnumerable<Product> products)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table>");
            sb.Append("<thead><tr><td>产品编号</td><td>产品名称</td><td>错误码</td><td>错误信息</td><td>详情</td></tr></thead>");
            sb.Append("<tbody>");
            foreach (var item in records)
            {
                var product = products.SingleOrDefault(p => p.Id == item.ProductId);
                var proName = product == null ? string.Empty : product.ProductName;
                sb.Append($"<tr><td>{item.ProductId}</td><td>{proName}</td><td>{item.ErrorCode}</td><td>{item.Message}</td><td><a href='{"https://www.chengangni.com/callChainParser/"}{item.TraceId}'>{item.TraceId}</a></td></tr>");
            }
            sb.Append("</tbody>");
            sb.Append("</table>");
            return sb.ToString();
        }

        private List<AlarmRecordModel> GetRecords(string result, DateTime now)
        {
            JsonElement jobj = JsonSerializer.Deserialize<JsonElement>(result);
            var records = new List<AlarmRecordModel>();
            if (jobj.GetProperty("hits").GetProperty("total").GetProperty("value").GetInt32() > 0)
            {
                foreach (var item in jobj.GetProperty("hits").GetProperty("hits").EnumerateArray())
                {
                    var record = new AlarmRecordModel() { Time = now };
                    var traceId = item.GetProperty("_source").GetProperty("traceId").GetString();
                    record.TraceId = traceId;
                    string proId = string.Empty;
                    string proName = string.Empty;
                    int code = 0;
                    string message = string.Empty;
                    if (item.GetProperty("_source").TryGetProperty("response", out var response))
                    {
                        if (response.TryGetProperty("proID", out var proIdPro))
                        {
                            proId = proIdPro.GetString();
                            record.ProductId = proId;
                        }
                        if (response.TryGetProperty("code", out var codePro))
                        {
                            if (codePro.ValueKind == JsonValueKind.Number)
                            {
                                code = codePro.GetInt32();
                            }
                            else
                            {
                                code = -99999;
                            }
                            record.ErrorCode = code;
                        }
                        if (response.TryGetProperty("message", out var messagePro) || response.TryGetProperty("errorMsg", out messagePro))
                        {
                            message = messagePro.GetString();
                            record.Message = message;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"数据异常：{result}");
                    }
                    records.Add(record);
                }
            }

            return records;
        }

        private async Task<string> BuildEmailContent(IOnDutyRepository onDutyRepository, string result)
        {
            JsonElement jobj = JsonSerializer.Deserialize<JsonElement>(result);
            StringBuilder sb = new StringBuilder();
            if (jobj.GetProperty("hits").GetProperty("total").GetProperty("value").GetInt32() > 0)
            {
                sb.Append("<table>");
                sb.Append("<thead><tr><td>产品编号</td><td>产品名称</td><td>错误码</td><td>错误信息</td><td>详情</td></tr></thead>");
                sb.Append("<tbody>");
                foreach (var item in jobj.GetProperty("hits").GetProperty("hits").EnumerateArray())
                {
                    var traceId = item.GetProperty("_source").GetProperty("traceId").GetString();
                    string proId = string.Empty;
                    string proName = string.Empty;
                    int code = 0;
                    string message = string.Empty;
                    if (item.GetProperty("_source").TryGetProperty("response", out var response))
                    {
                        if (response.TryGetProperty("proID", out var proIdPro))
                        {
                            proId = proIdPro.GetString();
                            var products = await onDutyRepository.GetAllProdcutAsync();
                            var product = products.SingleOrDefault(p => p.Id == proId);
                            proName = product == null ? string.Empty : product.ProductName;
                        }
                        if (response.TryGetProperty("code", out var codePro))
                        {
                            if (codePro.ValueKind == JsonValueKind.Number)
                            {
                                code = codePro.GetInt32();
                            }
                            else
                            {
                                code = -99999;
                            }
                        }
                        if (response.TryGetProperty("message", out var messagePro) || response.TryGetProperty("errorMsg", out messagePro))
                        {
                            message = messagePro.GetString();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"数据异常：{result}");
                        return null;
                    }

                    sb.Append($"<tr><td>{proId}</td><td>{proName}</td><td>{code}</td><td>{message}</td><td><a href='{"https://www.chengangni.com/callChainParser/"}{traceId}'>{traceId}</a></td></tr>");
                }
                sb.Append("</tbody>");
                sb.Append("</table>");
                return sb.ToString();
            }
            else
            {
                return null;
            }
        }

        private string BuildRequestBody(string time, IEnumerable<Product> products)
        {
            var jobj = new JObject();
            var query = new JObject();
            jobj.Add("query", query);
            query.Add("bool", new JObject());
            (query["bool"] as JObject).Add("must", new JArray());
            var temp = new JObject();
            temp.Add("term", new JObject());
            (temp["term"] as JObject).Add("_index", "response");

            (query["bool"]["must"] as JArray).Add(temp);
            temp = new JObject();
            temp.Add("term", new JObject());
            (temp["term"] as JObject).Add("responseTime", time);

            (query["bool"]["must"] as JArray).Add(temp);
            temp = new JObject();
            temp.Add("range", new JObject());
            var litter = new JObject();
            litter.Add("lt", 0);
            (temp["range"] as JObject).Add("response.code", litter);
            (query["bool"]["must"] as JArray).Add(temp);

            (query["bool"] as JObject).Add("must_not", new JArray());

            temp = new JObject();
            temp.Add("term", new JObject());
            (temp["term"] as JObject).Add("response.success", true);
            (query["bool"]["must_not"] as JArray).Add(temp);

            //筛选不发送邮件的产品
            var a = products.Where(o => o.IsDisable == true);
            foreach (var item in a)
            {
                temp = new JObject();
                temp.Add("term", new JObject());
                (temp["term"] as JObject).Add("response.proID", item.Id);
                (query["bool"]["must_not"] as JArray).Add(temp);
            }
            string content = jobj.ToString();
            return content;
        }
    }
}