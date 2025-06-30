# ✨ CelestialStars-Backend

This repository contains the **CelestialStars-Backend** code, which I use privately as a hobby project. The solution consists of multiple projects that collectively provide a REST API, database access, domain logic, and testing functionalities.

## Table of Contents
1. [Overview](#overview)
2. [Projects](#projects)
   - [CelestialStars-Api](#celestialstars-api)
   - [CelestialStars-Sql](#celestialstars-sql)
   - [CelestialStars-Domain](#celestialstars-domain)
   - [CelestialStars-UnitTests](#celestialstars-unittests)
4. [Contributing](#contributing)
5. [Contact](#contact)

---

## Overview

**CelestialStars-Backend** is a collection of projects that provide an ASP.NET Minimal API. It serves various functions such as Accounting, Webhook Management, Service Availability, and more, through REST endpoints.
Additionally, the repository includes database access via Entity Framework Core, shared domain classes, and automated tests.

---

## Projects

### CelestialStars-Api
- **Description**: Contains the Minimal API based on ASP.NET.
- **Function**: Provides REST endpoints for different functionalities (e.g., Accounting, Webhook Management, Service Availability).
- **Key Features**:
  - Minimal API architecture
  - HTTP endpoints for various operations and additional services
  - Easily extendable with new modules

### CelestialStars-Sql
- **Description**: Provides the Entity Framework Core DbContext.
- **Function**: Manages database access and migrations.
- **Key Features**:
  - Integrated with Sql Server
  - Serves as the main database access layer

### CelestialStars-Domain
- **Description**: Contains various entity classes such as DTOs and entities for Entity Framework.
- **Function**: Defines the core data structure and business logic.
- **Key Features**:
  - Separation of data structures and application logic
  - Shared models used across the other projects

### CelestialStars-UnitTests
- **Description**: Contains unit and integration tests for the API and other business logic.
- **Function**: Ensures that new changes do not break existing functionalities.
- **Key Features**:
  - Runs tests on every pull request and commit (CI integration)
  - Includes unit tests and integration tests
  - Helps maintain high code quality and reliability

---

## Contributing

Even though this is primarily a private hobby project, you are welcome to submit issues and pull requests. Please follow these guidelines:

1. **Code Style**: Follow standard .NET coding conventions.
2. **Tests**: Ensure that new changes include corresponding tests.
3. **Documentation**: Update the Swagger documentation if needed.
4. Do not break anything else :D

Before running, you'd also need to create the **CelestialStars-Api/appsettings.json** File containing sensitive Data.
Just fill it with the following scheme and you're ready to go:

```json
    {
      "Jwt": {
        "Key": "<INSERT RANDOM CHAR SEQUENCE> 512 BYTES",
        "Issuer": "https://api.aissa.dev",
        "Audience": "https://api.aissa.dev"
      },
      "ConnectionStrings": {
        "DefaultConnection": "<INSERT CONNECTION STRING IF NEEDED>"
      },
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning"
        }
      },
      "AllowedHosts": "*"
    }
```
---

## Contact

If you have any questions or suggestions, feel free to open an issue on GitHub or contact me directly on contact@aissa.dev.

---

✨*Enjoy working with the project, and thanks for your interest in **CelestialStars-Backend**!*✨
