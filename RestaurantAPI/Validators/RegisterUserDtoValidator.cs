using System.Linq;
using FluentValidation;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(RestaurantDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .MinimumLength(6);

            RuleFor(x => x.ConfirmPassword)
                .Equal(y => y.Password);

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.Users.Any(x => x.Email == value);

                    if (true == emailInUse)
                    {
                        context.AddFailure("Email", "Email is taken");
                    }
                });
        }
    }
}