using FluentValidation;

namespace Batch.Models
{
    public class StoreOrderValidator : AbstractValidator<StoreOrder>
    {
        public StoreOrderValidator()
        {
            RuleFor(x => x.ORDER_ID).NotEmpty().MaximumLength(20);//UNIQUE
            RuleFor(x => x.ORDER_DATE).NotEmpty();
            RuleFor(x => x.SHIP_DATE).NotEmpty();
            RuleFor(x => x.SHIP_MODE).MaximumLength(20);
            RuleFor(x => x.QUANTITY).NotEmpty();
            RuleFor(x => x.DISCOUNT).ScalePrecision(2,3);
            RuleFor(x => x.PROFIT).NotEmpty().ScalePrecision(2,6);
            RuleFor(x => x.PRODUCT_ID).NotEmpty().MaximumLength(20);//UNIQUE
            RuleFor(x => x.CUSTOMER_NAME).NotEmpty().MaximumLength(255);
            RuleFor(x => x.CATEGORY).NotEmpty().MaximumLength(255);
            RuleFor(x => x.CUSTOMER_ID).NotEmpty().MaximumLength(20);//UNIQUE
            RuleFor(x => x.PRODUCT_NAME).MaximumLength(255);
        }

        
    }
}