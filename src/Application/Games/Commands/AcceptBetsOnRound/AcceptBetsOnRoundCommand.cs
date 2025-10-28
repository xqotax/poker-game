namespace Application.Games.Commands.AcceptBetsOnRound;

public sealed record AcceptBetsOnRoundCommand(
	Guid GameId, 
	Guid RoundId, 
	Dictionary<Guid, int?> Bets): ICommand, ILoggingProperties;
