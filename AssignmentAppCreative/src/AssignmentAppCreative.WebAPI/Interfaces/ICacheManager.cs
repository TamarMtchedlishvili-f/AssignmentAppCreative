namespace AssignmentAppCreative.Interfaces;

public interface ICacheManager
{
    Task<string?> GetValueForAsync(string key);
    Task SetValueFor(string key, string? value);
}