using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace YouCan.Mvc;

public class EditViewModel
{
    [Required(ErrorMessage = "Заполните поле ФИО")]
    public string FullName { get; set; }
    [Required(ErrorMessage = "Заполните поле Номер телефона")]
    [RegularExpression(@"^0\d{9}$", ErrorMessage = "Заполните в формате x-цифра(0-9): 0 xxx xx xx xx")]
    [Remote(action: "CheckPhoneNumber", controller: "Validation", ErrorMessage = "Данный номер уже занят!")]
    public string PhoneNumber { get; set; }
    [Required(ErrorMessage = "Заполните поле Регион!")]
    public string District { get; set; }
    [Remote(action: "CheckBirthDate", controller: "Validation", ErrorMessage = "Дата рождения не может быть в будущем или не ранее 100 лет!")]
    public DateOnly BirthDate { get; set; }
    public IFormFile? UploadedFile { get; set; }
}
