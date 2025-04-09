using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OTUS_ProjectWork_Basic.DataBase;

namespace OTUS_ProjectWork_Basic.Models
{
    class UsersResponse
    {
        public UsersResponse() { }

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
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Флаг активности пользователя (true = активен, false = заблокирован/удален)
        /// </summary>
        public bool Isactive { get; set; }
        public UsersResponse(User user)
        {
            Id = user.Id;
            Accountname = user.Accountname;
            Username = user.Username;
            CreateDate = user.Createdat;
        }
    }
}
