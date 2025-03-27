using CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;
using Microsoft.Extensions.Configuration;
using Serilog;
using static CrossCuttingConcerns.Logging.Serilog.Messages.SerilogMessages;

namespace CrossCuttingConcerns.Logging.Serilog.Logger;

public class FileLogger : LoggerServiceBase
{
    public FileLogger(IConfiguration configuration)
    {
        var logConfig =
            configuration.GetSection("SerilogConfigurations:FileLogConfiguration").Get<FileLogConfiguration>() ??
            throw new ArgumentNullException(NullOptionsMessage);

        var logFilePath = string.Format("{0},{1}", Directory.GetCurrentDirectory() + logConfig.FolderPath, ".txt");

        Logger = new LoggerConfiguration().WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day,
                retainedFileTimeLimit: null, fileSizeLimitBytes: 5000000,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}")
            .CreateLogger();
    }
}