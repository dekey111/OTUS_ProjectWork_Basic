using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OTUS_ProjectWork_Basic.DataBase;

namespace OTUS_ProjectWork_Basic.Models
{
    class CategoriesResponse
    {
        public CategoriesResponse() { }

        /// <summary>
        /// Уникальный идентификатор категории
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название категории
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Описание категории
        /// </summary>
        public string? Description { get; set; }

        public CategoriesResponse(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            Description = category.Description;
        }
    }
}
