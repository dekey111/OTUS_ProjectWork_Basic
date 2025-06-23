using Microsoft.EntityFrameworkCore;
using OTUS_ProjectWork_Basic.DataBase;
using OTUS_ProjectWork_Basic.Interfaces;

namespace OTUS_ProjectWork_Basic.Repositories
{
public class CartRepository : Repository<Cart>, ICartRepository
{
    public CartRepository(CloudReaderContext context) : base(context) { }

    public IEnumerable<Cart> GetUserCart(int userId)
    {
            var userExists = _context.Users.Any(u => u.Id == userId);

            return _context.Carts
            .Include(c => c.Book)
            .ThenInclude(b => b.Author)
            .Where(c => c.Userid == userId)
            .ToList();
    }

        public void AddOrUpdateItem(int userId, int bookId, int quantity = 1)
        {
            // First check if user exists
            var userExists = _context.Users.Any(u => u.Id == userId);
            if (!userExists)
            {
                throw new ArgumentException($"User with ID {userId} does not exist");
            }

            // Then check if book exists
            var bookExists = _context.Books.Any(b => b.Id == bookId);
            if (!bookExists)
            {
                throw new ArgumentException($"Book with ID {bookId} does not exist");
            }

            // Proceed with cart operation
            var existingItem = _context.Carts
                .FirstOrDefault(c => c.Userid == userId && c.Bookid == bookId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                _context.Carts.Update(existingItem);
            }
            else
            {
                _context.Carts.Add(new Cart
                {
                    Userid = userId,
                    Bookid = bookId,
                    Quantity = quantity
                });
            }
            _context.SaveChanges();
        }

        public void RemoveItem(int cartItemId)
    {
        var item = _context.Carts.Find(cartItemId);
        if (item != null)
        {
            _context.Carts.Remove(item);
            _context.SaveChanges();
        }
    }

    public void ClearCart(int userId)
    {
        var items = _context.Carts.Where(c => c.Userid == userId).ToList();
        _context.Carts.RemoveRange(items);
        _context.SaveChanges();
    }

    public decimal CalculateTotal(int userId)
    {
        return _context.Carts
            .Include(c => c.Book)
            .Where(c => c.Userid == userId)
            .Sum(c => c.Book.Price * (c.Quantity ?? 1));
    }
}
}
