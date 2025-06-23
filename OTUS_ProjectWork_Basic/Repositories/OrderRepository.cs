using Microsoft.EntityFrameworkCore;
using OTUS_ProjectWork_Basic.DataBase;
using OTUS_ProjectWork_Basic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTUS_ProjectWork_Basic.Repositories
{
    public class OrderRepository : Repository<Usersticket>, IOrderRepository
    {
        public OrderRepository(CloudReaderContext context) : base(context) { }

        public IEnumerable<Usersticket> GetUserOrders(int userId)
        {
            return _context.Userstickets
                .Include(o => o.Book)
                .ThenInclude(b => b.Author)
                .Where(o => o.Userid == userId)
                .OrderByDescending(o => o.Purchasedate)
                .ToList();
        }

        public void CreateOrder(int userId, IEnumerable<Cart> cartItems)
        {
            var orderItems = cartItems.Select(item => new Usersticket
            {
                Userid = userId,
                Bookid = item.Bookid,
                Quantity = item.Quantity,
                Price = item.Book.Price * (item.Quantity ?? 1),
                Purchasedate = DateTime.Now
            }).ToList();

            _context.Userstickets.AddRange(orderItems);
            _context.SaveChanges();
        }
    }
}
