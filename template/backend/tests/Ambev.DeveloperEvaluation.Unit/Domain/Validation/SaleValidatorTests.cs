using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation
{
    /// <summary>
    /// Sale Validator Tests
    /// </summary>
    public class SaleValidatorTests
    {
        private readonly SaleValidator _validator;

        public SaleValidatorTests()
        {
            _validator = new SaleValidator();
        }

        /// <summary>
        /// Test - Valid sale should pass all validation rules.
        /// </summary>
        [Fact(DisplayName = "Valid sale should pass all validation rules")]
        public void Given_ValidSale_When_Validated_Then_ShouldNotHaveErrors()
        {
            var sale = SaleTestData.GenerateSale();

            var result = _validator.TestValidate(sale);

            result.ShouldNotHaveAnyValidationErrors();
        }

        /// <summary>
        /// Test - Invalid sale number should fail validation
        /// </summary>
        [Fact(DisplayName = "Invalid sale number should fail validation")]
        public void Given_InvalidNumber_When_Validated_Then_ShouldHaveError()
        {
            var sale = SaleTestData.GenerateSale();
            sale.Number = 0;

            var result = _validator.TestValidate(sale);

            result.ShouldHaveValidationErrorFor(x => x.Number);
        }

        /// <summary>
        /// Tests - Invalid sale items should fail validation
        /// </summary>
        [Fact(DisplayName = "Invalid sale items should fail validation")]
        public void Given_InvalidListOfItems_When_Validated_Then_ShouldHaveError()
        {
            var sale = SaleTestData.GenerateSale();
            sale.Items = new List<SaleItem>();

            var result = _validator.TestValidate(sale);

            result.ShouldHaveValidationErrorFor(x => x.Items);
        }

        /// <summary>
        /// Test - Invalid customerId should fail validation
        /// </summary>
        [Fact(DisplayName = "Invalid customerId should fail validation")]
        public void Given_InvalidCustomerId_When_Validated_Then_ShouldHaveError()
        {
            var sale = SaleTestData.GenerateSale();
            sale.CustomerId = Guid.Empty;

            var result = _validator.TestValidate(sale);

            result.ShouldHaveValidationErrorFor(x => x.CustomerId);
        }

        /// <summary>
        /// Test - Invalid branchId should fail validation
        /// </summary>
        [Fact(DisplayName = "Invalid branchId should fail validation")]
        public void Given_InvalidBranchId_When_Validated_Then_ShouldHaveError()
        {
            var sale = SaleTestData.GenerateSale();
            sale.BranchId = Guid.Empty;

            var result = _validator.TestValidate(sale);

            result.ShouldHaveValidationErrorFor(x => x.BranchId);
        }
    }
}