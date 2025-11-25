using FluentMigrator;

namespace Migrations.Migrations;

[TimestampedMigration(2025, 11, 22, 12, 00)]
public class AddRemainingModels : BaseMigration
{
    private const string AmusementParkSchemaName = "AmusementPark";
    private const string CardTypeTableName = "CardType";
    private const string ParkCardTableName = "ParkCard";
    private const string CardTransactionTableName = "CardTransaction";
    private const string RideSessionTableName = "RideSession";
    private const string VisitorTableName = "Visitor";
    
    public override void Up()
    {
        CreateTableIfNotExists(CardTypeTableName, AmusementParkSchemaName, table => table
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Name").AsString().NotNullable()
            .WithColumn("Discount").AsDecimal(18, 2).NotNullable());

        CreateTableIfNotExists(ParkCardTableName, AmusementParkSchemaName, table => table
            .WithColumn("Id").AsString().PrimaryKey()
            .WithColumn("VisitorId").AsInt32().NotNullable()
            .WithColumn("Balance").AsDecimal(18, 2).NotNullable()
            .WithColumn("IssueDate").AsDateTime().NotNullable()
            .WithColumn("IsActive").AsBoolean().NotNullable()
            .WithColumn("CardTypeId").AsInt32().NotNullable());
        
        // Foreign key to Visitor
        CreateForeignKeyIfNotExists(
            AmusementParkSchemaName,
            ParkCardTableName,
            "FK_ParkCard_Visitor",
            foreignKey => foreignKey.FromTable(ParkCardTableName).InSchema(AmusementParkSchemaName)
                .ForeignColumn("VisitorId")
                .ToTable(VisitorTableName).InSchema(AmusementParkSchemaName).PrimaryColumn("Id"));
        
        // Foreign key to CardType
        CreateForeignKeyIfNotExists(
            AmusementParkSchemaName,
            ParkCardTableName,
            "FK_ParkCard_CardType",
            foreignKey => foreignKey.FromTable(ParkCardTableName).InSchema(AmusementParkSchemaName)
                .ForeignColumn("CardTypeId")
                .ToTable(CardTypeTableName).InSchema(AmusementParkSchemaName).PrimaryColumn("Id"));

        CreateTableIfNotExists(CardTransactionTableName, AmusementParkSchemaName, table => table
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("ParkCardId").AsString().NotNullable()
            .WithColumn("Amount").AsDecimal(18, 2).NotNullable()
            .WithColumn("TransactionDate").AsDateTime().NotNullable()
            .WithColumn("Type").AsInt32().NotNullable()
            .WithColumn("Comment").AsString().Nullable());
        
        // Foreign key to ParkCard
        CreateForeignKeyIfNotExists(
            AmusementParkSchemaName,
            CardTransactionTableName,
            "FK_CardTransaction_ParkCard",
            foreignKey => foreignKey.FromTable(CardTransactionTableName).InSchema(AmusementParkSchemaName)
                .ForeignColumn("ParkCardId")
                .ToTable(ParkCardTableName).InSchema(AmusementParkSchemaName).PrimaryColumn("Id"));

        CreateTableIfNotExists(RideSessionTableName, AmusementParkSchemaName, table => table
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("ParkCardId").AsString().NotNullable()
            .WithColumn("AttractionId").AsInt32().NotNullable()
            .WithColumn("RideTime").AsDateTime().NotNullable()
            .WithColumn("PricePaid").AsDecimal(18, 2).NotNullable());
        
        // Foreign key to ParkCard
        CreateForeignKeyIfNotExists(
            AmusementParkSchemaName,
            RideSessionTableName,
            "FK_RideSession_ParkCard",
            foreignKey => foreignKey.FromTable(RideSessionTableName).InSchema(AmusementParkSchemaName)
                .ForeignColumn("ParkCardId")
                .ToTable(ParkCardTableName).InSchema(AmusementParkSchemaName).PrimaryColumn("Id"));
    }

    public override void Down()
    {
        if (Schema.Schema(AmusementParkSchemaName).Table(RideSessionTableName).Exists())
        {
            Delete.ForeignKey("FK_RideSession_ParkCard")
                .OnTable(RideSessionTableName).InSchema(AmusementParkSchemaName);
            Delete.Table(RideSessionTableName).InSchema(AmusementParkSchemaName);
        }

        if (Schema.Schema(AmusementParkSchemaName).Table(CardTransactionTableName).Exists())
        {
            Delete.ForeignKey("FK_CardTransaction_ParkCard")
                .OnTable(CardTransactionTableName).InSchema(AmusementParkSchemaName);
            Delete.Table(CardTransactionTableName).InSchema(AmusementParkSchemaName);
        }

        if (Schema.Schema(AmusementParkSchemaName).Table(ParkCardTableName).Exists())
        {
            Delete.ForeignKey("FK_ParkCard_Visitor")
                .OnTable(ParkCardTableName).InSchema(AmusementParkSchemaName);
            Delete.ForeignKey("FK_ParkCard_CardType")
                .OnTable(ParkCardTableName).InSchema(AmusementParkSchemaName);
            Delete.Table(ParkCardTableName).InSchema(AmusementParkSchemaName);
        }

        if (Schema.Schema(AmusementParkSchemaName).Table(CardTypeTableName).Exists())
        {
            Delete.Table(CardTypeTableName).InSchema(AmusementParkSchemaName);
        }
    }
}

