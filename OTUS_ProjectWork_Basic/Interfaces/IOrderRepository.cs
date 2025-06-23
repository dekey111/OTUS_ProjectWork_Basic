using OTUS_ProjectWork_Basic.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTUS_ProjectWork_Basic.Interfaces
{
    public interface IOrderRepository : IRepository<Usersticket>
    {
        IEnumerable<Usersticket> GetUserOrders(int userId);
        void CreateOrder(int userId, IEnumerable<Cart> cartItems);
    }
}
