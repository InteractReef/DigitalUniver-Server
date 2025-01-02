namespace InteractReef.Packets.Identity
{
	public enum IdentityStatusCode
	{
		Ok,
		InvalidLoginData,
		EmailAlreadyUsed
	}

	public record LoginRequest(string email, string password);
	public record LoginResponse(string token, IdentityStatusCode status);

	public record RegRequest(string email, string password);
	public record RegisterRequest(IdentityStatusCode status);
 }
