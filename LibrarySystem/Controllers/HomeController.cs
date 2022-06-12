using LibrarySystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IBookRepository _repository;

    public HomeController(IBookRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index() => View(_repository.Books);

    public ActionResult GetBooks() => Json(_repository.Books);

    [HttpPost]
    public IActionResult SaveBook(Book book)
    {
        _repository.SaveBook(book);
        return RedirectToAction("Index");
    }

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
