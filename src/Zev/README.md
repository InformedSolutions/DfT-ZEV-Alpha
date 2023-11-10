# Project: Zev Services

## Overview

This GitHub repository contains the source code for the Zev Services project, which is a .NET solution designed to handle various services related to zero-emission vehicle (ZEV) compliance. Below, you'll find information about the different services included in this solution, details on running unit tests, and guidelines for deployment.

# Table of Contents

- [Project Name: Zev Services](#project-name-zev-services)
  - [Overview](#overview)
  - [Services](#services)
    - [1. Compliance Calculation Service](#1-compliance-calculation-service)
      - [Deployment](#deployment)
  - [Running Unit Tests](#running-unit-tests)
  - [Deployment](#deployment)

## Services

### 1. Compliance Calculation Service

- **Type**: GCP Cloud Function
- **Description**: This service is responsible for importing vehicle data from a CSV file, validating it, and then inserting the data into a PostgreSQL database. It plays a crucial role in the compliance calculation process for zero-emission vehicles.

#### Deployment

To deploy the Compliance Calculation Service, use the following command:

```bash
gcloud functions deploy dev-zev-compliance-calculation-service --allow-unauthenticated --entry-point Zev.Services.ComplianceCalculationService.Handler.Function --gen2 --region europe-west1 --runtime dotnet6 --trigger-http --update-build-env-vars GOOGLE_BUILDABLE=./Zev.Services.ComplianceCalculationService.Handler
```
Make sure to replace dev-zev-compliance-calculation-service with an appropriate service name and adjust other parameters as needed.

## Running Tests
  1. Open terminal in the root directory of specific project.
  2. Run the following command
  ``` bash
  dotnet test
  ```
## Deployment Notice
Before deploying any service, ensure that you have the necessary credentials and configurations set up for your target environment. Refer to the service-specific deployment commands provided in the service descriptions.

## Local Development:
 This project utilizes docker-compose for creating local development environment. You can check `../../infrastructure/docker` directory for more info.
  All of environment variables required are located in `.env` file

 To run your local environment. Go to `../../infrastructure/docker` and run command
  ``` bash
  docker-compose up 
  ```

### Local Services
 1. Postgres:
   
    Default connection string: `Host=localhost; Database=zev-dev; Username=root; Password=root`
