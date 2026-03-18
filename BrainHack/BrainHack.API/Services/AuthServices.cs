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

            if (dto.Role.ToLower() == "eleve") dto.Role = "Etudiant";

            var existing = await _supabase
                .From<User>()
                .Where(u => u.Email == dto.Email)
                .Get();

            if (existing.Models.Any())
                return null;

            var authResponse = await _supabase.Auth.SignUp(dto.Email, dto.Password);
            if (authResponse == null || authResponse.User == null)
                return null;

            // Créer l'utilisateur avec un UUID
            var user = new User
            {
                IdCompte = authResponse.User.Id,
                Pseudo = dto.Pseudo,
                Email = dto.Email,
                Role = dto.Role,
                AvatarUrl = dto.AvatarUrl
            };

            var response = await _supabase.From<User>().Insert(user);
            if (!response.Models.Any()) return null;
            var created = response.Models.First();

            return new AuthResponseDTO
            {
                Token = authResponse.AccessToken,
                IdCompte = created.IdCompte,
                Pseudo = created.Pseudo,
                Email = created.Email,
                Role = created.Role,
                AvatarUrl = created.AvatarUrl
            };
        }

        public async Task<AuthResponseDTO?> Login(LoginDTO dto)
        {
            var authResponse = await _supabase.Auth.SignIn(dto.Email, dto.Password);

            if (authResponse == null || authResponse.User == null)
                return null;

            var response = await _supabase
                .From<User>()
                .Where(u => u.IdCompte == authResponse.User.Id)
                .Get();


            var user = response.Models.FirstOrDefault();
            if (user == null)
                return null;

            return new AuthResponseDTO
            {
                Token = authResponse.AccessToken ?? "",
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