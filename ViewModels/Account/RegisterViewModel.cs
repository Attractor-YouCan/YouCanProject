using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace YouCan.ViewModels.Account;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Заполните поле Эл почта")]
    [EmailAddress(ErrorMessage = "Некорректный адрес электронной почты")]
    [Remote(action: "CheckEmail", controller:"Validation", areaName: "", ErrorMessage = "Данный Email уже зарегистрирован!")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Заполните поле Номер телефона")]
    [RegularExpression(@"^0\d{9}$", ErrorMessage = "Заполните в формате x-цифра(0-9): 0 xxx xx xx xx")]
    [Remote(action: "CheckPhoneNumber", controller:"Validation", areaName:"", ErrorMessage = "Данный Номер телефона уже зарегистрирован!")]
    public string PhoneNumber { get; set; }
    [Required(ErrorMessage = "Заполните поле Никнейм")]
    [Remote(action: "CheckUsername", controller: "Validation", areaName: "", ErrorMessage = "Данный Никнейм уже занят!")]
    [RegularExpression(@"^\S+(?:\S+)?$", ErrorMessage = "Provide with no space!")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "Заполните поле Фамилия")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Заполните поле Имя")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Заполните поле Пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required(ErrorMessage = "Заполните поле Подтвердить пароль")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Не совпадают пароли, попробуйте еще раз!")]
    public string ConfirmPassword { get; set; }
    [Required(ErrorMessage = "Заполните поле Район!")]
    public string District { get; set; }
    [Required(ErrorMessage = "Заполните поле Дата рождения!")]
    [Remote(action: "CheckBirthDate", controller: "Validation", areaName: "", ErrorMessage = "Дата рождение не может быть в будущем или не ранее 100 лет!")]
    public DateTime BirthDate { get; set; }
    
}