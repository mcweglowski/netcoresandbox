using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly RestaurantDbContext _context;
        private readonly IPasswordHasher<User> _hasher;
        private readonly AuthenticationSettings _authentication;

        public AccountService(RestaurantDbContext context, IPasswordHasher<User> hasher, AuthenticationSettings authentication)
        {
            _context = context;
            _hasher = hasher;
            _authentication = authentication;
        }

        public string GenerateJwt(LoginDto dto)
        {
            var user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(x => x.Email == dto.Email);

            if (user is null)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (PasswordVerificationResult.Failed == result)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyyy-MM-dd")),
                new Claim("Nationality", user.Nationality)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authentication.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authentication.JwtExpireDays);

            var token = new JwtSecurityToken(_authentication.JwtIssuer,
                _authentication.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }

        public void RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Nationality = dto.Nationality,
                RoleId = dto.RoleId
            };

            var hashedPassword = _hasher.HashPassword(newUser, dto.Password);
            
            newUser.PasswordHash = hashedPassword;
            
            _context.Users.Add(newUser);

            _context.SaveChanges();
        }
    }
}