using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models;

public class Book
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введите название книги")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Введите год издания книги")]
    [Range(1, 2030, ErrorMessage = "Введите корректную дату")]
    public int Year { get; set; }

    [Required(ErrorMessage = "Введите название жанра книги")]
    public string Genre { get; set; }

    [Required(ErrorMessage = "Введите автора книги")]
    public string Author { get; set; }

}
