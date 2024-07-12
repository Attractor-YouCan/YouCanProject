using System.ComponentModel.DataAnnotations;
namespace YouCan.Entities;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Заполните поле Email")]
    //[Remote(action: "CheckEmail", controller:"Validation", ErrorMessage = "Данный Email уже занят!")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Заполните поле Phone Number")]
    [RegularExpression(@"^0\d{9}$", ErrorMessage = "Заполните в формате x-цифра(0-9): 0 xxx xx xx xx")]
    //[Remote(action: "CheckPhoneNumber", controller:"Validation", ErrorMessage = "Данный PhoneNumber уже занят!")]
    public string PhoneNumber { get; set; }
    [Required(ErrorMessage = "Заполните поле Username")]
    //[Remote(action: "CheckUsername", controller:"Validation", ErrorMessage = "Данный UserName уже занят!")]
    [RegularExpression(@"^\S+(?:\S+)?$", ErrorMessage = "Provide with no space!")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "Заполните поле LastName")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Заполните поле FirstName")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Заполните поле Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required(ErrorMessage = "Заполните поле Confirm Password")]
    [DataType(DataType.Password)]
    [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Неверный Confirm Password, попробуйте еще раз!")]
    public string ConfirmPassword { get; set; }
    [Required(ErrorMessage = "Заполните поле District!")]
    public string District { get; set; }
    [Required(ErrorMessage = "Заполните поле District!")]
    //[Remote(action: "CheckBirthDate", controller:"Validation", ErrorMessage = "Дата рождение не может быть в будущем или не ранее 100 лет!")]
    public DateTime BirthDate { get; set; }
    
}