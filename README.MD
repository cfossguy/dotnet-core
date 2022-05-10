## Quick Start Guide
Please watch [this 15 minute video](https://www.canva.com/design/DAE_fEkg8ZA/F6mN_nkTGn0ypRDF1YMbDw/watch?utm_content=DAE_fEkg8ZA&utm_campaign=designshare&utm_medium=link&utm_source=publishsharelink) 
to get a quick tour of the solution in this repository. Once you do that, 
open a [free grafana cloud account](http://grafana.com). 

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
The Grafana API documentation provides instructions on how to post an annotation to the Grafana DB. 
The full github actions workflow can be found [here](./.github/workflows/grafana.yml). This annotation
sets the 'app' tag = 'dotnet-core'. The pre-built dashboard is configured to show any annotations
that match that tag.

#### Links you should bookmark:
1. https://grafana.com/docs/grafana/latest/http_api/annotations/ 

### Add K6 OSS Soak Test to CD Pipeline
Running k6 OSS as a cronjob in k8s is a good way to setup an API soak test. 
The full github actions workflow can be found [here](./.github/workflows/k6.yml).
This cronjob runs a 15 minute [soak test](./soaktest.js) that ramps from 1 to 20 VUs every 15 minutes.
This ensures there is always some load running against the APIs. 

### Create Custom Grafana Dashboard
The [custom dashboard](./grafana-dashboard.json) for this demo uses the [READS metrics methodology](https://engineering.salesforce.com/reads-service-health-metrics-1bfa99033adc) that the Salesforce
engineering team blogged about in Feb 2022. 

### Run Capacity Tests in K6 Cloud
If you would like to run capacity tests for your APIs create a 
free cloud account at [k6 cloud](https://k6.io/cloud/). Grafana
also provides a [k6 app](https://grafana.com/grafana/plugins/grafana-k6-app/) plugin
that you can use to run k6 capacity tests directly in Grafana.

