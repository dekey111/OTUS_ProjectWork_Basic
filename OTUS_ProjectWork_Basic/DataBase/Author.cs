using System;
using System.Collections.Generic;

namespace OTUS_ProjectWork_Basic.DataBase;

public partial class Author
{
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
    public DateTime? Createdat { get; set; }

    /// <summary>
    /// Флаг активности автора (true = активен, false = скрыт из каталога)
    /// </summary>
    public bool Isactive { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
