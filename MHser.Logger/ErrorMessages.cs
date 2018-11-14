using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHser.Logger
{
    public class ErrorMessages
    {
        public static string ObjectNull( string nameOfObject ) => $"Объект {nameOfObject} не может быть пустым";

        public static string RequestPart( string userName, string path, string method ) =>
            $"Пользователь {userName}\t{path}\t{method}";

        public static string IdDismatch(int id, int catId) => $"id запроса {id} не совпадает с id объекта {catId}";

        public static string ObjectNotFound() => "Объект не существует или был удален ранее";
    }
}
