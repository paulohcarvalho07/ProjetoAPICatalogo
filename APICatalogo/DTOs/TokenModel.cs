using System.Reflection.Metadata.Ecma335;

namespace APICatalogo.DTOs;

public class TokenModel
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}
