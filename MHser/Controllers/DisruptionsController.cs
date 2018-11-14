using MHser.ActiveDirectoryInteraction;
using MHser.Domain.Entities;
using MHser.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MHser.Logger;
using MHser.Services;
using System.IO;
using System.Linq;

namespace MHser.Controllers
{
    /// <summary>
    /// Контроллер нарушений
    /// </summary>
    public class DisruptionsController : ApiController
    {
        private readonly MyLogger _logger = MyLogger.GetLogger();
        private readonly IUserData _userData = new UserData();
        private readonly IDisruptionData _disruptionData = new DisruptionData();
        private readonly IDisruptionLogData _disruptionLog = new DisruptionLogData();

        // GET: api/Disruptions
        /// <summary>
        /// Получить все нарушения
        /// </summary>
        /// <returns></returns>
        [MHserAuthorize( Roles = "user, writer, admin" )]
        [ResponseType( typeof( List<Disruption> ) )]
        public IHttpActionResult GetDisruptions()
        {
            #region Log
            _logger.WriteLog( "Info", ErrorMessages.RequestPart( User.Identity.Name, Request.RequestUri.AbsolutePath, Request.Method.Method ) );
            #endregion
            var user = _userData.GetUserByAdName( User.Identity.Name );
            var lastAccess = user.LastAccess;
            _userData.UpdateLastAccess( user.UserId );

            var disruptions = _disruptionData.GetDisruptions();
            return Ok( new { disruptions, lastAccess } );
        }

        // GET: api/Disruptions/WithFilter
        /// <summary>
        /// Получить нарушения в соответствии с фильтром
        /// </summary>
        /// <param name="disruptionFilter">Объект фильтрации нарушений</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "user, writer, admin" )]
        [Route( "api/Disruptions/WithFilter/" )]
        [AcceptVerbs( "POST" )]
        [ResponseType( typeof( List<Disruption> ) )]
        public IHttpActionResult GetDisruptionsWithFilter( DisruptionFilter disruptionFilter )
        {
            #region Log
            _logger.WriteLog( "Info", ErrorMessages.RequestPart( User.Identity.Name, Request.RequestUri.AbsolutePath, Request.Method.Method ) );
            #endregion
            var user = _userData.GetUserByAdName( User.Identity.Name );
            var lastAccess = user.LastAccess;
            _userData.UpdateLastAccess( user.UserId );

            var disruptions = _disruptionData.GetDisruptionsWithFilter( disruptionFilter );
            return Ok( new { disruptions, lastAccess } );
        }

        // GET: api/Disruptions/5
        /// <summary>
        /// Получить определенное нарушение
        /// </summary>
        /// <param name="id">id нарушения</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "user, writer, admin" )]
        [ResponseType( typeof( Disruption ) )]
        public IHttpActionResult GetDisruption( int id )
        {
            #region Log
            _logger.WriteLog( "Info", ErrorMessages.RequestPart( User.Identity.Name, Request.RequestUri.AbsolutePath, Request.Method.Method ) );
            #endregion
            var disruption = _disruptionData.GetDisruptionById( id );

            if ( disruption is null )
            {
                #region Log
                _logger.WriteLog( "Warn", ErrorMessages.ObjectNull( nameof( Disruption ) ) );
                #endregion
                return NotFound();
            }

            return Ok( disruption );
        }

        // POST: api/Disruptions
        /// <summary>
        /// Добавить новое нарушение
        /// </summary>
        /// <param name="disruption">Объект Disruption</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "user, writer, admin" )]
        [ResponseType( typeof( void ) )]
        public IHttpActionResult PostDisruption( Disruption disruption )
        {
            #region Log
            _logger.WriteLog( "Info", ErrorMessages.RequestPart( User.Identity.Name, Request.RequestUri.AbsolutePath, Request.Method.Method ) );
            #endregion
            if ( disruption is null )
            {
                #region Log
                _logger.WriteLog( "Warn", ErrorMessages.ObjectNull( nameof( Disruption ) ) );
                #endregion
                ModelState.AddModelError( "", ErrorMessages.ObjectNull( nameof( Disruption ) ) );
            }

            if ( !ModelState.IsValid )
            {
                #region Log
                _logger.WriteLog( "Warn", $"Объект {nameof( Disruption )} некорректен" );
                #endregion
                return BadRequest( ModelState );
            }

            var user = _userData.GetUserByAdName( User.Identity.Name );

            if ( user is null )
            {
                #region Log
                _logger.WriteLog( "Warn", ErrorMessages.ObjectNull( nameof( Domain.Entities.User ) ) );
                #endregion
                return BadRequest();
            }

            disruption.UserId = user.UserId;

            _disruptionData.AddDisruption( disruption );

            _disruptionLog.WriteLog( disruption );

            return CreatedAtRoute( "DefaultApi", new { id = disruption.DisruptionId }, disruption );
        }

        // PUT: api/Disruptions/5
        /// <summary>
        /// Редактирование нарушения
        /// </summary>
        /// <param name="id">id редактируемого нарушения</param>
        /// <param name="disruption">Объект Disruption</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "user, writer, admin" )]
        [ResponseType( typeof( void ) )]
        public IHttpActionResult PutDisruption( int id, Disruption disruption )
        {
            #region Log
            _logger.WriteLog( "Info", ErrorMessages.RequestPart( User.Identity.Name, Request.RequestUri.AbsolutePath, Request.Method.Method ) );
            #endregion
            if ( disruption is null )
            {
                #region Log
                _logger.WriteLog( "Warn", ErrorMessages.ObjectNull( nameof( Disruption ) ) );
                #endregion
                ModelState.AddModelError( "", ErrorMessages.ObjectNull( nameof( Disruption ) ) );
            }

            if ( !ModelState.IsValid )
            {
                #region Log
                _logger.WriteLog( "Warn", $"Объект {nameof( Disruption )} некорректен" );
                #endregion
                return BadRequest( ModelState );
            }

            if ( id != disruption.DisruptionId )
            {
                #region Log
                _logger.WriteLog( "Warn", ErrorMessages.IdDismatch( id, disruption.DisruptionId ) );
                #endregion
                return BadRequest();
            }

            var mUser = _userData.GetUserByAdName( User.Identity.Name );

            if ( mUser is null )
            {
                #region Log
                _logger.WriteLog( "Warn", ErrorMessages.ObjectNull( nameof( Domain.Entities.User ) ) );
                #endregion
                return BadRequest();
            }

            disruption.UpdateDate = DateTime.UtcNow;
            disruption.UpdateUserNameId = mUser.UserId;
            var updatedDisruption = _disruptionData.ModifyDisruption( disruption );
            _disruptionLog.WriteLog( updatedDisruption );

            return StatusCode( HttpStatusCode.NoContent );
        }

        // DELETE: api/Disruptions/5
        /// <summary>
        /// Удаление нарушения
        /// </summary>
        /// <param name="id">id удаляемого нарушения</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "admin" )]
        [ResponseType( typeof( void ) )]
        public IHttpActionResult DeleteDisruption( int id )
        {
            #region Log
            _logger.WriteLog( "Info", ErrorMessages.RequestPart( User.Identity.Name, Request.RequestUri.AbsolutePath, Request.Method.Method ) );
            #endregion
            var mUser = _userData.GetUserByAdName( User.Identity.Name );

            if ( mUser is null )
            {
                #region Log
                _logger.WriteLog( "Warn", ErrorMessages.ObjectNull( nameof( Domain.Entities.User ) ) );
                #endregion
                return BadRequest();
            }

            var deletedDisruption = _disruptionData.DeleteDisruption( id );

            if ( deletedDisruption is null ) return BadRequest( ErrorMessages.ObjectNotFound() );

            deletedDisruption.UpdateDate = DateTime.UtcNow;
            deletedDisruption.UpdateUserNameId = mUser.UserId;
            _disruptionLog.WriteLog( deletedDisruption );

            return Ok();
        }

        /// <summary>
        /// Общее число нарушений
        /// </summary>
        /// <returns></returns>
        [MHserAuthorize( Roles = "user, writer, admin" )]
        [Route( "api/Disruptions/Total" )]
        [HttpGet]
        public HttpResponseMessage Total()
        {
            #region Log
            _logger.WriteLog( "Info", ErrorMessages.RequestPart( User.Identity.Name, Request.RequestUri.AbsolutePath, Request.Method.Method ) );
            #endregion
            return Request.CreateResponse( HttpStatusCode.OK, _disruptionData.TotalDisruptions() );
        }

        /// <summary>
        /// Получить выгрузку общей таблицы по нарушениям в соответствии с выбранными фильтрами
        /// </summary>
        /// <param name="from">От даты</param>
        /// <param name="to">До даты</param>
        /// <param name="locationId">id местоположения</param>
        /// <param name="userId">id выявившего нарушение пользователя</param>
        /// <param name="categoryId">id категории нарушения</param>
        /// <param name="characterId">id характера нарушения</param>
        /// <param name="status">статус нарушения</param>
        /// <returns></returns>
        [MHserAuthorize( Roles = "user, writer, admin" )]
        [Route( "api/Disruptions/DisruptionsExcel" )]
        [AcceptVerbs( "GET", "POST" )]
        public HttpResponseMessage GetDisruptionsExcel( DateTime? from, DateTime? to, int? locationId, int? userId, int? categoryId, int? characterId, int? status )
        {
            var resData = _disruptionData.DisruptionsExcel( from, to, locationId, userId, categoryId, characterId, status );

            var dataStream = new MemoryStream( resData );

            HttpResponseMessage httpResponseMessage = Request.CreateResponse( HttpStatusCode.OK );
            httpResponseMessage.Content = new StreamContent( dataStream );
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue( "attachment" );
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = $"MHser {DateTime.Now.ToString( "yyyy-MM-dd" )}.xlsx";
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue( "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" );

            return httpResponseMessage;
        }
        /// <summary>
        /// Получить Excel отчёт в соответствии с формой #1
        /// </summary>
        /// <returns></returns>
        [Route( "api/Disruptions/Excel" )]
        public HttpResponseMessage GetExcel()
        {
            var resData = _disruptionData.Excel();
            var dataStream = new MemoryStream( resData );

            HttpResponseMessage httpResponseMessage = Request.CreateResponse( HttpStatusCode.OK );
            httpResponseMessage.Content = new StreamContent( dataStream );
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue( "attachment" );
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = $"MHser {DateTime.Now.ToString( "yyyy-MM-dd" )}.xlsx";
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue( "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" );

            return httpResponseMessage;
        }
    }
}
