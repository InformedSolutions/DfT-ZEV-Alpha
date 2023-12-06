# Table of Contents
  - [Compliance Calculation Service](#1-compliance-calculation-service)
    - [Deployment](#deployment)
  - [Migrator Service](#2-migrator-service)
    - [Deployment](#deployment-1)
  - [Zev Scheme Api](#3-zev-scheme-api)
    - [Deployment](#deployment-2)

## Services
### 1. Compliance Calculation Service

- **Type**: Cloud Function
- **Description**: This service is responsible for importing vehicle data from a CSV file, validating it, and then inserting the data into a PostgreSQL database. It plays a crucial role in the compliance calculation process for zero-emission vehicles.

#### Deployment

To deploy the Compliance Calculation Service, use the following command:
``` bash
gcloud functions deploy dev-zev-compliance-calculation-service --allow-unauthenticated --entry-point Zev.Services.ComplianceCalculation.Handler.Function --gen2 --region europe-west1 --runtime dotnet6 --trigger-http --update-build-env-vars GOOGLE_BUILDABLE=./Services/ComplianceCalculation/Zev.Services.ComplianceCalculation.Handler
```

### 2. Migrator Service

- **Type**: Cloud Function
- **Description**: This service is responsible for migrating database using ef core.

#### Deployment

To deploy migrator service, use the following command:
``` bash  
gcloud functions deploy dev-migrator --allow-unauthenticated --entry-point Zev.Services.Migrator.Handler.Function --gen2 --region europe-west1 --runtime dotnet6 --trigger-http --update-build-env-vars GOOGLE_BUILDABLE=./Services/Migrator/Migrator.Handler
```

### 3. Zev Scheme Api

- **Type**: Cloud Run
- **Description**: This rest api is responsible for managing zev scheme data, with process monitoring.

#### Deployment

W.IP