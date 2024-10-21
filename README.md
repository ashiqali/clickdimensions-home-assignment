# Click Dimensions - Take Home Assignment

## Overview - Social Sync Portal (Reddit and Tumblr)

SocialSyncPortal is a .NET 8.0 web API that integrates data from Reddit and Tumblr. The project fetches posts from these social platforms on a scheduled routine and stores them in both a database and in-memory for immediate access. The API provides endpoints to retrieve, add, update, and delete social posts, with authentication using JWT tokens.


## Table of Contents

- [Technologies Used](#technologies-used)
- [Setup and Installation](#setup-and-installation)
- [Running the Application](#running-the-application)
- [Testing](#testing)
- [Scheduler Configuration](#scheduler-configuration)
- [API Documentation](#api-documentation)
- [Authentication](#authentication)
- [Versioning](#versioning)
- [Logging](#logging)
- [Screenshots](#screenshots)


## Technologies Used

### Backend
- .NET Core 8
- Entity Framework Core
- SQLite Database
- JWT Authentication
- Quartz.NET (For scheduling jobs)
- AutoMapper
- Serilog (For structured logging)
- Swagger UI (For API documentation)
- RestSharp (For API calls to Reddit and Tumblr)
- xUnit

### Frontend
- React 18
- TypeScript
- Vite


## Setup and Installation

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js and npm](https://nodejs.org/)
- [Visual Studio or Visual Studio Code](https://visualstudio.microsoft.com/)

### Backend Setup

1. Clone the repository:
    ```sh
    https://github.com/ashiqali/clickdimensions-home-assignment.git
    ```

2. Navigate to the backend project directory:
    ```sh
    cd social-sync-portal-api
    ```

3. Restore the .NET dependencies:
    ```sh
    dotnet restore
    ```

### Frontend Setup

1. Navigate to the frontend project directory:
    ```sh
    cd social-sync-portal-ui
    ```

2. Install the npm dependencies:
    ```sh
    npm install
    ```

### Database Setup

1. Run Database Migrations:

SQLite has been used as database.
Apply the database migrations to create the tables. From a command line :
 ```sh
    cd SocialSyncPortal.DAL
    update-database
```

## Running the Application

### Backend

Start the .NET backend:
```sh
cd social-sync-portal-api
dotnet run
```

### Frontend

Start the React frontend:
```sh
npm start
```

The backend will be running at `https://localhost:5000` and the frontend at `http://localhost:3000`.

## Testing

### Demo Login 

Credentials:
```sh
http://localhost:3000
Username: admin
Password: 123
```

### Backend

To run the tests for the backend:
```sh
cd SocialSyncPortal.Test
dotnet test
```
## Scheduler Configuration

The project uses **Quartz.NET** to schedule the fetching of social posts from Reddit and Tumblr. The job scheduling now supports cron expressions, providing flexibility to control when the job runs.

### Default Configuration

By default, the scheduler is set to run every minute using the following cron expression:

```csharp
"0 0/1 * * * ?"
```

### Customizing the Schedule

You can modify the cron expression directly in the `Program.cs` file:

Or in the `Program.cs`:

```csharp
q.AddTrigger(opts => opts
    .ForJob(jobKey)
    .WithIdentity("FetchSocialPostsJob-trigger")
    .WithCronSchedule("0 0/1 * * * ?") // Customize the cron expression here
);
```

## API Documentation

The Swagger UI for the API can be accessed at:
```sh
https://localhost:5000/swagger/index.html
```
### Endpoints

- **Auth Endpoints**:
  - `POST /api/v1/auth/login`: Login and receive a JWT token.
  - `POST /api/v1/auth/register`: Register a new user.
  - `POST /api/v1/auth/refresh`: Refresh the JWT token.

- **Social Post Endpoints**:
  - `GET /api/v1/socialpost`: Get all social posts.
  - `GET /api/v1/socialpost/{postId}`: Get a specific post by ID.
  - `POST /api/v1/socialpost`: Add a new post.
  - `PUT /api/v1/socialpost`: Update an existing post.
  - `DELETE /api/v1/socialpost/{postId}`: Delete a post by ID.

- **User Endpoints**:
  - `GET /api/v1/user`: Get all users.
  - `GET /api/v1/user/{userId}`: Get a specific user by ID.
  - `POST /api/v1/user`: Add a new user.
  - `PUT /api/v1/user`: Update an existing user.
  - `DELETE /api/v1/user/{userId}`: Delete a user by ID.

You can use these endpoints to manage users and social posts within the system. Make sure to include the JWT token in the Authorization header when accessing these endpoints:

```
Authorization: Bearer {token}
```

## Authentication
- **JWT Authentication**: 
  - Authentication has been added. Endpoints require a JWT token; otherwise, a 401 response will be returned.
  - Users must log in or register to obtain a JWT token.
  - The token must be included in the Authorization header for requests:
    - Click the lock icon next to the particular endpoint and paste the token in the textbox on the Swagger page.
    - Alternatively, add the token to the Authorization header of the request.

To disable authentication for specific endpoints, remove the `Authorize` attribute from the respective controller.


## Versioning
- URL versioning has been implemented

## Logging
- **Structured Logging**: 
  - Serilog has been implemented for structured logging.
  - Logs are written to both file and console.
  - Configuration settings can be found in `appsettings.json`.
  - Examples of log message formatting and different logging levels are available in the `UserService`.
  - Logging will be improved over time.

## Screenshots

![image](https://github.com/user-attachments/assets/4f184fc1-44b1-4112-8503-2962d35b919a)

![image](https://github.com/user-attachments/assets/0d37d56e-027d-4282-8678-c3a97d675f78)

![image](https://github.com/user-attachments/assets/a66fc18e-b761-4b6a-b583-3bf66b84cbad)

![image](https://github.com/user-attachments/assets/14673a0e-771e-4b66-898d-ac906e2d123b)

![image](https://github.com/user-attachments/assets/c9ebd140-25c5-487f-b5d3-5bba79e1f86c)

![image](https://github.com/user-attachments/assets/bff7a4ec-d60f-4c82-b571-0f453ac1be32)

![image](https://github.com/user-attachments/assets/b97dc9c7-769b-4529-a06b-a8e34a35e0f0)






---


