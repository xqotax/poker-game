namespace Presentation.Users.ViewModels;
public sealed record UserViewModel(
	string Id,
	string Name,
	DateTime RegistrationDate);
