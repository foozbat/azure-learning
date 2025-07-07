resourceGroup=myResourceGroup
location=eastus
appConfigName=appconfigname$RANDOM

az login

az group create --name $resourceGroup --location $location

az appconfig create --location $location \
    --name $appConfigName \
    --resource-group $resourceGroup \
    --disable-local-auth true

userPrincipal=$(az rest --method GET --url https://graph.microsoft.com/v1.0/me \
    --headers 'Content-Type=application/json' \
    --query userPrincipalName --output tsv)

resourceID=$(az appconfig show --resource-group $resourceGroup \
    --name $appConfigName --query id --output tsv)

az role assignment create --assignee $userPrincipal \
    --role "App Configuration Data Reader" \
    --scope $resourceID

az appconfig kv set --name $appConfigName \
    --key Dev:conStr \
    --value connectionString