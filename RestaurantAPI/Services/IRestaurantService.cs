using System.Collections.Generic;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        void Delete(int id);
        RestaurantDto GetById(int id);
        IEnumerable<RestaurantDto> GetAll();
        int CreateRestaurant(CreateRestaurantDto request);
        void PutRestaurant(int id, UpdateRestaurantDto request);

    }
}