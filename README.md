# README

This project is a demonstration of the organization of the solution when writing a project in a microservice style.

To organize a set of commands into a project, use [Just](https://github.com/casey/just).

---

**The first deployment option** is to demonstrate how to set up [OpenID Connect](https://openid.net/connect) on both the server and the client using [SSO](https://auth0.com/intro-to-iam/what-is-single-sign-on-sso). This is a simplified deployment option that should not be used in real projects.

For demonstration use:

- SSO: [keycloak](https://keycloak.org)
  
- Web-client: react + typescript + [react-oidc-context](https://github.com/authts/react-oidc-context) (there is also a project version with [@axa-fr/react-oidc](https://github.com/erritis/asp-react-oidc-demo/tree/axa-fr_react-oidc))
  
- Microservice: [aspnet-webapi](https://github.com/dotnet/aspnetcore)

---

**The second deployment option** is the same as the first option, but in addition it also serves to demonstrate how CI/CD can be organized.

For demonstration use:

- Orchestrator: Kubernetes (used [minikube](https://minikube.sigs.k8s.io/docs/start) for this example)

- Container repository (this example uses [private repository](https://werf.io/documentation/v1.2/#check-the-result) deployed in [minikube](https://minikube.sigs.k8s.io/docs/start/))

- [Kompose](https://kompose.io/installation) - to make it easier to write configuration files in the style of docker-compose files. With subsequent conversion to werf templates.

- Tool for building and deploying: [Werf](https://werf.io/installation.html) - allows you to build a container only after a commit in the git with the subsequent deployment of containers to kubernetes according to the described templates.

---
### Users in keycloak:

| login        | password                         |
|--------------|:--------------------------------:|
| admin        | PdZoV9NpYuq#zdsYXfHo             |
| react-tester | ^J2WnUY#T3XSi4@AH4NEySqUAhfhid%o |

---

## The first deployment option

---

#### To run you need to run the command:

> docker compose up

---
### Links with ports in localhost:

- KeycloakDB (Postgres): http://localhost:52632

- Keycloak: http://localhost:52633

- Microservice on Aspnet WebApi: http://localhost:52634

- Web-client: http://localhost:52635

---
## The second deployment option

---
### Installation and setup

Install:

- [minikube](https://minikube.sigs.k8s.io/docs/start/)

- [helm](https://helm.sh/docs/intro/install/)

- [kompose](https://kompose.io/installation/)

- [werf](https://werf.io/installation.html)

Set up ingress in minikube like in [this article](https://minikube.sigs.k8s.io/docs/handbook/addons/ingress-dns/).

> **Warning**
> 
> Use only the zones specified in the article (For example, .test or .example)

Set up a private repository in minikube for werf (but any will do), as in [this article](https://werf.io/documentation/v1.2/#check-the-result).

> **Warning**
> 
> In case of problems with the deployment (and they will be), check out the [werf deployment section](https://github.com/erritis/asp-react-oidc-demo/tree/master/docs/werf-deployment.md)

---

### Setting up deployment configs

To change deployment configurations, edit **docker-compose.werf.yml**.

Run the command:

> just werf-convert

After this command, in the **./.helm/teemplates/** folder, the deployment templates should be replaced with the changes made.

Run:

> git add .
>
> git commit

---

### Build and Deploy

Set the host and port of the container repository to use:

> just werf-set-repo registry.test:80

Run the command:

> just werf-up

---

> **Warning**
> 
> In case of problems with the deployment (and they will be), check out the [werf deployment section](https://github.com/erritis/asp-react-oidc-demo/tree/master/docs/werf-deployment.md)

To remove containers from kubernetes run:

> just werf-down

For a complete cleaning:

> just werf-cleanup

---

### Links indicating ports in minikube:

- KeycloakDB (Postgres): http://keycloakdb.asp-react-oidc.test:5432

- Keycloak: http://keycloak.asp-react-oidc.test

- Microservice on Aspnet WebApi: http://backend.asp-react-oidc.test

- Web-client: http://asp-react-oidc.test
  
### Debugging in kubernetes

An article about how debugging works in Kubernetes: [Use Bridge to Kubernetes](https://learn.microsoft.com/en-us/visualstudio/bridge/bridge-to-kubernetes-vs-code?view=vs-2019)

> **Warning**
> 
> In case of problems with configuring the plugin (and they most likely will), read the section: [Problems when configuring a kubernetes plugin](https://github.com/erritis/asp-react-oidc-demo/tree/master/docs/bridge-to-kubernetes.md)

The project already contains ready-made configurations for replacing services in isolated mode

In order for debugging to work in this mode, you need to specify the **kubernetes-route-as** header in each request when accessing kubernetes addresses.

To simplify this task, you can use a browser plugin that automatically adds headers. I used the open source solution: [SimpleModifyHeaders](https://github.com/didierfred/SimpleModifyHeaders)

Here is an example of my settings:

>   **Url Patterns\***: http://asp-react-oidc.test/\*;http://\*.asp-react-oidc.test/\*
>
> >     Header Field Name: kubernetes-route-as
> >     Header Field Value: asp-react-oidc-demo-30dc

In case you do not need isolated mode, remove or comment out the **isolateAs** field in the **/.vscode/tasts.json** file of the project for which you want to disable this mode:

>       {
>       	"version": "2.0.0",
>       	"tasks": [
>       		{
>       			"label": "bridge-to-kubernetes.resource",
>                   ...
>       			# "isolateAs": "asp-react-oidc-demo-30dc"
>       		}
>       	]
>       }

When debugging a web-client, you can still access it by its domain name.