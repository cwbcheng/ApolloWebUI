using ApolloWebUI.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApolloWebUI.Data
{
    public class AlarmService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private IScheduler _scheduler;
        private IServiceProvider _serviceProvider;

        public AlarmService(ISchedulerFactory schedulerFactory, IServiceProvider serviceProvider,IConfiguration configuration)
        {
            _schedulerFactory = schedulerFactory;
            _serviceProvider = serviceProvider;
            Configuration = configuration;
        }

        public bool IsRunning { get; set; }
        public string ErrorMessage { get; set; }
        public IConfiguration Configuration { get; }

        public async Task Run()
        {
            try
            {
                //1、通过调度工厂获得调度器
                _scheduler = await _schedulerFactory.GetScheduler();
                //2、开启调度器
                await _scheduler.Start();
                //3、创建一个触发器
                var start = Configuration["emailTime:start"];
                var startTime = new TimeOfDay(int.Parse(start.Split(':')[0]), int.Parse(start.Split(':')[1]));

                var stop = Configuration["emailTime:stop"];
                var stopTime = new TimeOfDay(int.Parse(stop.Split(':')[0]), int.Parse(stop.Split(':')[1]));

                var trigger = TriggerBuilder.Create()
                                .WithDailyTimeIntervalSchedule(x=>x.StartingDailyAt(startTime).EndingDailyAt(stopTime).WithIntervalInMinutes(1))
                                //.WithSimpleSchedule(x => x. x.WithIntervalInMinutes(1).RepeatForever())//每一分钟执行一次
                                .Build();
                //4、创建任务
                var jobDetail = JobBuilder.Create<MyJob>()
                                .WithIdentity("job", "group")
                                .Build();
                //5、将触发器和任务器绑定到调度器中
                _scheduler.JobFactory = new MyJobFactory(_serviceProvider);
                await _scheduler.ScheduleJob(jobDetail, trigger);
                IsRunning = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
