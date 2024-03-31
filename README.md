# Motorcycle Delivery Rent Web API

### Sections

[Description](#description)

[Libraries](#libraries)

[Tools required](#tools-required)

[Get Start Docker](#get-start-docker)

[Get Start Souce Code](#get-start-souce-code)

[URIs](#uris)

[About Endpoints](#about-endpoints)

[endpoints](#endpoints)

## Description
Authentication with JWT to enhance system security, allowing users to securely log in.
Implementation of a permissions system for different types of users, ensuring that only those with appropriate authorization can access and modify certain parts of the system, such as registration and information alteration.
Motorcycle rental functionality, enabling users to rent motorcycles efficiently and conveniently.
Registration and management of rental plans, providing users with a variety of options to meet their specific motorcycle rental needs.
Implementation of a delivery system, with distinct functionalities for drivers and clients (admin) using RabbitMQ.

## Libraries
[BCrypt Net-Next](https://www.nuget.org/packages/BCrypt.Net-Next/4.0.3)

[Microsoft Asp.Net Core Authentication JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer/8.0.3)

[Microsoft Identity Model Tokens](https://www.nuget.org/packages/Microsoft.IdentityModel.Tokens/7.5.0)

[Microsoft VisualStudio Azure Containers Tools Targets](https://www.nuget.org/packages/Microsoft.VisualStudio.Azure.Containers.Tools.Targets/1.19.6)

[MongoDB Driver](https://www.nuget.org/packages/MongoDB.Driver/2.24.0)

[RabbitMQ Client](https://www.nuget.org/packages/RabbitMQ.Client/6.8.1)

[Swashbuckle Asp.Net Core](https://www.nuget.org/packages/Swashbuckle.AspNetCore/6.5.0)

[Swashbuckle Asp.Net Core Filters](https://www.nuget.org/packages/Swashbuckle.AspNetCore.Filters/8.0.1)

[System Identity Model Tokens Jwt](https://www.nuget.org/packages/System.IdentityModel.Tokens.Jwt/7.5.0)

## Tools required
To run the application, you will need the following tools:

  + [.NET 8](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)
  + [MongoDB](https://www.mongodb.com/docs/manual/installation/)
  + [RabbitMQ](https://www.rabbitmq.com)

Additionally, the application can be started more easily by using:

  + [Docker](https://www.docker.com/get-started/)

For viewing and managing the database, I recommend using the interface:

 + [MongoDB Compass](https://www.mongodb.com/try/download/compass)


## Get Start Docker
To begin, clone the application repository by running the following command in the terminal:

```Git Clone
git clone https://github.com/MatheusMGrassano/RentalWebAPI.git
```
Inside the MotorcycleDeliveryRentWebAPI folder, execute the following command to start the application within Docker:
```Docker Compose
docker-compose up --build
```
This command will build and start the necessary Docker containers to run the application.


## Get Start Souce Code
To get started, clone the application repository by running the following command in your terminal:
```Git Clone
git clone https://github.com/MatheusMGrassano/RentalWebAPI.git
```
**Running the application**
You will need to use the following command to start the application:
```.net
dotnet watch run
```

**Changing Connection String**
Install MongoDB and RabbitMQ, then make sure to update the connections in the appsettings.json file.
```Exemple
//Exemple
{
  "RabbitMQ": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "MyUser",
    "Password": "MyPassword"
  },
  "MongoDBSettings": {
    //"ConnectionString": "mongodb://MyUser:MyPassword@localhost:27017",
    "ConnectionString": "mongodb://MyUser:MyPassword@mongo:27017",
    "DatabaseName": "MotorcycleDeliveryRentalBD"
  }
}
```

Or you can start it using Docker Compose.
``` Exemple Docker-Compose
version: '3.4'

services:
  rabbitmq:
    image: rabbitmq:3.13-management
    container_name: 'rabbitmq'
    ports:
      - 5672:5672
      - 15672:15672
    environment:
      RABBITMQ_DEFAULT_USER: admin
      RABBITMQ_DEFAULT_PASS: admin
    volumes:
      - ~/.docker-conf/rabbitmq/data/:/var/lib/rabbitmq/
      - ~/.docker-conf/rabbitmq/log/:/var/log/rabbitmq

  mongo:
    image: mongo:latest
    environment:
      MONGO_INITDB_ROOT_USERNAME: admin
      MONGO_INITDB_ROOT_PASSWORD: admin
      MONGO_INITDB_DATABASE: DB_Motorbike_Rental
    ports:
      - 27017:27017
    volumes:
      - mongodb_data_container:/data/db

volumes:
  mongodb_data_container:
```
Then, run the following command to start RabbitMQ and MongoDB:
```Docker Compose
docker-compose up --build
```
### 🚀 All set and configured.

## URIs

The application will be started on port 8081. You can access it via the following URI:  [`https://localhost:8081`](https://localhost:8081).

Additionally, you can explore the API more easily using Swagger. Swagger can be accessed via the following URI: [`https://localhost:8081/swagger/index.html`](https://localhost:8081).

To connect to the database using MongoDB Compass, use the following URI: `mongodb://admin:admin@localhost:27017/`

To access RabbitMQ Management, visit [`http://localhost:15672`](http://localhost:15672) and use the following credentials:
- username: admin
- password: admin

# About Endpoints

To test all endpoints, you need to register and log in as both a user and an administrator to obtain the access token.

This access token should be inserted in the top right corner of the `Swagger`, by clicking on `Authorize`, or, if you are using `Postman`, go to the `authorization` tab and select `Bearer Token`.

`Token Exemple`: Bearer eyJhbGciOiJodHRwOi8vd3d3

## Endpoints

###Admin

|  Method  | Permission | Route                                   |                  Action                 |
|----------|------------|-----------------------------------------|-----------------------------------------|
|   POST   |      -     | https://localhost:8081/api/Admin        | Administrator Registration              |
|   POST   |      -     | https://localhost:8081/api/Admin/Login  | Admins login and token retrieval        |
|   GET    |    Admin   | https://localhost:8081/api/Admin        | Retrieve all registered admins          |
|   GET    |    Admin   | https://localhost:8081/api/Admin/{id}   | Retrieve a specific admin               |
|   PUT    |    Admin   | https://localhost:8081/api/Admin/{id}   | Update admin password                   |

###Driver

|  Method  | Permission | Route                                   |                  Action                 |
|----------|------------|-----------------------------------------|-----------------------------------------|
|   POST   |      -     | https://localhost:8081/api/Driver       | Driver Registration                     |
|   POST   |      -     | https://localhost:8081/api/Driver/Login | Driver login and token retrieval        |
|   POST   |    User    | https://localhost:8081/api/Driver/cnh/upload | Send a photo of the CNH to a location on disk |
|   GET    |      -     | https://localhost:8081/api/Driver       | Retrieve all registered drivers         |
|   GET    |      -     | https://localhost:8081/api/Driver/{id}  | Retrieve a specific driver              |
|   PUT    |    User    | https://localhost:8081/api/Driver/{id}  | Update driver information               |
|   PUT    |    User    | https://localhost:8081/api/Driver/Password/{id}  | Update driver password         |

###Motorcycle

|  Method  | Permission | Route                                   |                  Action                 |
|----------|------------|-----------------------------------------|-----------------------------------------|
|   POST   |    Admin   | https://localhost:8081/api/Motorcycle   | Motorcycle Registration                 |
|   GET    |      -     | https://localhost:8081/api/Motorcycle   | Retrieve all registered motorcycle      |
|   GET    |    Admin   | https://localhost:8081/api/Motorcycle/{id} | Retrieve a specific motorcycle       |
|   GET    |    Admin   | https://localhost:8081/api/Motorcycle/plate/{plateId} | Retrieve a specific motorcycle by plate |
|   GET    |      -     | https://localhost:8081/api/Motorcycle/Available  | Return the first available motorcycle  |
|   PUT    |    Admin   | https://localhost:8081/api/Motorcycle/{id}       | Update motorcycle model and year |
|   PUT    |    Admin   | https://localhost:8081/api/Motorcycle/plate/{id} | Update motorcycle license plate  |
|  DELETE  |    Admin   | https://localhost:8081/api/Motorcycle/{id}  | Delete motorcycle that has not been rented |

###Plan

|  Method  | Permission | Route                                   |                  Action                 |
|----------|------------|-----------------------------------------|-----------------------------------------|
|   POST   |    Admin   | https://localhost:8081/api/Plan         | Register a plan for the rental service  |
|   GET    |      -     | https://localhost:8081/api/Plan         | Retrieve all registered plans           |
|   GET    |      -     | https://localhost:8081/api/Plan/{id}    | Retrieve a specific plan                |
|   PUT    |    Admin   | https://localhost:8081/api/Plan/{id}    | Update plan information                 |


###Rent

|  Method  | Permission | Route                                   |                  Action                 |
|----------|------------|-----------------------------------------|-----------------------------------------|
|   POST   |    Admin   | https://localhost:8081/api/Rent/{planId} | Register a rented motorcycle           |
|   GET    |    Admin   | https://localhost:8081/api/Rent         | Retrieve all rented motorcycles         |
|   GET    |    User    | https://localhost:8081/api/Rent/{id}     | Retrieve specific rented motorcycles    |
|   GET    |    User    | https://localhost:8081/api/Rent/driver{driverId} | Retrieve specific motorcycles rented by drivers |
|   PUT    |    User    | https://localhost:8081/api/Rent/{id}    | Update the day the motorcycle was returned and return the price to be paid |

###Delivery

|  Method  | Permission | Route                                   |                  Action                 |
|----------|------------|-----------------------------------------|-----------------------------------------|
|   POST   |    Admin   | https://localhost:8081/api/Delivery     | Register a delivery with a price        |
|   GET    |      -     | https://localhost:8081/api/Delivery     | Retrieve all deliveries                 |
|   GET    |      -     | https://localhost:8081/api/Delivery/{id} | Retrieve specific delivery             |
|   GET    |      -     | https://localhost:8081/api/Delivery/Notification | Retrieve all delivery notifications |
|   PUT    |    User    | https://localhost:8081/api/Delivery/Accept/{id} | Update the delivery as accepted and assign the driverId |
|   PUT    |    User    | https://localhost:8081/api/Delivery/Delivery/{id} | Update the delivery as delivered |
|   PUT    |    Admin   | https://localhost:8081/api/Delivery/Cancel/{id} | Atualiza a entrega como cancelada |

