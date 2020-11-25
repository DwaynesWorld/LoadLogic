# Set Azure Container Registry Pull Secrets
k create secret docker-registry acr-registry-secrets \
  --docker-server=loadlogicacr.azurecr.io \
  --docker-username=$ACR_USERNAME \
  --docker-password=$ACR_PASSWORD \
  --docker-email=kthompson713@gmail.com


# Create DB Connections String Secrets
k create secret generic db-connection-strings \
  --from-literal=orderingdb=$ORDERING_DB \
  --from-literal=customersdb=$CUSTOMERS_DB \
  --from-literal=locationsdb=$LOCATIONS_DB
