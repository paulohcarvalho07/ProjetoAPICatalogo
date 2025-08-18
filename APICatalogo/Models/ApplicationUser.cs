using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata.Ecma335;

namespace APICatalogo.Models;

public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}
