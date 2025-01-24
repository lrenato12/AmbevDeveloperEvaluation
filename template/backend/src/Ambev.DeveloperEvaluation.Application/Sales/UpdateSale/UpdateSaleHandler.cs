using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleManagerService _saleService;
    private readonly IMapper _mapper;

    public UpdateSaleHandler(
        ISaleRepository saleRepository,
        ISaleManagerService saleService,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _saleService = saleService;
        _mapper = mapper;
    }

    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetWithItemsByIdAsync(request.SaleId, cancellationToken);

        if (sale is null)
            throw new KeyNotFoundException("Sale not found");

        var saleItems = _mapper.Map<List<SaleItem>>(request.Items);
        sale.Update(saleItems, request.Cancel);

        var updatedSale = await _saleService.UpdateSale(sale, cancellationToken);
        var result = _mapper.Map<UpdateSaleResult>(updatedSale);

        return result;
    }
}