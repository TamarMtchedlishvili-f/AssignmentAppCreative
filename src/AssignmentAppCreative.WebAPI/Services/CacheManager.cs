using AssignmentAppCreative.WebAPI.Extensions;
using AssignmentAppCreative.WebAPI.Interfaces;
using StackExchange.Redis;

namespace AssignmentAppCreative.WebAPI.Services;

public class CacheManager : ICacheManager
{
    readonly ILogger<CacheManager> _logger;
    readonly ConnectionMultiplexer _connectionMultiplexer;

    public CacheManager(ILogger<CacheManager> logger, ConnectionMultiplexer connectionMultiplexer)
    {
        _logger = logger;
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<string?> GetValueForAsync(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();

        var returnValue = (await db.StringGetAsync(key)).ToString();

        _logger.LogInformation(returnValue.IsNullOrEmpty()
            ? $"Return value for [{key}] was null or empty"
            : $"Returned [{returnValue}] for key [{key}]");

        return returnValue;
    }

    public async Task SetValueForAsync(string key, string? value)
    {
        var db = _connectionMultiplexer.GetDatabase();

        if ((await GetValueForAsync(key)).IsNotNullOrEmpty())
            throw new InvalidOperationException("Such value already exists");

        _logger.Log(LogLevel.Information, "Setting value");
        await db.StringSetAsync(key, value, TimeSpan.FromMinutes(1));
    }
}