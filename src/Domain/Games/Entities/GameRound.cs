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


	private static readonly int[] RegularNumbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1];

	public static readonly IReadOnlyCollection<(GameRoundType Type, uint Count)> RoundSequence =
	[
		(GameRoundType.Regular, 21),
		(GameRoundType.Dark, 3),
		(GameRoundType.Meager, 3),
		(GameRoundType.Trumpless, 3),
		(GameRoundType.Golden, 3),
		(GameRoundType.Forehead, 3)
	];

	public string GetDisplayName()
	{
		uint roundIndex = GeneralNumber;
		uint accumulated = 0;

		foreach (var (type, count) in RoundSequence)
		{
			if (roundIndex <= accumulated + count)
			{
				if (type == GameRoundType.Regular)
				{
					int regularIdx = (int)(roundIndex - accumulated - 1);
					if (regularIdx >= 0 && regularIdx < RegularNumbers.Length)
						return RegularNumbers[regularIdx].ToString();
					else
						return roundIndex.ToString();
				}
				else
				{
					return type.ToString();
				}
			}
			accumulated += count;
		}

		return GeneralNumber.ToString();
	}

	public Result AcceptBets(GameRoundBet[] bets)
	{
		if (Type is GameRoundType.Meager)
			return Result.Failure(GameDomainErrors.GameRound.CanAcceptBetsAsMeager);

		if (bets.Length == 0)
			return Result.Failure(GameDomainErrors.GameRound.NoBetsProvided);

		if (_bets.Count > 0)
			return Result.Failure(GameDomainErrors.GameRound.BetsAlreadyAccepted);

		var someBetsHasntAmount = bets.Any(b => b.Amount is null);

		if (someBetsHasntAmount)
			return Result.Failure(GameDomainErrors.GameRound.InvalidBetForRound);

		var duplicateBets = bets.Length != bets.Select(x => x.Id).Distinct().Count();

		if (duplicateBets)
			return Result.Failure(GameDomainErrors.GameRound.DuplicateBets);

		if (Type != GameRoundType.Forehead)
		{
			int sum = bets.Sum(b => b.Amount ?? 0);

			if (Type == GameRoundType.Regular)
			{
				int idx = (int)(GeneralNumber - 1);
				if (idx < 0 || idx >= RegularNumbers.Length || sum == RegularNumbers[idx])
					return Result.Failure(GameDomainErrors.GameRound.InvalidBetForRound);
			}
			else
			{
				if (sum == 10)
					return Result.Failure(GameDomainErrors.GameRound.InvalidBetForRound);
			}
		}


		_bets.UnionWith(bets);

		return Result.Success();
	}

	internal Result AcceptBribes(GameRoundBribe[] bribes)
	{
		if (bribes.Length == 0)
			return Result.Failure(GameDomainErrors.GameRound.NoBribesProvided);

		if (_bribes.Count > 0)
			return Result.Failure(GameDomainErrors.GameRound.BribesAlreadyAccepted);

		if (bribes.Length != _bets.Count && Type is not GameRoundType.Meager)
			return Result.Failure(GameDomainErrors.GameRound.BribeBetCountMismatch);

		var duplicateBribes = bribes.Length != bribes.Select(x => x.Id).Distinct().Count();

		if (duplicateBribes)
			return Result.Failure(GameDomainErrors.GameRound.DuplicateBribes);

		_bribes.UnionWith(bribes);

		int sum = bribes.Sum(b => b.Amount);

		if (Type == GameRoundType.Forehead)
		{
			if (sum != 1)
				return Result.Failure(GameDomainErrors.GameRound.InvalidBribeForRound);
		}
		else if (Type == GameRoundType.Regular)
		{
			int idx = (int)(GeneralNumber - 1);
			if (idx < 0 || idx >= RegularNumbers.Length || sum != RegularNumbers[idx])
				return Result.Failure(GameDomainErrors.GameRound.InvalidBribeForRound);
		}
		else
		{
			if (sum != 10)
				return Result.Failure(GameDomainErrors.GameRound.InvalidBribeForRound);
		}

		return Result.Success();
	}

	public Result<int> GetPoints(GameMember gameMember)
	{
		if (_bribes.Count == 0 || _bets.Count == 0 && Type is not GameRoundType.Meager)
			return Result.Failure<int>(GameDomainErrors.GameRound.RoundNotFinished);

		var bet = _bets.FirstOrDefault(b => b.MemberId == gameMember.UserId);
		var bribe = _bribes.FirstOrDefault(b => b.MemberId == gameMember.UserId);

		if (bribe is null)
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

		var betAmount = bet?.Amount ?? 0;

		var passSuccess = betAmount == bribe.Amount && bribe.Amount == 0;
		var betSuccess = betAmount == bribe.Amount && bribe.Amount != 0;
		var overBet = bribe.Amount > betAmount;
		var underBet = bribe.Amount < betAmount;

		if (passSuccess)
			return Result.Success(5 * coef);
		else if (betSuccess)
			return Result.Success(10 * coef * bribe.Amount);
		else if (overBet)
			return Result.Success(bribe.Amount * coef);
		else if (underBet)
			return Result.Success((betAmount - bribe.Amount) * -10 * coef);
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
