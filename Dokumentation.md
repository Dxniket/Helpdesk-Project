# Project Documentation

# Introduction

As part of a summer project, a web application for managing support tickets was developed. The application was implemented using ASP.NET Razor Pages, an ASP.NET Web API, and an Azure SQL Database.

Users can create tickets and track their current processing status. Administrators can additionally manage users, process tickets, and assign responsible users to tickets.

---

# Development Process

## Setting Up the Development Environment

The first step was to set up the development environment. An SQL Server (Logical Server) was created in the Azure Portal, followed by an Azure SQL Database.

During the setup, all required settings were configured. These included selecting the Azure region, configuring SQL authentication, setting up the firewall, and allowing the local client IP address to access the database.

An additional installation guide was created as a Markdown file to document the setup process and make it possible to recreate the database environment when required.

The guide describes all steps from creating the SQL Server to configuring the finished database.

---

## Database Planning

Before development began, an ER model was created to plan the required database tables and their relationships.

The following tables were initially planned for the project:

* Benutzer
* Logins
* Tickets
* Kommentare
* Status
* Priority

After completing the database model, the relationships between the tables were defined together with the required primary and foreign keys.

---

## Database Creation

All required tables were then created in the Azure SQL Database.

The necessary columns, data types, primary keys, and foreign keys were defined for each table.

### Benutzer Table

| Column          | Data Type     |
| --------------- | ------------- |
| BenutzerId (PK) | int           |
| Vorname         | nvarchar(100) |
| Nachname        | nvarchar(100) |
| Email           | nvarchar(255) |
| Telefonnummer   | nvarchar(20)  |
| CreationDate    | datetime      |

### Logins Table

| Column          | Data Type     |
| --------------- | ------------- |
| LoginId (PK)    | int           |
| BenutzerId (FK) | int           |
| PasswordHash    | nvarchar(200) |
| IsAdmin         | bit           |
| CreationDate    | datetime      |

### Tickets Table

| Column            | Data Type      |
| ----------------- | -------------- |
| TicketId (PK)     | int            |
| BenutzerId (FK)   | int            |
| BearbeiterId (FK) | int            |
| Titel             | nvarchar(120)  |
| Beschreibung      | nvarchar(2000) |
| StatusId (FK)     | int            |
| PriorityId (FK)   | int            |
| ErstelltAm        | datetime       |
| AktualisiertAm    | datetime       |

### TicketLogs Table

| Column           | Data Type     |
| ---------------- | ------------- |
| TicketLogId (PK) | int           |
| TicketId (FK)    | int           |
| Beschreibung     | nvarchar(500) |
| ErstelltAm       | datetime      |

### Kommentare Table

| Column           | Data Type      |
| ---------------- | -------------- |
| KommentarId (PK) | int            |
| TicketId (FK)    | int            |
| BenutzerId (FK)  | int            |
| Text             | nvarchar(2000) |
| ErstelltAm       | datetime       |

### Status Table

| Column        | Data Type    |
| ------------- | ------------ |
| StatusId (PK) | int          |
| WertStatus    | nvarchar(30) |

### Priority Table

| Column          | Data Type    |
| --------------- | ------------ |
| PriorityId (PK) | int          |
| WertPriority    | nvarchar(30) |

### Relationships

The following relationships were created between the tables:

* Benutzer → Logins
* Benutzer → Tickets
* Benutzer → Kommentare
* Tickets → Kommentare
* Status → Tickets
* Priority → Tickets
* Benutzer → Tickets (assigned user)

---

## Creating the Project Structure

After preparing the database, the initial application projects were created.

The following projects were set up:

* ASP.NET Web API
* ASP.NET Razor Pages

The Web API handles communication with the database, while the Razor Pages project provides the user interface of the ticket management system.

Both projects were then prepared for database connectivity and further application development.

---

## Creating the Class Library

A Class Library was added to simplify communication between the Razor Pages application and the Web API.

The Class Library contains the API client and the required classes for communication with the API.

The required Web API assembly was referenced by the Class Library, and the Class Library was then added as a dependency of the Razor Pages project.

This allows the Razor Pages application to access the classes and methods provided by the Class Library while maintaining a clear separation between the individual application components.

The final project structure therefore consists of:

* ASP.NET Web API
* Class Library (API Client)
* ASP.NET Razor Pages

---

## Deployment to Azure

After establishing the basic application structure, both the ASP.NET Web API and Razor Pages projects were deployed to Azure.

Visual Studio's Publish functionality was used for the deployment. Each project was deployed as a separate Azure App Service, allowing both the API and the web application to run and be tested in Azure.

The deployment process is also documented in the installation guide, which provides step-by-step instructions for publishing both projects and connecting them to the previously created Azure resources.

---

## Creating Database Procedures

After creating the database, the first Stored Procedures were implemented.

Stored Procedures were initially created for adding new users and login records. An additional procedure was developed to verify user credentials and validate login attempts.

---

## Implementing the Configuration Provider

The required methods were implemented in the Configuration Provider to provide controlled database access.

For authentication, a `ValidateLogin` method was implemented. This method receives the required parameters, executes the corresponding Stored Procedure, and returns the result to the application.

Database access is therefore handled through the Configuration Provider rather than directly from the controllers.

---

## Implementing the Login Functionality

After preparing the database connection, the login functionality was implemented.

The required login models were created first. A `LoginController` was then developed to receive login credentials, call the Configuration Provider, and return the result to the frontend.

A login page was subsequently created in the Razor Pages project. Both frontend and backend functionality were implemented, allowing users to enter their credentials and authenticate themselves.

---

## Creating a Base Class for Cookies

A base class was created to centrally manage authentication cookies.

This provides a single location for creating, reading, and deleting login cookies.

---

## Updating the Application Layout

The application's layout was updated alongside the development of the login functionality.

The basic application structure and navigation were prepared to provide a consistent layout across the application and allow additional pages to be integrated easily.

---

## Application Settings

The application configuration was extended to manage the required application URLs.

The URLs for the Web API and Razor Pages application are configured through `appsettings.json`. Dedicated configuration sections and extension methods were created to load and access these settings.

Sensitive credentials and database connection information are not part of the public project documentation.

---

## Creating the Administration Portal

The administration portal was developed to manage application users.

All users are displayed using Bootstrap tables. Administrators and standard users are displayed in separate tables to provide a clearer overview.

A search function was also integrated, allowing administrators to find users directly without manually browsing the complete list.

---

## Developing Administrator Functions

The required API endpoints and Stored Procedures were created for user administration.

These include functionality for creating new users and deleting existing users.

---

## Quick User Creation Dialog

A dialog was added to the administration portal to simplify the process of creating new users.

The dialog can be opened directly from the administration page and allows an administrator to create a user without leaving the current page. The dialog was implemented using JavaScript.

Before creating the user, the application checks whether the entered email address or phone number is already in use. If a duplicate is detected, an appropriate error message is displayed.

---

## Separate User Creation Page

In addition to the dialog, a dedicated page for creating new users was developed.

Administrators can access this page through the **Create New User** button. All required user information can be entered and saved through a more detailed form.

The application also verifies that the entered email address and phone number are not already assigned to another account.

Administrators therefore have two options for creating users: directly through the quick-create dialog or through the dedicated user creation page.

---

## User Details

A user details page was added to the administration portal.

This page displays the important information associated with the selected user. Administrators can review the user's information and delete the user when necessary.

---

## Profile Management

A **My Profile** page was created to allow users to manage their own account information.

A corresponding `ProfileController` was also implemented.

Users can update the following information:

* First name
* Last name
* Email address
* Phone number
* Password

All changes are stored in the database after saving.

---

## User Data Validation

Additional validation was implemented when updating profile information.

When a user changes their email address or phone number, the application checks whether the new value is already being used by another user.

If a duplicate is detected, an appropriate error message is displayed and the changes are not saved.

This prevents multiple users from using the same email address or phone number.

---

## Developing Ticket Management

After completing the initial user management functionality, development continued with the ticket management system.

The required Stored Procedures were created first. These procedures handle database operations for creating and managing tickets.

The corresponding models and `TicketController` were then developed. The controller processes requests from the frontend and accesses the relevant Stored Procedures through the Configuration Provider.

---

## My Tickets Page

A **My Tickets** page was created for authenticated users.

This page displays all tickets created by the currently authenticated user in a Bootstrap table.

The table provides a clear overview of the user's existing tickets and displays the most important information for each ticket.

---

## New Ticket Page

A **New Ticket** page was developed to allow users to create support tickets.

Users can enter the required information, including a title and description.

After submitting the form, the data is sent to the API and stored in the database using the corresponding Stored Procedure.

---

## Ticket Details

A dedicated ticket details page was created.

The **Ticket Details** page displays all relevant information about the selected ticket, including:

* Title
* Description
* Current status
* Priority
* Creator
* Assigned user
* Creation date
* Last update date

---

## Extending User Details

The existing **User Details** page was extended with ticket-related information.

In addition to general account information, the page displays the number of tickets created by the selected user.

Further statistics were later added as development progressed.

---

## Extending Ticket Functionality

The `TicketController`, ticket models, and corresponding Stored Procedures were extended to support the additional ticket functionality.

A Stored Procedure was created to retrieve all information associated with a ticket from the database.

The returned data is passed through the `TicketController` to the frontend and displayed on the Ticket Details page.

---

## Extending Ticket Management

Ticket management was further expanded by extending both the `TicketController` and `AdminController`.

Additional Stored Procedures, models, and Configuration Provider methods were implemented to support the new functionality and database communication.

---

## Administrator Ticket Management

A dedicated **Ticket Management** page was created for administrators.

This page displays all tickets from all users in a Bootstrap table, providing administrators with a complete overview of the available tickets.

Each ticket can be opened from the overview to view or modify additional information.

---

## Extending Ticket Details

The ticket details functionality was expanded to provide users with all relevant information about their tickets and allow them to track the current processing status.

---

## Administrator Ticket Details

A dedicated ticket details page was created for administrators.

Unlike the standard user view, administrators can modify additional ticket properties, including:

* Priority
* Status
* Assigned user

This allows incoming tickets to be assigned to a responsible user and managed throughout their lifecycle.

---

## Extending Database Integration

Additional models and Configuration Provider methods were created to support the new administrator functionality.

The required Stored Procedures were also developed to retrieve ticket information and store changes to the status, priority, or assigned user.

Ticket management therefore continues to be handled centrally through the API and Configuration Provider.

---

## Editing Tickets as an Administrator

The administrator ticket details functionality was expanded further.

Administrators can change the priority, status, and assigned user directly from the ticket.

After saving, the changes are sent to the database through the API.

Updated values are automatically reflected across all relevant parts of the application, including:

* Ticket Details
* Administrator Ticket Overview
* User Ticket Overview

This ensures that all users see the current processing status of a ticket.

---

## Ticket Logs

A new **TicketLogs** table was created to provide an audit trail of ticket changes.

Important modifications to a ticket are recorded in this table, including changes to:

* Status
* Priority
* Assigned user

The corresponding Stored Procedures were extended so that a new TicketLog entry is automatically created whenever one of these values changes.

This makes it possible to track what was changed and when the change occurred.

---

## Displaying the Last Modification

The **Ticket Details** page was extended to display the time of the most recent modification.

If a ticket has already been updated, users can immediately see when the latest change was made.

---

## Editing Own Tickets

Users can edit their own tickets after creation.

Using the **Edit** button, users can modify the title and description of a ticket.

After saving, the changes are stored in the database using the `TicketBenutzerUpdate` Stored Procedure.

This allows users to correct or update ticket information when necessary.

---

## Ticket Logs for Administrators

A dedicated **Ticket Logs** page was created for administrators.

It can be accessed through the **Logs** button on the administrator ticket details page.

All changes associated with the selected ticket are displayed in a Bootstrap table.

A filter function was also integrated, allowing administrators to quickly find individual log entries.

This provides a complete overview of the changes made to a ticket and when they occurred.

---

## Restrictions for Closed Tickets

Ticket management was extended so that closed tickets can no longer be modified.

Once a ticket has the status **Completed**, users can no longer change its title or description.

Administrators can also no longer modify the ticket's status, priority, or assigned user.

This preserves the final state of a completed ticket and prevents subsequent modifications.

---

## Input Validation

Character limits were added to the input fields used for creating and editing tickets.

The maximum number of characters corresponds to the field sizes defined in the database, preventing users from entering more data than can be stored.

A live character counter is also displayed below the relevant input fields.

Users can therefore see how many characters have already been entered and how many remain available.

This provides immediate feedback and prevents invalid input before the form is submitted.

---

## Time Zone Correction

During development, it was discovered that timestamps in the Azure SQL Database were stored in UTC, causing creation and modification times to appear offset in the application.

To resolve this issue, time zone conversion was implemented directly in the Stored Procedures using the following SQL expression:

```sql
SYSUTCDATETIME() AT TIME ZONE 'UTC' AT TIME ZONE 'W. Europe Standard Time'
```

This converts timestamps to Central European time and automatically accounts for daylight saving time.

As a result, timestamps displayed in the application remain correct throughout the year.

---

## Developing the Dashboard

A dashboard was developed as the user's landing page after authentication.

The dashboard provides a quick overview of the most important information related to the user's tickets.

Two additional Stored Procedures, a dedicated `DashboardController`, and four model classes were created to retrieve and transfer the required information between the database, API, and frontend.

The dashboard includes:

* Overview of all created tickets
* Number of open tickets
* Number of tickets in progress
* Number of closed tickets
* Recent ticket activity
* Recently created tickets
* Quick access to frequently used pages
* High-priority ticket overview
* Number of tickets created today
* Number of tickets created during the current month
* Ticket summary by priority

This provides users with an immediate overview of their current tickets after logging in.

---

## Extending User Details with Statistics

The **User Details** page was extended with additional ticket statistics.

In addition to the total number of tickets created by the selected user, administrators can see:

* Number of open tickets
* Number of tickets in progress
* Number of closed tickets
* Number of high-priority tickets

This provides administrators with a more detailed overview of each user's tickets.

---

## Correcting the Last Modified Display

The display of a ticket's most recent modification was revised.

After a ticket is changed, the correct modification date is stored and displayed in the application.

Users and administrators can therefore determine when a ticket was last updated.

---

## Improving User Name Display

The user name displayed on the dashboard was improved.

After authentication, the full name of the currently logged-in user is loaded and displayed in the dashboard's welcome section.

This makes it immediately clear which user account is currently active.

---

## Updating Ticket Details

The ticket details functionality was further improved.

After editing a ticket, the updated description is immediately displayed correctly on both the standard Ticket Details page and the Administrator Ticket Details page.

This prevents outdated ticket information from being displayed.

---

## Developing the Comment System

A comment system was developed to allow communication between users and administrators within individual tickets.

The required Stored Procedures, models, `CommentController`, and Configuration Provider methods were created.

Comments can therefore be stored and retrieved through the API.

The comment system was integrated into both the standard Ticket Details page and the Administrator Ticket Details page.

---

## Creating and Displaying Comments

New comments are created through a dialog window.

The maximum character limit is validated while the user is typing, and a live character counter provides immediate feedback.

After saving, comments are displayed in a chat-style interface, with the newest comment displayed first.

For improved readability, sent and received comments are visually separated:

* The user's own comments are displayed on the right in a blue message bubble.
* Comments from other users are displayed on the left.

Each comment also displays:

* Author name
* Author role
* Creation time
* Comment text

This creates a clear conversation between users and administrators directly within a ticket.

---

## Comment Restrictions for Closed Tickets

The comment functionality also respects the current ticket status.

Once a ticket has been closed, neither users nor administrators can add additional comments.

Existing comments remain visible but cannot be extended with new messages.

This preserves the complete conversation history of a closed ticket.

---

## Securing Ticket Access

Ticket access was revised to improve application security.

The `TicketController` and corresponding Stored Procedure were modified to verify whether the current user is authorized to access the requested ticket.

This prevents users from accessing tickets belonging to other users by manipulating the URL or ticket ID.

Error handling was also improved using `try-catch` blocks and nullable ticket objects to safely handle potential errors and prevent unexpected application crashes.

The functionality was tested after implementation to verify that unauthorized ticket access was successfully prevented.

---

## Comment Pagination

Pagination was added to the comment system.

The required properties were implemented and comments are loaded page by page using `Skip()` and `Take()`.

Both the administrator and user interfaces display four comments per page.

This keeps the interface manageable when tickets contain many comments and reduces the amount of data loaded at once.

---

## Ticket File Management

File management functionality was added to the ticket system.

Users can upload files when creating a new ticket. Uploaded files are stored together with the ticket and can later be downloaded.

Users can also remove their uploaded files while permitted by the ticket's access rules.

The required models, controllers, database tables, and Stored Procedures were implemented and integrated across both frontend and backend.

The following restrictions apply:

* Maximum of three files per ticket
* Maximum file size of 50 MB per file
* Multiple supported file types
* Uploaded files can be downloaded
* Administrators can download and review uploaded files
* Only the ticket creator can remove uploaded files

---

# Conclusion

During the project, a complete ticket and task management system was developed.

The application combines:

* Azure SQL Database
* ASP.NET Web API
* Class Library
* ASP.NET Razor Pages

The individual components communicate with each other to provide a complete web-based helpdesk system.

The implemented functionality includes user management, authentication, ticket management, administrator functionality, ticket history, comments, file management, input validation, access control, and a dashboard with multiple ticket statistics and overviews.

The project provided practical experience in developing a complete application across the database, backend, API, frontend, security, and cloud deployment layers.
