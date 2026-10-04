# 🤖 AI Enterprise Command Center

An AI-powered enterprise management and IT support dashboard built with **ASP.NET Core MVC, .NET 8, SQL Server, Entity Framework Core, and Generative AI**.

The system provides a centralized platform for managing employees, assets, projects, service-desk tickets, reports, notifications, and AI-powered assistance.

---

## 🚀 Features

### 👥 Employee Management

* Add, update, and manage employees
* Employee code, department, designation, contact and salary details
* Search and pagination
* Active/inactive employee management

### 💻 Asset Management

* Track company IT assets
* Asset code, category, brand, model and purchase information
* Assign assets to employees
* Search and manage asset availability

### 🎫 Service Desk

* Create and manage IT support tickets
* Track ticket status and priority
* AI-assisted ticket analysis
* Rule-based troubleshooting recommendations

### 📊 Dashboard

* Centralized enterprise dashboard
* Employee and asset statistics
* Service-desk overview
* Reports and business insights
* Visual dashboard metrics

### 🤖 AI Enterprise Assistant

* AI-powered chatbot integrated into the dashboard
* Ask questions about employees, assets and support tickets
* Tool-based AI responses using enterprise data
* AI-assisted troubleshooting
* Context-aware conversation history

### 🔔 Notifications

* Employee and system notifications
* Recent notification tracking
* Mark notifications as read
* Mark all notifications as read

### 🔐 Authentication & Authorization

* ASP.NET Core Identity
* User authentication
* Role-based access control
* Secure application architecture

---

## 🛠️ Technology Stack

| Technology                  | Usage                            |
| --------------------------- | -------------------------------- |
| **C#**                      | Backend development              |
| **.NET 8**                  | Application framework            |
| **ASP.NET Core MVC**        | Web application                  |
| **Entity Framework Core**   | ORM / Database access            |
| **SQL Server**              | Database                         |
| **ASP.NET Core Identity**   | Authentication & authorization   |
| **HTML / CSS / JavaScript** | Frontend                         |
| **Bootstrap**               | Responsive UI                    |
| **Generative AI / LLM**     | AI assistant and ticket analysis |
| **QuestPDF**                | Report generation                |

---

## 🏗️ Architecture

The project follows a **layered architecture**:

```text
AIEnterpriseCommandCenter
│
├── AIEnterpriseCommandCenter.Domain
│   └── Entities and core business models
│
├── AIEnterpriseCommandCenter.Application
│   └── Services, interfaces and business logic
│
├── AIEnterpriseCommandCenter.Infrastructure
│   └── Database, EF Core, repositories and external services
│
├── AIEnterpriseCommandCenter.Web
│   └── MVC controllers, views and UI
│
├── AIEnterpriseCommandCenter.Shared
│   └── Shared models and utilities
│
└── AIEnterpriseCommandCenter.Tests
    └── Application tests
```

---

## 🧠 AI Architecture

The AI assistant uses a **tool-first approach** to provide relevant enterprise information.

```text
User
 │
 ▼
AI Enterprise Assistant
 │
 ├── Employee Tool
 │
 ├── Asset Tool
 │
 ├── Service Desk Tool
 │
 └── Dashboard Tool
 │
 ▼
Enterprise Database / Business Logic
 │
 ▼
AI Response
```

This approach helps the assistant retrieve application-specific information instead of relying only on general AI knowledge.

---

## 🗄️ Database

The application uses **Microsoft SQL Server** with **Entity Framework Core**.

Major data areas include:

* Employees
* Assets
* Service Desk Tickets
* Projects
* Reports
* Notifications
* Users & Roles

---

## ⚙️ Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/Dhruvaa23/AIEnterpriseCommandCenter.git
```

### 2. Open the solution

Open:

```text
AIEnterpriseCommandCenter.sln
```

using **Visual Studio 2022**.

### 3. Configure SQL Server

Update the database connection string in:

```text
AIEnterpriseCommandCenter.Web/appsettings.json
```

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=EnterpriseDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

### 4. Apply database migrations

Open **Package Manager Console** in Visual Studio and run:

```powershell
Update-Database
```

Or use:

```bash
dotnet ef database update
```

### 5. Run the application

```bash
dotnet run
```

or press:

```text
F5
```

in Visual Studio.

---

## 🔑 AI Configuration

The AI assistant requires the configured AI provider/API settings.

Add the required configuration to your application settings or environment variables.

> **Important:** Never commit API keys, passwords, connection secrets, or other sensitive credentials to GitHub.

---

## 📁 Project Structure

```text
AIEnterpriseCommandCenter/
│
├── AIEnterpriseCommandCenter.Application/
├── AIEnterpriseCommandCenter.Domain/
├── AIEnterpriseCommandCenter.Infrastructure/
├── AIEnterpriseCommandCenter.Shared/
├── AIEnterpriseCommandCenter.Tests/
├── AIEnterpriseCommandCenter.Web/
├── AIEnterpriseCommandCenter.sln
└── .gitignore
```

---

## 🎯 Project Objective

The objective of this project is to build a centralized **AI-powered enterprise management platform** that combines traditional business management features with modern AI capabilities.

It demonstrates practical experience in:

* Full-stack .NET development
* RESTful/business-service architecture
* SQL Server database management
* Entity Framework Core
* Authentication and authorization
* Enterprise application architecture
* Generative AI integration
* AI tool-based workflows
* IT service-desk automation

---



## 👩‍💻 Developer

**Dhruvika Rajput**

B.Tech – Computer Science Engineering

Vadodara, Gujarat, India

### Links

* GitHub: https://github.com/Dhruvaa23


---

## ⭐ Project Highlights

⭐ AI-powered enterprise assistant
⭐ ASP.NET Core MVC + .NET 8
⭐ SQL Server + Entity Framework Core
⭐ Role-based authentication
⭐ Employee & asset management
⭐ IT service-desk management
⭐ AI-assisted troubleshooting
⭐ Layered enterprise architecture
⭐ Automated PDF reporting

---

## 📄 License

This project is developed for **educational, portfolio, and demonstration purposes**.
