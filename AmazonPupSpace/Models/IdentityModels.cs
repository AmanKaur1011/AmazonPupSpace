using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AmazonPupSpace.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
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

        }
        //Add an art entity to the system
        public DbSet<Post> Posts { get; set; }
        //Add a comments entity to the system
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Dog> Dogs { get; set; }
        //Adding Employee Entity to the database
        public DbSet<Employee> Employees { get; set; }

        //Adding Department Entity to the database
        public DbSet<Department> Departments { get; set; }
        public DbSet<Trick> Tricks { get; set; }
        public DbSet<DogxTrick> DogxTricks { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}