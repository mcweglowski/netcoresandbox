using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Entities;
using RestaurantAPI.Services;

namespace RestaurantAPI.Authorization
{
    public class MinimumRestaurantsCreatedHandler : AuthorizationHandler<MinimumRestaurantsCreated>
    {
        private readonly ILogger<MinimumRestaurantsCreatedHandler> _logger;
        private readonly RestaurantDbContext _dbContext;

        public MinimumRestaurantsCreatedHandler(ILogger<MinimumRestaurantsCreatedHandler> logger, RestaurantDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumRestaurantsCreated requirement)
        {
            var id = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var userEmail = context.User.FindFirst(c => c.Type == ClaimTypes.Name);

            var createdCount = _dbContext
                .Restaurants
                .Where(x => x.CreatedById == id)
                .Count();

            _logger.LogInformation($"User: {userEmail} created [{createdCount}] restaurants");

            if (createdCount >= requirement.RestaurantCount)
            {
                _logger.LogInformation("Authorization succedde");
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogInformation("Authorization failed");
            }

            return Task.CompletedTask;
        }
    }
}