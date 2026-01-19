using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model;
using Model.Data;
using Model.Domain;
using Model.Services;

namespace Benchmarks;

[SimpleJob(RuntimeMoniker.Net90)]
[MemoryDiagnoser]
[MinColumn, MaxColumn, MeanColumn, MedianColumn]
public class VisitorServiceBenchmarks
{
    private IServiceProvider _dapperServiceProvider = null!;
    private IServiceProvider _efServiceProvider = null!;
    private readonly List<Visitor> _testVisitors = new();
    private readonly Random _random = new(1337); // Fixed seed for reproducibility

    [GlobalSetup]
    public async Task GlobalSetup()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connectionString = configuration.GetConnectionString("AmusementParkDB")
            ?? throw new InvalidOperationException("Connection string not found");
        
        _dapperServiceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .AddSingleton<IDbConnectionFactory, NpgsqlDbConnectionFactory>(_ =>
                new NpgsqlDbConnectionFactory(connectionString))
            .AddDapperData()
            .AddModel()
            .BuildServiceProvider();
        
        _efServiceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .AddDbContextFactory<DatabaseContext>(opt =>
                opt.UseNpgsql(connectionString,
                    cfg => cfg.MigrationsAssembly("Model")))
            .AddEfData()
            .AddModel()
            .AddLogging()
            .BuildServiceProvider();
        
        await SeedTestDataAsync();
    }

    [GlobalCleanup]
    public async Task GlobalCleanup()
    {
        await CleanupTestDataAsync();
    }

    #region TryGetAsync Benchmarks

    [Benchmark(Baseline = true)]
    public async Task<Visitor?> TryGetAsync_Dapper()
    {
        var service = _dapperServiceProvider.GetRequiredService<IVisitorService>();
        var visitorId = _testVisitors[_random.Next(_testVisitors.Count)].Id;
        return await service.TryGetAsync(visitorId);
    }

    [Benchmark]
    public async Task<Visitor?> TryGetAsync_EFCore()
    {
        var service = _efServiceProvider.GetRequiredService<IVisitorService>();
        var visitorId = _testVisitors[_random.Next(_testVisitors.Count)].Id;
        return await service.TryGetAsync(visitorId);
    }

    #endregion

    #region CreateAsync Benchmarks

    [Benchmark]
    public async Task CreateAsync_Dapper()
    {
        var service = _dapperServiceProvider.GetRequiredService<IVisitorService>();
        var visitor = CreateTestVisitor();
        var createVisitorResult = await service.CreateAsync(visitor);
        if (createVisitorResult is { IsSuccess: true, Value: not null })
        {
            _testVisitors.Add(createVisitorResult.Value);
        }
    }

    [Benchmark]
    public async Task CreateAsync_EFCore()
    {
        var service = _efServiceProvider.GetRequiredService<IVisitorService>();
        var visitor = CreateTestVisitor();
        var createVisitorResult = await service.CreateAsync(visitor);
        
        if (createVisitorResult is { IsSuccess: true, Value: not null })
        {
            _testVisitors.Add(createVisitorResult.Value);
        }
    }

    #endregion

    #region UpdateAsync Benchmarks

    [Benchmark]
    public async Task UpdateAsync_Dapper()
    {
        var service = _dapperServiceProvider.GetRequiredService<IVisitorService>();
        var originalVisitor = _testVisitors[_random.Next(_testVisitors.Count)];
        // Create a copy to avoid modifying the cached object
        var visitorToUpdate = new Visitor
        {
            Id = originalVisitor.Id,
            Name = $"Updated Name {_random.Next(1000)}",
            Email = $"updated{_random.Next(1000)}@example.com",
            Phone = originalVisitor.Phone,
            RegistrationDate = originalVisitor.RegistrationDate
        };
        await service.UpdateAsync(visitorToUpdate);
    }

    [Benchmark]
    public async Task UpdateAsync_EFCore()
    {
        var service = _efServiceProvider.GetRequiredService<IVisitorService>();
        var originalVisitor = _testVisitors[_random.Next(_testVisitors.Count)];
        // Create a copy to avoid modifying the cached object
        var visitorToUpdate = new Visitor
        {
            Id = originalVisitor.Id,
            Name = $"Updated Name {_random.Next(1000)}",
            Email = $"updated{_random.Next(1000)}@example.com",
            Phone = originalVisitor.Phone,
            RegistrationDate = originalVisitor.RegistrationDate.ToUniversalTime()
        };
        await service.UpdateAsync(visitorToUpdate);
    }

    #endregion

    #region DeleteAsync Benchmarks

    [Benchmark]
    public async Task CreateAndDeleteAsync_Dapper()
    {
        var service = _dapperServiceProvider.GetRequiredService<IVisitorService>();

        var visitor = CreateTestVisitor();
        var createVisitorResult = await service.CreateAsync(visitor);
        var createdVisitor = createVisitorResult.Value!;
        
        await service.DeleteAsync(createdVisitor.Id);
    }

    [Benchmark]
    public async Task CreateAndDeleteAsync_EFCore()
    {
        var service = _efServiceProvider.GetRequiredService<IVisitorService>();
        
        var visitor = CreateTestVisitor();
        var createVisitorResult = await service.CreateAsync(visitor);
        var createdVisitor = createVisitorResult.Value!;
        
        await service.DeleteAsync(createdVisitor.Id);
    }

    #endregion

    #region GetAll Benchmarks

    [Benchmark]
    public async Task GetAll_Dapper()
    {
        var service = _dapperServiceProvider.GetRequiredService<IVisitorService>();
        await service.GetAll(null, 100);
    }

    [Benchmark]
    public async Task GetAll_EFCore()
    {
        var service = _efServiceProvider.GetRequiredService<IVisitorService>();
        await service.GetAll(null, 100);
    }

    [Benchmark]
    public async Task GetAllSinceId_Dapper()
    {
        var service = _dapperServiceProvider.GetRequiredService<IVisitorService>();
        var sinceId = _testVisitors[_random.Next(_testVisitors.Count)].Id;
        await service.GetAll(sinceId, 50);
    }

    [Benchmark]
    public async Task GetAllSinceId_EFCore()
    {
        var service = _efServiceProvider.GetRequiredService<IVisitorService>();
        var sinceId = _testVisitors[_random.Next(_testVisitors.Count)].Id;
        await service.GetAll(sinceId, 50);
    }

    #endregion

    #region Helper Methods

    private Visitor CreateTestVisitor()
    {
        var newVisitor = new Visitor
        {
            Name = "Test Visitor",
            Email = "visitor@example.com",
            Phone = $"+7{_random.NextInt64(1000000000, 9999999999)}",
            RegistrationDate = DateTime.UtcNow
        };
        return newVisitor;
    }

    private async Task SeedTestDataAsync()
    {
        const int seedCount = 1000;
        
        var dapperService = _dapperServiceProvider.GetRequiredService<IVisitorService>();
        for (int i = 0; i < seedCount; i++)
        {
            var visitor = new Visitor
            {
                Name = $"Seed Visitor {i}",
                Email = $"seed{i}@example.com",
                Phone = $"+7{_random.NextInt64(1000000000, 9999999999)}",
                RegistrationDate = DateTime.UtcNow
            };

            var result = await dapperService.CreateAsync(visitor);
            if (result.IsSuccess && result.Value != null)
            {
                _testVisitors.Add(result.Value);
            }
        }
    }

    private async Task CleanupTestDataAsync()
    {
        try
        {
            var dapperService = _dapperServiceProvider.GetRequiredService<IVisitorService>();
            foreach (var visitor in _testVisitors)
            {
                try
                {
                    await dapperService.DeleteAsync(visitor.Id);
                }
                catch
                {
                    // Ignore cleanup errors
                }
            }
        }
        catch
        {
            // Ignore cleanup errors
        }
    }

    #endregion
}
