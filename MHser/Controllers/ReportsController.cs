using MHser.Domain.Interfaces;
using System.Web.Http;
using System.Web.Http.Description;
using MHser.ActiveDirectoryInteraction;
using MHser.Services;

namespace MHser.Controllers
{
    /// <summary>
    /// Контроллер отчётов
    /// </summary>
    public class ReportsController : ApiController
    {
        private readonly IDisruptionData _disruptionData = new DisruptionData();

        /// <summary>
        /// Возвращает данные для отрисовки представления Органайзер
        /// </summary>
        /// <returns></returns>
        [MHserAuthorize( Roles = "user, writer, admin" )]
        [Route( "api/Reports/Organizer" )]
        [ResponseType( typeof(object))]
        public IHttpActionResult GetOrganaizer()
        {
            var orgData = _disruptionData.GetOrgData();
            return Ok( orgData );
        }

        /// <summary>
        /// Возвращает данные для отрисовки таблицы с количеством нарушений по месяцам
        /// </summary>
        /// <returns></returns>
        [MHserAuthorize( Roles = "user, writer, admin" )]
        [Route( "api/Reports/MainReport" )]
        [ResponseType( typeof( object ) )]
        public IHttpActionResult GetMainReport()
        {
            var resData = _disruptionData.GetMainReport();
            return Ok( resData );
        }
    }
}
