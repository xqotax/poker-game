using Domain.Common;
using Domain.Games;
using Domain.Games.Entities;
using Domain.Games.Repositories;
using Domain.Games.ValueObjects;

namespace Application.Games.Commands.Create;

public sealed class CreateGameCommandHandler(
	IGamesRepository _gamesRepository,
	IUnitOfWork _unitOfWork) : ICommandHandler<CreateGameCommand, Guid>
{
	public async Task<Result<Guid>> Handle(CreateGameCommand request, CancellationToken cancellationToken)
	{
		var members = request.MemberIdToIndex
			.Select(kvp => new GameMember(kvp.Key, kvp.Value))
			.ToArray();

		var createGameNameResult = GameName.Create(request.Name);

		if (createGameNameResult.IsFailure)
			return Result.Failure<Guid>(createGameNameResult.Error);

		var game = Game.Create(createGameNameResult.Value, members);

		if (game.IsFailure)
			return Result.Failure<Guid>(game.Error);

		await _gamesRepository.Add(game.Value, cancellationToken);

		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return Result.Success(game.Value.Id);
	}
}
