# importing the required modules
import click
import os
from dotenv import load_dotenv
import requests

#load_dotenv(override=True, verbose=True)
#gcp_project = os.getenv('gcp_project')
#gcp_zone = os.getenv('gcp_zone')
#gcp_service_account = os.getenv('gcp_service_account')

@click.group()
def cli():
    pass

@click.command()
def registry_upload():
    """Build/Tag/Upload image to GCR"""
    docker_build = f"cd dotnet-core; docker build -t dotnet-core:1.0 . --platform linux/amd64;"
    docker_tag = f"docker tag dotnet-core:1.0 us-central1-docker.pkg.dev/solutions-engineering-248511/se-jwilliams-sandbox/dotnet-core:1.0;"
    docker_push = f"docker push us-central1-docker.pkg.dev/solutions-engineering-248511/se-jwilliams-sandbox/dotnet-core:1.0;"
    print("<DEMO COMMANDS>", docker_build, docker_tag, docker_push, "</DEMO COMMANDS>", sep=os.linesep)
    os.system(docker_build)
    os.system(docker_tag)
    os.system(docker_push)
    
cli.add_command(registry_upload)

@click.command()
def k8s_deploy():
    """Deploy app to k8s"""
    kubectl_deploy = f"kubectl apply -f ./deployment.yaml; kubectl rollout restart deployment dotnet-core;"
    print("<DEMO COMMANDS>", kubectl_deploy, "</DEMO COMMANDS>", sep=os.linesep)
    os.system(kubectl_deploy)

cli.add_command(k8s_deploy)

@click.command()
def k8s_delete_evicted_pods():
    """Delete all evicted pods in default namespace"""
    kubectl_delete = f"kubectl get pod | grep Evicted | awk '{{print $1}}' | xargs kubectl delete pod;"
    print("<DEMO COMMANDS>", kubectl_delete, "</DEMO COMMANDS>", sep=os.linesep)
    os.system(kubectl_delete)

cli.add_command(k8s_delete_evicted_pods)

@click.command()
def set_annotation():
    """Provision a global annotation"""
    gcloud_api_key_dashboards = os.getenv('gcloud_api_key_dashboards')
    gcloud_domain_dashboards = os.getenv('gcloud_domain_dashboards')
    url = f"{gcloud_domain_dashboards}/api/annotations"

    dashboard = open('release-annotation.json')
    headers = {"Accept": "application/json",
               "Content-Type": "application/json",
               "Authorization": f"Bearer {gcloud_api_key_dashboards}"}
    response = requests.post(url=url, data=dashboard, headers=headers)
    r = requests.Request(requests.post(url=url, data=dashboard, headers=headers))
    print(r)
   
    try:
       print(f"Success: {response.text}")
    except KeyError:
        print(f"Error occurred: {response.text}")
        
cli.add_command(set_annotation)
    
if __name__ == "__main__":
    cli()