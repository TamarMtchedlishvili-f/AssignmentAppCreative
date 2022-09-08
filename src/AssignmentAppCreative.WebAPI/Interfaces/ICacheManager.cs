namespace AssignmentAppCreative.WebAPI.Interfaces;

public interface ICacheManager
{
    Task<string?> GetValueForAsync(string key);
    Task SetValueForAsync(string key, string? value);
}