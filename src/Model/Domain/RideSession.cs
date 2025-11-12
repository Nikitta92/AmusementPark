namespace Model.Domain;

public sealed class RideSession
{
    public int Id { get; set; }
    public int ParkCardId { get; set; }
    public int AttractionId { get; set; }
    public DateTime RideTime { get; set; }
    public decimal PricePaid { get; set; }
}