using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace YouCan.Mvc.ViewModels.Account;

public class RegisterViewModel
{
    [Required(ErrorMessage = "EmailRequired")]
    [EmailAddress(ErrorMessage = "EmailAddress")]
    [Remote(action: "CheckEmail", controller:"Validation", areaName: "", ErrorMessage = "RemoteCheckEmail")]
    public string Email { get; set; }
    [Required(ErrorMessage = "PhoneNumberRequired")]
    [RegularExpression(@"^0\d{9}$", ErrorMessage = "PhoneNumberRegularExpression")]
    [Remote(action: "CheckPhoneNumber", controller: "Validation", areaName: "", ErrorMessage = "RemoteCheckPhoneNumber")]
    public string PhoneNumber { get; set; }
    [Required(ErrorMessage = "UserNameRequired")]
    [Remote(action: "CheckUsername", controller: "Validation", areaName: "", ErrorMessage = "RemoteCheckUserName")]
    [RegularExpression(@"^\S+(?:\S+)?$", ErrorMessage = "Provide with no space!")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "LastNameRequired")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "FirstNameRequired")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "PasswordRequired")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required(ErrorMessage = "ConfirmPasswordRequired")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "ConfirmPasswordCompare")]
    public string ConfirmPassword { get; set; }
    [Required(ErrorMessage = "DistrictRequired")]
    public string District { get; set; }
    [Required(ErrorMessage = "BirthDateRequired")]
    [Remote(action: "CheckBirthDate", controller: "Validation", areaName: "", ErrorMessage = "RemoteCheckBirthDate")]
    public DateTime BirthDate { get; set; }
    
}