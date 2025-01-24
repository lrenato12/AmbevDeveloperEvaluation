using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;

public class GetCustomerCommand : IRequest<GetCustomerResult>
{
    /// <summary>
    /// The unique identifier of the customer to retrieve
    /// </summary>
    public Guid Id { get; }

    public GetCustomerCommand(Guid id)
    {
        Id = id;
    }
}