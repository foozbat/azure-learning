using Microsoft.Graph;
using Azure.Identity;
using dotenv.net;

// Load environment variables from .env file (if present)
DotEnv.Load();
var envVars = DotEnv.Read();

// Read Azure AD app registration values from environment
string clientId = envVars["CLIENT_ID"];
string tenantId = envVars["TENANT_ID"];

// Validate that the required environment variables are set
if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(tenantId))
{
    Console.WriteLine("Please set CLIENT_ID and TENANT_ID environment variables.");
    return;
}

// Define the Microsoft Graph permission scopes required by this app
var scopes = new[] { "User.Read" };

// Configure interactive browser authentication for the user
var options = new InteractiveBrowserCredentialOptions
{
    ClientId = clientId,
    TenantId = tenantId,
    RedirectUri = new Uri("http://localhost") // Redirect URI for auth flow
};
var credential = new InteractiveBrowserCredential(options);

// Create a Microsoft Graph client using the credential
var graphClient = new GraphServiceClient(credential);

Console.WriteLine("Retrieving user profile...");
await GetUserProfile(graphClient);

async Task GetUserProfile(GraphServiceClient graphClient)
{
    try
    {
        // Call Microsoft Graph /me endpoint to get user info
        var me = await graphClient.Me.GetAsync();
        Console.WriteLine($"Display Name: {me?.DisplayName}");
        Console.WriteLine($"Principal Name: {me?.UserPrincipalName}");
        Console.WriteLine($"User ID: {me?.Id}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error retrieving user profile: {ex.Message}");
    }
}
