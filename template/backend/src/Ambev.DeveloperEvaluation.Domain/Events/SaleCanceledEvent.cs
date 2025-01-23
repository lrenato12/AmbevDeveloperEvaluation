using Ambev.DeveloperEvaluation.Common.EventBroker;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleCanceledEvent : BaseEvent<Sale>
{
    public SaleCanceledEvent(Sale data, string message) : base(data, message)
    {
    }
}