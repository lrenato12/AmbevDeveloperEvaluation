using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Integration.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Application;

/// <summary>
/// Create User Handler Tests
/// </summary>
public class CreateUserHandlerTests
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly CreateUserHandler _handler;

    /// <summary>
    /// Create User Handler Tests
    /// </summary>
    public CreateUserHandlerTests()
    {
        _userService = Substitute.For<IUserService>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateUserHandler(_userService, _mapper);
    }

    /// <summary>
    /// Test - Given valid user data When creating user Then returns success response
    /// </summary>
    [Fact(DisplayName = "Given valid user data When creating user Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        var command = CreateUserHandlerTestData.GenerateValidCommand();
        var user = CreateUserHandlerTestData.CreateUserFromCommand(command);

        var result = new CreateUserResult
        {
            Id = user.Id,
        };

        _mapper.Map<User>(command).Returns(user);
        _mapper.Map<CreateUserResult>(user).Returns(result);

        _userService.CreateUserAsync(Arg.Any<User>(), Arg.Any<CancellationToken>()).Returns(user);

        var createUserResult = await _handler.Handle(command, CancellationToken.None);

        createUserResult.Should().NotBeNull();
        createUserResult.Id.Should().Be(user.Id);
    }

    /// <summary>
    /// Test - Given invalid user data When creating user Then throws validation exception
    /// </summary>
    [Fact(DisplayName = "Given invalid user data When creating user Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        var command = new CreateUserCommand(); // Empty command will fail validation

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Test - Given user creation request When handling Then password is hashed
    /// </summary>
    [Fact(DisplayName = "Given user creation request When handling Then password is hashed")]
    public async Task Handle_ValidRequest_HashesPassword()
    {
        var command = CreateUserHandlerTestData.GenerateValidCommand();
        const string hashedPassword = "h@shedPassw0rd";
        var user = CreateUserHandlerTestData.CreateUserFromCommand(command);

        _mapper.Map<User>(command).Returns(user);
        _userService.CreateUserAsync(Arg.Any<User>(), Arg.Any<CancellationToken>()).Returns(user);

        await _handler.Handle(command, CancellationToken.None);

        await _userService.Received(1).CreateUserAsync(
            Arg.Is<User>(u => u.Password == hashedPassword),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Test - Given valid command When handling Then maps command to user entity
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then maps command to user entity")]
    public async Task Handle_ValidRequest_MapsCommandToUser()
    {
        var command = CreateUserHandlerTestData.GenerateValidCommand();
        var user = CreateUserHandlerTestData.CreateUserFromCommand(command);

        _mapper.Map<User>(command).Returns(user);
        _userService.CreateUserAsync(Arg.Any<User>(), Arg.Any<CancellationToken>()).Returns(user);

        await _handler.Handle(command, CancellationToken.None);

        _mapper.Received(1).Map<User>(Arg.Is<CreateUserCommand>(c =>
            c.Username == command.Username &&
            c.Email == command.Email &&
            c.Phone == command.Phone &&
            c.Status == command.Status &&
            c.Role == command.Role));
    }
}