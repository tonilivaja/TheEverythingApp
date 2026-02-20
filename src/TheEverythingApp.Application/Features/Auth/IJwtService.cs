namespace TheEverythingApp.Application.Features.Auth;

public interface IJwtService
{
    string GenerateToken(User user);
}
