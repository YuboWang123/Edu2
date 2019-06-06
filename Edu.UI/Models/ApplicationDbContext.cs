using Edu.Entity;
using Edu.Entity.Account;
using Edu.Entity.CitySchool;
using Edu.Entity.School;
using Edu.Entity.SchoolFinance;
using Edu.Entity.TrainBase;
using Edu.Entity.TrainLesson;
using Edu.UI.Areas.School.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Linq;
using Edu.Entity.Live;
using Edu.Entity.UserLesson;

namespace Edu.UI.Models
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
 
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SideBarRole>().HasKey(a => new
            {
                a.ApplicationRoleId,
                a.ConsoleSideMenuId
            });
            modelBuilder.Entity<UserLesson>().HasKey(a => new
            {
                a.UserId,
                a.TrainBaseLessonId,
                a.VcrId
            });

            modelBuilder.Entity<FINCardConfig>()
                .HasMany(a => a.FinCards);
           
            base.OnModelCreating(modelBuilder);
        }


        public DbSet<UserLesson> UserLessons { get; set; }
        
        public DbSet<TempUser> SchoolTempUsers { get; set; }
        public DbSet<ConsoleTopMenu> ConsoleTopMenus { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<ConsoleSideMenu> ConsoleSideMenus { get; set; }
        public DbSet<SideBarRole> SideBarRoles { get; set; }
        public DbSet<Base_Period> Base_Period { get; set; }
        public DbSet<Base_DataBind> Base_DataBind { get; set; }
        public DbSet<Base_Subject> Base_Subject { get; set; }       
        public DbSet<Base_Genre> Base_Genres { get; set; }
        public DbSet<Base_Grade> Base_Grades { get; set; }
        public DbSet<TrainBaseLesson> TrainBaseLessons { get; set; }
        public DbSet<Vcr> TrainVcrs { get; set; } //videos of a single lesson
        public DbSet<VcrFile> TrainVcrFiles { get; set; }
        public DbSet<VcrTest> VcrTests { get; set; }
        
        #region school database setting
        public  DbSet<FINCard> FINCards { get; set; }
        public  DbSet<FINCardConfig> FINCardConfigs { get; set; }
        public  DbSet<City> Base_Cities { get; set; }
        public DbSet<SchoolEntity> Schools { get; set; }
        #endregion


        public DbSet<LiveHostShow> LiveHostShows { get; set; }
        public DbSet<LiveAudiance> LiveAudiances { get; set; }
        public DbSet<UserMsg> UserMsgs { get; set; }
    }
}