using System.ComponentModel.DataAnnotations;

namespace TheEverythingApp.Application.Features.Auth;

public record RegisterRequest(
    [Required]
    [MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
    [MaxLength(15, ErrorMessage = "Username cannot exceed 15 characters.")]
    string Username,
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    string Password);

public record LoginRequest(string Username, string Password);
public record AuthResponse(string Token, string Username);
