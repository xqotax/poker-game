using Domain.Users;
using Presentation.Users.ViewModels;

namespace Presentation.Users.Extensions;

public static class UsersExtensions
{
	extension(User user)
	{
		public UserViewModel ToViewModel() => new(
				user.Id,
				user.Username.Value,
				user.RegistrationDate);
	}
}
