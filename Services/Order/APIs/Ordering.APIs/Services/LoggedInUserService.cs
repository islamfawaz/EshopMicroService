using Ordering.Domain.Abstractions;
using System.Security.Claims;

namespace Ordering.APIs.Services
{
    public class LoggedInUserService : ILoggedInUserService
    {
        private readonly IHttpContextAccessor _httpContext;

        public string? UserId { get; private set; }

        public LoggedInUserService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;

            var context = _httpContext.HttpContext;
            if (context?.User?.Identity?.IsAuthenticated == true)
            {
                UserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            else
            {
                UserId = null; 
            }
        }
    }
}
