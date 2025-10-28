using Domain.Games.Models;

namespace Application.Games.Queries.GetAll;

public sealed record GetGamesQuery: IQuery<GamePreviewInformation[]>, ILoggingProperties;
