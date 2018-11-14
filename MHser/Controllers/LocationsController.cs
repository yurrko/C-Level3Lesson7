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
    /// Контроллер местоположений
    /// </summary>
    public class LocationsController : ApiController
    {
        private readonly ILocationData _locationData = new LocationData();

        // GET: api/Locations
        /// <summary>
        /// Получить все местоположения
        /// </summary>
        /// <returns></returns>
        [MHserAuthorize( Roles = "user, writer, admin" )]
        public IEnumerable<Location> GetLocations()
        {
            return _locationData.GetLocations();
        }

        // GET: api/Locations/5
        /// <summary>
        /// Получить определенное местоположение
        /// </summary>
        /// <param name="id">id местоположения</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "user, writer, admin" )]
        [ResponseType( typeof( Location ) )]
        public IHttpActionResult GetLocation( int id )
        {
            var location = _locationData.GetLocationById( id );
            if ( location == null )
                return NotFound();

            return Ok( location );
        }

        // POST: api/Locations
        /// <summary>
        /// Добавить местоположение
        /// </summary>
        /// <param name="location">Объект Location</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "admin" )]
        [ResponseType( typeof( Location ) )]
        public IHttpActionResult PostLocation( Location location )
        {
            if ( location == null )
            {
                ModelState.AddModelError( "", "Объект Location не может быть пустым" );
            }

            // ReSharper disable once PossibleNullReferenceException
            if ( !(location.TypeOfObject == Constants.Messoyaha
                || location.TypeOfObject == Constants.Contractor) )
            {
                ModelState.AddModelError( "", $"Неверное значение в поле TypeOfObject. Допустимы {Constants.Messoyaha} или {Constants.Contractor}");
            }

            if ( !ModelState.IsValid )
            {
                return BadRequest( ModelState );
            }

            var loc = _locationData.AddLocation( location );

            return CreatedAtRoute( "DefaultApi", new { id = loc.LocationId }, loc );
        }

        // PUT: api/Locations/5
        /// <summary>
        /// Редактирование местоположения
        /// </summary>
        /// <param name="id">id редактируемого местоположения</param>
        /// <param name="location">Объект Location</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "admin" )]
        [ResponseType( typeof( void ) )]
        public IHttpActionResult PutLocation( int id, Location location )
        {
            if ( location == null )
            {
                ModelState.AddModelError( "", "Объект Location не может быть пустым" );
            }

            if ( !(location.TypeOfObject == Constants.Messoyaha
                || location.TypeOfObject == Constants.Contractor) )
            {
                ModelState.AddModelError( "", $"Неверное значение в поле TypeOfObject. Допустимы {Constants.Messoyaha} или {Constants.Contractor}");
            }

            if ( !ModelState.IsValid )
            {
                return BadRequest( ModelState );
            }

            if ( id != location.LocationId )
            {
                return BadRequest();
            }

            _locationData.ModifyLocation( location );

            return StatusCode( HttpStatusCode.NoContent );
        }

        // DELETE: api/Locations/5
        /// <summary>
        /// Удаление местоположения
        /// </summary>
        /// <param name="id">id удаляемого местоположения</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "admin" )]
        [ResponseType( typeof( void ) )]
        public IHttpActionResult DeleteLocation( int id )
        {
            _locationData.DeleteLocation( id );

            return Ok();
        }
    }
}