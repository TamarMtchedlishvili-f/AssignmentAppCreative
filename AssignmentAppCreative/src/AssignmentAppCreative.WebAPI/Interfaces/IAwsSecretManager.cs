namespace AssignmentAppCreative.Interfaces;

public interface IAwsSecretManager
{
    Task<string> GetSecret(string secretName);
}