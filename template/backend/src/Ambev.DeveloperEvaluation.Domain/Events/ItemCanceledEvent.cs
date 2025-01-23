using Ambev.DeveloperEvaluation.Common.EventBroker;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class ItemCanceledEvent : BaseEvent<SaleItem>
{
    public ItemCanceledEvent(SaleItem data, string message) : base(data, message)
    {
    }
}