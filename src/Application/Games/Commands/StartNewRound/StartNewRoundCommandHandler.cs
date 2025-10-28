using Domain.Common;
using Domain.Games.DomainErrors;
using Domain.Games.Repositories;

namespace Application.Games.Commands.StartNewRound;

public sealed class StartNewRoundCommandHandler(
	IGamesRepository _gamesRepository,
	IUnitOfWork _unitOfWork) : ICommandHandler<StartNewRoundCommand, Guid>
{
	public async Task<Result<Guid>> Handle(StartNewRoundCommand request, CancellationToken cancellationToken)
	{
		var game = await _gamesRepository.GetById(request.GameId, cancellationToken);

		if (game is null)
			return Result.Failure<Guid>(GameDomainErrors.Game.NotFound);

		var startNewRoundResult = game.StartNewRound();

		if (startNewRoundResult.IsFailure)
			return Result.Failure<Guid>(startNewRoundResult.Error);

		await _unitOfWork.SaveChangesAsync(cancellationToken);

		var lastRound = game.Rounds.OrderByDescending(r => r.GeneralNumber).First();

		return lastRound.Id;
	}
}
