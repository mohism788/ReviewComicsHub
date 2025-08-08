# 📚 Comic Review Hub

Comic Review Hub is a modular ASP.NET Core Web API solution built to manage comics, their issues, and user reviews. It includes secure user authentication and role-based authorization using JWT.

---

## 🔧 Project Overview

The solution is composed of multiple APIs:

- **ComicsAPI** – Manages comic entries and user authentication
- **IssuesAPI** – Handles comic issues and associated reviews

---

## 🚀 Features

- User registration & login with JWT authentication
- Role-based authorization (`User`, `Moderator`)
- Comic management (CRUD)
- Issue management per comic (CRUD)
- Review system with per-issue reviews
- Image upload for comics
- Modular microservice communication via `HttpClient`
- Swagger UI for testing endpoints

---

## 🛠 Technologies Used

- **ASP.NET Core 9**
- **Entity Framework Core** (Code-First & Migrations)
- **ASP.NET Identity**
- **JWT Authentication & Authorization**
- **Microsoft SQL Server**
- **Swagger / OpenAPI**
- **C#**
- **HttpClient** for internal API communication

---

## 🧪 Roles & Permissions

| Role       | Permissions                                           |
|------------|--------------------------------------------------------|
| User       | view issues, add/delete own reviews                   |
| Moderator  | Delete any review, issue, or comic                    |


