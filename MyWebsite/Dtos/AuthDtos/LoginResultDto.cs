﻿public class LoginResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}
