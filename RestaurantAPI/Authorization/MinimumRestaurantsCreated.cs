using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class MinimumRestaurantsCreated : IAuthorizationRequirement
    {
        public int RestaurantCount { get; }

        public MinimumRestaurantsCreated(int restaurantCount)
        {
            RestaurantCount = restaurantCount;
        }
    }
}