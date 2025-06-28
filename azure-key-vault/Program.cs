/**
  This code snippet demonstrates how to create a KeyClient instance using Azure Identity's ClientSecretCredential.

  Author: Aaron Bishop
  Email:  github@foozbat.net
*/

using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;

// Example data to encrypt and decrypt
string myData = "This is a secret message.";

// Get the KeyClient using ClientSecretCredential

ClientSecretCredential clientSecretCredential = new ClientSecretCredential(
    "<tenant-id>",
    "<client-id>>",
    "<client-secret>"
);

KeyClient keyClient = new KeyClient(
    new Uri("https://<vault-name>.vault.azure.net/"),
    clientSecretCredential
);

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