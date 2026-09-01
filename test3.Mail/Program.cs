using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using test3.Common;
using test3.DAL;

namespace test3.Mail
{
    public class Program
    {
        public static async Task Main(String[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            #region Serilog
            var logPath = builder.Configuration["LogPath"] ?? "C:\\JiaFuHo - GF66\\Programs\\Others\\test3\\test3.Log\\test3.Aux\\test3.Mail\\Log_.txt";

            Log.Logger = new LoggerConfiguration()
                                   .WriteTo.Console(
                                       outputTemplate: "{Timestamp:HH:mm} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                                       restrictedToMinimumLevel: LogEventLevel.Information
                                   )
                                   .WriteTo.Async(x => x.File(
                                       outputTemplate: "{Timestamp:HH:mm} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                                       path: logPath,
                                       retainedFileCountLimit: null,
                                       rollingInterval: RollingInterval.Day,
                                       shared: true
                                   ))
                                   .MinimumLevel.Verbose()
                                   .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                                   .MinimumLevel.Override("System", LogEventLevel.Warning)
                                   .CreateLogger();

            builder.Services.AddSerilog();

            _logX.Decorator = new LoggerConfiguration()
                                           .WriteTo.Console(
                                                outputTemplate: "{Message:lj}{NewLine}",
                                                restrictedToMinimumLevel: LogEventLevel.Information
                                           )
                                           .WriteTo.File(
                                                outputTemplate: "{Message:lj}{NewLine}",
                                                path: logPath,
                                                retainedFileCountLimit: null,
                                                rollingInterval: RollingInterval.Day,
                                                shared: true
                                           )
                                           .MinimumLevel.Verbose()
                                           .CreateLogger();
            #endregion

            #region Options
            var opt = builder.Configuration.GetSection("SystemOptions") ?? throw new Exception("System Opt Error");

            builder.Services.Configure<AppO>(opt);
            #endregion

            #region BLL
            builder.Services.AddTransient<AppL>();
            #endregion

            #region DAL
            var test3 = builder.Configuration.GetConnectionString("test3") ?? throw new Exception("System Para Error: test3 ConnStr");

            builder.Services.ConnDB(test3);
            #endregion

            #region Controllers
            builder.Services.AddTransient<AppC>();
            #endregion

            var app = builder.Build();

            var C = app.Services.GetRequiredService<AppC>();

            await C.Run();
        }
    }
}