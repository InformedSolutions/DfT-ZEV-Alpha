# Project: Zev Services

## Overview

This GitHub repository contains the source code for the Zev Services project, which is a .NET solution designed to handle various services related to zero-emission vehicle (ZEV) compliance. Below, you'll find information about the different services included in this solution, details on running unit tests, and guidelines for deployment.

# Table of Contents

- [Project Name: Zev Services](#project-name-zev-services)
  - [Overview](#overview)
  - [Core](#core)
  - [Services](#services)
  - [Running Unit Tests](#running-tests)
  - [Deployment](#deployment)
  - [Local Development](#local-development)
      - [Docker](#docker)
      - [Running dotnet apps locally](#running-dotnet-apps-locally)
      - [Working with Entity Framework](#working-with-entity-framework)

## Core
This directory contains shared code for this solution, it resemblies onion structure. In there you can find:

 - Domain (Entity models, Value object, Interfaces for repositories/services, validation rules)
 - Infrastructure (Implementations of repositories, all of common infrastructure, entity configurations, logging etc)
 - Application (strictly for buisness logic)

## Services
This directory contains all services for this solution. Including cloud runs, cloud functions etc. You can find out more about services in `./Services/README.md`

## Deployment Notice
Before deploying any service, ensure that you have the necessary credentials and configurations set up for your target environment. Refer to the service-specific deployment commands provided in the service descriptions.

## Local Development:

## Running Tests
  1. Open terminal in the root directory of specific project.
  2. Run the following command
  ``` bash
  dotnet test
  ``` 

### Docker
This project utilizes docker-compose for creating local development environment. You can check `../../infrastructure/docker` directory for more info.
  All of environment variables required are located in `.env` file


 To run your local environment. Go to `../../infrastructure/docker` and run command
  ``` bash
  docker-compose up 
  ```

#### Docker Services
 1. Postgres:
   
    Default connection string: `Host=localhost; Database=zev-dev; Username=root; Password=root`

### Running dotnet apps locally
  For runnig GCP apps there's no need to install external tools, although I highly remommend installing [Google Cloud CLI](https://cloud.google.com/sdk/docs/install). 

### Working with Entity Framework
Project shares DAL between all services. All table configurations, DbContext is located in `Zev.Core.Infrastructure` project. It also utilizes `IDesignTimeDbContextFactory`, as there isnt main startup project. While working on the project you need to use `dotnet ef` cli. To connect to the right db, you need to add argument with connection string to the db or use it in environment variable (`POSTGRES_CONNECTION_STRING`). Like this:

``` bash
dotnet ef <command> -- <connection_string>
```
#### Examples

Creating migration:
``` bash
dotnet ef migrations add TestMigration \
 -- "Host=localhost; Database=zev-dev; Username=root; Password=root"
```

Removing migration:
``` bash
dotnet ef migrations remove TestMigration \
 -- "Host=localhost; Database=zev-dev; Username=root; Password=root"
```

Updating database:
``` bash
dotnet ef database update \
 -- "Host=localhost; Database=zev-dev; Username=root; Password=root"
```