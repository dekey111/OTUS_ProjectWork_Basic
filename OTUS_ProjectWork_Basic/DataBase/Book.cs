using System;
using System.Collections.Generic;

namespace OTUS_ProjectWork_Basic.DataBase;

public partial class Book
{
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
    public int? Authorid { get; set; }

    /// <summary>
    /// Описание книги
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Год публикации книги
    /// </summary>
    public int? Publicationyear { get; set; }

    /// <summary>
    /// Цена книги
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Количество экземпляров книги в наличии
    /// </summary>
    public int? Stockquantity { get; set; }

    /// <summary>
    /// Дата добавления книги в систему
    /// </summary>
    public DateTime? Createdat { get; set; }

    /// <summary>
    /// Флаг активности книги (true = доступна, false = недоступна)
    /// </summary>
    public bool? Isactive { get; set; }

    public virtual Author? Author { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Usersticket> Userstickets { get; set; } = new List<Usersticket>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
