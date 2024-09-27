using Microsoft.EntityFrameworkCore;

namespace DataAcxess.LogContext
{
    public partial class LogContext(DbContextOptions<LogContext> options) : DbContext(options)
    {
        public virtual DbSet<Sys_ApiLog> SYS_ApiLog { get; set; }
        public virtual DbSet<Sys_UserRecord> SYS_UserRecord { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sys_ApiLog>(entity =>
            {
                entity.Property(e => e.ControllerName).IsUnicode(false);
                entity.Property(e => e.FunctionName).IsUnicode(false);
                entity.Property(e => e.SessionId).IsUnicode(false);
                entity.Property(e => e.Source).IsUnicode(false);
                entity.Property(e => e.System).IsUnicode(false);
            });

            modelBuilder.Entity<Sys_UserRecord>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.UR_RecordDateTime, e.RecordEvent });
                entity.Property(e => e.UserId).IsUnicode(false).HasComment("使用者");
                entity.Property(e => e.UR_RecordDateTime).HasComment("紀錄時間");
                entity.Property(e => e.RecordEvent).HasComment("紀錄API");
                entity.Property(e => e.IP).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
