using OTUS_ProjectWork_Basic.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTUS_ProjectWork_Basic.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetOrCreateUserAsync(int telegramId, string accountName, string username, DateTime createdate, bool isActiv);
    }
}
