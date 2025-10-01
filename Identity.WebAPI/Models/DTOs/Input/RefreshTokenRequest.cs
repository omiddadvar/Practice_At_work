namespace Identity.WebAPI.Models.DTOs.Input;

public class RefreshTokenRequest
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}