# Job Board Platform 🚀

A modern Web Application for managing job postings, built with **ASP.NET Core MVC**. This platform allows employers to post job openings and candidates to browse and apply for them.

## ✨ Features

- **User Authentication & Authorization**: Secure login and registration using ASP.NET Core Identity.
- **Role-Based Access Control (RBAC)**: Distinct permissions for **Admins**, **Employers**, and **Candidates**.
- **Job Management**: Full CRUD (Create, Read, Update, Delete) functionality for job postings.
- **Company Profiles**: Association of job postings with specific companies.
- **Modern UI**: Clean and responsive design using Bootstrap and custom CSS.
- **Statistics Module**: Real-time data insights using LINQ queries.
- **Automatic Database Seeding**: Pre-configured roles and admin accounts upon first launch.

## 🛠️ Tech Stack

- **Framework**: .NET 8 / ASP.NET Core MVC
- **Database**: Microsoft SQL Server
- **ORM**: Entity Framework Core (Code First / EnsureCreated)
- **Identity**: Microsoft.AspNetCore.Identity
- **Frontend**: Razor Views, HTML5, CSS3, Bootstrap 5

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server Express (LocalDB)
- JetBrains Rider 

### Setup
1. **Clone the repository**:
   ```bash
   git clone <https://github.com/codingburgas/2526-11V-SD-IPVoynov22.git>
   ```
2. **Go to project directory**
```bash
cd 2526-11V-SD-IPVoynov22
```

3. **Install EF Core CLI (if not installed)**
   ```bash
   dotnet tool install --global dotnet-ef
   ```
4. **Restore dependencies**
```bash
dotnet restor
```
5. **Build the project**
```bash
dotnet build
```
6. **Apply the database migrations**
```bash
dotnet ef database update
```
7. **Run the application**
```bash
dotnet run
```
   
