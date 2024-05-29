using System.ComponentModel.DataAnnotations;

namespace DelTSZ.Models.Users;

public class PasswordChange
{
    public string? CurrentPassword { get; init; }
    public string? NewPassword { get; init; }
    
    [Compare("NewPassword", ErrorMessage = "Confirm new password does not match.")]
    public string? ConfirmNewPassword { get; init; }
}