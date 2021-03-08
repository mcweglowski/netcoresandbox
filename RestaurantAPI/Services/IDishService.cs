using System.Collections.Generic;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IDishService
    {
        int Create(int restaurantId, CreateDishDto request);
        DishDto GetById(int restaurantId, int dishId);
        IEnumerable<DishDto> GetAll(int restaurantId);
    }
}