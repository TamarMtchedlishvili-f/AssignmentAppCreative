namespace AssignmentAppCreative.WebAPI.Interfaces;

public interface IAwsSecretManager
{
    Task<string> GetSecret(string secretName);
}