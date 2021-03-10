using System.Collections.Generic;
using System.Security.Claims;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        void Delete(int id, ClaimsPrincipal user);
        RestaurantDto GetById(int id);
        IEnumerable<RestaurantDto> GetAll();
        int CreateRestaurant(CreateRestaurantDto request, int userId);
        void PutRestaurant(int id, UpdateRestaurantDto request, ClaimsPrincipal user);

    }
}