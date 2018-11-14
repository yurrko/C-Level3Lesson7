using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MHser.Domain.Entities;

namespace MHser.Domain.Interfaces
{
    public interface IRolesData
    {
        IEnumerable<Role> GetRoles();
        Role GetRoleById(int roleId);
    }
}
