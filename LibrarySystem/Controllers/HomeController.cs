using LibrarySystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers;

public class HomeController : Controller
{
    private readonly IBookRepository _repository;

    public HomeController(IBookRepository repository)
    {
        _repository = repository;
    }

    [Authorize]
    public IActionResult Index() => View(_repository.Books);

    [Authorize]
    public ViewResult Create() => View("Edit", new Book());

    [Authorize]
    public ViewResult Edit(int bookId) => View(_repository.Books
        .FirstOrDefault(b => b.Id == bookId));

    [HttpPost]
    public IActionResult Edit(Book book)
    {
        if (ModelState.IsValid)
        {
            _repository.SaveBook(book);
            TempData["message"] = $"Изменения в книге {book.Title} были сохранены";
            return RedirectToAction("Index");
        }
        else
            return View(book);
    }

    [HttpPost]
    public IActionResult Delete(int bookId)
    {
        _repository.Delete(bookId);
        return RedirectToAction("Index");
    }
}
