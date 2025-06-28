/**
 * Azure Key Vault Example
 *
 * This example assumes you have a Key Vault set up.
 * Replace <tenant-id>, <client-id>, <client-secret>, <vault-name>, and <key-name> with your actual values.
 * Ensure you have a role assignment for the client ID in your Key Vault with appropriate permissions.
 *
 * @author: Aaron Bishop (github.com/foozbat)
 */

using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Azure.Security.KeyVault.Secrets;

string tenantId = "<tenant-id>";
string clientId = "<client-id>";
string clientSecret = "<client-secret>";
string keyVaultUri = "https://<vault-name>.vault.azure.net/";

ClientSecretCredential clientSecretCredential = new (tenantId, clientId, clientSecret);

/**
 * Example 1: Use Azure Key Vault to encrypt and decrypt data
 */

// Example data to encrypt and decrypt
string myData = "This is a secret message.";

KeyClient keyClient = new (new Uri(keyVaultUri), clientSecretCredential);

var key = keyClient.GetKey("<key-name>");

Console.WriteLine($"Key Name: {key.Value.Name}");
Console.WriteLine($"Key ID: {key.Value.Id}");

// Encrypt the data using the key

var cryptoClient = keyClient.GetCryptographyClient(key.Value.Name, key.Value.Properties.Version);

EncryptResult encryptedResult = cryptoClient.Encrypt(
    EncryptionAlgorithm.RsaOaep,
    System.Text.Encoding.UTF8.GetBytes(myData)
);
string encryptedData = System.Text.Encoding.UTF8.GetString(encryptedResult.Ciphertext);

Console.WriteLine($"Encrypted Data: {encryptedData}");

// Decrypt the data using the key

DecryptResult decryptedResult = cryptoClient.Decrypt(
    EncryptionAlgorithm.RsaOaep,
    encryptedResult.Ciphertext
);
string decryptedData = System.Text.Encoding.UTF8.GetString(decryptedResult.Plaintext);

Console.WriteLine($"Decrypted Data: {decryptedData}");

/**
 * Example 2: Use Azure Key Vault retrieving a secret
 */

string secretName = "dbpassword";

SecretClient secretClient = new SecretClient(
    new Uri("keyVaultUri"),
    clientSecretCredential
);

var secret = secretClient.GetSecret(secretName);

Console.WriteLine($"Secret Name: {secret.Value.Name}");
Console.WriteLine($"Secret Value: {secret.Value.Value}");