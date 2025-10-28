namespace Application.Games.Commands.AddBribesOnRound;

public sealed record AddBribesOnRoundCommand(
	Guid GameId,
	Dictionary<Guid, int> Bribes) : ICommand, ILoggingProperties;
