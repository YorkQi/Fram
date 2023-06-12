using Frame.Scheduler;

var builder = WebApplication.CreateBuilder(args);

//ע����ȼƻ�
builder.Services.AddScheduler();

var app = builder.Build();


#region ��һ�ַ�ʽ ��ȡ���м̳�IScheduler�Ĺ����࣬PS:���������SchedulerCronAttribute����
app.UseScheduler();
#endregion


#region  �ڶ��ַ�ʽ ���εķ�ʽ
List<SchedulerOption> options = new()
{
    new SchedulerOption
    {
        SchedulerName = "Test",
        SchedulerAssmbly = "Job.Tasks.TestScheduler",
        Cron = "0/5 * * * * ?"
    },
    new SchedulerOption
    {
        SchedulerName = "Test2",
        SchedulerAssmbly = "Job.Tasks.Test2Scheduler",
        Cron = "0/5 * * * * ?"
    }
};
app.UseScheduler(options);
#endregion


app.Run();
