using Ambev.DeveloperEvaluation.Common.EventBroker;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleModifiedEvent : BaseEvent<Sale>
{
    public SaleModifiedEvent(Sale sale, string message) : base(sale, message)
    {
    }
}