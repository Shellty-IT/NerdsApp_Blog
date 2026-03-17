# Shellty Blog 🐢

A modern, containerized CMS/blog application built with .NET 8 and ASP.NET Core MVC. Created as part of the Nerds Family Engineering Academy with focus on clean code, design patterns and best practices.

## 🌐 Demo

[Live Demo on Render](https://shellty-blog.onrender.com)

Demo Credentials:
- **Email:** admin@shellty.com
- **Password:** Admin123!

> ⚠️ First load may take ~30s due to Render free tier cold start.

## 📖 About

Shellty Blog is a content management system where administrators can create, edit and manage blog posts with image support. The application features a unique admin approval workflow — new administrators must be unanimously accepted by all existing admins through a voting system.

### Features

- **Blog Management:** create, edit and delete posts with rich content and cover images
- **Image Upload:** upload cover images with client-side preview, server-side validation (2 MB, JPG/PNG/WebP/GIF)
- **Category Filtering:** organize and filter posts by categories
- **User Authentication:** registration, login and role-based authorization via ASP.NET Core Identity
- **Admin Voting System:** unanimous approval required from all current admins to grant admin role
- **User Management Panel:** list users, delete accounts, remove admin privileges
- **Role-Based UI:** write/edit/delete buttons visible only to administrators
- **Responsive Design:** modern UI with Bootstrap 5, custom CSS variables and hover effects
- **Containerized Deployment:** Docker + Render with Neon serverless PostgreSQL

## 🛠️ Tech Stack

| Layer | Technology |
|-------|-----------|
| Framework | .NET 8, ASP.NET Core MVC |
| Language | C# 12 |
| Database | PostgreSQL (Neon) |
| ORM | Entity Framework Core 8 + Npgsql |
| Auth | ASP.NET Core Identity |
| Frontend | Razor Views, Bootstrap 5, Bootstrap Icons |
| Deployment | Docker, Render |


## 🚀 Getting Started

### Prerequisites

- .NET 8 SDK
- PostgreSQL (or Neon account)
- Docker (optional)

### Local Development

```bash
git clone https://github.com/your-username/Shellty_Blog.git
cd Shellty_Blog

Update connection string in appsettings.Development.json:

JSON

{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=shellty;Username=postgres;Password=yourpassword"
  }
}

Run the application:

Bash

dotnet ef database update
dotnet run

Docker

Bash

docker build -t shellty-blog .
docker run -p 10000:10000 -e DATABASE_URL="your-connection-string" shellty-blog

🏗️ Architecture

text

Shellty_Blog/
├── Controllers/        # MVC controllers (Home, BlogPost, Account, Admin)
├── Models/             # Domain models and ViewModels
├── Views/              # Razor views organized by controller
├── Data/               # EF Core DbContext and configuration
├── Migrations/         # EF Core database migrations
├── wwwroot/            # Static files (CSS, JS, uploads, favicon)
└── Program.cs          # Application entry point and service configuration

Key Design Decisions

    Code First approach with EF Core migrations
    File-scoped namespaces and nullable reference types enabled
    No comments in code — clean, self-documenting code
    Zero compiler warnings policy
    DateTime.UtcNow used consistently across all models and controllers
    Anti-forgery tokens on all POST forms
    Separated JavaScript into dedicated files

✅ Best Practices

    Gitflow: feature branches with pull requests
    Commit convention: short, lowercase messages
    Clean code: no comments, no warnings, proper naming
    Security: role-based authorization, input validation, anti-forgery protection
    Responsive UI: mobile-first approach with Bootstrap 5
    Containerization: reproducible builds with Docker

📋 Backlog

    Comments under posts
    Likes / favorites
    User profile (edit display name)
    Post search
    Cloud image storage (Cloudinary/S3)
    Integration tests for roles and authorization

📝 License

This project was created for educational purposes as part of the Nerds Family Engineering Academy.