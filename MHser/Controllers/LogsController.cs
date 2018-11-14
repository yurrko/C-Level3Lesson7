using MHser.ActiveDirectoryInteraction;
using MHser.Domain.Interfaces;
using MHser.Logger;
using MHser.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace MHser.Controllers
{
    /// <summary>
    /// Контроллер логов
    /// </summary>
    public class LogsController : ApiController
    {
        private readonly MyLogger _logger = MyLogger.GetLogger();
        private readonly ILogsData _logsData = new LogsData();

        // GET: api/Logs
        /// <summary>
        /// Получить лог файл в формате Excel
        /// </summary>
        /// <returns></returns>
        [MHserAuthorize( Roles = "admin" )]
        public HttpResponseMessage GetLogs()
        {
            var fullPath = System.Web.Hosting.HostingEnvironment.MapPath( @"~/Log/" );
            var logFile = _logsData.GetLogFile( fullPath );

            var dataStream = new MemoryStream( logFile );

            HttpResponseMessage httpResponseMessage = Request.CreateResponse( HttpStatusCode.OK );
            httpResponseMessage.Content = new StreamContent( dataStream );
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue( "attachment" );
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = $"MHser {DateTime.Now.ToString( "yyyy-MM-dd" )}.txt";
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue( "text/plain" );

            return httpResponseMessage;
        }
    }
}
