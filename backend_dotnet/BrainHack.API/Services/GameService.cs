using BrainHack.API.DTOs;
using BrainHack.API.Models;

namespace BrainHack.API.Services
{
    public class GameService
    {
        private readonly Supabase.Client _supabase;

        public GameService(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<GameSessionResponseDTO?> SaveGameSession(string userId, SaveGameSessionDTO dto)
        {
            var userResponse = await _supabase
                .From<User>()
                .Where(u => u.Id == userId)
                .Get();

            var user = userResponse.Models.FirstOrDefault();
            if (user == null)
            {
                return null;
            }

            var minigameId = await ResolveMinigameId(dto);
            if (string.IsNullOrWhiteSpace(minigameId))
            {
                return null;
            }

            var session = new GameSession
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                MinigameId = minigameId,
                Score = dto.Score,
                XpEarned = dto.XpEarned,
                CompletedAt = DateTime.UtcNow
            };

            var insertResponse = await _supabase.From<GameSession>().Insert(session);
            var inserted = insertResponse.Models.FirstOrDefault();
            if (inserted == null)
            {
                return null;
            }

            user.TotalXp += dto.XpEarned;
            var userUpdate = await _supabase.From<User>().Update(user);
            var updatedUser = userUpdate.Models.FirstOrDefault();
            if (updatedUser == null)
            {
                return null;
            }

            return new GameSessionResponseDTO
            {
                SessionId = inserted.Id,
                UserId = inserted.UserId,
                MinigameId = inserted.MinigameId,
                Score = inserted.Score,
                XpEarned = inserted.XpEarned,
                CompletedAt = inserted.CompletedAt,
                UpdatedTotalXp = updatedUser.TotalXp
            };
        }

        private async Task<string?> ResolveMinigameId(SaveGameSessionDTO dto)
        {
            var minigameIdCandidate = (dto.MinigameId ?? string.Empty).Trim();
            if (!string.IsNullOrWhiteSpace(minigameIdCandidate))
            {
                var byId = await _supabase
                    .From<MiniGame>()
                    .Where(m => m.Id == minigameIdCandidate)
                    .Get();

                var existingById = byId.Models.FirstOrDefault();
                if (existingById != null)
                {
                    return existingById.Id;
                }
            }

            var minigameKey = (dto.MinigameKey ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(minigameKey) && !Guid.TryParse(minigameIdCandidate, out _))
            {
                minigameKey = minigameIdCandidate;
            }

            if (string.IsNullOrWhiteSpace(minigameKey))
            {
                return null;
            }

            var byName = await _supabase
                .From<MiniGame>()
                .Where(m => m.Name == minigameKey)
                .Get();

            var existingByName = byName.Models.FirstOrDefault();
            if (existingByName != null)
            {
                return existingByName.Id;
            }

            var newMiniGame = new MiniGame
            {
                Id = Guid.NewGuid().ToString(),
                Name = minigameKey,
                Description = $"Auto-created minigame entry for {minigameKey}",
                MaxXpPossible = Math.Max(dto.XpEarned, 100)
            };

            var insertMiniGame = await _supabase.From<MiniGame>().Insert(newMiniGame);
            var createdMiniGame = insertMiniGame.Models.FirstOrDefault();
            return createdMiniGame?.Id;
        }
    }
}
