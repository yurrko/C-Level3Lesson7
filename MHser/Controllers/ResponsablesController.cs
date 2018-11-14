using MHser.ActiveDirectoryInteraction;
using MHser.Domain.Entities;
using MHser.Domain.Interfaces;
using MHser.Services;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace MHser.Controllers
{
    /// <summary>
    /// Контроллер ответственных на объекте
    /// </summary>
    public class ResponsablesController : ApiController
    {
        private readonly IResponsablesData _responsablesData = new ResponsablesData();

        /// <summary>
        /// Получить все ответственных на объектах
        /// </summary>
        /// <returns></returns>
        [MHserAuthorize( Roles = "user, writer, admin" )]
        public IEnumerable<Responsable> GetResponsables()
        {
            return _responsablesData.GetResponsables();
        }

        // GET: api/Responsables/5
        /// <summary>
        /// Получить определенного ответственного на объекте
        /// </summary>
        /// <param name="id">id ответственного на объекте</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "user, writer, admin" )]
        [ResponseType( typeof( Responsable ) )]
        public IHttpActionResult GetResponsable( int id )
        {
            var responsable = _responsablesData.GetResponsableById( id );
            if ( responsable == null )
                return NotFound();

            return Ok( responsable );
        }

        // POST: api/Responsables
        /// <summary>
        /// Добавить ответственного на объекте
        /// </summary>
        /// <param name="responsable">объект Responsable</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "admin" )]
        [ResponseType( typeof( Responsable ) )]
        public IHttpActionResult PostResponsable( Responsable responsable )
        {
            if ( responsable == null )
            {
                ModelState.AddModelError( "", "Объект Responsable не может быть пустым" );
            }

            if ( !ModelState.IsValid )
            {
                return BadRequest( ModelState );
            }

            var res = _responsablesData.AddResponsable( responsable );

            return CreatedAtRoute( "DefaultApi", new { id = res.ResponsableId }, res );
        }

        // PUT: api/Responsables/5
        /// <summary>
        /// Отредактировать ответственного на объекте
        /// </summary>
        /// <param name="id">id редактируемого ответственного на объекте</param>
        /// <param name="responsable">объект Responsable</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "admin" )]
        [ResponseType( typeof( void ) )]
        public IHttpActionResult PutResponsable( int id, Responsable responsable )
        {
            if ( responsable == null )
            {
                ModelState.AddModelError( "", "Объект Responsable не может быть пустым" );
            }

            if ( !ModelState.IsValid )
            {
                return BadRequest( ModelState );
            }

            if ( id != responsable.ResponsableId )
            {
                return BadRequest();
            }
            _responsablesData.ModifyResponsable( responsable );

            return StatusCode( HttpStatusCode.NoContent );
        }


        // DELETE: api/Responsables/5
        /// <summary>
        /// Удаление ответственного на объекте
        /// </summary>
        /// <param name="id">id удаляемого ответственного на объекте</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "admin" )]
        [ResponseType( typeof( void ) )]
        public IHttpActionResult DeleteResponsable( int id )
        {
            _responsablesData.DeleteResponsable( id );

            return Ok();
        }
    }
}