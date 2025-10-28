using Domain.Games.Enums;

namespace Infrastructure.Tables;

internal class GameStates
{
	public GameState Id { get; set; }
	public string Name { get; set; } = null!;
}
