using System.Collections.Generic;
using System.Linq;
using RestaurantAPI.Entities;

namespace RestaurantAPI
{
    public class DatabaseSeeder
    {
        private readonly RestaurantDbContext _dbContext;

        private DatabaseSeeder()
        {

        }

        public DatabaseSeeder(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (false == _dbContext.Database.CanConnect())
            {
                return;
            }

            if (false == _dbContext.Roles.Any())
            {
                var roles = GetRoles();
                _dbContext.Roles.AddRange(roles);
                _dbContext.SaveChanges();
            }

            if (false == _dbContext.Restaurants.Any())
            {
                var restaurants = GetRestaurants();
                _dbContext.Restaurants.AddRange(restaurants);
                _dbContext.SaveChanges();
            }
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name = "KFC",
                    Category = "Fast food",
                    Description = "KFC is an American fast food restaurant.",
                    ContactEmail = "contact@kfc.com",
                    HasDelivery = true,
                    DishList = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Nashville Hot Chicken",
                            Price = 10.30M,
                        },
                        new Dish()
                        {
                            Name = "Chicken Nuggets",
                            Price = 5.30M,
                        }
                    },
                    Address = new Address()
                    {
                        City = "Rzeszów",
                        Street = "Witosa 11",
                        PostalCode = "35-114",
                    }
                },
                new Restaurant()
                {
                    Name = "McDonalds",
                    Category = "Fast food",
                    Description = "McDonalds is an American fast food restaurant.",
                    ContactEmail = "contact@mcdonalds.com",
                    HasDelivery = true,
                    DishList = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Cheeseburger",
                            Price =4M,
                        },
                        new Dish()
                        {
                            Name = "BigMac",
                            Price = 11.50M,
                        }
                    },
                    Address = new Address()
                    {
                        City = "Rzeszów",
                        Street = "Powstańców Warszawy 44B",
                        PostalCode = "35-100",
                    }
                },
            };

            return restaurants;
        }

        public IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Manager"
                },
                new Role()
                {
                    Name = "Admin"
                }
            };

            return roles;
        }
    }
}