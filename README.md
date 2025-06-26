# Microservices News Aggregation System

## Overview

This project is a simplified microservices-based news aggregation system, designed as a proof of concept for evaluating service separation, JWT authentication, and periodic background processing.

## Project Structure

- **JwtUsers** – Manages user registration, login, and JWT issuance. Also includes a subscription endpoint that updates the user's `NewSubscription` flag using their token.
- **NewsApi** – Serves authorized endpoints to query news data (by date, ticker, text, etc.). Also includes a public endpoint for the latest news.
- **NewsFetcherService** – Background worker that fetches external news articles from Polygon.io every hour and persists new ones.
- **News.Contracts** – Shared NuGet package that contains the `NewsItem` model. Used to share structure between `NewsApi` and `NewsFetcherService` without tight coupling.
- **Common** – Shared interfaces (e.g., `IRepository<T>`), extensions, and utility code.
- **/db folder** – Contains local `.db` files for SQLite used by each service. For simplicity, the `News` table is flattened and denormalized to avoid introducing relational joins in this version.

## Run Instructions

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Swagger UI is used for testing (no frontend needed)

### Run

```bash
cd JwtUsers
 dotnet run

cd NewsApiService
 dotnet run

cd NewsFetcherService
 dotnet run
```

### Swagger URLs

- **JwtUsers** → [https://localhost:7052/swagger/index.html](https://localhost:7052/swagger/index.html)
- **NewsApi** → [https://localhost:7200/swagger/index.html](https://localhost:7200/swagger/index.html)

> You can also export Swagger JSON and import it into Postman.

---

## Endpoints Overview

### JwtUsers (User Service)

| Endpoint             | Method | Description                                        |
| -------------------- | ------ | -------------------------------------------------- |
| `/JwtUsers/register` | POST   | Register new user                                  |
| `/JwtUsers/login`    | POST   | Login and receive JWT                              |
| `/Subscription`      | POST   | Mark user as subscribed (based on JWT email claim) |

### NewsApi

| Endpoint                        | Method | Description                               |
| ------------------------------- | ------ | ----------------------------------------- |
| `/api/News`                     | GET    | Get all news                              |
| `/api/News/range/{days}`        | GET    | Filter by date range                      |
| `/api/News/instrument/{ticker}` | GET    | Filter by ticker symbol (limit via query) |
| `/api/News/search/{text}`       | GET    | Search by title/description/ticker        |
| `/api/News/{id}`                | GET    | Get single news item by ID                |

### Public

| Endpoint             | Method | Description                                   |
| -------------------- | ------ | --------------------------------------------- |
| `/api/public/latest` | GET    | Latest 1 article per instrument (top 5 total) |

---

## NewsFetcherService

A background service that fetches financial news from Polygon.io every hour, stores it in the `NewsApi`'s database using dependency-injected repository.

- Uses `HttpClientFactory`
- Parses articles and skips duplicates by `ExternalId`
- Delay interval: 1 hour

---

## Two Architecture Options

### Monolithic Architecture

A single application that includes all functionality in one codebase, making it simpler to start but harder to scale or maintain.

### Microservices-Based Architecture

An approach where each functionality is split into separate, independently deployable services for better scalability and modularity.

### Comparison Table

| Aspect                     | Monolithic                      | Microservices                                          |
| -------------------------- | ------------------------------- | ------------------------------------------------------ |
| **Simplicity**             | Easier to develop/deploy        | Requires setup for communication and service discovery |
| **Scalability**            | Hard to scale specific features | Each service can scale independently                   |
| **Separation of concerns** | All logic in one place          | Clear separation of responsibilities                   |
| **Fault Isolation**        | One bug may crash the whole app | Isolated failures per service                          |
| **Development Speed**      | Faster for small teams          | Suitable for large, parallel teams                     |
| **Flexibility**            | Tied to one tech stack          | Each service can use its own language/tools            |

### Recommendation

Use **microservices** for the long-term benefits in scalability, maintainability, and modularity. While the monolith is good for MVPs or internal tools, this case benefits from clear separation and potential for scaling.

---

## Future Improvements & Scalability Suggestions

- **API Gateway** (YARP/Ocelot): Centralized routing and rate limiting
- **Message Bus** (Kafka/RabbitMQ): For async events like "user subscribed"
- **Caching**: Use Redis or MemoryCache for `/public/latest`
- **PostgreSQL/SQL Server**: Use full-featured DB with better performance, indexes, FTS
- **GraphQL**: Support real-time subscriptions for logged-in users to receive updates by ticker

---

## Task List with High-Level Estimation

| Task                                       | Estimate |
| ------------------------------------------ | -------- |
| Project scaffolding and microservice setup | 1.5h     |
| JwtUsers + JWT Auth                        | 1.5h     |
| News API logic (CRUD, filtering)           | 2h       |
| Background worker to fetch news            | 2h       |
| Subscription endpoint                      | 0.5h     |
| Testing and Swagger config                 | 1h       |
| Architecture + documentation               | 2.5h     |
| Final review + polish                      | 1h       |

**Total: \~12 hours**

