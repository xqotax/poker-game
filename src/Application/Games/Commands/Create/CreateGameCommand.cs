namespace Application.Games.Commands.Create;

public sealed record CreateGameCommand(string Name, Dictionary<Guid, uint> MemberIdToIndex): ICommand<Guid>, ILoggingProperties;
