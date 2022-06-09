using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models.ViewModels;

public class RegisterModel
{
    [Required (ErrorMessage = "Введите логин")]
    public string Login { get; set; }

    [DataType(DataType.Password)]
    [Required (ErrorMessage = "Введите пароль")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; }
}
