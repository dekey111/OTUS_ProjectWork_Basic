using System;
using System.Collections.Generic;

namespace OTUS_ProjectWork_Basic.DataBase;

public partial class Cart
{
    /// <summary>
    /// Уникальный идентификатор записи в корзине
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public int? Userid { get; set; }

    /// <summary>
    /// Идентификатор книги
    /// </summary>
    public int? Bookid { get; set; }

    /// <summary>
    /// Количество единиц книги в корзине
    /// </summary>
    public int? Quantity { get; set; }

    public virtual Book? Book { get; set; }

    public virtual User? User { get; set; }
}
