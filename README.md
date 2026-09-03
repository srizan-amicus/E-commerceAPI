# EcommerceAPI

A RESTful e-commerce Web API built with **ASP.NET Core .NET 8**, using **ADO.NET**, **SQL Server**, DTOs, validation, AutoMapper, JWT authentication, middleware, and stored procedures.

## Features

* Product CRUD operations
* Product search, filtering, sorting, and pagination
* Category and brand APIs
* Product price management
* Product availability and inventory validation
* Shopping cart APIs
* Customer registration and login
* JWT authentication and role-based authorization
* Request audit logging
* Global exception handling with ProblemDetails
* Database-persisted error and request logs
* FluentValidation
* AutoMapper
* CancellationToken support for asynchronous APIs

## Technologies

* .NET 8
* ASP.NET Core Web API
* C#
* SQL Server
* ADO.NET
* JWT Authentication
* AutoMapper
* FluentValidation

## Project Structure

```text
EcommerceAPI
│
├── Controllers
├── DTOs
├── Middleware
├── Models
├── Repositories
│   ├── Interfaces
│   └── Implementations
├── Services
│   ├── Interfaces
│   └── Implementations
├── Validators
├── Profiles
└── Program.cs
```

## Getting Started

### Prerequisites

* .NET 8 SDK
* SQL Server
* Visual Studio 2022 or another .NET-compatible IDE
* Postman (recommended for API testing)

### Clone the Repository

```bash
git clone https://github.com/srizan-amicus/EcommerceAPI.git
cd EcommerceAPI
```

### Restore and Build

```bash
dotnet restore
dotnet build
```

### Database Configuration

Configure the SQL Server connection string in the application's configuration.

The project uses the database and the database contains the required tables, stored procedures, and table-valued type used by the application.

## Database Objects

### Tables

```text
Srizan_Categories
Srizan_Brands
Srizan_Products
Srizan_ProductPrices
Srizan_ErrorLogs
Srizan_RequestAuditLogs
```

### Table-Valued Type

```text
Srizan_InventoryItemType
```

### Stored Procedures

The application uses the following stored procedures:

```text
Srizan_GetAllProducts
Srizan_GetProductById
Srizan_SearchProducts
Srizan_CreateProduct
Srizan_UpdateProduct
Srizan_DeleteProduct

Srizan_GetProductAvailability
Srizan_CheckInventory

Srizan_GetCart
Srizan_AddCartItem
Srizan_UpdateCartItem
Srizan_RemoveCartItem
Srizan_ClearCart
Srizan_GetCartItemCount
Srizan_GetCartSubtotal

Srizan_LogError
Srizan_LogRequest
```

## Authentication and Authorization

The API uses JWT authentication.

Two roles are supported:

* **Customer** — access to customer-specific operations such as cart APIs
* **Admin** — access to administrative inventory operations

Protected endpoints require a valid JWT token.

## Middleware

### Request Logging Middleware

Records request information including:

* User ID
* Username
* HTTP method
* Request path
* Request parameters
* Response status
* Timestamp

Request audit records are stored in:

```text
Srizan_RequestAuditLogs
```

### Global Exception Handling Middleware

Handles unhandled exceptions centrally and returns responses using the `ProblemDetails` format.

Exception details are stored in:

```text
Srizan_ErrorLogs
```

## API Testing

The API can be tested using **Swagger** or **Postman**.

Test the following areas:

* Authentication and registration
* Product CRUD
* Product search and filtering
* Product availability
* Inventory validation
* Cart operations
* Role-based authorization
* Validation errors
* Not Found responses
* Unauthorized and Forbidden responses
* Global exception handling
* Request and error logging

## Running the API

Run the application using:

```bash
dotnet run
```

When Swagger is enabled, it can be accessed through the application's Swagger endpoint.

