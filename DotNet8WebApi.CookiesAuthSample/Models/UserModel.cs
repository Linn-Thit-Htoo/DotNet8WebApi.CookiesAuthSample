namespace DotNet8WebApi.CookiesAuthSample.Models;

public class UserModel
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string UserRole { get; set; }
    public bool IsActive { get; set; }
}
