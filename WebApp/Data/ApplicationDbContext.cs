using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;      // пространство имён с вашими моделями
using WebApp.Data;        // если ApplicationUser вы сделали в Data

namespace WebApp.Data
{
    // 1. Указываем ваш класс пользователя
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // 2. Добавляем DbSet для каждой из ваших таблиц
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<CameraSetting> CameraSettings { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        // 3. (Опционально) Конфигурация сущностей
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Уникальные индексы
            builder.Entity<Zone>()
                   .HasIndex(z => z.Name)
                   .IsUnique();

            builder.Entity<UserProfile>()
                   .HasIndex(up => up.UserId)
                   .IsUnique();

            builder.Entity<CameraSetting>()
                   .HasIndex(c => c.Name)
                   .IsUnique();

            // Явная настройка связей, если нужно
            builder.Entity<Schedule>()
                   .HasOne(s => s.Zone)
                   .WithMany()
                   .HasForeignKey(s => s.ZoneId);

            builder.Entity<Event>()
                   .HasOne(e => e.Zone)
                   .WithMany()
                   .HasForeignKey(e => e.ZoneId);

            builder.Entity<UserProfile>()
                   .HasOne(up => up.User)
                   .WithOne(u => u.Profile)       // если в ApplicationUser добавили navigation
                   .HasForeignKey<UserProfile>(up => up.UserId);
        }
    }
}
