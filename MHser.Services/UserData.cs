using MHser.DAL.Contexts;
using MHser.Domain.Entities;
using MHser.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MHser.Services
{
    public class UserData : IUserData, IRolesData
    {
        public IEnumerable<User> GetUsers()
        {
            using ( var context = new MessoyahaContext() )
            {
                return context.Users.Include( "Roles" ).ToList();
            }
        }

        public User GetUserById( int id )
        {
            using ( var context = new MessoyahaContext() )
            {
                return context.Users.Include( "Roles" ).FirstOrDefault( d => d.UserId == id );
            }
        }

        public User GetUserByAdName( string userName )
        {
            using ( var context = new MessoyahaContext() )
            {
                return context.Users.Include( "Roles" ).FirstOrDefault( d => d.AdName == userName );
            }
        }

        public User AddUser( string userName )
        {
            var user = new User
            {
                AdName = userName,
            };
            using ( var context = new MessoyahaContext() )
            {
                context.Users.Add( user );
                context.SaveChanges();

                user = ModifyRoles( user.UserId, new[] { "user" } );
                return user;
            }
        }

        public User AddUser( User user )
        {
            var checkUser = GetUserByAdName( user.AdName );
            if ( checkUser is null )
            {
                using ( var context = new MessoyahaContext() )
                {
                    context.Users.Add( user );
                    context.SaveChanges();
                }
            }
            else
            {
                user = checkUser;
            }
            return user;
        }

        public User AddUser( UserActiveDirectory adUser )
        {
            if ( adUser is null ) throw new ArgumentNullException( "adUser не может быть пустым" );
            var newUser = new User
            {
                AdName = $@"GAZPROM-NEFT\{adUser.Login}",
                Name = adUser.Name,
                Position = adUser.Position,
                Company = adUser.Company,
                LastAccess = DateTime.UtcNow,
            };

            using ( var context = new MessoyahaContext() )
            {
                context.Users.Add( newUser );
                context.SaveChanges();
                newUser = ModifyRoles( newUser.UserId, new[] { "user" } );
                return newUser;
            }
        }
        public User CheckOrAddUser( string userName )
        {
            return GetUserByAdName( userName ) ?? AddUser( userName );
        }

        public void DeleteUser( int id )
        {
            var user = GetUserById( id );

            if ( user is null ) return;

            using ( var context = new MessoyahaContext() )
            {
                context.Users.Remove( user );
                context.SaveChanges();
            }
        }

        public void ModifyUser( User user )
        {
            using ( var context = new MessoyahaContext() )
            {
                context.Users.Attach( user );
                context.Entry( user ).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void UpdateLastAccess( int id )
        {
            var user = GetUserById( id );

            if ( user is null ) return;

            using ( var context = new MessoyahaContext() )
            {
                context.Users.Attach( user );
                user.LastAccess = DateTime.UtcNow;
                context.SaveChanges();
            }
        }

        public DateTime? GetLastAccess( int id )
        {
            return GetUserById( id )?.LastAccess;
        }

        public IEnumerable<Role> GetRoles()
        {
            using ( var context = new MessoyahaContext() )
            {
                return context.Roles.ToList();
            }
        }

        public Role GetRoleById( int roleId )
        {
            using ( var context = new MessoyahaContext() )
            {
                return context.Roles.FirstOrDefault( r => r.RoleId == roleId );
            }
        }

        private Role GetRoleByName( string role )
        {
            using ( var context = new MessoyahaContext() )
            {
                return context.Roles.FirstOrDefault( r => r.RoleName == role );
            }
        }

        public User ModifyRoles( int userId, string[] roles )
        {
            using ( var context = new MessoyahaContext() )
            {
                var user = context.Users.Include( u => u.Roles ).FirstOrDefault( r => r.UserId == userId );

                if ( user is null )
                {
                    throw new ArgumentNullException( "Пользователя с таким Id не существует" );
                }

                user.Roles.Clear();

                foreach ( var role in roles )
                {
                    var tRole = context.Roles.FirstOrDefault( r => r.RoleName == role );
                    user.Roles.Add( tRole );
                }
                context.SaveChanges();

                return user;
            }
        }

        //public User AddRoleToUser( int userId, int roleId )
        //{
        //    using ( var context = new MessoyahaContext() )
        //    {
        //        var user = GetUserById( userId );
        //        if ( user is null )
        //        {
        //            throw new ArgumentNullException( "Пользователя с таким Id не существует" );
        //        }

        //        if ( user.Roles.Any( r => r.RoleId == roleId ) ) return user;

        //        var role = GetRoleById( roleId );
        //        if ( role is null ) throw new ArgumentNullException( "Роли с таким Id не существует" );

        //        context.Users.Attach( user );
        //        context.Roles.Attach( role );
        //        user.Roles.Add( role );
        //        context.SaveChanges();

        //        return user;
        //    }
        //}

        //public User RemoveRoleFromUser( int userId, int roleId )
        //{
        //    using ( var context = new MessoyahaContext() )
        //    {
        //        var user = GetUserById( userId );
        //        if ( user is null ) throw new NullReferenceException( "Пользователя с таким Id не существует" );

        //        if ( user.Roles.All( r => r.RoleId != roleId ) ) return user;

        //        var role = GetRoleById( roleId );
        //        if ( role is null ) throw new NullReferenceException( "Роли с таким Id не существует" );

        //        context.Users.Attach( user );
        //        context.Roles.Attach( role );
        //        user.Roles.Remove( role );
        //        context.SaveChanges();

        //        return user;
        //    }
        //}

        //public User ModifyUserRoles( int userId, ICollection<Role> roles )
        //{
        //    var user = GetUserById( userId );
        //    using ( var context = new MessoyahaContext() )
        //    {
        //        context.Users.Attach( user );
        //        user.Roles = roles;
        //        context.SaveChanges();
        //        return user;
        //    }
        //}
    }
}