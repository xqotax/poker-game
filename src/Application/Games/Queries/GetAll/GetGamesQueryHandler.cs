using Domain.Games.Models;
using Domain.Games.Repositories;

namespace Application.Games.Queries.GetAll;

public sealed class GetGamesQueryHandler(IGamesRepository _gamesRepository) : IQueryHandler<GetGamesQuery, GamePreviewInformation[]>
{
	public async Task<Result<GamePreviewInformation[]>> Handle(GetGamesQuery request, CancellationToken cancellationToken)
	{
		var games = await _gamesRepository.GetAll(cancellationToken);

		return games;
	}
}
