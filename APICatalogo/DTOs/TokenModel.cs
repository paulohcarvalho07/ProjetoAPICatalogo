using System.Reflection.Metadata.Ecma335;

namespace APICatalogo.DTOs;

public class TokenModel
{
    public string? AcessToken { get; set; }
    public string? RefreshToken { get; set; }
}
