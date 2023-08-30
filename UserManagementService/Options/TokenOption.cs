namespace UserManagementService.Options;

public class TokenOption
{
    public string Audience { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public string Key { get; set; } = default!;
    public int ExpirationMinutes { get; set; } 
}
