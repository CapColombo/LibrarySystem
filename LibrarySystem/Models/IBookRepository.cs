namespace LibrarySystem.Models
{
    public interface IBookRepository
    {
        IQueryable<Book> Books { get; }
        void Add(Book book);
        void SaveBook(Book book);
        void Delete(int id);
    }
}
