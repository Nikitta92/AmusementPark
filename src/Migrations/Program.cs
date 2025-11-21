using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.Development.json")
    .Build();

var connectionString = GetConnectionString();
var executingAssembly = Assembly.GetExecutingAssembly();

var serviceProvider = new ServiceCollection()
    .AddFluentMigratorCore()
    .ConfigureRunner(builder =>
        builder
            .AddPostgres()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(executingAssembly))
    .AddLogging(lb => lb.AddFluentMigratorConsole())
    .Configure<FluentMigratorLoggerOptions>(options =>
        {
            options.ShowSql = true;
            options.ShowElapsedTime = true;
        }
    )
    .BuildServiceProvider();

var migrator = serviceProvider.GetRequiredService<IMigrationRunner>();

migrator.MigrateUp();

string GetConnectionString()
{
    var connection = configuration.GetConnectionString("AmusementParkDB");

    if (connection is null)
        throw new InvalidOperationException("Connection string is null.");

    return connection;
}