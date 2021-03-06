
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantDbContext _dbContext;

        public RestaurantController(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll()
        {
            var restaurants = _dbContext
                .Restaurants
                .ToList();

            var restaurantsDto = restaurants
                .Select(x => new RestaurantDto()
                {
                    Category = x.Category,
                    City = x.Address?.City,
                    Description = x.Description,
                    DishList = x.DishList?.Select(x => new DishDto()
                    {
                        Description = x.Description,
                        Id = x.Id,
                        Name = x.Name,
                        Price = x.Price
                    }).ToList(),
                    HasDelivery = x.HasDelivery,
                    Id = x.Id,
                    Name = x.Name,
                    PostalCode = x.Address?.PostalCode,
                    Street = x.Address?.Street
                });

            return Ok(restaurantsDto);
        }

        [HttpGet("{id}")]
        public ActionResult<Restaurant> GetAction([FromRoute]int id)
        {
            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(x => x.Id == id);

            if (restaurant is null)
            {
                return NotFound();
            }

            return Ok(restaurant);
        }
    }
}