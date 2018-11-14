using MHser.DAL.Contexts;
using MHser.Domain.Entities;
using MHser.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using MHser.Logger;
using ClosedXML.Excel;
using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MHser.Services
{
    public class DisruptionData : IDisruptionData
    {
        private readonly MyLogger _logger = MyLogger.GetLogger();
        public IEnumerable<Disruption> GetDisruptions()
        {
            try
            {
                using ( var context = new MessoyahaContext() )
                {
                    var disruptions = context.Disruptions
                        .Include( "User" )
                        .Include( "Location" )
                        .Include( "Category" )
                        .Include( "Character" )
                        .Include( "Responsable" )
                        .Include( "UpdateUserName" )
                        .ToList();
                    foreach ( var item in disruptions )
                    {
                        item.DetectionTime = item.DetectionTime.Date;
                        item.ExecuteUntil = item.ExecuteUntil.Date;
                        item.UpdateDate = item.UpdateDate;
                    }

                    return disruptions;
                }
            }
            catch ( Exception ex )
            {
                _logger.WriteLog( "Error", ex );
                throw;
            }
        }

        public IEnumerable<Disruption> GetDisruptionsWithFilter( DisruptionFilter df )
        {
            try
            {
                using ( var context = new MessoyahaContext() )
                {
                    IQueryable<Disruption> disruptions = context.Disruptions
                        .Include( "User" )
                        .Include( "Location" )
                        .Include( "Category" )
                        .Include( "Character" )
                        .Include( "Responsable" )
                        .Include( "UpdateUserName" );

                    if ( df.From.HasValue ) disruptions = disruptions.Where( d => d.DetectionTime >= df.From );
                    if ( df.To.HasValue ) disruptions = disruptions.Where( d => d.DetectionTime <= df.To );
                    if ( df.CategoryIds.Length > 0 ) disruptions = disruptions.Where( d => df.CategoryIds.Contains( d.CategoryId ) );
                    if ( df.CharacterIds.Length > 0 ) disruptions = disruptions.Where( d => df.CharacterIds.Contains( d.CharacterId ) );

                    
                    foreach ( var item in disruptions )
                    {
                        item.DetectionTime = item.DetectionTime.Date;
                        item.ExecuteUntil = item.ExecuteUntil.Date;
                        item.UpdateDate = item.UpdateDate;
                    }

                    return disruptions.ToList();
                }
            }
            catch ( Exception ex )
            {
                _logger.WriteLog( "Error", ex );
                throw;
            }
        }
        public Disruption GetDisruptionById( int id )
        {
            try
            {
                using ( var context = new MessoyahaContext() )
                {
                    return context.Disruptions
                    .Include( "User" )
                    .Include( "Location" )
                    .Include( "Category" )
                    .Include( "Character" )
                    .Include( "Responsable" )
                    .Include( "UpdateUserName" )
                    .FirstOrDefault( d => d.DisruptionId == id );
                }
            }
            catch ( Exception ex )
            {
                _logger.WriteLog( "Error", ex );
                throw;
            }
        }

        public Disruption AddDisruption( Disruption disruption )
        {
            try
            {
                using ( var context = new MessoyahaContext() )
                {
                    disruption.CreationDate = DateTime.UtcNow;
                    context.Disruptions.Add( disruption );
                    context.SaveChanges();

                    return disruption;
                }
            }
            catch ( Exception ex )
            {
                _logger.WriteLog( "Error", ex );
                throw;
            }
        }

        public Disruption ModifyDisruption( Disruption disruption )
        {
            try
            {
                using ( var context = new MessoyahaContext() )
                {
                    context.Disruptions.Attach( disruption );
                    context.Entry( disruption ).State = EntityState.Modified;
                    context.SaveChanges();

                    return disruption;
                }
            }
            catch ( DbUpdateConcurrencyException dbex )
            {
                _logger.WriteLog( "Warn", dbex );
                throw new DbUpdateConcurrencyException( "Объект был ранее обновлен." );
            }
            catch ( Exception ex )
            {
                _logger.WriteLog( "Error", ex );
                throw;
            }
        }

        public Disruption DeleteDisruption( int id )
        {
            try
            {
                using ( var context = new MessoyahaContext() )
                {
                    var disruptionToDelete = GetDisruptionById( id );

                    if ( disruptionToDelete is null ) return null;

                    context.Disruptions.Attach( disruptionToDelete );
                    context.Entry( disruptionToDelete ).State = EntityState.Deleted;
                    context.SaveChanges();

                    return disruptionToDelete;
                }
            }
            catch ( Exception ex )
            {
                _logger.WriteLog( "Error", ex );
                throw;
            }
        }

        public int TotalDisruptions()
        {
            try
            {
                using ( var context = new MessoyahaContext() )
                {
                    return context.Disruptions.Count();
                }
            }
            catch ( Exception ex )
            {
                _logger.WriteLog( "Error", ex );
                throw;
            }
        }

        public object GetOrgData()
        {
            var expired = GetExpired().Count();
            var todayDeadLine = GetDeadlineToday().Count();
            var todayAdded = GetAddedToday().Count();
            var important = GetImportant().Count();
            var soon = GetSoon().Count();
            var thisWeek = GetThisWeek().Count();
            var completed = GetCompleted().Count();
            var all = GetAll().Count();

            return new { expired, todayDeadLine, todayAdded, important, soon, thisWeek, completed, all };
        }

        public IEnumerable<Disruption> GetExpired()
        {
            try
            {
                using ( var context = new MessoyahaContext() )
                {
                    return context.Disruptions.Where( d => d.ExecuteUntil < DateTime.UtcNow ).ToList();
                }
            }
            catch ( Exception ex )
            {
                _logger.WriteLog( "Error", ex );
                throw;
            }
        }

        public IEnumerable<Disruption> GetDeadlineToday()
        {
            var todayStart = DateTime.UtcNow.Date;
            var todayEnd = DateTime.UtcNow.Date.AddDays( 1 );
            try
            {
                using ( var context = new MessoyahaContext() )
                {
                    return context.Disruptions.Where( d => d.ExecuteUntil >= todayStart && d.ExecuteUntil < todayEnd ).ToList();
                }
            }
            catch ( Exception ex )
            {
                _logger.WriteLog( "Error", ex );
                throw;
            }
        }

        public IEnumerable<Disruption> GetAddedToday()
        {
            var todayStart = DateTime.UtcNow.Date;
            var todayEnd = DateTime.UtcNow.Date.AddDays( 1 );
            try
            {
                using ( var context = new MessoyahaContext() )
                {
                    return context.Disruptions.Where( d => d.DetectionTime >= todayStart && d.DetectionTime < todayEnd ).ToList();
                }
            }
            catch ( Exception ex )
            {
                _logger.WriteLog( "Error", ex );
                throw;
            }
        }

        public IEnumerable<Disruption> GetImportant()
        {
            try
            {
                using ( var context = new MessoyahaContext() )
                {
                    return context.Disruptions.Where( d => d.IsCritical ).ToList();
                }
            }
            catch ( Exception ex )
            {
                _logger.WriteLog( "Error", ex );
                throw;
            }
        }

        public IEnumerable<Disruption> GetSoon()
        {
            var soonDate = DateTime.UtcNow.Date.AddDays( 2 );
            try
            {
                using ( var context = new MessoyahaContext() )
                {
                    return context.Disruptions.Where( d => d.ExecuteUntil < soonDate && d.ExecuteUntil > DateTime.UtcNow ).ToList();
                }
            }
            catch ( Exception ex )
            {
                _logger.WriteLog( "Error", ex );
                throw;
            }
        }

        public IEnumerable<Disruption> GetThisWeek()
        {
            //Старт недели//
            var firstDateOfWeek = DateTime.UtcNow;
            while ( firstDateOfWeek.DayOfWeek != DayOfWeek.Monday )
                firstDateOfWeek = firstDateOfWeek.AddDays( -1 );
            //Конец недели//
            var lastDateOfWeek = firstDateOfWeek.AddDays( 6 );

            try
            {
                using ( var context = new MessoyahaContext() )
                {
                    return context.Disruptions.Where( d => d.ExecuteUntil >= firstDateOfWeek && d.ExecuteUntil <= lastDateOfWeek ).ToList();
                }
            }
            catch ( Exception ex )
            {
                _logger.WriteLog( "Error", ex );
                throw;
            }
        }

        public IEnumerable<Disruption> GetCompleted()
        {
            try
            {
                using ( var context = new MessoyahaContext() )
                {
                    return context.Disruptions.Where( d => d.IsDone ).ToList();
                }
            }
            catch ( Exception ex )
            {
                _logger.WriteLog( "Error", ex );
                throw;
            }
        }

        public IEnumerable<Disruption> GetAll()
        {
            try
            {
                using ( var context = new MessoyahaContext() )
                {
                    return context.Disruptions.ToList();
                }
            }
            catch ( Exception ex )
            {
                _logger.WriteLog( "Error", ex );
                throw;
            }
        }

        public IEnumerable<ResultReport> GetMainReport()
        {
            try
            {
                using ( var context = new MessoyahaContext() )
                {
                    return context.Disruptions.Where( y => y.DetectionTime.Year == DateTime.UtcNow.Year ).GroupBy( d => d.DetectionTime.Month ).Select( r => new ResultReport
                    {
                        MonthId = r.Key,
                        IndustrialSafety = r.Count( e => e.CategoryId == 1 ),
                        FireSafety = r.Count( e => e.CategoryId == 2 ),
                        ElectroSafety = r.Count( e => e.CategoryId == 3 ),
                        WorkWithPS = r.Count( e => e.CategoryId == 4 ),
                        TransportSafety = r.Count( e => e.CategoryId == 5 ),
                        EcoSafety = r.Count( e => e.CategoryId == 6 ),
                        WorkOnHeight = r.Count( e => e.CategoryId == 7 ),
                        FireWork = r.Count( e => e.CategoryId == 8 ),
                        Other = r.Count( e => e.CategoryId == 9 ),
                    } ).ToList();
                }
            }
            catch ( Exception ex )
            {
                _logger.WriteLog( "Error", ex );
                throw;
            }
        }

        public IEnumerable<Disruption> GetDisruptionDataForExcel( DateTime? from, DateTime? to, int? locationId, int? userId, int? categoryId, int? characterId, int? status )
        {
            try
            {
                using ( var context = new MessoyahaContext() )
                {
                    IQueryable<Disruption> disruptions = context.Disruptions
                        .Include( "User" )
                        .Include( "Location" )
                        .Include( "Category" )
                        .Include( "Character" )
                        .Include( "Responsable" )
                        .OrderBy( i => i.DisruptionId );

                    if ( from.HasValue )
                        disruptions = disruptions.Where( f => f.DetectionTime >= from.Value );
                    if ( to.HasValue )
                        disruptions = disruptions.Where( t => t.DetectionTime <= to.Value );
                    if ( locationId.HasValue )
                        disruptions = disruptions.Where( l => l.LocationId == locationId.Value );
                    if ( userId.HasValue )
                        disruptions = disruptions.Where( u => u.UserId == userId.Value );
                    if ( categoryId.HasValue )
                        disruptions = disruptions.Where( c => c.CategoryId == categoryId.Value );
                    if ( characterId.HasValue )
                        disruptions = disruptions.Where( ch => ch.CharacterId == characterId.Value );
                    if ( status.HasValue )
                    {
                        switch ( status.Value )
                        {
                            case 1:
                                disruptions = disruptions.Where( s => s.IsDone );
                                break;
                            case 2:
                                disruptions = disruptions.Where( s => !s.IsDone && s.ExecuteUntil < DateTime.UtcNow );
                                break;
                            case 3:
                                disruptions = disruptions.Where( s => !s.IsDone && s.ExecuteUntil >= DateTime.UtcNow );
                                break;
                        }

                    }
                    return disruptions.ToList();
                }
            }
            catch ( Exception ex )
            {
                _logger.WriteLog( "Error", ex );
                throw;
            }
        }

        public byte[] DisruptionsExcel( DateTime? from, DateTime? to, int? locationId, int? userId, int? categoryId, int? characterId, int? status )
        {
            var excelData = GetDisruptionDataForExcel( from, to, locationId, userId, categoryId, characterId, status );

            using ( var workbook = new XLWorkbook( XLEventTracking.Disabled ) )
            {
                var worksheet = workbook.Worksheets.Add( $"MHser {DateTime.Now.ToString( "yyyy-MM-dd" )}" );

                //Шапка
                worksheet.Cell( 1, "A" ).Value = "№ п/п";
                worksheet.Cell( 1, "B" ).Value = "Дата предписания (обнаружения)";
                worksheet.Cell( 1, "C" ).Value = "Подрядная организация, либо объект";
                worksheet.Cell( 1, "D" ).Value = @"Ф.И.О., должность работника ответственного на объекте";
                worksheet.Cell( 1, "E" ).Value = "Наименование выявленных несоответсвий";
                worksheet.Cell( 1, "F" ).Value = @"Категория
нарушения";
                worksheet.Cell( 1, "G" ).Value = @"Характер нарушения
ОУ или ОД";
                worksheet.Cell( 1, "H" ).Value = "Отметка о выполнении";
                worksheet.Cell( 1, "I" ).Value = "Нарушенный пункт НМД";
                worksheet.Cell( 1, "J" ).Value = "Фактический срок устранения";

                //Тело
                int i = 2;
                foreach ( var item in excelData )
                {
                    worksheet.Cell( i, "A" ).Value = i - 1;
                    worksheet.Cell( i, "B" ).Value = item.DetectionTime.Date;
                    worksheet.Cell( i, "B" ).Style.Alignment.WrapText = true;
                    worksheet.Cell( i, "C" ).Value = item.Location.Name;
                    worksheet.Cell( i, "D" ).Value = $"{MakeFio( item.User.Name )} {item.User.Position}";
                    worksheet.Cell( i, "E" ).Value = item.Description;
                    worksheet.Cell( i, "E" ).Style.Alignment.WrapText = true;
                    worksheet.Cell( i, "F" ).Value = item.Category.ShortName;
                    worksheet.Cell( i, "G" ).Value = item.Character.ShortName;
                    worksheet.Cell( i, "H" ).Value = item.IsDone ? "устранено" : "не устранено";
                    worksheet.Cell( i, "I" ).Value = item.Documentation;
                    worksheet.Cell( i, "I" ).Style.Alignment.WrapText = true;
                    worksheet.Cell( i, "J" ).Value = item.IsDone ? item.UpdateDate : null;
                    i++;
                }
                var rngData = worksheet.Range( String.Concat( "A1:J", ( i - 1 ).ToString() ) );
                var excelTable = rngData.CreateTable();

                //Стиль тела
                worksheet.Columns().AdjustToContents();
                worksheet.Column( 2 ).Width = 17;
                worksheet.Column( 5 ).Width = 60;
                worksheet.Column( 9 ).Width = 60;

                using ( var stream = new MemoryStream() )
                {
                    workbook.SaveAs( stream );
                    stream.Flush();

                    return stream.ToArray();
                }
            }
        }

        public byte[] Excel()
        {
            var excelData = GetDisruptions().GroupBy( d => new { d.LocationId, d.Location } );

            using ( var workbook = new XLWorkbook( XLEventTracking.Disabled ) )
            {
                var ws = workbook.Worksheets.Add( $"MHser {DateTime.Now.ToString( "yyyy-MM-dd" )}" );

                //Шапка
                ws.Cell( 1, "A" ).Value = "№";
                ws.Cell( 1, "B" ).Value = "Нарушение";
                ws.Cell( 1, "C" ).Value = "Месторасположение нарушения";
                ws.Cell( 1, "D" ).Value = "Описание нарушения";
                ws.Cell( 1, "E" ).Value = "Предложения по устранению нарушения/ Дата устранения нарушения";
                ws.Cell( 1, "F" ).Value = "Дата, время возникновения нарушения";
                ws.Cell( 1, "G" ).Value = "Ф.И.О., должность выявившего  нарушение";
                ws.Cell( 1, "H" ).Value = "Статус нарушения";
                ws.Cell( 1, "I" ).Value = "Категория нарушений";
                ws.Cell( 2, "I" ).Value = "Опасные действия";
                ws.Cell( 2, "J" ).Value = "Опасные условия";
                ws.Cell( 1, "K" ).Value = "Ф.И.О., должность ответственного за устранение нарушения";
                ws.Cell( 1, "L" ).Value = "Срок устранения";

                var startCell = ws.Cell( 3, "A" );
                startCell.Value = 1;
                for ( int i = 1 ; i < 12 ; i++ )
                {
                    startCell = startCell.CellRight();
                    startCell.Value = i + 1;
                }

                //Тело
                int rowNum = 4;
                int locationNum = 1;
                foreach ( var location in excelData )
                {
                    var tempCell = ws.Cell( rowNum, "A" );
                    tempCell.Value = location.Key.Location.Name;
                    ws.Range( $"A{rowNum}", $"L{rowNum}" ).Merge();
                    tempCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    tempCell.Style.Font.Bold = true;
                    rowNum++;

                    foreach ( var item in location )
                    {
                        ws.Cell( rowNum, "A" ).Value = rowNum - 3 - locationNum;
                        ws.Cell( rowNum, "B" ).Value = item.Documentation;
                        ws.Cell( rowNum, "B" ).Style.Alignment.WrapText = true;
                        ws.Cell( rowNum, "C" ).Value = item.Place;
                        ws.Cell( rowNum, "D" ).Value = item.Description;
                        ws.Cell( rowNum, "D" ).Style.Alignment.WrapText = true;
                        ws.Cell( rowNum, "E" ).Value = item.IsDone ? ( item.UpdateDate.HasValue ? item.UpdateDate.Value.ToString( "dd.MM.yyyy" ) : null ) : item.Events;
                        ws.Cell( rowNum, "F" ).Value = item.DetectionTime;
                        ws.Cell( rowNum, "G" ).Value = $"{MakeFio( item.User.Name )} {item.User.Position}";
                        ws.Cell( rowNum, "G" ).Style.Alignment.WrapText = true;
                        ws.Cell( rowNum, "H" ).Value = item.IsDone ? "устранено" : "не устранено";
                        ws.Cell( rowNum, "I" ).Value = item.CharacterId == 1 ? item.Character.ShortName : null;
                        ws.Cell( rowNum, "J" ).Value = item.CharacterId == 2 ? item.Character.ShortName : null;
                        ws.Cell( rowNum, "K" ).Value = $"{item.Responsable.Name} {item.Responsable.Position}";
                        ws.Cell( rowNum, "K" ).Style.Alignment.WrapText = true;
                        ws.Cell( rowNum, "L" ).Value = item.ExecuteUntil;

                        var colNum = 0;
                        for ( int i = 0 ; i < 12 ; i++ )
                        {
                            ++colNum;
                            var tmpCell = ws.Cell( rowNum, colNum );
                            tmpCell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                            tmpCell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                            tmpCell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                            tmpCell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        }
                        rowNum++;
                    }
                    locationNum++;
                }
                //var rngData = ws.Range( String.Concat( "A1:J", ( i - 1 ).ToString() ) );
                //var excelTable = rngData.CreateTable();

                //Стиль шапки
                ws.Range( "A1:A2" ).Column( 1 ).Merge();
                ws.Range( "B1:B2" ).Column( 1 ).Merge();
                ws.Range( "C1:C2" ).Column( 1 ).Merge();
                ws.Range( "D1:D2" ).Column( 1 ).Merge();
                ws.Range( "E1:E2" ).Column( 1 ).Merge();
                ws.Range( "F1:F2" ).Column( 1 ).Merge();
                ws.Range( "G1:G2" ).Column( 1 ).Merge();
                ws.Range( "H1:H2" ).Column( 1 ).Merge();
                ws.Range( "I1:J1" ).Row( 1 ).Merge();
                ws.Range( "K1:K2" ).Column( 1 ).Merge();
                ws.Range( "L1:L2" ).Column( 1 ).Merge();

                var headerRng = ws.Range( "A1:L2" );
                headerRng.Style.Alignment.WrapText = true;
                headerRng.Style.Fill.BackgroundColor = XLColor.Blue;
                headerRng.Style.Font.FontColor = XLColor.White;

                for ( int i = 1 ; i < 5 ; i++ )
                {
                    for ( int j = 1 ; j < 13 ; j++ )
                    {
                        ws.Cell( i, j ).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        ws.Cell( i, j ).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        ws.Cell( i, j ).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        ws.Cell( i, j ).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    }
                }

                var numRng = ws.Range( "A1:L3" );
                numRng.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                numRng.Style.Font.Bold = true;


                //Стиль тела
                ws.Columns().AdjustToContents();
                ws.Columns( 2, 5 ).Width = 35;
                ws.Column( 7 ).Width = 20;
                ws.Column( 11 ).Width = 20;
                ws.Columns( 9, 10 ).Width = 9;
                ws.SheetView.FreezeRows( 3 );

                using ( var stream = new MemoryStream() )
                {
                    workbook.SaveAs( stream );
                    stream.Flush();

                    return stream.ToArray();
                }
            }
        }

        private string MakeFio( string userName )
        {
            var temp = userName.Split( ' ' );
            var name = "";
            name += $"{temp[0]} ";
            for ( int i = 1 ; i < temp.Length ; i++ )
            {
                name += $"{temp[i][0]}.";
            }
            return name;
        }
    }
}
