namespace Application.Games.Commands.Create;

public sealed record CreateGameCommand(string Name, Dictionary<Guid, int> MemberIdToIndex): ICommand<Guid>, ILoggingProperties;
