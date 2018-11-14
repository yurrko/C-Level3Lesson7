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
    /// Контроллер характеров нарушений
    /// </summary>
    public class CharactersController : ApiController
    {
        private readonly ICharactersData _characterData = new CharacterData();

        // GET: api/Characters
        /// <summary>
        /// Возвращает все характеры нарушений
        /// </summary>
        /// <returns></returns>
        [MHserAuthorize( Roles = "user, writer, admin" )]
        public IEnumerable<Character> GetCharacters()
        {
            return _characterData.GetCharacters();
        }

        // GET: api/Characters/5
        /// <summary>
        /// Возвращает определенный характер нарушений
        /// </summary>
        /// <param name="id">id характера нарушений</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "user, writer, admin" )]
        [ResponseType( typeof( Character ) )]
        public IHttpActionResult GetCharacter( int id )
        {
            var character = _characterData.GetCharacterById( id );
            if ( character is null )
                return NotFound();

            return Ok( character );
        }

        // POST: api/Characters
        /// <summary>
        /// Добавить характер нарушений
        /// </summary>
        /// <param name="character">Объект Character</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "admin" )]
        [ResponseType( typeof( Character ) )]
        public IHttpActionResult PostCharacter( Character character )
        {
            if ( character == null )
            {
                ModelState.AddModelError( "", "Объект Character не может быть пустым" );
            }

            if ( !ModelState.IsValid )
            {
                return BadRequest( ModelState );
            }

            var cha = _characterData.AddCharacter( character );

            return CreatedAtRoute( "DefaultApi", new { id = cha.CharacterId }, cha );
        }

        // PUT: api/Characters/5
        /// <summary>
        /// Отредактировать характер нарушений
        /// </summary>
        /// <param name="id">id редактируемого характера нарушений</param>
        /// <param name="character">Объект Character</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "admin" )]
        [ResponseType( typeof( void ) )]
        public IHttpActionResult PutCharacter( int id, Character character )
        {
            if ( character == null )
            {
                ModelState.AddModelError( "", "Объект Character не может быть пустым" );
            }

            if ( !ModelState.IsValid )
            {
                return BadRequest( ModelState );
            }

            if ( id != character.CharacterId )
            {
                return BadRequest();
            }

            _characterData.ModifyCharacter( character );

            return StatusCode( HttpStatusCode.NoContent );
        }

        // DELETE: api/Characters/5
        /// <summary>
        /// Удалить характер нарушений
        /// </summary>
        /// <param name="id">id удаляемого характера нарушений</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "admin" )]
        [ResponseType( typeof( Character ) )]
        public IHttpActionResult DeleteCharacter( int id )
        {
            _characterData.DeleteCharacter( id );

            return Ok();
        }
    }
}