FROM envoyproxy/envoy:v1.16.0
COPY api-gateway/envoy.yaml /etc/envoy/envoy.yaml
CMD /usr/local/bin/envoy -c /etc/envoy.yaml