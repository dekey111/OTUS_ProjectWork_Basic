using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OTUS_ProjectWork_Basic.DataBase;

namespace OTUS_ProjectWork_Basic.Models
{
    class AuthorsResponse
    {
        public AuthorsResponse() { }
        /// <summary>
        /// Уникальный идентификатор автора
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Полное имя автора
        /// </summary>
        public string Fullname { get; set; } = null!;

        /// <summary>
        /// Дата добавления автора в систему
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// Флаг активности автора (true = активен, false = скрыт из каталога)
        /// </summary>
        public bool? IsActive { get; set; }

        public AuthorsResponse(Author author)
        {
            Id = author.Id;
            Fullname = author.Fullname;
            CreateDate = author.Createdat;
            IsActive = author.Isactive;
        }
    }
}
