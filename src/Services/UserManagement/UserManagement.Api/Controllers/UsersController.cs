using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Api.Authorization;
using UserManagement.Application.Users.Commands.CreateUser;
using UserManagement.Application.Users.Commands.UpdateUser;
using UserManagement.Core.Entities;

namespace UserManagement.Api.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class UsersController(UserManager<ApplicationUser> userManager) : ControllerBase
{
    [Authorize(Policy = AuthorizationPolicies.ApiScopePolicy)]
    [HttpGet]
    public async Task<ApplicationUser?> Get(string username)
    {
        return await userManager.FindByNameAsync(username);
    }

    [Authorize(Policy = AuthorizationPolicies.UserScopePolicy)]
    [HttpGet("me")]
    public async Task<ActionResult<ApplicationUser>> GetCurrent()
    {
        var currentUser = await userManager.GetUserAsync(User);

        return currentUser == null
            ? NotFound()
            : currentUser;
    }

    [Authorize(Policy = AuthorizationPolicies.UserScopePolicy)]
    [HttpDelete("me")]
    public async Task<ActionResult<IdentityResult>> RemoveCurrent()
    {
        var currentUser = await userManager.GetUserAsync(User);

        return currentUser == null
            ? NotFound()
            : await userManager.DeleteAsync(currentUser);
    }

    [Authorize(Policy = AuthorizationPolicies.ApiScopePolicy)]
    [HttpGet("all")]
    public IEnumerable<ApplicationUser> GetAll()
    {
        return userManager.Users.AsEnumerable();
    }
    
    [Authorize(Policy = AuthorizationPolicies.ApiScopePolicy)]
    [HttpPost]
    public async Task<IdentityResult> Add([FromBody] CreateUserCommand command)
    {
        return await userManager.CreateAsync(
            command.User,
            command.Password);
    }

    [Authorize(Policy = AuthorizationPolicies.ApiScopePolicy)]
    [HttpPut]
    public async Task<IdentityResult> Update([FromBody] UpdateUserCommand command)
    {
        var targetUser = await userManager.FindByNameAsync(command.UserName);

        if (targetUser == null)
            return IdentityResult.Failed(
                new IdentityError
                {
                    Description = $"User {command.UserName} not found"
                });

        targetUser.NickName = command.NickName ?? targetUser.NickName;
        targetUser.Email = command.Email ?? targetUser.Email;
        targetUser.Level = command.Level ?? targetUser.Level;

        return await userManager.UpdateAsync(targetUser);
    }

    [Authorize(Policy = AuthorizationPolicies.ApiScopePolicy)]
    [HttpDelete]
    public async Task<IdentityResult> Remove([FromBody] string userName)
    {
        var targetUser = await userManager.FindByNameAsync(userName);

        if (targetUser == null)
            return IdentityResult.Failed(
                new IdentityError
                {
                    Description = $"User {userName} not found"
                });
        
        return await userManager.DeleteAsync(targetUser);
    }

    [Authorize(Policy = AuthorizationPolicies.ApiScopePolicy)]
    [HttpPut("changePassword")]
    public async Task<IdentityResult> ChangePassword([FromBody] UpdatePasswordCommand command)
    {
        var targetUser = await userManager.FindByNameAsync(command.UserName);
        
        if (targetUser == null)
            return IdentityResult.Failed(
                new IdentityError
                {
                    Description = $"User {command.UserName} not found"
                });

        return await userManager.ChangePasswordAsync(
            targetUser,
            command.CurrentPassword,
            command.NewPassword
        );
    }

    [Authorize(Policy = AuthorizationPolicies.ApiScopePolicy)]
    [HttpPut("addRoles")]
    public async Task<IdentityResult> AddRoles([FromBody] UpdateUserRolesCommand command)
    {
        var targetUser = await userManager.FindByNameAsync(command.UserName);
        
        if (targetUser == null)
            return IdentityResult.Failed(
                new IdentityError
                {
                    Description = $"User {command.UserName} not found"
                });
        
        return await userManager.AddToRolesAsync(
            targetUser,
            command.RolesNames);
    }
    
    [Authorize(Policy = AuthorizationPolicies.ApiScopePolicy)]
    [HttpPut("removeRoles")]
    public async Task<IdentityResult> RemoveRoles([FromBody] UpdateUserRolesCommand command)
    {
        var targetUser = await userManager.FindByNameAsync(command.UserName);
        
        if (targetUser == null)
            return IdentityResult.Failed(
                new IdentityError
                {
                    Description = $"User {command.UserName} not found"
                });
        
        return await userManager.RemoveFromRolesAsync(
            targetUser,
            command.RolesNames);
    }
}