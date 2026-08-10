global using Serilog;

namespace test3.Test
{
    /// <summary> 
    /// App流程: App初始化 →
    /// 
    /// </summary>
    internal class Program
    {
        static void Main(String[] args)
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (AppRunner._loggerO != null) { AppRunner._loggerO!.Fatal(ex, "StatusCode = 5200, Message = App Error, ex = "); }
                else
                {
                    Log.Logger = new LoggerConfiguration()
                                    .WriteTo.Console(
                                         outputTemplate: "{Timestamp:HH:mm} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                                     )
                                    .WriteTo.File(
                                         outputTemplate: "{Timestamp:HH:mm} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                                         path: "C:\\JiaFuHo - GF66\\Programs\\Others\\test3\\test3.Log\\test3.Aux\\test3.Test\\Log_.txt",
                                         retainedFileCountLimit: null,
                                         rollingInterval: RollingInterval.Day,
                                         shared: true
                                     )
                                    .MinimumLevel.Verbose()
                                    .CreateLogger();

                    Log.Fatal(ex, "StatusCode = 5200, Message = App Error, ex = ");
                }
            }
        }
    }
}