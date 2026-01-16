using Model.Domain;
using Service.Contracts.Requests;
using Service.Contracts.Responses;

namespace Service.Mapping;

public static class VisitorContractMappingExtensions
{
    public static Visitor ToModel(this VisitorCreateRequest visitorCreateRequest)
    {
        return new Visitor
        {
            Name = visitorCreateRequest.Name,
            Email = visitorCreateRequest.Email,
            Phone = visitorCreateRequest.Phone
        };
    }

    public static VisitorResponse ToResponse(this Visitor visitor)
    {
        return new VisitorResponse(visitor.Id, visitor.Name, visitor.Email, visitor.Phone, visitor.RegistrationDate);
    }

    public static Visitor ToModel(this VisitorUpdateRequest visitor)
    {
        return new Visitor
        {
            Id = visitor.Id,
            Name = visitor.Name,
            Email = visitor.Email,
            Phone = visitor.Phone
        };
    }
}