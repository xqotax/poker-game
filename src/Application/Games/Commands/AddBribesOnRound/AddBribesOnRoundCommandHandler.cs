using Domain.Common;
using Domain.Games.DomainErrors;
using Domain.Games.Entities;
using Domain.Games.Repositories;

namespace Application.Games.Commands.AddBribesOnRound;

public sealed class AddBribesOnRoundCommandHandler(
	IGamesRepository _gamesRepository,
	IUnitOfWork _unitOfWork) : ICommandHandler<AddBribesOnRoundCommand>
{
	public async Task<Result> Handle(AddBribesOnRoundCommand request, CancellationToken cancellationToken)
	{
		var game = await _gamesRepository.GetById(request.GameId, cancellationToken);

		if (game is null)
			return Result.Failure(GameDomainErrors.Game.NotFound);

		var bribes = request.Bribes
			.Select(b => new GameRoundBribe(b.Key, b.Value))
			.ToArray();

		var addBribesResult = game.AddBribes(bribes);

		if (addBribesResult.IsFailure)
			return Result.Failure(addBribesResult.Error);

		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}
