using Domain.Games;
using Domain.Games.Models;
using Presentation.Common;
using Presentation.Games.ViewModels;

namespace Presentation.Games.Extensions;

public static class GamesExtensions
{
	extension(Game game)
	{
		public GameViewModel ToViewModel(GameMemberViewModel[] members)
		{
			var memberPointsDict = game.Members.ToDictionary(m => m.UserId, _ => 0); 

			var rounds = game.Rounds
				.OrderBy(r => r.GeneralNumber)
				.Select(round =>
				{
					var memberPoints = game.Members
						.Select(member =>
						{
							var bet = round.Bets.FirstOrDefault(b => b.MemberId == member.UserId);
							var bribe = round.Bribes.FirstOrDefault(b => b.MemberId == member.UserId);

							uint? betCount = (uint?)bet?.Amount;
							uint bribeCount = (uint)(bribe?.Amount ?? 0);

							int before = memberPointsDict[member.UserId];

							var pointsResult = round.GetPoints(member);
							int delta = pointsResult.IsSuccess ? pointsResult.Value : 0;

							int after = before + delta;
							memberPointsDict[member.UserId] = after;

							return new RoundMebmerPointsViewModel(
								member.UserId,
								betCount,
								bribeCount,
								before,
								after,
								delta
							);
						})
						.ToArray();

					return new RoundViewModel(
						round.Id.ToString(),
						round.Type,
						round.GeneralNumber,
						round.GetDisplayName(game.Members.Count),
						memberPoints
					);
				})
				.ToArray();

			return new GameViewModel(
				game.Id.ToString(),
				game.Name.Value,
				game.CreatedOnUtc,
				game.State,
				[.. game.Members.Select(m =>
					{
						var memberViewModel = members.FirstOrDefault(vm => vm.Id == m.UserId.ToString())
							?? throw new InvalidOperationException($"Member view model not found for member ID: {m.Id}");

						return memberViewModel;
					})
				],
				Rounds: rounds);
		}
	}

	extension(GamePreviewInformation game)
	{
		public GamePreviewViewModel ToViewModel(StringDictionaryModel[] members)
		{
			return new GamePreviewViewModel(
				game.Id.ToString(),
				game.Name.Value,
				game.CreatedOnUtc,
				game.State,
				members);
		}
	}
}
