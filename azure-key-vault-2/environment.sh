resourceGroup=myResourceGroup
location=eastus
keyVaultName=mykeyvaultname$RANDOM

az login

az group create --name $resourceGroup --location $location

az keyvault create --name $keyVaultName --resource-group $resourceGroup --location $location

userPrincipal=$(az rest --method GET --url https://graph.microsoft.com/v1.0/me \
    --headers 'Content-Type=application/json' \
    --query userPrincipalName --output tsv)

resourceID=$(az keyvault show --resource-group $resourceGroup \
    --name $keyVaultName --query id --output tsv)

az role assignment create --assignee $userPrincipal \
    --role "Key Vault Secrets Officer" \
    --scope $resourceID

az keyvault secret set --vault-name $keyVaultName \
    --name "MySecret" --value "My secret value"

az keyvault secret show --name "MySecret" --vault-name $keyVaultName