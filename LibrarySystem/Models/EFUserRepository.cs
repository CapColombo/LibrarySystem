namespace LibrarySystem.Models;

public class EFUserRepository : IUserRepository
{
    private readonly LibraryDbContext _context;

    public EFUserRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public IQueryable<User> Users => _context.Users;

    public void Add(User user)
    {
        _context.Users.Add(user);
        _context.SaveChangesAsync();
    }
}
