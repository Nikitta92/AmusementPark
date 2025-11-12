namespace Model.Domain;

public sealed class CardType
{
    public int Id { get; init; }
    public string Name { get; set; } = null!;
    public decimal Discount { get; set; }
}