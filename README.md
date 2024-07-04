# Order App

This repository contains two projects: an Angular application (frontend) located in the `app/` directory and a .NET Core API (backend) located in the `api/` directory. The backend is configured to run in Docker along with the database using Docker Compose, while the Angular frontend runs locally.

## Setup for API Project (backend)

### Prerequisites

- Docker installed

### Running the API with Docker Compose

1. Navigate to the `api/` directory:

   ```sh
   cd api
   ```
2. Start the project using Docker Compose:

```sh
docker-compose up
```
This will start the API and PostgreSQL database in Docker containers.

### Additional Configuration
- Ensure that the database connection settings are correct in the `api/appsettings.json` file.

## Setup for Angular Project (frontend)

### Prerequisites
- Node.js and npm installed

### Running Angular locally
1. Navigate to the `app/` directory:
```sh
cd app
```

2. Install project dependencies:

```sh
npm install
```

3. Start the development server:

```sh
ng serve
```

The application will be available at `http://localhost:4200/`.

## Technical Notes - Future Improvements
- Full Test Coverage: Implement comprehensive unit and integration tests for both frontend and backend.

- Creation of Endpoints for OrderItems and Products: Expand the API to include dedicated endpoints for managing OrderItems and Products as required by the application.

- Angular Testing: Implement automated tests for Angular using Jasmine and Karma to ensure frontend code quality.

- Enhanced Complexity in Angular: Improve user experience and efficiency in the Angular application, especially in operations involving adding multiple items on a single screen.