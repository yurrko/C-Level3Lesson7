using MHser.ActiveDirectoryInteraction;
using MHser.Domain.Entities;
using MHser.Domain.Interfaces;
using MHser.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Linq;

namespace MHser.Controllers
{
    /// <summary>
    /// Контроллер управления пользователями
    /// </summary>
    public class UsersController : ApiController
    {
        private readonly IUserData _userData = new UserData();

        // GET: api/Users
        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        /// <returns></returns>
        [MHserAuthorize( Roles = "user, writer, admin" )]
        public Array GetUsers()
        {
            return _userData.GetUsers().Select( u => new
            {
                u.UserId,
                u.Name,
                u.AdName,
                u.Position,
                u.Company,
                u.LastAccess,
                Roles = u.Roles.Select( r => r.RoleName ).ToArray(),
            } ).ToArray();
        }

        // GET: api/Users/5
        /// <summary>
        /// Получить определенного пользователя
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "user, writer, admin" )]
        [ResponseType( typeof( User ) )]
        public IHttpActionResult GetUser( int id )
        {
            var userById = _userData.GetUserById( id );
            if ( userById is null )
            {
                return NotFound();
            }

            return Ok( userById );
        }

        // POST: api/Users
        /// <summary>
        /// Добавить пользователя
        /// </summary>
        /// <param name="user">Объект User</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "admin" )]
        [ResponseType( typeof( User ) )]
        public IHttpActionResult PostUser( User user )
        {
            if ( user == null )
            {
                ModelState.AddModelError( "", "Объект User не может быть пустым" );
            }

            if ( !ModelState.IsValid )
            {
                return BadRequest( ModelState );
            }

            var dec = _userData.AddUser( user );

            return CreatedAtRoute( "DefaultApi", new { id = dec.UserId }, dec );
        }

        // PUT: api/Users/5
        /// <summary>
        /// Отредактировать пользователя
        /// </summary>
        /// <param name="id">id редактируемого пользователя</param>
        /// <param name="user">Объект User</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "admin" )]
        [ResponseType( typeof( void ) )]
        public IHttpActionResult PutUser( int id, User user )
        {
            if ( user == null )
            {
                ModelState.AddModelError( "", "Объект User не может быть пустым" );
            }

            if ( !ModelState.IsValid )
            {
                return BadRequest( ModelState );
            }

            // ReSharper disable once PossibleNullReferenceException
            if ( id != user.UserId )
            {
                return BadRequest();
            }

            _userData.ModifyUser( user );

            return StatusCode( HttpStatusCode.NoContent );
        }

        // DELETE: api/Users/5
        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="id">id удаляемого пользователя</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "admin" )]
        [ResponseType( typeof( void ) )]
        public IHttpActionResult DeleteUser( int id )
        {
            _userData.DeleteUser( id );

            return Ok();
        }

        /// <summary>
        /// Редактирование ролей пользователя
        /// </summary>
        /// <param name="userId">id пользователя</param>
        /// <param name="roles">массив ролей</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "admin" )]
        [Route( "api/Users/{userId}/modifyRoles" )]
        [HttpPut]
        public IHttpActionResult PutModifyRoles( int userId, string[] roles )
        {
            return Ok( _userData.ModifyRoles( userId, roles ) );
        }

        /// <summary>
        /// Получить авторизационную информацию о себе
        /// </summary>
        /// <returns></returns>
        [Route( "api/Users/Me" )]
        [HttpGet]
        public IHttpActionResult GetMe()
        {
            var adName = User.Identity.Name;

            if ( Debugger.IsAttached )
                adName = String.IsNullOrWhiteSpace( adName ) ? "GAZPROM-NEFT\\Denisevich.YuV" : adName;

            var user = _userData.GetUserByAdName( adName );
            if ( user is null )
            {
                try
                {
                    var adProv = new ProviderActiveDirectory();
                    var adNameWithoutGaz = adName.Remove( 0, adName.IndexOf( "\\" ) + 1 );
                    var adUser = adProv.GetUserFromAD( adNameWithoutGaz );
                    user = _userData.AddUser( adUser );
                }
                catch ( Exception )
                {
                    user = _userData.AddUser( adName );
                }
            }

            var strRoles = user.Roles.Select( r => r.RoleName ).ToArray();

            return Ok( new
            {
                user.UserId,
                user.Name,
                user.AdName,
                user.Position,
                user.Company,
                user.LastAccess,
                Roles = strRoles,
            } );
        }
    }
}