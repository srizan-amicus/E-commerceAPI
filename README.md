# EcommerceAPI

A RESTful e-commerce Web API built with **ASP.NET Core .NET 8**, using **ADO.NET**, **SQL Server**, DTOs, validation, AutoMapper, JWT authentication, middleware, API versioning, caching, and stored procedures.

## Features

### Product Management

* Product CRUD operations
* Product search, filtering, sorting, and pagination
* Category and brand APIs
* Product price management
* Product availability and inventory validation
* Product image upload and download
* Image type and file-size validation

### Customer & Authentication

* Customer registration and login
* JWT authentication
* Role-based authorization
* Customer and Admin roles

### Shopping Cart

* Add items to cart
* Update cart items
* Remove cart items
* Clear cart
* Get cart details
* Cart item count
* Cart subtotal

### Checkout

* Shipping information
* Shipping method selection
* Shipping charge calculation
* Tax calculation
* Subtotal and final total calculation
* Cart price validation
* Stock availability validation
* Checkout validation before order creation

### Orders

* Create orders
* Retrieve order details
* Order items
* Order totals
* Order shipping information
* Order payment information
* Order status management
* Customer order history
* Order history status filtering
* Order history pagination
* Reorder previous orders

### Payment

* Mock payment API
* Payment method handling
* Transaction reference generation using UUID
* Payment status tracking
* Order status update after successful payment

### API Versioning

* URL-based API versioning
* Version 1.0 APIs
* Version 2.0 APIs
* Versioned Product, Cart, Checkout, and Order endpoints

Example:

```text
/api/v1/products
/api/v2/products

/api/v1/cart
/api/v2/cart

/api/v1/checkout
/api/v2/checkout

/api/v1/orders
/api/v2/orders
```

### Caching

* In-memory caching using `IMemoryCache`
* Response caching for product listings
* Per-customer product caching
* Cached product listing responses

### Logging & Error Handling

* Request audit logging
* Global exception handling
* ProblemDetails responses
* Database-persisted error logs
* Database-persisted request logs
* Structured logging using `ILogger`

### Validation

* Data Annotation validation
* FluentValidation
* Cart and checkout validation
* Product availability and stock validation

### Async Operations

* `CancellationToken` support throughout repository, service, and controller layers
* Asynchronous database operations using ADO.NET

---

## Postman Documentation

The API endpoints are organized and documented in a Postman collection covering:

* Authentication
* Products
* Categories
* Prices
* Availability
* Cart
* Checkout
* Payments
* Orders
* Order History

Each endpoint includes its HTTP method, URL, authentication requirements,
parameters, request body, example response, and expected HTTP status codes.

The Postman collection can be imported into Postman for API testing and
documentation.

---
## Technologies

* .NET 8
* ASP.NET Core Web API
* C#
* SQL Server
* ADO.NET
* JWT Authentication
* AutoMapper
* FluentValidation
* IMemoryCache
* Response Caching
* API Versioning
* Postman
* Swagger

---

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

---

## Architecture

The API follows a layered architecture:

```text
Controller
    ↓
Service
    ↓
Repository Interface
    ↓
Repository Implementation
    ↓
Stored Procedure
    ↓
SQL Server
```

DTOs and AutoMapper are used between the API layer and application models.

---

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

---

## Database Configuration

Configure the SQL Server connection string in the application's configuration.

The project uses the `TrainingDB` database and the database contains the required tables, stored procedures, and table-valued type used by the application.

---

## Database Objects

### Tables

```text
Srizan_Customers
Srizan_Categories
Srizan_Brands
Srizan_Products
Srizan_ProductPrices

Srizan_Carts
Srizan_CartItems

Srizan_Orders
Srizan_OrderItems
Srizan_OrderShipping
Srizan_OrderPayments

Srizan_ErrorLogs
Srizan_RequestAuditLogs
```

### Table-Valued Type

```text
Srizan_InventoryItemType
```

---

## Stored Procedures

### Products

```text
Srizan_GetAllProducts
Srizan_GetProductById
Srizan_SearchProducts
Srizan_CreateProduct
Srizan_UpdateProduct
Srizan_DeleteProduct
Srizan_UpdateProductImage
```

### Product Availability & Inventory

```text
Srizan_GetProductAvailability
Srizan_CheckInventory
```

### Cart

```text
Srizan_GetCart
Srizan_AddCartItem
Srizan_UpdateCartItem
Srizan_RemoveCartItem
Srizan_ClearCart
Srizan_GetCartItemCount
Srizan_GetCartSubtotal
```

### Orders

```text
Srizan_CreateOrder
Srizan_CreateOrderItem
Srizan_CreateOrderShipping
Srizan_CreateOrderPayment
Srizan_GetOrderById
Srizan_GetOrderItems
Srizan_GetOrderShipping
Srizan_GetOrderPayment
Srizan_UpdateOrderStatus
Srizan_GetOrderHistory
```

### Logging

```text
Srizan_LogError
Srizan_LogRequest
```

---

## Authentication and Authorization

The API uses **JWT authentication**.

Two roles are supported:

* **Customer** — access to customer-specific operations such as cart, checkout, and orders
* **Admin** — access to administrative operations

Protected endpoints require a valid JWT token.

Role-based authorization is implemented using:

```csharp
[Authorize(Roles = "Customer")]
```

and:

```csharp
[Authorize(Roles = "Admin")]
```

---

## API Versioning

The API uses URL-based versioning.

### Version 1

```text
/api/v1/products
/api/v1/cart
/api/v1/checkout
/api/v1/orders
```

### Version 2

```text
/api/v2/products
/api/v2/cart
/api/v2/checkout
/api/v2/orders
```

API versioning is configured using `Asp.Versioning.Mvc`.

---

## Checkout Flow

The checkout process validates the customer's cart before creating an order.

```text
Customer Cart
     ↓
Validate Stock
     ↓
Validate Current Prices
     ↓
Calculate Subtotal
     ↓
Calculate Shipping
     ↓
Calculate Tax
     ↓
Calculate Final Total
     ↓
Create Pending Order
     ↓
Process Mock Payment
     ↓
Store Transaction Reference
     ↓
Update Order Status
```

---

## Payment Flow

The application contains a mock payment API for testing.

```text
Checkout
   ↓
Pending Order
   ↓
Mock Payment API
   ↓
Generate UUID Transaction Reference
   ↓
Create Order Payment
   ↓
Payment Status = Success
   ↓
Order Status = Placed
```

The transaction reference is stored in:

```text
Srizan_OrderPayments
```

---

## Order History

Customers can retrieve their previous orders.

Supported features:

* Customer-specific order history
* Status filtering
* Pagination
* Reordering previous orders

Example:

```text
GET /api/v1/orders/history
```

With filtering and pagination:

```text
GET /api/v1/orders/history?status=Pending&page=1&pageSize=10
```

Reorder:

```text
POST /api/v1/orders/{orderId}/reorder
```

---

## Caching

The application uses `IMemoryCache` for product listing caching.

Product listing responses also use ASP.NET Core response caching.

Example:

```csharp
[ResponseCache(Duration = 60)]
```

Product data is cached per customer to align the cache lifetime with the application's JWT login session.

---

## Product Images

Products support image upload and download.

### Upload

```text
POST /api/v1/products/{id}/image
```

Supported image extensions:

```text
.jpg
.jpeg
.png
.webp
```

Maximum file size:

```text
5 MB
```

Uploaded images are stored under:

```text
wwwroot/product-images
```

The image path is stored in the `Srizan_Products.ImagePath` column.

### Download

```text
GET /api/v1/products/{id}/image
```

---

## Middleware

### Request Logging Middleware

Records request information including:

* User ID
* Username
* HTTP method
* Request path
* Request parameters
* Response status
* Request duration
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

---

## API Testing

The API can be tested using **Swagger** or **Postman**.

Test areas include:

* Authentication and registration
* JWT authentication
* Role-based authorization
* Product CRUD
* Product search
* Filtering
* Sorting
* Pagination
* Product availability
* Inventory validation
* Cart operations
* Checkout
* Shipping calculation
* Tax calculation
* Mock payment
* Order creation
* Order retrieval
* Order history
* Order status filtering
* Order pagination
* Reorder
* API versioning
* Caching
* Product image upload
* Product image download
* Validation errors
* Not Found responses
* Unauthorized and Forbidden responses
* Global exception handling
* Request logging
* Error logging

---

## Running the API

Run the application using:

```bash
dotnet run
```

When Swagger is enabled, it can be accessed through the application's Swagger endpoint.

---

## Example API Flow

A typical customer purchase flow is:

```text
Register
   ↓
Login
   ↓
Receive JWT
   ↓
Browse Products
   ↓
Add Product to Cart
   ↓
Checkout
   ↓
Validate Price & Stock
   ↓
Create Pending Order
   ↓
Mock Payment
   ↓
Payment Success
   ↓
Order Placed
   ↓
View Order History
   ↓
Reorder if Required
```

---

## Error Response

Unhandled exceptions are returned using the standard `ProblemDetails` format.

Example:

```json
{
  "type": "about:blank",
  "title": "An unexpected error occurred.",
  "status": 500,
  "detail": "An error occurred while processing the request."
}
```

