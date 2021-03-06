namespace LibrarySystem.Models
{
    public interface IBookRepository
    {
        IQueryable<Book> Books { get; }
        void SaveBook(Book book);
        void Delete(int id);
    }
}
