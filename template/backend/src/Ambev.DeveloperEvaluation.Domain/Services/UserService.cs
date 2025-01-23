using Ambev.DeveloperEvaluation.Common.EventBroker;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Domain.Services;

/// <summary>
/// Handles operations User entity, triggering events that could be used in other services
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly Publisher _publisher;

    public UserService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IEventBroker eventBroker)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _publisher = new Publisher(eventBroker);
    }

    /// <summary>
    /// Create a new user verifying if it not exists on the system.
    /// After the operation, trigger an event that could be used on other services
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<User?> CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(user.Email, cancellationToken);
        if (existingUser != null)
            throw new InvalidOperationException($"User with email {user.Email} already exists");

        user.Password = _passwordHasher.HashPassword(user.Password);

        var createdUser = await _userRepository.CreateAsync(user, cancellationToken);

        if (createdUser != null)
            _publisher.PublishMessage(new UserRegisteredEvent(createdUser, "User Created"));

        return createdUser;
    }
}