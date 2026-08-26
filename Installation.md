# Installation Guide

This guide describes how to set up the required Azure resources and deploy the Helpdesk application.

The application uses:

* Azure SQL Server
* Azure SQL Database
* Azure App Service
* ASP.NET Web API
* ASP.NET Razor Pages

---

# 1. Create an SQL Server (Logical Server)

1. Open the **Azure Portal**.
2. Navigate to the provided **Resource Group**.
3. Click **+ Create**.
4. Search for **SQL Server**.
5. Select **SQL Server (Logical Server)** (blue SQL icon).
6. Click **Create**.

## Basic Settings

| Setting                | Value                                  |
| ---------------------- | -------------------------------------- |
| Subscription           | Provided subscription                  |
| Resource Group         | Provided resource group                |
| Server Name            | e.g. `helpdeskdb`                      |
| Region                 | **(Europe) Germany West Central**      |
| Authentication         | **SQL Authentication**                 |
| Administrator Username | Choose a username                      |
| Password               | Choose a password and save it securely |

> **Note:** The administrator username and password will be required later to connect to the database.

## Networking

Set the firewall rules to **Off**.

## Security

Skip this section.

## Additional Settings

Select **Not now**.

## Tags

Skip this section.

## Create

1. Select **Review + create**.
2. Verify the configuration.
3. Click **Create**.

---

# 2. Create the SQL Database

1. Open the previously created **SQL Server**.
2. Click **+ Create database** in the upper-left corner.

## Basic Settings

| Setting              | Value                            |
| -------------------- | -------------------------------- |
| Database Name        | Choose a name, e.g. `HelpdeskDB` |
| Use SQL Elastic Pool | **No**                           |
| Workload Environment | **Development**                  |

### Compute + Storage

1. Select **Configure database**.
2. Choose the smallest available configuration.
3. Use the following settings:

| Setting           | Value             |
| ----------------- | ----------------- |
| Cores             | Minimum available |
| Auto-pause        | **Off**           |
| Maximum Data Size | **2 GB**          |

4. Click **Apply**.

### Backup

| Setting                   | Value                                      |
| ------------------------- | ------------------------------------------ |
| Backup Storage Redundancy | **Locally-redundant backup storage (LRS)** |

## Networking

1. Under **Public network access**, select **Selected networks**.
2. Under **Firewall rules**, click **Add your client IPv4 address**.
3. Your current IP address will be added automatically.
4. Keep **Allow Azure services and resources to access this server** disabled for now.

## Security

| Setting                    | Value       |
| -------------------------- | ----------- |
| Microsoft Defender for SQL | **Not now** |
| Enable Secure Enclaves     | **Off**     |

## Additional Settings

| Setting            | Value                    |
| ------------------ | ------------------------ |
| Use Existing Data  | **None**                 |
| Database Collation | **Latin1_General_CS_AI** |

## Create

1. Select **Review + create**.
2. Verify the configuration.
3. Click **Create**.

---

# 3. Deploy the Project to Azure

The following deployment process must be completed for both the **Web API project** and the **Razor Pages project**.

1. Open the solution in **Visual Studio**.
2. In the **Solution Explorer**, right-click the project you want to deploy.
3. Select **Publish...**.
4. Check that the correct **Azure account** is signed in in the upper-right corner. If not, sign in first.

## Create a Publish Profile

1. Select **Azure** as the publish target and click **Next**.
2. Select **Azure App Service (Windows)** and click **Next**.
3. Select the provided directory **Azubi_Menderes**.
4. Click **Create new** in the upper-right corner.

---

## Web API

Use the following setting:

| Setting  | Value          |
| -------- | -------------- |
| App Name | `helpdesk-api` |

Then:

1. Click **Create**.
2. Click **Next**.
3. Under **API Management**, select **Skip this step**.
4. Click **Finish**.
5. Click **Publish**.

After the deployment has finished, the Web API is hosted using Azure App Service.

---

## Razor Pages Application

Repeat the deployment process for the Razor Pages project.

Use the following setting:

| Setting  | Value          |
| -------- | -------------- |
| App Name | `helpdesk-web` |

Then:

1. Click **Create**.
2. Click **Next**.
3. Click **Finish**.
4. Click **Publish**.

After the deployment has finished, the Razor Pages application is hosted using Azure App Service.

---

# 4. Configure the SQL Server Firewall

After deployment, an additional firewall setting must be configured so that the Azure Web App can connect to the Azure SQL Database.

Without this setting, errors may occur when trying to log in or retrieve data from the database.

1. Open the **Azure Portal**.

2. Select the **Azure SQL Server** — not the SQL Database.

3. Navigate to **Networking**.

4. Under **Exceptions**, enable:

   **Allow Azure services and resources to access this server**

5. Click **Save**.

6. Wait briefly for the firewall configuration to be applied.

After the setting has been applied, the Azure Web App can successfully connect to the Azure SQL Database.

---

# Deployment Complete

After completing all steps, the Helpdesk application consists of:

* An **Azure SQL Server**
* An **Azure SQL Database**
* A deployed **ASP.NET Web API**
* A deployed **ASP.NET Razor Pages application**
* A configured connection between the Azure App Services and the SQL Database

The application can now be accessed through the deployed Azure App Service.
