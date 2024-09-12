namespace PS_Utils.Services
{
    public class AuthService
    {
        public async Task<string> LoginAsync(string domain, string username, string password)
        {
            // AD Authentication using LogonUser
            bool isAuthenticated = await Task.Run(() => ADLoginService.AuthenticateUser(domain, username, password));
            if (!isAuthenticated)
            {
                throw new UnauthorizedAccessException("Invalid login credentials.");
            }

            // Generate JWT token upon successful authentication
            return await Task.Run(() => JwtTokenService.GenerateJwtToken(username));
        }
    }

    public class AuthResult
    {
        public string? Token { get; set; }
    }
}
