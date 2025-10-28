using Domain.Common;
using Domain.Games.DomainErrors;
using Domain.Games.Entities;
using Domain.Games.Repositories;

namespace Application.Games.Commands.AcceptBetsOnRound;

public sealed class AcceptBetsOnRoundCommandHandler(
	IGamesRepository _gamesRepository,
	IUnitOfWork _unitOfWork)
	: ICommandHandler<AcceptBetsOnRoundCommand>
{
	public async Task<Result> Handle(AcceptBetsOnRoundCommand request, CancellationToken cancellationToken)
	{
		var game = await _gamesRepository.GetById(request.GameId, cancellationToken);

		if (game is null)
			return Result.Failure(GameDomainErrors.Game.NotFound);

		var round = game.Rounds.OrderByDescending(x => x.GeneralNumber).FirstOrDefault();

		if (round is null)
			return Result.Failure(GameDomainErrors.Game.NoActiveRound);

		var bets = request.Bets
			.Select(bet => new GameRoundBet(bet.Key, bet.Value))
			.ToArray();

		var acceptResult = round.AcceptBets(bets);

		if (acceptResult.IsFailure)
			return Result.Failure(acceptResult.Error);

		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}
