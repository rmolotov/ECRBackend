using MediatR;

namespace RemoteConfig.Application.Enemies.Commands.DeleteEnemy;

public class DeleteEnemyCommand : IRequest
{
    public string Id { get; set; }
}