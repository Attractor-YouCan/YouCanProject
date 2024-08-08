using System.ComponentModel.DataAnnotations;

namespace YouCan.Mvc;
public class LoginViewModel
{
    [Required(ErrorMessage = "Заполните поле!")]
    public string LoginValue { get; set; }
    [Required(ErrorMessage = "Заполните пароль")]
    public string Password { get; set; }
    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
   
    public string? ReturnUrl { get; set; }
}