using Domain.Users;
using Presentation.Common;
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

		public StringDictionaryModel ToStringDictionaryModel() => new(
			user.Id.ToString(),
			user.Username.Value);

		public GameMemberViewModel ToGameViewModel(int orderIndex, bool winner) => new(
				user.Id.ToString(),
				user.Username.Value,
				orderIndex,
				winner);
	}
}
