using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models.ViewModels;

public class LoginModel
{
    [Required(ErrorMessage = "Введите логин")]
    public string Login { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Введите пароль")]
    public string Password { get; set; }
}
