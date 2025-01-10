namespace Order.Infrastructure;
public class TokenSettings
{
    public string Key { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string Issuer { get; set; } = default!;
}