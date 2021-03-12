using System.Linq;
using FluentValidation;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Validators
{
    public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
    {
        private int[] allowedPageSizes = new[] { 5, 10, 5 };
        private string[] allowedSortByColumnNames = 
            { nameof(Restaurant.Name), nameof(Restaurant.Category), nameof(Restaurant.Description) };

        public RestaurantQueryValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if (false == allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must be in [{string.Join(",", allowedPageSizes)}]");
                }
            });
            RuleFor(r => r.SortBy).Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                .WithMessage($"Sortby is optional or must be in [{string.Join(",", allowedSortByColumnNames)}]");
        }
    }
}