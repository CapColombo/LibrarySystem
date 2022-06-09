namespace LibrarySystem.Models;

public interface IUserRepository
{
    IQueryable<User> Users { get; }
    void Add(User user);
}
