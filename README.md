<div align="center">

# 🎯 TalentForge

**Job Portal & Applicant Tracking System — built in .NET / C#**

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![CI](https://img.shields.io/badge/CI-passing-brightgreen?style=for-the-badge&logo=githubactions)](../../actions)
[![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?style=for-the-badge&logo=docker)](https://www.docker.com/)
[![Deployed on Railway](https://img.shields.io/badge/Deployed-Railway-0B0D0E?style=for-the-badge&logo=railway)](https://railway.app)


*A production-shaped backend for job matching — built solo, phase by phase, from raw C# to a live deployed system.*

[**Live Demo**](#) · [**API Docs**](#) · [**Report Bug**](../../issues)

</div>

---

## 📌 What is this

TalentForge is a job portal + ATS backend built solo in ASP.NET Core / C#. Candidates and recruiters interact through a REST API backed by real infrastructure: auth, caching, background jobs, and live notifications — the kind of system design a real backend team ships, not a CRUD tutorial.

## ✨ Features

| Feature | How it's built |
|---|---|
| 🔐 Authentication | JWT bearer tokens, role-based `[Authorize]` guards |
| 🗄️ Data access | EF Core + Repository Pattern over SQL Server |
| ⚡ Caching | Redis — candidate list caching with cache-hit speedup |
| 🔄 Background jobs | Hangfire — async resume processing, off the request thread |
| 📡 Live updates | SignalR — real-time notification hub |
| ✅ Testing | xUnit — repository & service layer coverage |
| 📦 Containerized | Single `docker compose up` — app + SQL + Redis, one command |
| 🚀 CI/CD | GitHub Actions — restore → build → test on every push |
| ☁️ Deployed | Live on Railway — app, SQL, and Redis as linked services |

## 🏗️ Architecture

```mermaid
flowchart LR
    Client[Client / Postman] -->|JWT Bearer| API[TalentForge API]
    API --> Repo[Repository Layer]
    Repo --> SQL[(SQL Server)]
    API --> Cache[(Redis Cache)]
    API --> Jobs[Hangfire Jobs]
    Jobs --> SQL
    API --> Hub[SignalR Hub]
    Hub -->|live push| Client
```

## 🛠️ Tech Stack

<div align="center">

![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=c-sharp&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![EF Core](https://img.shields.io/badge/EF_Core-512BD4?style=flat-square)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-DC382D?style=flat-square&logo=redis&logoColor=white)
![Hangfire](https://img.shields.io/badge/Hangfire-1a1a1a?style=flat-square)
![SignalR](https://img.shields.io/badge/SignalR-512BD4?style=flat-square)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat-square&logo=docker&logoColor=white)
![xUnit](https://img.shields.io/badge/xUnit-1a1a1a?style=flat-square)
![GitHub Actions](https://img.shields.io/badge/GitHub_Actions-2088FF?style=flat-square&logo=githubactions&logoColor=white)
![Railway](https://img.shields.io/badge/Railway-0B0D0E?style=flat-square&logo=railway&logoColor=white)

</div>

## 📊 Build Progress

| Phase | Scope | Status |
|---|---|---|
| 0–1 | C# fundamentals, project scaffolding | ✅ |
| 2 | EF Core + Repository Pattern | ✅ |
| 3 | Web API + Controllers | ✅ |
| 4 | JWT Authentication | ✅ |
| 5 | Redis + Hangfire + SignalR | ✅ |
| 6 | Additional API features & polish | ✅ |
| 7 | xUnit Test Suite | ✅ |
| 8 | Docker Compose + CI/CD + Cloud Deploy | ✅ |

## 🚀 Getting Started (local)

```bash
git clone https://github.com/ShreyaasAI/TalentForge.git
cd TalentForge.Api
docker compose up --build -d
dotnet ef database update --connection "Server=localhost,1433;Database=TalentForgeDb;User Id=sa;Password=<your-pw>;TrustServerCertificate=True"
```

App runs at `http://localhost:8080` · Swagger UI at `/swagger`.

## 📡 Core Endpoints

| Method | Route | Description |
|---|---|---|
| `POST` | `/api/auth/register` | Create account |
| `POST` | `/api/auth/login` | Get JWT token |
| `GET` | `/api/candidates` | List candidates (Redis-cached) |
| `POST` | `/api/candidates` | Add candidate (triggers Hangfire job) |
| `POST` | `/api/candidates/{id}/resume` | Upload resume |
| `GET` | `/api/jobs` | List jobs |
| `POST` | `/api/jobs` | Create job posting |

## 🧪 Testing

```bash
dotnet test TalentForge.slnx
```


---

<div align="center">
Built solo by <a href="https://github.com/ShreyaasAI">Shreyas</a> — one phase at a time.
</div>
