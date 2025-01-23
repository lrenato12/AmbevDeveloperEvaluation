using Ambev.DeveloperEvaluation.Common.EventBroker;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleCreatedEvent : BaseEvent<Sale>
{
    public SaleCreatedEvent(Sale sale, string message) : base(sale, message)
    {
    }
}