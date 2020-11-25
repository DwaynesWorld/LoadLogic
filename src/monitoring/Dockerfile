FROM prom/prometheus:latest
COPY monitoring/prometheus.yaml /etc/prometheus/prometheus.yaml
ENTRYPOINT [ "/bin/prometheus" ]
CMD        [ "--config.file=/etc/prometheus/prometheus.yml", "--storage.tsdb.path=/prometheus" ]