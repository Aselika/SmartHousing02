using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
              .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddRazorPages();

            var app = builder.Build();

            // --- НАЧАЛО: Seed ролей и первичного Администратора ---
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                // создаём роли, если их нет
                foreach (var roleName in new[] { "Admin", "User" })
                {
                    if (!roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                        roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }

                // создаём администратора, если его нет
                var adminEmail = "admin@ex.com";
                var adminPassword = "Admin#123";  // поменяйте на свой пароль

                var admin = userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();
                if (admin == null)
                {
                    admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail };
                    var result = userManager.CreateAsync(admin, adminPassword).GetAwaiter().GetResult();
                    if (!result.Succeeded)
                        throw new Exception("Не удалось создать администратора: " +
                            string.Join("; ", result.Errors.Select(e => e.Description)));

                    var token = userManager.GenerateEmailConfirmationTokenAsync(admin).GetAwaiter().GetResult();
                    userManager.ConfirmEmailAsync(admin, token).GetAwaiter().GetResult();
                }

                // назначаем роль Admin
                if (!userManager.IsInRoleAsync(admin, "Admin").GetAwaiter().GetResult())
                    userManager.AddToRoleAsync(admin, "Admin").GetAwaiter().GetResult();
            }
            // --- КОНЕЦ: Seed ролей и администратора ---


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
