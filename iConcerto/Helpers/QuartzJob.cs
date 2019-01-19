using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using iConcerto.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Job;

namespace iConcerto.Helpers
{
    public class MailJob : IJob
    {
        public List<UserData> users;
        public Task Execute(IJobExecutionContext context)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                DateTime nextDay = DateTime.Now.AddDays(1);
                List<Events> events = db.Events.Where(e => e.Date <= nextDay).ToList<Events>();
                foreach (Events item in events)
                {
                    var client = new SmtpClient("smtp.gmail.com", 587)
                    {
                        Credentials = new System.Net.NetworkCredential("testiconcerto@gmail.com", "concerto1234567789!"),
                        EnableSsl = true
                    };
                    MailMessage mailMessage = new MailMessage()
                    {
                        From = new MailAddress("testiconcerto@gmail.com","iConcerto"),
                        Subject = "iConcerto Notification - " + item.Name,
                        Body = "Tommorow is " + item.Name + " at " + item.Date
                    };
                    foreach (UserData userData in item.EventToUser.Where(ue => ue.Notified == false).Select(ue => ue.UserData).ToList())
                    {
                        mailMessage.To.Add(userData.FirstMidName);
                        EventToUser eventToUser = db.EventToUser.SingleOrDefault(eu => eu.UserData.UserDataId == userData.UserDataId && eu.Events.EventId == item.EventId);
                        eventToUser.Notified = true;
                        db.SaveChanges();
                    }
                    if(mailMessage.To != null && mailMessage.To.Count > 0)
                        client.Send(mailMessage);
                }


            }
            return null;
        }
    }

    public class JobScheduler
    {
        public static async Task StartAsync()
        {
            // construct a scheduler factory
            NameValueCollection props = new NameValueCollection
            {
                { "quartz.serializer.type", "binary" }
            };
            StdSchedulerFactory factory = new StdSchedulerFactory(props);

            // get a scheduler
            IScheduler sched =  await factory.GetScheduler();
             sched.Start();

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<MailJob>()
                .WithIdentity("myJob", "group1")
                .Build();

            // Trigger the job to run now, and then every 40 seconds
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(120)
                    .RepeatForever())
            .Build();

            await sched.ScheduleJob(job, trigger);
        }
    }
}