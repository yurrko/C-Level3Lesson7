using System;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.IO;
using MHser.Domain.Entities;
using MHser.Logger;

namespace MHser.ActiveDirectoryInteraction
{
    public class ProviderActiveDirectory
    {
        #region generalValues
        string _domain;

        string defaultOU = "OU=TSK,DC =gazprom-neft,DC=local";
        string defaultRootOU = "DC =gazprom-neft,DC=local";
        private static readonly MyLogger _logger = MyLogger.GetLogger();

        public string DefaultRootOU
        {
            get => defaultRootOU;

            set => defaultRootOU = value;
        }
        public string DefaultDomain
        {
            get => _domain;

            set => _domain = value;
        }
        #endregion

        #region Constructors
        public ProviderActiveDirectory()
        {
            //_domain = GetNearestDomain();
            _domain = "msk01-dc03.gazprom-neft.local";
        }

        public ProviderActiveDirectory( string domain )
        {
            _domain = domain;
        }

        public ProviderActiveDirectory( string domain, string defaultRootOU )
        {
            _domain = domain;
            this.defaultRootOU = defaultRootOU;
        }


        #endregion

        #region Methods

        private static string GetNearestDomain()
        {
            try
            {
                var cmdCommand = new ProcessStartInfo( @"cmd.exe", @"/C set logon /all" )
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process cmdProcess = Process.Start( cmdCommand );
                StreamReader cmdReader = cmdProcess.StandardOutput;

                cmdProcess.WaitForExit();

                var outputLine = cmdReader.ReadLine().Replace( @"\", "" ).ToLower();

                cmdReader.Close();

                return ( outputLine.Remove( 0, outputLine.Length - outputLine.LastIndexOf( "=" ) + 1 ) + ".gazprom-neft.local" );
            }
            catch ( Exception e )
            {
                _logger.WriteLog( "Error", e );

                return "msk01-dc03.gazprom-neft.local";
            }
        }

        public UserActiveDirectory GetUserFromAD( string login )
        {

            PrincipalContext context = GetPrincipalContext();

            var user = new UserPrincipal( context ) { SamAccountName = login + "*" };

            var search = new PrincipalSearcher( user );
            var result = search.FindOne();

            var userModel = new UserActiveDirectory();

            try
            {
                var de = result.GetUnderlyingObject() as DirectoryEntry;
                userModel = FillUserData( de );
            }
            catch ( Exception e )
            {
                throw;
                ///write to log e.message
            }

            return userModel;

        }

        private PrincipalContext GetPrincipalContext()
        {

            return new PrincipalContext( ContextType.Domain, _domain, defaultRootOU );//, ServiceUser, ServicePassword);
        }

        private UserActiveDirectory FillUserData( DirectoryEntry dE )
        {
            var user = new UserActiveDirectory
            {
                Login = Convert.ToString( dE.Properties["samAccountName"].Value ),
                Company = ( Convert.ToString( dE.Properties["company"].Value ) ).Replace( @"\", "" ),
                Name = Convert.ToString( dE.Properties["name"].Value ),
                Position = Convert.ToString( dE.Properties["title"].Value )
            };

            var accountInfo = Convert.ToInt32( dE.Properties["userAccountControl"].Value ) - 2;

            user.ActiveLogin = accountInfo % 16 != 0 ? "Включен" : "Отключен";

            var prop = dE.Properties["msRTCSIP-PrimaryUserAddress"].Value;

            user.Mail = prop is null ? "" : prop.ToString().Replace( "sip:", "" );

            return user;
        }
        #endregion
    }
}

