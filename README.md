# Helpdesk – Ticket Management System

#### Video Demo: https://youtu.be/aF2EPzRz6E0

#### Description:

Helpdesk is an internal web application for creating, managing and tracking support tickets. I developed the application independently as a four-week summer project during my software development apprenticeship.

The goal of the project was to create a practical internal helpdesk system that could be used by employees to report problems and by administrators to manage and process those requests. Users can create tickets, edit their own tickets, add comments, upload files and keep track of their requests. Administrators have additional functionality for managing users and processing tickets, including changing the status, priority and responsible person.

The project is divided into three main parts: an ASP.NET Razor Pages frontend, an ASP.NET Web API backend and a Class Library used as an API client. The frontend communicates with the backend through the REST API, while the API handles communication with the SQL Server database through Stored Procedures.

I developed the complete application during the four-week project period. This included planning the application, designing the database, creating the Stored Procedures, implementing the API and frontend, adding authentication and authorization, implementing the ticket and file management functionality, testing the application and deploying it to Microsoft Azure.



---

## Project Overview

**Project:** Internal Helpdesk and Ticket Management System

**Project Type:** Four-week summer project during my software development apprenticeship

**Development Time:** Approximately 4 weeks

**Developer:** Daniel

The application consists of three projects:

* **ASP.NET Razor Pages** – Frontend
* **ASP.NET Web API** – Backend
* **Class Library** – API client

The frontend does not communicate directly with the database. Requests are sent through the Web API, which handles the application logic and database communication.

The application is deployed using Microsoft Azure.

---

## Technologies

* C#
* ASP.NET Razor Pages
* ASP.NET Web API
* SQL Server
* Azure SQL Database
* Azure App Service
* JavaScript
* HTML
* CSS
* Bootstrap
* REST API
* SQL Stored Procedures

---

## Live Demo

The application is currently deployed on Microsoft Azure.

**Web Application:**
https://helpdesk-web-ere2edcnh8grd6ab.germanywestcentral-01.azurewebsites.net

**API / Swagger:**
private

The user interface is currently available in German.



## Features

### User Features

Normal users can:

* Log in and log out
* View their dashboard
* View their own tickets
* Create new tickets
* Edit their own tickets
* Add comments
* Upload files
* Download files
* Remove their own uploaded files
* Edit their profile
* Change their password
* Change their email address
* Change their phone number

Users can only access tickets belonging to them.

---

### Administrator Features

Administrators have additional functionality for managing the helpdesk:

* Manage users
* Create users
* Delete users
* View user details
* View all tickets
* Change ticket status
* Change ticket priority
* Assign responsible users
* Manage comments
* View ticket history
* Download ticket files
* Access the administration dashboard

---

## Ticket Management

The central part of the application is the ticket management system.

A ticket contains information such as its title, description, status, priority and responsible user.

The ticket system supports:

* Creating tickets
* Editing tickets
* Viewing ticket details
* Changing status
* Changing priority
* Assigning responsible users
* Adding comments
* Uploading files
* Downloading files
* Removing files
* Viewing ticket history
* Character limits
* Live character counters
* Comment pagination

Editing is disabled once a ticket has been closed.

---

## Ticket Logs

Changes made to tickets are recorded in the `TicketLogs` table.

The log provides information about:

* Who made a change
* What was changed
* When the change was made
* Status changes
* Priority changes
* Changes to the responsible user

This makes important changes to a ticket traceable.

---

## File Upload

Users can attach files to their tickets.

The current implementation allows:

* Up to 3 files per ticket
* A maximum file size of 50 MB per file
* File uploads when creating a ticket
* File downloads
* File removal according to the user's permissions

File-related actions are handled by the backend and associated with the corresponding ticket.

---

## Dashboard

The application contains a dashboard with an overview of the current ticket situation.

It includes information such as:

* Total number of tickets
* Open tickets
* Tickets in progress
* Closed tickets
* High-priority tickets
* Recently created tickets
* Recent activities
* Tickets created today
* Tickets created this month
* Quick access to relevant areas

Administrators have access to additional information and management functions.

---

## Database

The application uses a SQL Server database hosted on Azure SQL.

The main database tables are:

* `Benutzer`
* `Logins`
* `Tickets`
* `Kommentare`
* `TicketLogs`
* `Priority`
* `Status`
* `TicketDateien`

Database access is handled through SQL Stored Procedures.

The Razor Pages frontend communicates with the Web API, and the Web API communicates with the database. This separates the user interface from the database layer.

---

## Authentication and Security

The application includes several authentication, authorization and validation mechanisms.

These include:

* Password hashing
* Cookie-based authentication
* Login validation
* Administrator authorization
* User-specific ticket access
* Input validation
* Unique email addresses
* Unique phone numbers
* Error handling
* Restrictions for closed tickets

Users cannot access another user's tickets simply by changing a ticket identifier. Unauthorized ticket access is rejected by the application.

---

## Project Structure

### ASP.NET Razor Pages

The Razor Pages project contains the frontend of the application.

It is responsible for:

* Rendering the user interface
* Displaying forms
* Handling user interactions
* JavaScript functionality
* Sending requests to the Web API
* Displaying API responses

### ASP.NET Web API

The Web API contains the backend functionality.

It handles:

* Authentication and validation
* User management
* Ticket management
* Comments
* File handling
* Dashboard data
* Ticket logs
* Communication with the SQL database

### Class Library

The Class Library contains reusable API client functionality used by the frontend to communicate with the Web API.

---

## Development

The application was developed during a four-week summer project as part of my software development apprenticeship.

I worked independently on the complete application. The development process included:

1. Planning the application
2. Designing the database
3. Creating database tables
4. Creating SQL Stored Procedures
5. Implementing authentication
6. Developing the Web API
7. Developing the Razor Pages frontend
8. Implementing JavaScript functionality
9. Implementing ticket management
10. Implementing comments
11. Implementing file management
12. Implementing administrator functionality
13. Implementing ticket logs
14. Creating the dashboard
15. Testing and debugging
16. Deploying the application to Microsoft Azure

The application was developed iteratively. Features were implemented and tested throughout the project, and errors and usability issues were corrected during development.

---

## Installation

A detailed installation guide is available in:

[`Installation.md`](Installation.md)

The installation guide explains the setup and deployment process, including:

* Creating the Azure SQL Server
* Creating the Azure SQL Database
* Configuring the database
* Configuring firewall settings
* Deploying the Web API
* Deploying the Razor Pages application
* Configuring the Azure services


---

## Additional Documentation

Further information about the development and implementation of the project is available in:

[`Dokumentation.md`](Dokumentation.md)

The documentation contains additional information about the project, its architecture, database and implemented functionality.



---

## Author

**Daniel**

Software Development Apprentice

**GitHub:** Dxniket

**Project:** Helpdesk – Ticket Management System

**Development Time:** 4 weeks
