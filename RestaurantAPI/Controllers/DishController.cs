using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.DishController
{
    [Route("api/restaurant/{restaurantId}/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _service;

        public DishController(IDishService service)
        {
            _service = service;
        }

        [HttpPost]
        public ActionResult Post([FromRoute]int restaurantId, [FromBody]CreateDishDto request)
        {
            var newDishId = _service.Create(restaurantId, request);

            return Created($"api/restaurant/{restaurantId}/dish/{newDishId}", null);
        }

        [HttpGet("{dishId}")]
        public ActionResult<DishDto> Get([FromRoute]int restaurantId, [FromRoute]int dishId)
        {
            var dish = _service.GetById(restaurantId, dishId);

            return Ok(dish);
        }

        [HttpGet]
        public ActionResult<IEnumerable<DishDto>> Get([FromRoute]int restaurantId)
        {
            var result = _service.GetAll(restaurantId);

            return Ok(result);
        }
    }
}