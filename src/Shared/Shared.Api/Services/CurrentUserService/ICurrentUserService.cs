namespace Shared.Api.Services.CurrentUserService;

public interface ICurrentUserService
{
    Guid UserId { get; }
}