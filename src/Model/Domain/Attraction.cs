namespace Model.Domain;

public sealed class Attraction
{
    public int Id { get; init; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public ushort MinAge { get; set; }
}