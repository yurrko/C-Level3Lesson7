using System;
using MHser.Domain.Interfaces;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using MHser.Domain.Entities;
using MHser.Services;

namespace MHser.ActiveDirectoryInteraction
{
    public class MHserAuthorizeAttribute : AuthorizeAttribute
    {

        private string[] _allowedUsers = { };
        private string[] _allowedRoles = { };
        private readonly IUserData _userData;

        public MHserAuthorizeAttribute()
        {
            _userData = new UserData();
        }

        private bool User()
        {
            if ( _allowedUsers.Length <= 0 )
                return true;

            return _allowedUsers.Contains( HttpContext.Current.User.Identity.Name );
        }

        private bool Role( User user )
        {
            if ( user == null ) throw new ArgumentNullException( nameof( user ) );

            if ( _allowedRoles.Length <= 0 ) return true;

            return ( from role in _allowedRoles from userRole in user.Roles where role == userRole.RoleName select role ).Any();
        }
        protected override bool IsAuthorized( HttpActionContext actionContext )
        {
            var adName = HttpContext.Current.User.Identity.Name;
            if (Debugger.IsAttached)
                adName = String.IsNullOrWhiteSpace( adName ) ? "GAZPROM-NEFT\\Denisevich.YuV" : adName;

            var user = _userData.GetUserByAdName( adName );
            if ( user is null )
            {
                try
                {
                    var adProv = new ProviderActiveDirectory();
                    var adNameWithoutGaz = adName.Remove( 0, adName.IndexOf( "\\" ) + 1 );
                    var adUser = adProv.GetUserFromAD( adNameWithoutGaz );
                    _userData.AddUser( adUser );
                }
                catch ( Exception ex)
                {
                    _userData.AddUser( adName );
                }
            }

            if ( !string.IsNullOrEmpty( Users ) )
            {
                _allowedUsers = Users.Split( ',' );
                for ( var i = 0 ; i < _allowedUsers.Length ; i++ )
                {
                    _allowedUsers[i] = _allowedUsers[i].Trim();
                }
            }

            if ( !string.IsNullOrEmpty( Roles ) )
            {
                _allowedRoles = Roles.Split( ',' );
                for ( var i = 0 ; i < _allowedRoles.Length ; i++ )
                {
                    _allowedRoles[i] = _allowedRoles[i].Trim();
                }
            }

            //return User() && Role(user);
            return HttpContext.Current.User.Identity.IsAuthenticated && User() && Role( user );
        }
    }
}