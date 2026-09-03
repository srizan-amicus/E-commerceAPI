using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using EcommerceAPI.DTOs;
using EcommerceAPI.Models;
using EcommerceAPI.Repositories.Interfaces;
using EcommerceAPI.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace EcommerceAPI.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IAuthRepository repository,
            IConfiguration configuration,
            ILogger<AuthService> logger)
        {
            _repository = repository;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<int> RegisterAsync(RegisterDto dto)
        {
            var existingCustomer =
                await _repository.GetByEmailAsync(dto.Email);

            if (existingCustomer != null)
            {
                throw new InvalidOperationException(
                    "An account with this email already exists.");
            }

            var customer = new Customer
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(
                    dto.Password),
                Role = "Customer"
            };

            var customerId =
                await _repository.CreateCustomerAsync(customer);

            _logger.LogInformation(
                "Customer {CustomerId} registered successfully",
                customerId);

            return customerId;
        }

        public async Task<LoginResponseDto?> LoginAsync(
            LoginDto dto)
        {
            var customer =
                await _repository.GetByEmailAsync(dto.Email);

            if (customer == null)
            {
                _logger.LogWarning(
                    "Login failed for email {Email}",
                    dto.Email);

                return null;
            }

            var passwordValid =
                BCrypt.Net.BCrypt.Verify(
                    dto.Password,
                    customer.PasswordHash);

            if (!passwordValid)
            {
                _logger.LogWarning(
                    "Invalid password for email {Email}",
                    dto.Email);

                return null;
            }

            var token = GenerateToken(customer);

            _logger.LogInformation(
                "Customer {CustomerId} logged in successfully",
                customer.CustomerId);

            return new LoginResponseDto
            {
                Token = token,
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Role = customer.Role
            };
        }

        private string GenerateToken(Customer customer)
        {
            var jwtKey =
                _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException(
                    "JWT key is not configured.");

            var issuer =
                _configuration["Jwt:Issuer"];

            var audience =
                _configuration["Jwt:Audience"];

            var claims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    customer.CustomerId.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    customer.Name),

                new Claim(
                    ClaimTypes.Email,
                    customer.Email),

                new Claim(
                    ClaimTypes.Role,
                    customer.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}