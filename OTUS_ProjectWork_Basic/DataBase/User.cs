using System;
using System.Collections.Generic;

namespace OTUS_ProjectWork_Basic.DataBase;

public partial class User
{
    /// <summary>
    /// Уникальный идентификатор пользователя
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Логин пользователя (уникальный)
    /// </summary>
    public string Accountname { get; set; } = null!;

    /// <summary>
    /// Имя пользователя (обязательное )
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    /// Дата регистрации пользователя
    /// </summary>
    public DateTime Createdat { get; set; }

    /// <summary>
    /// Флаг активности пользователя (true = активен, false = заблокирован/удален)
    /// </summary>
    public bool Isactive { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Usersticket> Userstickets { get; set; } = new List<Usersticket>();
}
