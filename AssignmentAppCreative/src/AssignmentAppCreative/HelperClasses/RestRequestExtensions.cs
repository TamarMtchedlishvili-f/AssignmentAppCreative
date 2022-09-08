using System.Collections.Immutable;
using System.Globalization;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Kralizek.Extensions.Configuration.Internal;
using RestSharp;

namespace AssignmentAppCreative.HelperClasses;

public static class RestRequestExtensions
{
    public static bool IsNullOrEmpty(this string? text) => string.IsNullOrEmpty(text);
    public static bool IsNotNullOrEmpty(this string? text) => !text.IsNullOrEmpty();

    public static string ToTitleCase(this string title) => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());

    
    // ReSharper disable once InconsistentNaming
    public static RestRequest AddAPIKey(this RestRequest request, string key)
        => request.AddParameter("appid", key);

    public static async Task<IEnumerable<T>> WhenAll<T>(this IEnumerable<Task<T>> items)
    {
        var itemsList = items.ToImmutableList();
        await Task.WhenAll(itemsList);

        return itemsList.Select(i => i.Result);
    }
}

public interface IAwsSecretManager
{
    Task<string> GetSecret(string secretName);
}

public class AwsSecretManager : IAwsSecretManager
{
    public async Task<string> GetSecret(string secretName)
    {
        // var secretName = "APIkey_AssignmentAppCreativeTM__Value";
        var region = "us-east-1";
        // var secret = "";

        var client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

        var request = new GetSecretValueRequest
        {
            SecretId = secretName,
            VersionStage = "AWSCURRENT" // VersionStage defaults to AWSCURRENT if unspecified.
        };

        // GetSecretValueResponse response = null;

        // In this sample we only handle the specific exceptions for the 'GetSecretValue' API.
        // See https://docs.aws.amazon.com/secretsmanager/latest/apireference/API_GetSecretValue.html
        // We rethrow the exception by default.

        var response = await client.GetSecretValueAsync(request);

        // Decrypts secret using the associated KMS key.
        // Depending on whether the secret is a string or binary, one of these fields will be populated.
        if (response.SecretString != null)
        {
            var secret = response.SecretString;
            return secret;
        }
        else
        {
            var memoryStream = response.SecretBinary;
            var reader = new StreamReader(memoryStream);
            var decodedBinarySecret = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
            return decodedBinarySecret;
        }

        // Your code goes here.
    }

}