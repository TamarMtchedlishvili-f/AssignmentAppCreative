using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using AssignmentAppCreative.Interfaces;

namespace AssignmentAppCreative.Services;

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