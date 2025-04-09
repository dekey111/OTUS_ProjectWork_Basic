using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OTUS_ProjectWork_Basic.DataBase;

namespace OTUS_ProjectWork_Basic.Models
{
    class CartResponse
    {
        public CartResponse() { }

        /// <summary>
        /// Уникальный идентификатор записи в корзине
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int? UserID { get; set; }

        /// <summary>
        /// Идентификатор книги
        /// </summary>
        public int? BookID { get; set; }

        /// <summary>
        /// Количество единиц книги в корзине
        /// </summary>
        public int? Quantity { get; set; }

        public CartResponse(Cart cart)
        {
            Id = cart.Id;
            UserID = cart.Userid;
            BookID = cart.Bookid;
            Quantity = cart.Quantity;
        }
    }
}
