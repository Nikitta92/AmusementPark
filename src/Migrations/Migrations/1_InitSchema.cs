using FluentMigrator;

namespace Migrations.Migrations;

[TimestampedMigration(2025, 11, 15, 12, 00)]
public class InitSchema : Migration
{
    public override void Up()
    {
        Create.Schema("AmusementPark");
    }

    public override void Down()
    {
        Delete.Schema("AmusementPark");
    }
}