using FluentMigrator;

namespace Migrations.Migrations;

[TimestampedMigration(2025, 11, 25, 12, 00)]
public class AddAttractionTable : BaseMigration
{
    private const string AmusementParkSchemaName = "AmusementPark";
    private const string AttractionTableName = "Attraction";
    private const string RideSessionTableName = "RideSession";

    public override void Up()
    {
        CreateTableIfNotExists(AttractionTableName, AmusementParkSchemaName, table => table
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Name").AsString().NotNullable()
            .WithColumn("Description").AsString().Nullable()
            .WithColumn("Price").AsDecimal(18, 2).NotNullable()
            .WithColumn("MinAge").AsInt16().NotNullable());

        CreateForeignKeyIfNotExists(
            AmusementParkSchemaName,
            RideSessionTableName,
            "FK_RideSession_Attraction",
            foreignKey => foreignKey.FromTable(RideSessionTableName).InSchema(AmusementParkSchemaName)
                .ForeignColumn("AttractionId")
                .ToTable(AttractionTableName).InSchema(AmusementParkSchemaName).PrimaryColumn("Id"));
    }

    public override void Down()
    {
        if (Schema.Schema(AmusementParkSchemaName).Table(RideSessionTableName).Exists())
        {
            Delete.ForeignKey("FK_RideSession_Attraction")
                .OnTable(RideSessionTableName).InSchema(AmusementParkSchemaName);
        }

        if (!Schema.Schema(AmusementParkSchemaName).Table(AttractionTableName).Exists())
            return;

        Delete.Table(AttractionTableName).InSchema(AmusementParkSchemaName);
    }
}


