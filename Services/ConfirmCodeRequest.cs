namespace YouCan.Services;

public class ConfirmCodeRequest
{
    public string Email { get; set; }
    public string Code { get; set; }
}