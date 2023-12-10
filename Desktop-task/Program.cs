using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using Desktop_task;
using Desktop_task.Services.Data;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

public class Program
{
    [STAThread]
    public static void Main()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<App>();
                services.AddSingleton<MainWindow>();
                var databaseConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                services.AddDbContext<FinanceDbContext>(options =>
                    options.UseSqlServer(databaseConnectionString));

            })
            .Build();
        var app = host.Services.GetService<App>();
        app?.Run();
    }
}
