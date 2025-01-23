# My Application
This application, according to the requirements, retrieves and modifies product data. It uses Swagger with versioning. It is built using the Traditional Layered Architecture in .NET, divided into two layers: Presentation, Application, and Data, maintaining a reference structure of Presentation -> Application -> Data. MSSQL is used as the database server, which is connected through a connection string found in the appsettings. The appsettings also include an option to use mock data via UseMockData (default is false). The technologies used for mapping are a mapper, and for query optimization, MediatR is employed. Each layer has its own corresponding test project. 

https://localhost:7092/api/v1/Product To get a list of products.
https://localhost:7092/api/v2/Product To get a list of products with pagination

## Prerequisites
- Microsoft Visual Studio 2022 
- MS SQL Server 2022

## Running the Unit Tests
To run the unit tests for this project, follow these steps:

1. Open the project in your preferred Integrated Development Environment (IDE), such as PyCharm, Visual Studio, or IntelliJ.
2. Locate the test file or folder (e.g., `tests/`).
3. Click on "Run All Tests" or the equivalent button in your IDE. This will automatically execute the tests and show the results.
