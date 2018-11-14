using MHser.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHser.Services
{
    public class LogsData : ILogsData
    {
        public byte[] GetLogFile( string path )
        {
            var files = Directory.GetFiles( path );
            var maxDate = new DateTime( 1900, 1, 1 );
            string filePath = "";

            foreach ( var file in files )
            {
                var tFile = new FileInfo( file );
                var tData = DateTime.Parse( tFile.Name.Replace( " mhser.txt", "" ) );
                if ( tData > maxDate )
                {
                    filePath = tFile.FullName;
                }
            }

            //open file from the disk (file path is the path to the file to be opened)
            using ( var fileStream = File.OpenRead( filePath ) )
            {
                //create new MemoryStream object
                var memStream = new MemoryStream();
                memStream.SetLength( fileStream.Length );
                //read file to MemoryStream
                fileStream.Read( memStream.GetBuffer(), 0, (int)fileStream.Length );

                return memStream.ToArray();
            }
        }
    }
}