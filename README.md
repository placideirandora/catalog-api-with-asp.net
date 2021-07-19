# CATALOG API (ASP.NET)

A Backend REST Web API with ASP.NET

You can only choose one method below for starting the API Server:

## Method 1 - With DB only as a Container

### Run the Tests

- `cd Catalog.UnitTests` and Run `dotnet test`
- They should all pass

### Start Mongo DB Container

- Make sure you have Docker Engine installed and running.
- Run `docker run -d -p 3800:27017 --name mongo_db_host -v catalogapi_mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=Pass123 mongo`
- Verify that the container has started running `docker ps`

### Start the API Server

- `cd Catalog.Api`
- Create Dotnet User Secret for Mongo DB Password Environment Variable: dotnet user-secrets set "MongoDbSettings:Password" "AnyPassW0rd!"
- Run `dotnet run` or `dotnet watch run` to start the server
- You will be redirected to the Swagger Documentation of the API where you can begin using the endpoints. Or, you can use your favorite API Testing Software such as Postman or Insomnia
- Once done, stop the Mongo Db Container: `docker stop mongodb-container-name-or-id-here` or `docker rm -f mongodb-container-name-or-id-here`

## Method 2 - With App and Db as Containers

- Create a Docker Image for the App. `cd Catalog.Api` and run `docker build -t catalog_api:v1.0 .`
- Verify whether the Image has been created: `docker image list`
- Create a .env file and copy the content of the file: `.env.example` into it. Then, provide values to the specified environment variables
- Start the Mongo Db and App containers with Docker Compose: `docker-compose up -d`
- Verify that the containers have started running: `docker ps`
- Now, you can access the API endpoints with the exposed port.
- Once done, stop the containers: `docker-compose down`

## Method 3 - With Kubernetes for Container Orchestration

- Make sure that you have a Kubernetes Cluster setup and running
- Configure the current Kubernetes context: `kubectl config current-context`
- `cd Catalog.Api/kubernetes`
- Create a Secret for Mongo Db Password: `kubectl create secret generic catalog-secrets --from-literal=mongodb-password='AnyPassW0rd!`
- Run `kubectl apply -f mongodb.yml` to start the Mongo Db POD as a Service
- Verify that the POD is running: `kubectl get pods -w`. The `mongodb-statefulset-0` should have the status as `running` and ready as `1/1`. If not, wait for it to be ready or troubleshoot any issues.
- Run `kubectl apply -f catalog.yml` to deploy our containerized App on the Kubernetes Cluster (note: we will be deploying my public container image from the Docker Hub. If you have published your own copy to any container registry, you can edit the file and speficy it).
- Verify that the POD is running: `kubectl get pods -w`. The `catalog-api-deployment-randomId` should have the status as `running` and ready as `1/1`. If not, wait for it to be ready or troubleshoot any issues.
- If all PODs are running and have ready as `1/1`, you are ready to call the API with the exposed port: `80`.
- Once done, delete the PODs: `kubectl get pods` and `kubectl delete pods pod-one-name-here pod-two-name-here`. Delete the services: `kubectl get services` and `kubectl delete services mongo-service-name-here catalog-service-name-here`. Delete the deployment: `kubectl get deployments` and `kubectl delete deployment deployment-name-here`. Delete the Statefulset as well: `kubectl get statefulsets` and `kubectl delete statefulset mongo-statefulset-name-here`
