using Domain.Games;
using Presentation.Games.ViewModels;

namespace Presentation.Games.Extensions;

public static class GamesExtensions
{
	extension(Game game)
	{
		public GameViewModel ToViewModel(GameMemberViewModel[] members)
		{
			return new GameViewModel(
				game.Id.ToString(),
				game.Name.Value,
				game.CreatedOnUtc,
				game.State,
				[.. game.Members.Select(m =>
					{
						var memberViewModel = members.FirstOrDefault(vm => vm.Id == m.Id.ToString())
							?? throw new InvalidOperationException($"Member view model not found for member ID: {m.Id}");

						return memberViewModel;
					})
				],
				Rounds: []);
		}
	}
}
