using FluentValidation;
using Model.Data;
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

    public async Task<bool> DeleteAsync(int id)
    {
        var isDeleted = await unitOfWork.ExecuteAndCommitAsync(x => x.VisitorRepository.DeleteAsync(id));
        return isDeleted;
    }

    public Task<IReadOnlyCollection<Visitor>> GetAll()
    {
        return unitOfWork.ExecuteWithoutCommitAsync(repositories =>
            repositories.VisitorRepository.GetAllAsync());
    }
}