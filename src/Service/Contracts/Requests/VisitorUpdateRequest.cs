using System.Runtime.Serialization;

namespace Service.Contracts.Requests;

[DataContract]
public sealed record VisitorUpdateRequest(
    [property: DataMember(Name = "id")] int Id,
    [property: DataMember(Name = "name")] string Name,
    [property: DataMember(Name = "email")] string Email,
    [property: DataMember(Name = "phone")] string Phone);