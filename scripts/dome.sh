# Infrastructure
docker compose \
    -f docker-compose.yaml \
    -f docker-compose.override.yaml \
    -p LoadLogic \
    up -d --no-deps envoy monitoring-service rabbitmq-service elasticsearch kibana sqlserver

# Kibana
docker run --platform linux/amd64 -p 5601:5601 kibana:7.12.1


# APIs
docker compose \
    -f docker-compose.yaml \
    -f docker-compose.override.yaml \
    -p LoadLogic \
    up --build locations-api-service ordering-api-service customers-api-service userprofile-api-service