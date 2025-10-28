namespace Application.Games.Commands.AcceptBetsOnRound;

public sealed record AcceptBetsOnRoundCommand(
	Guid GameId, 
	Dictionary<Guid, int?> Bets): ICommand, ILoggingProperties;
