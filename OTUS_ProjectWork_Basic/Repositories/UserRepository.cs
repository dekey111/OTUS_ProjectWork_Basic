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
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(CloudReaderContext context) : base(context) { }

        public User GetOrCreateUserAsync(int telegramId, string accountName, string username, DateTime createdate, bool isActiv)
        {
            var user =  _context.Users
                .FirstOrDefault(u => u.Id == telegramId);

            if (user == null)
            {
                user = new User
                {
                    Id = telegramId,
                    Accountname = accountName,
                    Username = username,
                    Createdat = createdate,
                    Isactive = isActiv
                };

                _context.Users.Add(user);
                _context.SaveChanges();
            }

            return user;
        }
    }
}
