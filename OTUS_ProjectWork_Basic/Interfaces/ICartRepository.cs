using OTUS_ProjectWork_Basic.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTUS_ProjectWork_Basic.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        IEnumerable<Cart> GetUserCart(int userId);
        void AddOrUpdateItem(int userId, int bookId, int quantity = 1);
        void RemoveItem(int cartItemId);
        void ClearCart(int userId);
        decimal CalculateTotal(int userId);
    }
}
