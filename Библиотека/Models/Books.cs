using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Библиотека.Models
{
    public class Books
    {
        public int ID_Книга { get; set; }
        public string Название { get; set; }
        public string Автор { get; set; }
        public string Год_написания { get; set; }
        public string Описание { get; set; }
        public byte[] Фото { get; set; }
    }
}