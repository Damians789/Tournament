using System.Security.Claims;

namespace Zawody.Services
{
    public interface IUserContextService
    {
        ClaimsPrincipal User { get; }
        string? GetUserId { get; }
    }

    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

        public string? GetUserId =>
            User is null ? null : User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
    }
}
