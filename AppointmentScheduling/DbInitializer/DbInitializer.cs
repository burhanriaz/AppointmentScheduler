using AppointmentScheduling.DbInitializer;
using AppointmentScheduling.Models;
using AppointmentScheduling.Models.AppDbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace AppointmentScheduling.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly AppDbContext _appDbContext;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(AppDbContext appDbContext, UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _roleManager = roleManager;

        }

        public async void Initialize()
        {
            try
            {
                if(_appDbContext.Database.GetPendingMigrations().Count()>0)
                {
                    _appDbContext.Database.Migrate();
                }
            }
            catch (System.Exception)
            {

                throw;
            }
            if (_appDbContext.Roles.Any(x => x.Name == Helper.Helper.Admin)) return;

           
            _roleManager.CreateAsync(new IdentityRole(Helper.Helper.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Helper.Helper.Doctor)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Helper.Helper.Patient)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                Name ="Admin Ali",
            },"Admin@11").GetAwaiter().GetResult();
           ApplicationUser user = _appDbContext.Users.FirstOrDefault(u=>u.Email=="admin@gmail.com");
             _userManager.AddToRoleAsync(user, Helper.Helper.Admin).GetAwaiter().GetResult();
        }
    }
}
