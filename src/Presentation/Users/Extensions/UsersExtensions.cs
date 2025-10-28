using Domain.Users;
using Presentation.Games.ViewModels;
using Presentation.Users.ViewModels;

namespace Presentation.Users.Extensions;

public static class UsersExtensions
{
	extension(User user)
	{
		public UserViewModel ToViewModel() => new(
				user.Id.ToString(),
				user.Username.Value,
				user.RegistrationDate);

		public GameMemberViewModel ToGameViewModel(int orderIndex) => new(
				user.Id.ToString(),
				user.Username.Value,
				orderIndex);
	}
}
