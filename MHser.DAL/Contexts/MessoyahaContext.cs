using MHser.DAL.Data;
using MHser.Domain.Entities;
using System.Data.Entity;

namespace MHser.DAL.Contexts
{
    public class MessoyahaContext : DbContext
    {
        static MessoyahaContext()
        {
            Database.SetInitializer<MessoyahaContext>( new DbInitializer() );
        }

        public MessoyahaContext() : base( "DBConnection" ) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Responsable> Responsables { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Disruption> Disruptions { get; set; }
        public DbSet<DisruptionLog> DisruptionsLog { get; set; }
    }
}