using FluentValidation;
using Model.Data;
using Model.Data.Repository;
using Model.Domain;
using Model.Results;

namespace Model.Services;

public class VisitorService(
    IValidator<Visitor> visitorValidator,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork) : IVisitorService
{
    public Task<Visitor?> TryGetAsync(int id)
    {
        return unitOfWork.ExecuteWithoutCommitAsync(repositories =>
            repositories.VisitorRepository.TryGetAsync(id));
    }

    public async Task<Result<Visitor>> CreateAsync(Visitor visitor)
    {
        var validationResult = await visitorValidator.ValidateAsync(visitor);

        if (!validationResult.IsValid)
        {
            return Result<Visitor>.Failure(validationResult.ToDictionary());
        }

        visitor.RegistrationDate = dateTimeProvider.GetUtcNow();

        var createdVisitor = await unitOfWork.ExecuteAndCommitAsync(async repositories =>
            await repositories.VisitorRepository.CreateAsync(visitor));

        return Result<Visitor>.Success(createdVisitor);
    }

    public async Task<Result<Visitor>> UpdateAsync(Visitor visitor)
    {
        var validationResult = await visitorValidator.ValidateAsync(visitor);

        if (!validationResult.IsValid)
        {
            return Result<Visitor>.Failure(validationResult.ToDictionary());
        }

        var updated = await unitOfWork.ExecuteAndCommitAsync(async repositories =>
            await repositories.VisitorRepository.UpdateAsync(visitor));

        return Result<Visitor>.Success(updated);
    }

    public Task DeleteAsync(int id)
    {
        return unitOfWork.ExecuteAndCommitAsync(x => x.VisitorRepository.DeleteAsync(id));
    }

    public Task<IReadOnlyCollection<Visitor>> GetAll()
    {
        return unitOfWork.ExecuteWithoutCommitAsync(repositories =>
            repositories.VisitorRepository.GetAllAsync());
    }
}