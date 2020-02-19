using Microsoft.EntityFrameworkCore;
using SocialNetwork.ConfigSetting.Dal.Models;

namespace SocialNetwork.ConfigSetting.Dal.Context
{
    public class ConfigSettingContext : DbContext
    {
        private readonly string _connectionString;

        public ConfigSettingContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ConfigSettingContext(DbContextOptions<ConfigSettingContext> options, string connectionString)
            : base(options)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Setting<int>>(entity =>
            {
                entity.ToTable("SettingNumber");
                entity.HasKey(x => x.Key);
            });
            modelBuilder.Entity<Setting<string>>(entity =>
            {
                entity.ToTable("SettingText");
                entity.HasKey(x => x.Key);
            });
        }
    }
}
