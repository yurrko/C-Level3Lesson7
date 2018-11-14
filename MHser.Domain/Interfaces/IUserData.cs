using MHser.Domain.Entities;
using System;
using System.Collections.Generic;

namespace MHser.Domain.Interfaces
{
    public interface IUserData
    {
        IEnumerable<User> GetUsers();
        User GetUserById( int id );
        User GetUserByAdName( string userName );
        User AddUser( string userName );
        User AddUser( UserActiveDirectory user );
        User AddUser( User user );
        void ModifyUser( User user );
        User CheckOrAddUser( string userName );
        void DeleteUser( int id );
        void UpdateLastAccess( int id );
        DateTime? GetLastAccess( int id );
        //User ModifyUserRoles( int userId, ICollection<Role> roles );
        //User AddRoleToUser( int userId, int roleId );
        //User RemoveRoleFromUser( int userId, int roleId );
        User ModifyRoles( int userId, string[] roles );
    }
}