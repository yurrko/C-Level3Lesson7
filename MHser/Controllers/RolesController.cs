using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MHser.ActiveDirectoryInteraction;
using MHser.Domain.Entities;
using MHser.Domain.Interfaces;
using MHser.Services;

namespace MHser.Controllers
{
    /// <summary>
    /// Контроллер ролей пользователей
    /// </summary>
    public class RolesController : ApiController
    {
        private readonly IRolesData _userData = new UserData();
        // GET: api/Roles
        /// <summary>
        /// Получить все роли
        /// </summary>
        /// <returns></returns>
        [MHserAuthorize( Roles = "user, writer, admin" )]
        [ResponseType( typeof( List<Role>) )]
        public IEnumerable<Role> GetRoles()
        {
            return _userData.GetRoles();
        }

        // GET: api/Roles/5
        /// <summary>
        /// Получить определенную роль
        /// </summary>
        /// <param name="id">id роли</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "user, writer, admin" )]
        [ResponseType( typeof( Role ) )]
        public IHttpActionResult GetRole( int id )
        {
            var roleById = _userData.GetRoleById( id );
            if ( roleById is null )
            {
                return NotFound();
            }

            return Ok( roleById );
        }
    }
}
