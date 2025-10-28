using AvtMedia.CleanArchitecture.DomainLayer.Extensions.Primitives;
using AvtMedia.CleanArchitecture.DomainLayer.Extensions.Shared;
using Domain.Games.DomainErrors;
using Domain.Games.Enums;

namespace Domain.Games.Entities;

public sealed partial class GameRound : Entity<Guid>
{
	public GameRoundType Type { get; private set; }
	public uint GeneralNumber { get; private set; }


	private readonly HashSet<GameRoundBribe> _bribes = [];
	public ICollection<GameRoundBribe> Bribes => [.. _bribes];


	private readonly HashSet<GameRoundBet> _bets = [];
	public ICollection<GameRoundBet> Bets => [.. _bets];

	public static Result<GameRound> Create(GameRoundType type, uint generalNumber)
	{
		if (generalNumber is 0)
			return Result.Failure<GameRound>(GameDomainErrors.GameRound.InvalidGeneralNumber);

		if (type is GameRoundType.None)
			return Result.Failure<GameRound>(GameDomainErrors.GameRound.InvalidType);

		var round = new GameRound(type, generalNumber);

		return round;
	}

	public Result AcceptBets(GameRoundBet[] bets)
	{
		if (bets.Length == 0)
			return Result.Failure(GameDomainErrors.GameRound.NoBetsProvided);

		if (_bets.Count > 0)
			return Result.Failure(GameDomainErrors.GameRound.BetsAlreadyAccepted);

		var someBetsHasntAmount = bets.Any(b => b.Amount is null);

		if (someBetsHasntAmount && Type is not GameRoundType.Meager)
			return Result.Failure(GameDomainErrors.GameRound.InvalidBetForRound);

		var duplicateBets = bets.Length != bets.Select(x => x.Id).Distinct().Count();

		if (duplicateBets)
			return Result.Failure(GameDomainErrors.GameRound.DuplicateBets);

		_bets.UnionWith(bets);

		return Result.Success();
	}

	internal Result AcceptBribes(GameRoundBribe[] bribes)
	{
		if (bribes.Length == 0)
			return Result.Failure(GameDomainErrors.GameRound.NoBribesProvided);

		if (_bribes.Count > 0)
			return Result.Failure(GameDomainErrors.GameRound.BribesAlreadyAccepted);

		if (_bribes.Count != _bets.Count)
			return Result.Failure(GameDomainErrors.GameRound.BribeBetCountMismatch);

		var duplicateBribes = bribes.Length != bribes.Select(x => x.Id).Distinct().Count();

		if (duplicateBribes)
			return Result.Failure(GameDomainErrors.GameRound.DuplicateBribes);

		_bribes.UnionWith(bribes);

		return Result.Success();
	}

	public Result<int> GetPoints(GameMember gameMember)
	{
		if (_bets.Count == 0 || _bribes.Count == 0)
			return Result.Failure<int>(GameDomainErrors.GameRound.RoundNotFinished);

		var bet = _bets.FirstOrDefault(b => b.MemberId == gameMember.Id);
		var bribe = _bribes.FirstOrDefault(b => b.MemberId == gameMember.Id);

		if (bet is null || bribe is null)
			return Result.Failure<int>(GameDomainErrors.GameRound.MemberNotInRound);

		if (Type is GameRoundType.Meager)
		{
			if (bribe.Amount == 0)
				return Result.Success(5);

			return Result.Success(-10 * bribe.Amount);
		}

		var coef = Type switch
		{
			GameRoundType.Forehead => 10,
			GameRoundType.Golden => 2,
			_ => 1
		};

		var passSuccess = bet.Amount == bribe.Amount && bribe.Amount == 0;
		var betSuccess = bet.Amount == bribe.Amount && bribe.Amount != 0;
		var overBet = bribe.Amount > bet.Amount;
		var underBet = bribe.Amount < bet.Amount;

		if (passSuccess)
			return Result.Success(5 * coef);
		else if (betSuccess)
			return Result.Success(10 * coef);
		else if (overBet)
			return Result.Success(bribe.Amount * coef);
		else if (underBet)
			return Result.Success((bet.Amount!.Value - bribe.Amount) * -10 * coef);
		else
			return Result.Failure<int>(GameDomainErrors.GameRound.FailedToDetirminePoints);
	}

	private GameRound(GameRoundType type, uint generalNumber)
	{
		Id = Guid.CreateVersion7();
		Type = type;
		GeneralNumber = generalNumber;
	}
}
