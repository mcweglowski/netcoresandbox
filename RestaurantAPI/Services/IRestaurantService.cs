using System.Collections.Generic;
using System.Security.Claims;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        void Delete(int id);
        RestaurantDto GetById(int id);
        IEnumerable<RestaurantDto> GetAll();
        int Create(CreateRestaurantDto request);
        void Update(int id, UpdateRestaurantDto request);

    }
}