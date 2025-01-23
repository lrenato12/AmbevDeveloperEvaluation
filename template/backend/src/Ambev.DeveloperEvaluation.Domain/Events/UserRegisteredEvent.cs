using Ambev.DeveloperEvaluation.Common.EventBroker;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class UserRegisteredEvent : BaseEvent<User>
{
    public UserRegisteredEvent(User user, string message) : base(user, message)
    {
    }
}