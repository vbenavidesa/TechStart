# TechStart
 TechStart Code

TECHNOLOGIES USED
The following technologies have been used according to the document given:

- Authorization/Authentication has been implemented using IdentityServer4 JWT Tokens.
- Logging framework implemented using Serilog on a JSON file.
- Basic ASP.Net Core health checks have been implemented(network response time, database sql health check and system disk space)
- The API versioning scheme used has been URL, it is denoted on the controllers.
- The ORM used for the demo has been EntityFrameworkCore


FOLDER STRUCTURE
The following folders have been created to order the project files
- Controllers: Contains all the controllers endpoints
- Core: Contains the interfaces for dependency injection
- DbContexts: Contains the dbcontexts used for the project
- Dtos: Contains all data transfer objects created to be used by the frontend
- HealthChecks: Contains the health checks classes, can be extended to more health checks as needed
- Mapping: Contains automapper mapping structure for Dtos and Models
- Migrations: Automated folder containing the migrations for the EF database
- Models: Contains all models and its properties for the database tables
- Persistence: Contains the repositories for the database CRUD operations

