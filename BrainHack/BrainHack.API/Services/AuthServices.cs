using BrainHack.API.DTOs;
using BrainHack.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BrainHack.API.Services
{
    public class AuthService
    {
        private readonly Supabase.Client _supabase;
        private readonly IConfiguration _config;

        public AuthService(Supabase.Client supabase, IConfiguration config)
        {
            _supabase = supabase;
            _config = config;
        }

        public async Task<AuthResponseDTO?> Register(RegisterDTO dto)
        {
            // Vérifier si l'email existe déjà
            var existing = await _supabase
                .From<User>()
                .Where(u => u.Email == dto.Email)
                .Get();

            if (existing.Models.Any())
                return null;

            // Hasher le mot de passe
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // Créer l'utilisateur avec un UUID
            var user = new User
            {
                IdCompte = Guid.NewGuid().ToString(),
                Pseudo = dto.Pseudo,
                Email = dto.Email,
                Role = dto.Role,
                AvatarUrl = dto.AvatarUrl
            };

            var response = await _supabase.From<User>().Insert(user);
            var created = response.Models.First();

            return new AuthResponseDTO
            {
                Token = GenerateToken(created),
                IdCompte = created.IdCompte,
                Pseudo = created.Pseudo,
                Email = created.Email,
                Role = created.Role,
                AvatarUrl = created.AvatarUrl
            };
        }

        public async Task<AuthResponseDTO?> Login(LoginDTO dto)
        {
            var response = await _supabase
                .From<User>()
                .Where(u => u.Email == dto.Email)
                .Get();

            var user = response.Models.FirstOrDefault();
            if (user == null)
                return null;

            return new AuthResponseDTO
            {
                Token = GenerateToken(user),
                IdCompte = user.IdCompte,
                Pseudo = user.Pseudo,
                Email = user.Email,
                Role = user.Role,
                AvatarUrl = user.AvatarUrl
            };
        }

        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!)
            );

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.IdCompte),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("pseudo", user.Pseudo)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(
                    int.Parse(_config["Jwt:ExpiryInDays"]!)
                ),
                signingCredentials: new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256
                )
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}