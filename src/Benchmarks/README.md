# VisitorService Benchmark - Dapper vs EF Core

This project benchmarks and compares the performance of `VisitorService` operations using Dapper and Entity Framework Core.

## Prerequisites

1. PostgreSQL database running (can use docker-compose from project root)
2. Database schema initialized (run migrations)
3. Update `appsettings.json` with correct connection string

## Running Benchmarks

From the project root:

```bash
dotnet run --project src/Benchmarks/Benchmarks.csproj --configuration Release
```

## Benchmark Operations

The following operations are benchmarked:

1. **TryGetAsync** - Single record retrieval by ID
2. **CreateAsync** - Insert new visitor record
3. **UpdateAsync** - Update existing visitor record
4. **DeleteAsync** - Delete visitor record (creates then deletes)
5. **GetAll** - Bulk retrieval with pagination (with and without `sinceId`)

## Notes

- Dapper is set as the baseline (1.00) for relative comparisons
- Each benchmark uses seeded test data (1000 visitors)
- Memory diagnostics are enabled to compare memory allocations

