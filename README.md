# 💰 FinanceTracker Enterprise API

> A robust, scalable, and secure Financial Management System built with **.NET 10** and designed for modern **Angular** frontends.

![Build Status](https://github.com/YOUR_USERNAME/FinanceTracker/actions/workflows/dotnet.yml/badge.svg)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Angular](https://img.shields.io/badge/angular-%23DD0031.svg?style=for-the-badge&logo=angular&logoColor=white)
![SQLite](https://img.shields.io/badge/sqlite-%2307405e.svg?style=for-the-badge&logo=sqlite&logoColor=white)
![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white)

## 📖 Overview

**FinanceTracker** is a high-performance RESTful API engineered to handle personal financial data with precision and security. It implements industry-standard practices including **Clean Architecture**, **JWT Authentication**, and **Result Pattern** error handling.

This project serves as the backend core for a Personal Finance SaaS platform, supporting multi-tenancy (user data isolation) and complex reporting queries.

---

## 🏗️ Architecture & Design Patterns

The solution is architected with scalability and maintainability in mind, avoiding common "spaghetti code" pitfalls.

*   **Layered Architecture**: Strict separation of concerns (API $\to$ Services $\to$ Data Access).
*   **Result Pattern**: Replaced generic Exceptions with a typed `ServiceResult<T>` pattern for predictable control flow.
*   **Global Error Handling**: Centralized `GlobalExceptionHandler` middleware ensures consistent API error responses (RFC 7807 Problem Details).
*   **DTO Pattern**: Data Transfer Objects isolate our internal Domain Models from the external API contract.
*   **Repository Pattern (via EF Core)**: Abstraction over data access logic using Entity Framework Core.
*   **Dependency Injection**: Extensive use of .NET Core's built-in DI container for loose coupling.

---

## 🛠️ Tech Stack

### **Backend**
*   **Framework**: .NET 10 (Preview/RC)
*   **ORM**: Entity Framework Core
*   **Database**: SQLite (Dev) / PostgreSQL (Prod ready)
*   **Auth**: JWT (JSON Web Tokens) with Role-Based Access Control
*   **Documentation**: Swagger / OpenAPI v3
*   **Testing**: xUnit with InMemory & SQLite mocking

### **Frontend** (In Development)
*   **Framework**: Angular 18+
*   **Styling**: TailwindCSS
*   **State Management**: NgRx / Signals

---

## 🚀 Getting Started

### Prerequisites
*   [.NET 10 SDK](https://dotnet.microsoft.com/download)
*   [Node.js](https://nodejs.org/) (for Angular frontend)

### Installation

1.  **Clone the repository**
    ```bash
    git clone https://github.com/yourusername/FinanceTracker.git
    cd FinanceTracker
    ```

2.  **Restore Dependencies**
    ```bash
    dotnet restore
    ```

3.  **Configuration**
    The API requires a JWT Key to run. Copy the example config file and update it with your own secure key:
    ```bash
    cd FinanceTracker.API
    cp appsettings.development.json.example appsettings.development.json
    ```
    *Note: Ensure the `Key` in `appsettings.development.json` is at least 32 characters long.*

4.  **Database Migration**
    Initialize the SQLite database:
    ```bash
    cd FinanceTracker.API
    dotnet ef database update
    ```

4.  **Run the API**
    ```bash
    dotnet run
    ```
    The API will be available at `https://localhost:7045` (or similar).

### Exploring the API
Once running, navigate to the **Swagger UI** to explore the endpoints interacting with the API:
> `https://localhost:7045/swagger/index.html`

---

## 🔑 Key Features

### 1. **Smart Transaction Management**
*   **On-the-Fly Categorization**: Create transactions and new categories in a single request.
*   **Fallbacks**: Graceful handling of null categories via "Uncategorized" safety nets.

### 2. **Financial Reporting**
*   **Monthly Summaries**: Aggregated views of Income vs. Expenses.
*   **Date Filtering**: Precision querying by month and year.

### 3. **Security First**
*   **Data Isolation**: Strict `UserId` checks on every CRUD operation prevent horizontal privilege escalation (IDOR protection).
*   **Stateless Auth**: Bearer Token authentication for scalable stateless sessions.

---

## 🧪 Testing

The solution includes a comprehensive test suite in `FinanceTracker.Tests`, covering:
*   **Unit Tests**: Service layer logic verification.
*   **Integration Tests**: Database interactions using EF Core InMemory.

To run tests:
```bash
dotnet test
```

---

## 🗺️ Roadmap

*   [x] **Docker Support**: Containerization for easy deployment.
*   [x] **CI/CD Pipeline**: GitHub Actions for automated testing.
*   [ ] **Recurring Transactions**: Background jobs (Quartz.net) for automated inputs.
*   [ ] **Multi-Currency Support**: Integration with external Exchange Rate APIs.

---

Made with ❤️ by Hector Castro

