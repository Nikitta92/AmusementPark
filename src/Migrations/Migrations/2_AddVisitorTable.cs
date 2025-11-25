using FluentMigrator;

namespace Migrations.Migrations;

[TimestampedMigration(2025, 11, 19, 12, 00)]
public class AddVisitorTable : BaseMigration
{
    private const string VisitorTableName = "Visitor";
    private const string AmusementParkSchemaName = "AmusementPark";
    
    public override void Up()
    {
        CreateTableIfNotExists(VisitorTableName, AmusementParkSchemaName, table => table
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Name").AsString().NotNullable()
            .WithColumn("Email").AsString().NotNullable()
            .WithColumn("Phone").AsString().NotNullable()
            .WithColumn("RegistrationDate").AsDateTime().NotNullable());
    }

    public override void Down()
    {
        if (!Schema.Table(VisitorTableName).Exists())
            return;
        
        Delete.Table(VisitorTableName).InSchema(AmusementParkSchemaName);
    }
}