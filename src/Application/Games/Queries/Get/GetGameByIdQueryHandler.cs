using Domain.Games;
using Domain.Games.DomainErrors;
using Domain.Games.Repositories;

namespace Application.Games.Queries.Get;

public sealed class GetGameByIdQueryHandler(
	IGamesRepository _gamesRepository) : IQueryHandler<GetGameByIdQuery, Game>
{
	public async Task<Result<Game>> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
	{
		var game = await _gamesRepository.GetById(request.Id, cancellationToken);

		if (game is null)
			return Result.Failure<Game>(GameDomainErrors.Game.NotFound);

		return game;
	}
}
