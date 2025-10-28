using static Presentation.Games.ViewModels.GameCreateViewModel;

namespace Presentation.Games.ViewModels;

public sealed record GameCreateViewModel(
	string Name,
	GameMemberCreateViewModel[] Members
)
{
	public sealed record GameMemberCreateViewModel(
		string MemberId,
		int OrderIndex
	);
}
