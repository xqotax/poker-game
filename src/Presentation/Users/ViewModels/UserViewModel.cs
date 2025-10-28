namespace Presentation.Users.ViewModels;
public sealed record UserViewModel(
	Guid Id,
	string Name,
	DateTime RegistrationDate);
