using Microsoft.AspNetCore.Authorization;

namespace MotoShop.Data.ActionFilters
{
    public class HasPrivilegeAttribute : AuthorizeAttribute
    {
        public HasPrivilegeAttribute(params string[] roles)
        {
            if (roles.Length == 0)
                roles = new[] { "Admin", "Moderator" };
            
            Roles = string.Join(",", roles);
        }
    }
}
