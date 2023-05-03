using Microsoft.Extensions.Logging;

namespace MockILoggerWithMoq;

public class SystemUnderTest
{
    public SystemUnderTest(ILogger logger)
    {
        logger.LogError("Some log message");
    }
}
