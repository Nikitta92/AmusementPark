using FluentMigrator.Builders.Create.ForeignKey;
using FluentMigrator.Builders.Create.Table;
using Migration = FluentMigrator.Migration;

namespace Migrations.Migrations;

public abstract class BaseMigration : Migration
{
    protected void CreateTableIfNotExists(
        string tableName,
        string schemaName,
        Action<ICreateTableWithColumnSyntax> tableDefinition)
    {
        if (Schema.Schema(schemaName).Table(tableName).Exists())
            return;

        var tableBuilder = Create.Table(tableName).InSchema(schemaName);
        tableDefinition(tableBuilder);
    }

    protected void CreateForeignKeyIfNotExists(
        string schemaName,
        string tableName,
        string foreignKeyName,
        Action<ICreateForeignKeyFromTableSyntax> tableDefinition)
    {
        if (Schema.Schema(schemaName).Table(tableName).Constraint(foreignKeyName).Exists())
            return;

        var constraint = Create.ForeignKey(foreignKeyName);
        tableDefinition(constraint);
    }
}

