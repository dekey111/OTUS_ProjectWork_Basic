using OTUS_ProjectWork_Basic.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTUS_ProjectWork_Basic.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        IEnumerable<Book> GetByAuthor(int authorId);
        IEnumerable<Book> GetByCategory(int categoryId);
        IEnumerable<Book> Search(string searchTerm);
        IEnumerable<Category> GetAllCategories();
        IEnumerable<Author> GetAllActiveAuthors();
    }
}
