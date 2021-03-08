using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantAPI.Services
{
    public class DishService : IDishService
    {
        private readonly RestaurantDbContext _context;
        private readonly IMapper _mapper;

        public DishService(RestaurantDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public int Create(int restaurantId, CreateDishDto request)
        {
            var restaurant = _context.Restaurants.FirstOrDefault(x => x.Id == restaurantId);

            if (restaurant is null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            var dishEntity = _mapper.Map<Dish>(request);
            dishEntity.RestaurantId = restaurantId;

            _context.Dishes.Add(dishEntity);

            _context.SaveChanges();

            return dishEntity.Id;
        }

        public DishDto GetById(int restaurantId, int dishId)
        {
            var restaurant = _context.Restaurants.FirstOrDefault(x => x.Id == restaurantId);

            if (restaurant is null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            var dish = _context.Dishes.FirstOrDefault(d => d.Id == dishId);

            if (dish is null)
            {
                throw new NotFoundException("Dish not found");
            }

            if (dish.RestaurantId != restaurantId)
            {
                throw new NotFoundException("Dish not found in restaurant");
            }

            var dishDto = _mapper.Map<DishDto>(dish);

            return dishDto;
        }

        public IEnumerable<DishDto> GetAll(int restaurantId)
        {
            var restaurant = _context
                .Restaurants
                .Include(d => d.DishList)
                .FirstOrDefault(x => x.Id == restaurantId);

            if (restaurant is null)
            {
                throw new NotFoundException("Restaurant not found");
            }
           
            var dishDtos = _mapper.Map<List<DishDto>>(restaurant.DishList);

            return dishDtos;
        }
    }
}