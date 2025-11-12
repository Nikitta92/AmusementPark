namespace Model.Domain;

public sealed class CardTransaction
{
    public int Id { get; init; }
    public int ParkCardId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public TransactionType Type { get; set; }
    public string? Comment { get; set; }
}