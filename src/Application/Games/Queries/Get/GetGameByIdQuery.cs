using Domain.Games;

namespace Application.Games.Queries.Get;

public sealed record GetGameByIdQuery(Guid Id): IQuery<Game>, ILoggingProperties;
