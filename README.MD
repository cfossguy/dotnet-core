## Quick Start Guide
Please watch [this 15 minute video](https://www.canva.com/design/DAE_fEkg8ZA/F6mN_nkTGn0ypRDF1YMbDw/watch?utm_content=DAE_fEkg8ZA&utm_campaign=designshare&utm_medium=link&utm_source=publishsharelink) 
to get a 15 minute tour of the solution in this repository.

### OTEL Enable .NET Core API
You will need to add OpenTelemetry and Prometheus to your code so that metrics, logs and traces can be collected from a data collection agent.
This solutions uses the Grafana Agent because it supports metrics+logs+traces but you can deploy alternative agent(s) if desired.

#### Links you should bookmark: 
1. https://opentelemetry.io/docs/instrumentation/net
2. https://grafana.com/blog/2021/02/11/instrumenting-a-.net-web-api-using-opentelemetry-tempo-and-grafana-cloud/ 

### Update Grafana Agent Configuration for k8s
Once you have the grafana agent working for k8s, you will want to add this markup to the bottom of your `agent-config-map.yaml` file.
All of the grafana agent configuration files used in the demo video are available [here](./grafana-agent). 

```
- job_name: integrations/dotnet-core
         metrics_path: /metrics
         kubernetes_sd_configs:
           - role: endpoints
         relabel_configs:
           - source_labels: [ __meta_kubernetes_endpoints_name ]
             target_label: app
             action: replace
             replacement: $1
```

#### Links you should bookmark:
1. https://grafana.com/docs/agent/latest/getting-started/#grafana-cloud-kubernetes-quickstart-guides

### Add Grafana Release Annotation to CD Pipeline
TODO

### Add K6 OSS Soak Test to CD Pipeline
TODO

### Create Custom Grafana Dashboard
TODO

### Run Capacity Tests in K6 Cloud
TODO

