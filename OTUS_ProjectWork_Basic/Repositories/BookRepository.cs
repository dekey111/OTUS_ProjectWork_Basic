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
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(CloudReaderContext context) : base(context) { }

        public IEnumerable<Book> GetByAuthor(int authorId)
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Categories)
                .Where(b => b.Authorid == authorId && b.Isactive)
                .ToList();
        }

        public IEnumerable<Book> GetByCategory(int categoryId)
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Categories)
                .Where(b => b.Categories.Any(c => c.Id == categoryId) && b.Isactive)
                .ToList();
        }

        public IEnumerable<Book> Search(string searchTerm)
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Categories)
                .Where(b => (b.Title.Contains(searchTerm) ||
                            b.Description.Contains(searchTerm)) &&
                            b.Isactive)
                .ToList();
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public IEnumerable<Author> GetAllActiveAuthors()
        {
            return _context.Authors.Where(a => a.Isactive).ToList();
        }
    }
}
