# CATALOG API (ASP.NET)

A Backend REST Web API with ASP.NET for Creating, Reading, Updating, and Deleting Catalog Items such as a Game or Library Catalog.

You can only choose one method below for starting the API Server:

## Method 1 - With DB only as a Container

## Clone the Repository

- **SSH**: git clone git@github.com:placideirandora/catalog-api-with-asp.net.git
- **HTTP**: git clone https://github.com/placideirandora/catalog-api-with-asp.net.git

### Run the Tests

- Make sure you have **.NET SDK version 5+** installed and running.
- `cd Catalog.UnitTests` and Run `dotnet test`
- They should all pass.

### Start Mongo DB Container

- Make sure you have **Docker Engine** installed and running.
- Run `docker run -d -p 3800:27017 --rm --name mongo_db_host -v catalogapi_mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=AnyPassW0rd mongo`
- Verify that the container has started running `docker ps`

### Start the API Server

- `cd Catalog.Api`
- Create a **Dotnet User Secret for Mongo DB Password** environment variable: `dotnet user-secrets set "MongoDbSettings:Password" "AnyPassW0rd!"`
- Run `dotnet run` or `dotnet watch run` to start the server
- You will be redirected to the **Swagger Documentation** of the API where you can begin using the endpoints. Or, you can use your favorite **API Testing Software** such as **Postman or Insomnia**.
- Once done, stop the **Mongo Db Container**: `docker stop mongodb-container-name-or-id-here` or `docker rm -f mongodb-container-name-or-id-here`

## Method 2 - With App and Db as Containers

- Pull an already published image of the app: `docker pull placideirandora/catalog_api:v1.3` or Create yours. `cd Catalog.Api` and run `docker build -t catalog_api:v1.0 .`
- Verify whether the Image has been created/pulled: `docker image list`
- Create a `.env` file at the root folder and copy the content of the file: `.env.example` into it. Then, provide values to the specified environment variables. The `HOST=catalog-api-with-aspnet_db_1` and `APP_IMAGE=placideirandora/catalog_api:v1.3` or `APP_IMAGE=catalog_api:v1.0` accordingly.
- Start the **Mongo Db and App containers** with **Docker Compose**: `docker-compose up -d`
- Verify that the containers have started running: `docker ps`
- Now, you can access the API endpoints with the exposed port.
- Once done, stop the containers: `docker-compose down`

## Method 3 - With Kubernetes for Container Orchestration

- Make sure that you have a **Kubernetes Cluster** setup and running
- Configure the current **Kubernetes Context**: `kubectl config current-context`
- `cd Catalog.Api/kubernetes`
- Create a **Secret** for Mongo Db Password: `kubectl create secret generic catalog-secrets --from-literal=mongodb-password='AnyPassW0rd!`
- Run `kubectl apply -f mongodb.yml` to create a **StatefulSet** for Mongo DB **POD** as a Service.
- Verify that the POD is running: `kubectl get pods -w`. The `mongodb-statefulset-0` should have the status as `running` and ready as `1/1`. If not, wait for it to be ready or troubleshoot any issues.
- Run `kubectl apply -f catalog.yml` to deploy our containerized App on the Kubernetes Cluster (note: we will be deploying my public container image from the **Docker Hub**. If you have published your own copy to any container registry or a local one, you can edit the file and speficy it).
- Verify that the POD is running: `kubectl get pods -w`. The `catalog-api-deployment-randomId` should have the status as `running` and ready as `1/1`. If not, wait for it to be ready or troubleshoot any issues.
- If all PODs are running and have ready as `1/1`, you are ready to call the API with the exposed port: `80`.
- Once done, delete the PODs: `kubectl get pods` and `kubectl delete pods pod-one-name-here pod-two-name-here`. Delete the **Services**: `kubectl get services` and `kubectl delete services mongo-service-name-here catalog-service-name-here`. Delete the **Deployment**: `kubectl get deployments` and `kubectl delete deployment deployment-name-here`. Delete the **Statefulset** as well: `kubectl get statefulsets` and `kubectl delete statefulset mongo-statefulset-name-here`
