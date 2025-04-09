using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OTUS_ProjectWork_Basic.DataBase;

namespace OTUS_ProjectWork_Basic.Models
{
    class UsersTickerResponse
    {
        public UsersTickerResponse() { }

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

        public UsersTickerResponse(Usersticket usersticket)
        {
            Id = usersticket.Id;
            Userid = usersticket.Userid;
            Bookid = usersticket.Bookid;
            Quantity = usersticket.Quantity;
            Price = usersticket.Price;
            Purchasedate = usersticket.Purchasedate;
        }
    }
}
