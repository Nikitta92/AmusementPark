namespace Model.Data;

public static class QueryStore
{
    public const string InsertVisitorQuery =
        """
        INSERT INTO "AmusementPark"."Visitor"
        ("Name", "Email", "Phone", "RegistrationDate") VALUES (@Name, @Email, @Phone, @RegistrationDate)
        RETURNING *;
        """;

    public const string SelectVisitorById = """SELECT * FROM "AmusementPark"."Visitor" WHERE "Id" = @Id""";

    public const string UpdateVisitorById =
        """
        UPDATE "AmusementPark"."Visitor" 
        SET 
            "Name" = @Name, 
            "Email" = @Email,
            "Phone" = @Phone
        WHERE "Id" = @Id
        RETURNING *;
        """;

    public const string DeleteVisitorById =
        """
        DELETE FROM "AmusementPark"."Visitor" 
        WHERE "Id" = @Id;
        """;

    public const string SelectAllVisitors =
        """
        SELECT * FROM "AmusementPark"."Visitor" ORDER BY "Id";
        """;
}