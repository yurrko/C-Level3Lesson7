using MHser.ActiveDirectoryInteraction;
using MHser.Domain.Entities;
using MHser.Domain.Interfaces;
using MHser.Services;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using MHser.Logger;
using Swashbuckle.Swagger.Annotations;

namespace MHser.Controllers
{
    /// <summary>
    /// Контроллер категорий нарушений
    /// </summary>
    public class CategoriesController : ApiController
    {
        private readonly MyLogger _logger = MyLogger.GetLogger();
        private readonly ICategoriesData _categoriesData = new CategoriesData();

        // GET: api/Categories
        /// <summary>
        /// Возвращает все категории
        /// </summary>
        /// <returns></returns>
        [MHserAuthorize( Roles = "user, writer, admin" )]
        public IEnumerable<Category> GetCategories()
        {
            #region Log
            _logger.WriteLog( "Info", ErrorMessages.RequestPart( User.Identity.Name, Request.RequestUri.AbsolutePath, Request.Method.Method ) );
            #endregion
            return _categoriesData.GetCategories();
        }

        // GET: api/Categories/5
        /// <summary>
        /// Возвращает определенную категорию
        /// </summary>
        /// <param name="id">id категории</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "user, writer, admin" )]
        [ResponseType( typeof( Category ) )]
        public IHttpActionResult GetCategory( int id )
        {
            #region Log
            _logger.WriteLog( "Info", ErrorMessages.RequestPart( User.Identity.Name, Request.RequestUri.AbsolutePath, Request.Method.Method ) );
            #endregion
            var category = _categoriesData.GetCategoryById( id );
            if ( category is null )
            {
                #region Log
                _logger.WriteLog( "Warn", ErrorMessages.ObjectNull( nameof( Category ) ) );
                #endregion
                return NotFound();
            }

            return Ok( category );
        }

        // POST: api/Categories
        /// <summary>
        /// Добавляет категорию
        /// </summary>
        /// <param name="category">Объект Category</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "admin" )]
        [ResponseType( typeof( Category ) )]
        public IHttpActionResult PostCategory( Category category )
        {
            #region Log
            _logger.WriteLog( "Info", ErrorMessages.RequestPart( User.Identity.Name, Request.RequestUri.AbsolutePath, Request.Method.Method ) );
            #endregion
            if ( category is null )
            {
                #region Log
                _logger.WriteLog( "Warn", ErrorMessages.ObjectNull( nameof( Category ) ) );
                #endregion
                ModelState.AddModelError( "", ErrorMessages.ObjectNull( nameof( Category ) ) );
            }

            if ( !ModelState.IsValid )
            {
                #region Log
                _logger.WriteLog( "Warn", $"Объект {nameof( Category )} некорректен" );
                #endregion
                return BadRequest( ModelState );
            }

            var cat = _categoriesData.AddCategory( category );

            return CreatedAtRoute( "DefaultApi", new { id = cat.CategoryId }, cat );
        }

        // PUT: api/Categories/5
        /// <summary>
        /// Редактирует категорию
        /// </summary>
        /// <param name="id">id редактируемой категории</param>
        /// <param name="category">Объект Category</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "admin" )]
        [ResponseType( typeof( void ) )]
        public IHttpActionResult PutCategory( int id, Category category )
        {
            #region Log
            _logger.WriteLog( "Info", ErrorMessages.RequestPart( User.Identity.Name, Request.RequestUri.AbsolutePath, Request.Method.Method ) );
            #endregion
            if ( category is null )
            {
                #region Log
                _logger.WriteLog( "Warn", ErrorMessages.ObjectNull( nameof( Category ) ) );
                #endregion
                ModelState.AddModelError( "", ErrorMessages.ObjectNull( nameof( Category ) ) );
            }

            if ( !ModelState.IsValid )
            {
                #region Log
                _logger.WriteLog( "Warn", $"Объект {nameof( Category )} некорректен" );
                #endregion
                return BadRequest( ModelState );
            }

            if ( id != category.CategoryId )
            {
                #region Log
                _logger.WriteLog( "Warn", ErrorMessages.IdDismatch( id, category.CategoryId ) );
                #endregion
                return BadRequest();
            }

            _categoriesData.ModifyCategory( category );

            return StatusCode( HttpStatusCode.NoContent );
        }

        // DELETE: api/Categories/5
        /// <summary>
        /// Удаляет категорию
        /// </summary>
        /// <param name="id">id удаляемой категории</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "admin" )]
        [ResponseType( typeof( void ) )]
        public IHttpActionResult DeleteCategory( int id )
        {
            #region Log
            _logger.WriteLog( "Info", ErrorMessages.RequestPart( User.Identity.Name, Request.RequestUri.AbsolutePath, Request.Method.Method ) );
            #endregion
            _categoriesData.DeleteCategory( id );

            return Ok();
        }
    }
}