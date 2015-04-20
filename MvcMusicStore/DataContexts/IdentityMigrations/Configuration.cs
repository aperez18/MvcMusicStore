namespace MvcMusicStore.DataContexts.IdentityMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using MvcMusicStore.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<MvcMusicStore.DataContexts.IdentityDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DataContexts\IdentityMigrations";
        }

        protected override void Seed(MvcMusicStore.DataContexts.IdentityDb context)
        {
            //  This method will be called after migrating to the latest version.

            // Check if an "AppAdmin" role exists in the database
            if (!context.Roles.Any(r => r.Name == "AppAdmin"))
            {
                // Create "AppAdmin" user role
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "AppAdmin" };
                manager.Create(role);
            }

            // Check if the user "aperez@okaydrummer.com" exists in the database
            if (!context.Users.Any(u => u.UserName == "aperez@okaydrummer.com"))
            {
                // Create user "aperez@okaydrummer.com"
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "aperez@okaydrummer.com" };

                manager.Create(user, "drumordie69");
                // Assign a role to the new "aperez@okaydrummer.com" user ("AppAdmin" role)
                manager.AddToRole(user.Id, "AppAdmin");
            }

            if (!context.Roles.Any(r => r.Name == "CanEdit"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "CanEdit" };

                manager.Create(role);
            }
        }
    }
}
