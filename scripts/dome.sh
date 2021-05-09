# Infrastructure
docker compose \
    -f docker-compose.yaml \
    -f docker-compose.override.yaml \
    -p LoadLogic \
    up -d --no-deps envoy monitoring-service rabbitmq-service seq-service sqlserver

# APIs
docker compose \
    -f docker-compose.yaml \
    -f docker-compose.override.yaml \
    -p LoadLogic \
    up --build locations-api-service ordering-api-service customers-api-service userprofile-api-service