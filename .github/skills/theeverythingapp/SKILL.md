---
name: theeverythingapp
description: 'Project context for TheEverythingApp — a personal assistant .NET 10 + React hobby app. Use when: discussing architecture, adding features, writing code, reviewing decisions, or answering questions about this project. Contains: stack decisions, architecture, implemented features, coding conventions, planned features, and useful commands.'
---

# TheEverythingApp — Project Context

## What This Is
A personal assistant hobby app for tracking habits, training, diet, body status, utility bills, meals, and more. Single user (the owner). Deployed to Azure.

## Stack
- **Backend**: .NET 10 Web API, EF Core + Npgsql (PostgreSQL), JWT Bearer auth, Scalar (OpenAPI UI)
- **Frontend**: Vite + React + TypeScript + MUI (not yet started)
- **Database**: PostgreSQL (Docker locally, Azure Database for PostgreSQL Flexible Server in prod)
- **CI/CD**: GitHub Actions
- **Hosting**: Azure Container Apps (backend), Azure Static Web Apps (frontend), Azure Container Registry (images)

## Key References

- [Architecture & project structure](./references/architecture.md)
- [Implemented features](./references/features.md)
- [Coding conventions](./references/conventions.md)
- [Planned features & roadmap](./references/roadmap.md)
- [Useful commands](./references/commands.md)
