using AvtMedia.CleanArchitecture.DomainLayer.Extensions.Shared;

namespace Domain.Users.DomainErrors;

public static class UserDomainErrors
{
	public static class User
	{
		public static readonly Error NotFound = new("User.NotFound");
		public static readonly Error AlreadyExistWithGivenName = new("User.AlreadyExistWithGivenName");
	}

	public static class UserName
	{
		public static readonly Error TooLong = new("UserName.TooLong");
		public static readonly Error TooShort = new("UserName.TooShort");
		public static readonly Error Empty = new("UserName.Empty");
	}
}
