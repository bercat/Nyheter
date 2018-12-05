using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ContextLayer
{
    public class Context : IdentityDbContext<ApplicationUser>
    {
        #region c-tors
        public Context()
            : base("LocalSqlServer")
        {
            //Database.SetInitializer(new EntitiesContextInitializer());
            // Database.SetInitializer<Context>(null);
            //Database.SetInitializer(new DropCreateDatabaseTables());
        }
        public static Context Create()
        {
            return new Context();
        }
        #endregion

        #region savechanges
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is IdentityUserRole || entry.Entity is IdentityRole || entry.Entity is ApplicationUser)
                {

                }
                else
                {
                    var entity = (EntityBase)entry.Entity;

                    if (entry.State == EntityState.Added)
                    {
                        entity.Id = Guid.NewGuid();
                        entity.DateCreated = DateTime.Now;
                        entity.DateUpdated = DateTime.Now;
                        entity.Status = true;
                    }
                    else if (entry.State == EntityState.Modified)
                        entity.DateUpdated = DateTime.Now;
                }
            }
            return base.SaveChanges();
        }
        #endregion

        #region entities
        public DbSet<Forfatter> Forfatter { get; set; }
        public DbSet<Nyhet> Nyhet { get; set; }
        #endregion

        #region seeding
        public class EntitiesContextInitializer : CreateDatabaseIfNotExists<Context>
        {
            protected override void Seed(Context context)
            {
                // context.SaveChanges();
            }
        }
        #endregion

        #region modeloverride
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Project>().HasMany(p => p.Questions).WithRequired(q => q.Project);            
        }

        #endregion
    }

    public class DropCreateDatabaseTables : IDatabaseInitializer<Context>
    {

        #region IDatabaseInitializer<Context> Members

        public void InitializeDatabase(Context context)
        {
            bool dbExists;
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                dbExists = context.Database.Exists();
            }

            if (dbExists)
            {
                // remove all tables
                context.Database.ExecuteSqlCommand("EXEC sp_MSforeachtable @command1 = \"DROP TABLE ?\"");
                // create all tables
                var dbCreationScript = ((IObjectContextAdapter)context).ObjectContext.CreateDatabaseScript();
                context.Database.ExecuteSqlCommand(dbCreationScript);
                Seed(context);
                context.SaveChanges();
            }
            else
            {
                throw new ApplicationException("No database instance");
            }
        }
        #endregion
        #region Methods
        protected virtual void Seed(Context context)
        {
            /// TODO: put here your seed creation
        }
        #endregion
    }
}
