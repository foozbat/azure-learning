using Microsoft.Identity.Client;
using dotenv.net;

// Load environment variables from .env file
DotEnv.Load();
var envVars = DotEnv.Read();
string _clientId = envVars["CLIENT_ID"];
string _tenantId = envVars["TENANT_ID"];

// Define the scopes required for authentication
string[] _scopes = { "User.Read" };

// Build the MSAL public client application with authority and redirect URI
var app = PublicClientApplicationBuilder.Create(_clientId)
    .WithAuthority(AzureCloudInstance.AzurePublic, _tenantId)
    .WithDefaultRedirectUri()
    .Build();

// Attempt to acquire an access token silently or interactively
AuthenticationResult result;
try
{
    // Try to acquire token silently from cache for the first available account
    var accounts = await app.GetAccountsAsync();
    result = await app.AcquireTokenSilent(_scopes, accounts.FirstOrDefault())
        .ExecuteAsync();
}
catch (MsalUiRequiredException)
{
    // If silent token acquisition fails, prompt the user interactively
    result = await app.AcquireTokenInteractive(_scopes)
        .ExecuteAsync();
}

Console.WriteLine($"Access Token: {result.AccessToken}");
Console.WriteLine($"Expires On: {result.ExpiresOn}");
Console.WriteLine($"Account: {result.Account.Username}");