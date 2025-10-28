namespace Application.Games.Commands.StartNewRound;

public sealed record StartNewRoundCommand(Guid GameId): ICommand<Guid>, ILoggingProperties;
