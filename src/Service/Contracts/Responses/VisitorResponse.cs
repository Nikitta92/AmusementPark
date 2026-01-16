using System.Runtime.Serialization;

namespace Service.Contracts.Responses;

[DataContract]
public sealed record VisitorResponse(
    [property: DataMember(Name = "id")] int Id,
    [property: DataMember(Name = "name")] string Name,
    [property: DataMember(Name = "email")] string Email,
    [property: DataMember(Name = "phone")] string Phone,
    [property: DataMember(Name = "registrationDate")] DateTime RegistrationDate);