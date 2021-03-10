using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using RestaurantAPI.Authorization;

namespace RestaurantAPI.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IAuthorizationService _authorizationService;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger, 
            IAuthorizationService autorizationService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = autorizationService;
        }

        public void Delete(int id, ClaimsPrincipal user)
        {
            _logger.LogWarning($"Restaurant with id: {id} DELETE - ACTION INVOKE");

            var restaurant = _dbContext.Restaurants.FirstOrDefault(x => x.Id == id);

            if (restaurant is null)
            {
                throw new NotFoundException("Restaurant not found");
            }
            
            var authorizationResult = _authorizationService.AuthorizeAsync(user, restaurant, 
                new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();
        }

        public RestaurantDto GetById(int id)
        {
            var restaurant = _dbContext
                .Restaurants
                .Include(x => x.Address)
                .Include(x => x.DishList)
                .FirstOrDefault(x => x.Id == id);

            if (restaurant is null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

            return restaurantDto;
        }

        public IEnumerable<RestaurantDto> GetAll()
        {
            var restaurants = _dbContext
                .Restaurants
                .Include(x => x.Address)
                .Include(x => x.DishList)
                .ToList();

            var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);

            return restaurantsDto;            
        }

        public int CreateRestaurant(CreateRestaurantDto request, int userId)
        {
           Restaurant restaurant = _mapper.Map<Restaurant>(request);
           restaurant.CreatedById = userId;

            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();

            return restaurant.Id;
        }

        public void PutRestaurant(int id, UpdateRestaurantDto request, ClaimsPrincipal user)
        {
            var restaurant = _dbContext.Restaurants.FirstOrDefault(x => x.Id == id);

            if (restaurant is null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            var authorizationResult = _authorizationService.AuthorizeAsync(user, restaurant, 
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            restaurant.Name = request.Name;
            restaurant.Description = request.Description;
            restaurant.HasDelivery = request.HasDelivery;

            _dbContext.SaveChanges();
        }
    }
}