using AvtMedia.CleanArchitecture.DomainLayer.Extensions.Primitives;
using AvtMedia.CleanArchitecture.DomainLayer.Extensions.Shared;
using Domain.Games.DomainErrors;
using Domain.Games.Entities;
using Domain.Games.Enums;
using Domain.Games.Models;
using Domain.Games.ValueObjects;

namespace Domain.Games;

public sealed partial class Game : AggregateRoot<Guid>, IAuditableEntity
{
	public GameName Name { get; private set; }
	public GameState State { get; private set; }


	private readonly HashSet<GameMember> _members = [];
	public ICollection<GameMember> Member => [.. _members];

	private readonly HashSet<GameRound> _rounds = [];
	public ICollection<GameRound> Rounds => [.. _rounds];

	public static Result<Game> Create(GameName name, GameMember[] members)
	{
		if (members.Length < 3)
			return Result.Failure<Game>(GameDomainErrors.Game.InvalidUserCount);

		var duplicateMembers = members.Length != members.Select(x => x.Id).Distinct().Count();

		if (duplicateMembers)
			return Result.Failure<Game>(GameDomainErrors.Game.DuplicateMembers);

		var orderIndexes = members.Select(x => x.OrderIndex).ToArray();

		for (int i = 0; i < members.Length; i++)
		{
			if (orderIndexes.Contains(i) is false)
				return Result.Failure<Game>(GameDomainErrors.Game.InvalidMemberOrder);
		}

		var game = new Game(name, members);

		return game;
	}

	private Game(GameName name, GameMember[] members)
	{
		Id = Guid.CreateVersion7();
		Name = name;
		State = GameState.NotStarted;
		_members = [..members];
	}

	public Result StartNewRound()
	{
		if (State == GameState.Finished)
			return Result.Failure(GameDomainErrors.Game.AlreadyFinished);

		var roundSequence = new (GameRoundType Type, uint Count)[]
		{
			(GameRoundType.Regular, 21),
			(GameRoundType.Dark, 3),
			(GameRoundType.Meager, 3),
			(GameRoundType.Trumpless, 3),
			(GameRoundType.Golden, 3),
			(GameRoundType.Forehead, 3)
		};

		uint nextGeneralNumber = (uint)(_rounds.Count + 1);

		uint accumulated = 0;
		GameRoundType? nextType = null;
		uint nextTypeNumber = 0;

		foreach (var (type, count) in roundSequence)
		{
			if (nextGeneralNumber <= accumulated + count)
			{
				nextType = type;
				nextTypeNumber = nextGeneralNumber - accumulated;
				break;
			}
			accumulated += count;
		}

		if (nextType is null)
			return Result.Failure(GameDomainErrors.Game.DuplicateRound); 

		var roundResult = GameRound.Create(nextType.Value, nextGeneralNumber, nextTypeNumber);

		if (roundResult.IsFailure)
			return Result.Failure(roundResult.Error);

		var round = roundResult.Value;

		_rounds.Add(round);

		if (State == GameState.NotStarted)
			State = GameState.Active;

		return Result.Success();
	}

	public Result AcceptBribes(GameRoundBribe[] bribes)
	{
		var currentRound = _rounds.OrderByDescending(r => r.GeneralNumber).FirstOrDefault();

		if (currentRound is null)
			return Result.Failure(GameDomainErrors.Game.NoActiveRound);

		var acceptBribesResult = currentRound.AcceptBribes(bribes);

		if (acceptBribesResult.IsFailure)
			return acceptBribesResult;

		bool isLastGameRound = currentRound.Type == GameRoundType.Forehead && currentRound.TypeNumber == 3;

		if (isLastGameRound)
		{
			State = GameState.Finished;

			var winnerResult = MakeWinner();

			if (winnerResult.IsFailure)
				return winnerResult;
		}

		return Result.Success();
	}

	private Result MakeWinner()
	{
		var memberPointsResults = _members
			.Select(m => GetMemberPoints(m.Id))
			.ToArray();

		var failedMemberPoints = memberPointsResults.FirstOrDefault(r => r.IsFailure);

		if (failedMemberPoints is not null)
			return Result.Failure(failedMemberPoints.Error);

		var winnerPoints = memberPointsResults
			.Select(r => r.Value)
			.MaxBy(mp => mp.TotalPoints);

		if (winnerPoints is null)
			return Result.Failure(GameDomainErrors.Game.FailedToDetermineWinner);

		var winner = _members.First(m => m.Id == winnerPoints.MemberId);

		winner.MarkAsWinner();

		return Result.Success();
	}

	public Result<GameMemberPoints> GetMemberPoints(Guid memberId)
	{
		var member = _members.FirstOrDefault(m => m.Id == memberId);

		if (member is null)
			return Result.Failure<GameMemberPoints>(GameDomainErrors.Game.MemberNotFound);

		var roundPoints = Rounds
			.Select(r =>
			{
				var pts = r.GetPoints(member);

				if (pts.IsFailure)
					return Result.Failure<GameMemberRoundPoints>(pts.Error);

				return new GameMemberRoundPoints(
					r.Id,
					r.Type,
					r.GeneralNumber,
					pts.Value
				);
			})
			.ToArray();

		var failedRoundPoint = roundPoints.FirstOrDefault(p => p.IsFailure);

		if (failedRoundPoint is not null)
			return Result.Failure<GameMemberPoints>(failedRoundPoint.Error);

		var memberPoints = new GameMemberPoints(
			GameId: Id,
			MemberId: memberId,
			RoundsPoints: [.. roundPoints.Select(p => p.Value)]
		);

		return memberPoints;
	}

	public DateTime CreatedOnUtc { get; init; }
	public DateTime ModifiedOnUtc { get; init; }
}
