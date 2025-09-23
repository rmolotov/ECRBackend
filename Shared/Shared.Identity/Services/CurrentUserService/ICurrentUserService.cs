namespace Shared.Identity.Services.CurrentUserService;

public interface ICurrentUserService
{
    Guid UserId { get; }
}