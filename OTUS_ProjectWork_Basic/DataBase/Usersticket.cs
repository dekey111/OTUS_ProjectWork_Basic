using System;
using System.Collections.Generic;

namespace OTUS_ProjectWork_Basic.DataBase;

public partial class Usersticket
{
    /// <summary>
    /// Уникальный идентификатор записи о покупке
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
    /// Количество кинг
    /// </summary>
    public int? Quantity { get; set; }

    /// <summary>
    /// Итоговая цена покупки
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Дата покупки
    /// </summary>
    public DateTime? Purchasedate { get; set; }

    public virtual Book? Book { get; set; }

    public virtual User? User { get; set; }
}
