using System.Runtime.Serialization;

namespace Service.Contracts.Requests;

[DataContract]
public sealed record VisitorCreateRequest(
    [property: DataMember(Name = "name")] string Name,
    [property: DataMember(Name = "email")] string Email,
    [property: DataMember(Name = "phone")] string Phone);