using FluentMigrator;

namespace Migrations.Migrations;

[TimestampedMigration(2025, 11, 19, 12, 00)]
public class AddVisitorTable : Migration
{
    private const string VisitorTableName = "Visitor";
    private const string AmusementParkSchemaName = "AmusementPark";
    
    public override void Up()
    {
        if (Schema.Table(VisitorTableName).Exists())
            return;
        
        Create.Table(VisitorTableName)
            .InSchema(AmusementParkSchemaName)
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Name").AsString().NotNullable()
            .WithColumn("Email").AsString().NotNullable()
            .WithColumn("Phone").AsString().NotNullable()
            .WithColumn("RegistrationDate").AsDateTime().NotNullable();
    }

    public override void Down()
    {
        if (!Schema.Table(VisitorTableName).Exists())
            return;
        
        Delete.Table(VisitorTableName).InSchema(AmusementParkSchemaName);
    }
}