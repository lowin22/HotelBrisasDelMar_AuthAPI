using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AdminRepository : IAdminRepository
    {
        private readonly UserManager<Admin> _userManager;
        private readonly SignInManager<Admin> _signInManager;
        private readonly IConfiguration _configuration;
        public AdminRepository(UserManager<Admin> userManager, SignInManager<Admin> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }


        public async Task<string> AuthWithCredentials(Admin admin, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(admin.UserName, password, false, false);
            if (!result.Succeeded)
                return null; // O manejar error

            // Generar token JWT aquí si es necesario
            return GenerateJwtToken(admin); // Reemplazar con la lógica de generación de token
        }

        public async Task<bool> RegisterNewAdmin(Admin admin, string password)
        {
            admin.UserName = admin.UserName.Trim();
            var existingAdmin = await _userManager.FindByNameAsync(admin.UserName);
            if (existingAdmin != null)
            {
                Console.WriteLine("El admin ya existe");
                return false;
            }
            if (string.IsNullOrEmpty(admin.Email))
            {
                admin.Email = $"{admin.UserName}@example.com"; // Puedes cambiar esto según la lógica de tu aplicación
                admin.EmailConfirmed = true; // Opcional, pero evita validaciones adicionales
            }
            var result = await _userManager.CreateAsync(admin, password);
            if (result.Succeeded)
            {
                Console.WriteLine("Admin registrado correctamente");
                return true;
            }
            return false;
        }

        public async Task<bool> VerifyToken(Token token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);

            try
            {
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // Opcional: evita que los tokens expiren con un margen extra
                };

                tokenHandler.ValidateToken(token.TokenString, parameters, out SecurityToken validatedToken);
                return true; // El token es válido
            }
            catch
            {
                return false; // El token no es válido
            }
        }

        private string GenerateJwtToken(Admin admin)
        {
            var secret = _configuration["Jwt:Secret"];
            if (string.IsNullOrEmpty(secret))
            {
                throw new Exception("La clave JWT no está configurada en appsettings.json");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
    new Claim(JwtRegisteredClaimNames.Sub, admin.UserName),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    new Claim(ClaimTypes.Name, admin.UserName),
    new Claim(ClaimTypes.Email, admin.Email ?? ""),
    new Claim("role", "Admin")
};

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
