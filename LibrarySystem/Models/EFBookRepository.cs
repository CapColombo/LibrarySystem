namespace LibrarySystem.Models;

public class EFBookRepository : IBookRepository
{
    private readonly LibraryDbContext _context;

    public EFBookRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public IQueryable<Book> Books => _context.Books;

    public void Add(Book book)
    {
        _context.Books.Add(book);
        _context.SaveChanges();
    }

    public void SaveBook(Book book)
    {
        if (book.Id == 0)
            _context.Books.Add(book);
        else
        {
            Book dbEntry = _context.Books
                .FirstOrDefault(b => b.Id == book.Id);

            if (dbEntry != null)
            {
                dbEntry.Title = book.Title;
                dbEntry.Year = book.Year;
                dbEntry.Genre = book.Genre;
                dbEntry.Author = book.Author;
            }
        }
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == id);
        _context.Remove(book);
        _context.SaveChanges();
    }
}
