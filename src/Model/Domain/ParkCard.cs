namespace Model.Domain;

public sealed class ParkCard
{
    public string Id { get; init; } = null!;
    public int VisitorId { get; set; }
    public decimal Balance { get; set; }
    public DateTime IssueDate { get; set; }
    public bool IsActive { get; set; }
    public int CardTypeId { get; set; }
}