using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OTUS_ProjectWork_Basic.DataBase;

namespace OTUS_ProjectWork_Basic.Models
{
    class BooksResponse
    {
        public BooksResponse() { }

        /// <summary>
        /// Уникальный идентификатор книги
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название книги
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// Идентификатор автора книги
        /// </summary>
        public int? AuthorID { get; set; }

        /// <summary>
        /// Описание книги
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Год публикации книги
        /// </summary>
        public int? PublicationYear { get; set; }

        /// <summary>
        /// Цена книги
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Количество экземпляров книги в наличии
        /// </summary>
        public int? StockQuantity { get; set; }

        /// <summary>
        /// Дата добавления книги в систему
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// Флаг активности книги (true = доступна, false = недоступна)
        /// </summary>
        public bool? IsActive { get; set; }

        public BooksResponse(Book book)
        {
            Id = book.Id;
            Title = book.Title;
            AuthorID = book.Authorid;
            Description = book.Description;
            PublicationYear = book.Publicationyear;
            Price = book.Price;
            StockQuantity = book.Stockquantity;
            CreateDate = book.Createdat;
            IsActive = book.Isactive;
        }
    }
}
