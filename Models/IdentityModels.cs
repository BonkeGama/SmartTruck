using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;



namespace SmartTruckerApp5.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new CreateDatabaseIfNotExists<ApplicationDbContext>());
        }

        public DbSet<CargoDetails> cargoDetails { get; set; }
        public DbSet<CargoType> cargoTypes { get; set; }
       
        public DbSet<Location> locations { get; set; }
        public DbSet<Transactions> transactions { get; set; }
        public DbSet<Trucks> trucks { get; set; }
       // public DbSet<UserDetails> userDetails { get; set; }
        public DbSet<DriverTransactions> driverTransactionsObj { get; set; }
        public DbSet<userRole> userRoles { get; set; }
        public DbSet<payment> Paymentobj { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //PROPERTY CONFIGURATION
            //modelBuilder.Entity<UserDetails>().HasKey(s => s.UserDetailsKey);
            //modelBuilder.Entity<UserDetails>().Property(c => c.UserDetailsKey).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //modelBuilder.Entity<UserDetails>().Property(c => c.Username).HasColumnName("User Name").IsRequired();
            //modelBuilder.Entity<UserDetails>().Property(c => c.Password).HasColumnName("Password").IsRequired();
            //modelBuilder.Entity<UserDetails>().Property(c => c.Cellphone).HasColumnName("Cell Phone").HasMaxLength(10).IsOptional();
            //modelBuilder.Entity<UserDetails>().Property(c => c.UserRole).HasColumnName("User Role").IsOptional();
            //modelBuilder.Entity<UserDetails>().Property(c => c.EmailAddress).HasColumnName("Email Address").IsRequired();


          
         
            modelBuilder.Entity<Trucks>().HasKey(b => b.TrucksKey);
            modelBuilder.Entity<payment>().HasKey(li => li.paymentKey);
            modelBuilder.Entity<payment>().Property(k => k.paymentKey).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Trucks>().Property(c => c.TrucksKey).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Trucks>().Property(c => c.RegistrationNo).HasColumnName("Registration number").IsRequired();
            modelBuilder.Entity<Trucks>().Property(c => c.TruckStatus).HasColumnName("Truck Status").IsRequired();
            modelBuilder.Entity<Trucks>().Property(c => c.TruckPrice).HasColumnName("Truck Price").IsRequired();

            modelBuilder.Entity<userRole>().HasKey(n => new { n.userKey, n.roleKey });

            modelBuilder.Entity<Transactions>().HasKey(v => v.TransactionsKey);
            modelBuilder.Entity<Transactions>().Property(c => c.TransactionsKey).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Transactions>().Property(v => v.EstimatedDelivery).HasColumnName("Estimated Delivery date and time").IsRequired();
            //modelBuilder.Entity<Transactions>().Property(v => v.Earnings).HasColumnName("Earnings ");
            modelBuilder.Entity<Transactions>().Property(v => v.StartDateTime).HasColumnName("Depature Date and Time").IsRequired();
          //  modelBuilder.Entity<Transactions>().Property(v => v.FuelConsumed).HasColumnName("Fuel  Cost");


            modelBuilder.Entity<CargoDetails>().HasKey(c => c.CargoDetailsKey);
            modelBuilder.Entity<CargoDetails>().Property(c => c.CargoDetailsKey).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<CargoDetails>().Property(n => n.CargoPayment).HasColumnName("Cargo Payment").IsRequired();
            modelBuilder.Entity<CargoDetails>().Property(n => n.CargoStatus).HasColumnName("Cargo Status").IsRequired();
            modelBuilder.Entity<CargoDetails>().Property(n => n.EstimatedTravellig).HasColumnName("Estimated Distance To Be travelled").IsRequired();
            // modelBuilder.Entity<CargoDetails>().Property(n => n.CargoWeight).HasColumnName("Cargo Weight In (Litres/Kilograms)").IsRequired();
            modelBuilder.Entity<CargoDetails>().Property(n => n.Destination).HasColumnName("Destination").IsRequired();
            modelBuilder.Entity<CargoDetails>().Property(n => n.PickPoint).HasColumnName("Pick up Point").IsRequired();

            modelBuilder.Entity<CargoType>().HasKey(v => v.CargoTypeKey);
            modelBuilder.Entity<CargoType>().Property(c => c.CargoTypeKey).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<CargoType>().Property(v => v.CargoName).HasColumnName("Cargo Name");
            modelBuilder.Entity<CargoType>().Property(b => b.CargoValue).HasColumnName("Cargo value");

            
            modelBuilder.Entity<Location>().HasKey(v => v.LocationKey);

            modelBuilder.Entity<Location>().Property<int>(v => v.LocationKey);
            modelBuilder.Entity<Location>().Property(c => c.LocationKey).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Location>().Property(v => v.LocationName).HasColumnName("Location Name");

            modelBuilder.Entity<DriverTransactions>().HasKey(n => new { n.UserDetailsKey, n.TransactionsKey });
            //ENTITY CONFIGURATION

            modelBuilder.Entity<Transactions>().HasRequired(c => c.trucks).WithMany(v => v.transactions).HasForeignKey(v => v.TrucksID);
          
            // modelBuilder.Entity<Transactions>().HasMany(v => v.Drivers).WithRequired(v => v.transactions).HasForeignKey(v=>v.TransactionId);
            modelBuilder.Entity<Transactions>().HasMany(c => c.Cargos).WithRequired(v => v.transaction).HasForeignKey(c => c.TransactionId);
            //modelBuilder.Entity<Location>().HasMany(b => b.cargoDetails).WithRequired(n => n.Locations);
            modelBuilder.Entity<CargoType>().HasMany(j => j.crgDetails).WithRequired(l => l.CrgType).HasForeignKey(f => f.cargNo);
            //modelBuilder.Entity<Customers>().HasMany(l => l.CargoDetails).WithRequired(h => h.User).HasForeignKey(kl => kl.CustomerID);
            modelBuilder.Entity<payment>().HasRequired(kl => kl.cargodetails).WithMany(m => m.payments).HasForeignKey(hj => hj.cargoID);

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}