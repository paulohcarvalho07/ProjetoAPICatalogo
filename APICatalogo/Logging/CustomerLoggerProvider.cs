using System.Collections.Concurrent;

namespace APICatalogo.Logging;

public class CustomerLoggerProvider : ILoggerProvider
{
    readonly CustomerLoggerProviderConfiguration loggerconfig;

    readonly ConcurrentDictionary<string, CustomerLogger> loggers = new ConcurrentDictionary<string, CustomerLogger>();

    public CustomerLoggerProvider(CustomerLoggerProviderConfiguration config)
    {
        loggerconfig = config;  
    }

    public ILogger CreateLogger(string categoryName)
    {
        return loggers.GetOrAdd(categoryName, name => new CustomerLogger(name, loggerconfig));
    }

    public void Dispose()
    {
        loggers.Clear();
    }
}
