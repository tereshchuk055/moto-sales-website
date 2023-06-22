using Microsoft.AspNetCore.Authorization;

namespace MotoShop.Services
{
    public class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoleRequirementHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var user = httpContext?.User;

            if (user is not null && user.Identity?.IsAuthenticated == true)
            {
                if (requirement.Roles is null || !requirement.Roles.Any())
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }

                foreach (var role in requirement.Roles)
                {
                    if (user.IsInRole(role))
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }
                }
            }

            context.Fail();
            return Task.CompletedTask;
        }
    }
}
