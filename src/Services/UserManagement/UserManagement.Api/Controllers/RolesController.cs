using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Roles.UpdateRole;

using static Shared.Api.Constants.Authentication;

namespace UserManagement.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class RolesController(RoleManager<IdentityRole> roleManager) : ControllerBase
{
    [Authorize(Policy = AuthorizationPolicies.ApiScopePolicy)]
    [HttpGet]
    public async Task<IdentityRole?> Get(string roleName)
    {
        return await roleManager.FindByNameAsync(roleName);
    }

    [Authorize(Policy = AuthorizationPolicies.ApiScopePolicy)]
    [HttpGet("all")]
    public IEnumerable<IdentityRole> GetAll()
    {
        return roleManager.Roles.AsEnumerable();
    }
    
    [Authorize(Policy = AuthorizationPolicies.ApiScopePolicy)]
    [HttpPost]
    public async Task<IdentityResult> Add([FromBody] string roleName)
    {
        return await roleManager.CreateAsync(new IdentityRole(roleName));
    }
    
    [Authorize(Policy = AuthorizationPolicies.ApiScopePolicy)]
    [HttpPut]
    public async Task<IdentityResult> Update([FromBody] UpdateRoleCommand command)
    {
        var targetRole = await roleManager.FindByNameAsync(command.currentRoleName);

        if (targetRole == null)
            return IdentityResult.Failed(
                new IdentityError
                {
                    Description = $"Role {command.currentRoleName} not found"
                });

        targetRole.Name = command.NewRoleName;

        return await roleManager.UpdateAsync(targetRole);
    }
    
    [Authorize(Policy = AuthorizationPolicies.ApiScopePolicy)]
    [HttpDelete]
    public async Task<IdentityResult> Remove([FromBody] string roleName)
    {
        var targetRole = await roleManager.FindByNameAsync(roleName);

        if (targetRole == null)
            return IdentityResult.Failed(
                new IdentityError
                {
                    Description = $"Role {roleName} not found"
                });
        
        return await roleManager.DeleteAsync(targetRole);
    }
}