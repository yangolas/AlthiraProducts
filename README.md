# AlthiraProducts 🚀

Professional Microservices-based Product Catalog built with **.NET 10**, designed to demonstrate high-scalability patterns and modern Cloud-Native infrastructure.

## 🏗️ Architectural Overview

This project follows **Clean Architecture** and **Hexagonal Architecture** principles to ensure a decoupled, maintainable, and testable codebase.

* **Pattern Implementation:** CQRS (Command Query Responsibility Segregation) with **MediatR**.
* **Domain-Driven Design (DDD):** Rich domain models, entities, and value objects.
* **Asynchronous Communication:** Prepared for event-driven consistency using message brokers.
* **Persistence:** Entity Framework Core with optimized query patterns.

## 🛠️ Technology Stack

* **Backend:** .NET 10 (C# 14)
* **Database:** SQL Server (EF Core)
* **Containerization:** Docker with multi-stage builds for optimized production images.
* **Orchestration:** Kubernetes (AKS) manifests included in the `/k8s` directory.
* **CI/CD:** Automated workflows via **Azure Pipelines** (`azure-pipelines.yml`).

## 🚀 Infrastructure & DevOps

The repository is "Production-Ready", featuring enterprise-grade configurations:

* **Docker Optimization:** Uses specialized SDK and ASP.NET runtime stages.
* **Pipeline Automation:** Full CI/CD integration for seamless cloud delivery.

## 🏁 Getting Started

1. Clone the repository.
2. Build the image: `docker build -t althira-products .`
3. Deploy to your local cluster using the provided `/k8s` manifests.

---
*Developed by Daniel Yanguas Durán*
