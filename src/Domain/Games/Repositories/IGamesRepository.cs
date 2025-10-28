using Domain.Games.ValueObjects;

namespace Domain.Games.Repositories;

public interface IGamesRepository
{
	Task Add(Game game, CancellationToken cancellationToken);
	Task<Game?> GetById(Guid id, CancellationToken cancellationToken);

	Task<bool> NameAlreadyExist(GameName gameName, Guid? exceptId, CancellationToken cancellationToken);
}
