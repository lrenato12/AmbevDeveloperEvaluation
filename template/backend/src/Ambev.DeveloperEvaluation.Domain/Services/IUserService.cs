using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Services;

public interface IUserService
{
    Task<User?> CreateUserAsync(User user, CancellationToken cancellationToken);
}