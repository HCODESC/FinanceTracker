# 💰 Personal Finance Tracker

A full-stack Blazor + .NET Web API application to help users track income, expenses, budgets, and generate visual financial reports.

---

## 📌 Project Goals

- Allow users to track income and expenses
- Categorize transactions
- Set monthly budgets
- Generate reports with charts
- Provide a clean, modern UI and secure authentication
- Easily extensible for future features (e.g., recurring bills, savings goals)

---
## 📁 Project Structure
--- -
## ✅ Core To-Dos

### 🔧 Initial Setup

- [x] Create a new .NET solution: `FinanceTracker.sln`
- [x] Add projects:
    - [x] `FinanceTracker.API` - ASP.NET Core Web API
    - [ ] `FinanceTracker.Blazor` - Blazor WebAssembly (or Server)
    - [x] `FinanceTracker.Shared` - Shared Class Library
- [x] Add EF Core packages to API project
- [x] Add authentication setup (ASP.NET Identity or JWT)

---

### 🗃️ Database & Models

- [x] Create `AppDbContext` with DbSet properties:
    - [x] `Users`
    - [x] `Transactions`
    - [x] `Categories`
    - [x] `Budgets`
- [x] Set up entity relationships (User → Transactions, Categories, Budgets)
- [x] Add initial migrations and apply database

---

### 🧩 Backend API

#### Models (EF Core)

- [x] `User`
- [x] `Transaction`
- [x] `Category`
- [x] `Budget`

#### DTOs

- [x] `TransactionDto`
- [x] `BudgetDto`
- [x] `CategoryDto`

#### Controllers

- [x] `AuthController` – Register, Login, JWT
- [x] `TransactionsController` – CRUD
- [x] `CategoryController` - CRUD
- [x] `BudgetsController` – Set/View Budgets
- [x] `ReportsController` – Return monthly/annual summaries

#### Services

- [x] `ITransactionService`
- [x] `ICategoryService`
- [x] `IBudgetService`
- [x] `IReportService`

---

### 🎨 React UI

#### Pages

- [ ] `/login` – User login
- [ ] `/register` – New user registration
- [ ] `/dashboard` – Overview of financial health
- [ ] `/transactions` – List/add/edit transactions
- [ ] `/budgets` – Set/view budget limits
- [ ] `/reports` – Visualize financial data

#### Components

- [ ] `<TransactionForm />` – Add/edit transactions
- [ ] `<TransactionList />` – List of all transactions
- [ ] `<BudgetCard />` – Budget overview
- [ ] `<ReportChart />` – Line/bar/pie charts

#### Services (API Integration)

- [x] `AuthService`
- [x] `TransactionService`
- [x] `BudgetService`
- [x] `ReportService`

---

### 📊 Charts & Reports

- [ ] Integrate charting library (ChartJs.Blazor, LiveCharts, or Blazorise)
- [ ] Create reports grouped by:
    - [ ] Category
    - [ ] Date (month/year)
    - [ ] Budget status

---

### 🔐 Authentication & Security

- [x] Add JWT to API
- [x] Secure endpoints with `[Authorize]`
- [ ] React Auth:
    - [ ] Store auth token in localStorage
    - [ ] Show/hide pages based on login state
    - [ ] Logout + token expiry handling

---

### 📦 Deployment

- [ ] Setup database in Azure/AWS/PostgreSQL
- [ ] Deploy API using:
    - [ ] Azure App Service / Railway / Render
- [ ] Deploy React UI using:
    - [ ] Azure Static Web Apps / Vercel / Netlify
- [ ] Setup CI/CD (GitHub Actions or Azure DevOps)
- [ ] Add environment configs and connection strings

---

### 🧪 Testing

- [ ] Add unit tests for services (xUnit/NUnit)
- [ ] Add integration tests for controllers
- [ ] UI tests (optional with Playwright or bUnit)

---

### ✨ Stretch Goals 

- [ ] PDF/CSV export of monthly reports
- [ ] Recurring transactions
- [ ] Savings goal tracker
- [ ] Mobile support (Blazor WASM or MAUI Hybrid)
- [ ] Dark mode & responsive design
- [ ] PWA support (offline access)
- [ ] Notifications/reminders for upcoming bills

---

## 🧠 Tips

- Follow **Clean Architecture**: Controllers → Services → Repositories
- Use dependency injection for services
- Keep DTOs separate from EF Core models
- Document your API with Swagger

---

## 🏁 Final Output Checklist

- [ ] Responsive and visually polished Blazor frontend
- [ ] Secure and scalable Web API
- [ ] Fully functional database and migration history
- [ ] Deployed app with live demo URL
- [ ] Public GitHub repo with README, screenshots, and tech stack

---




