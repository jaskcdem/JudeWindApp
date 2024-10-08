using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataAcxess.ProjectContext
{
    public partial class ProjectContext(DbContextOptions<ProjectContext> options) : DbContext(options)
    {
        public virtual DbSet<Sys_Type> SysType { get; set; }
        public virtual DbSet<Sys_Code> SysCode { get; set; }
        public virtual DbSet<Sys_SystemSetting> SystemSetting { get; set; }
        public virtual DbSet<Sys_RoleGroup> RoleGroup { get; set; }
        public virtual DbSet<Sys_RoleGroupPermission> RoleGroupPermission { get; set; }
        public virtual DbSet<Sys_RoleGroupUser> RoleGroupUser { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sys_Code>(entity =>
            {
               entity.HasKey(e => new { e.TYPE, e.CODE, e.LEVEL, e.PARENT_PATH });
               entity.ToTable(e => e.HasComment("系統代碼檔"));
               entity.Property(e => e.TYPE).HasComment("類型");
               entity.Property(e => e.CODE).HasComment("代碼");
               entity.Property(e => e.LEVEL).HasComment("代碼層級");
               entity.Property(e => e.PARENT_PATH).HasComment("上層路徑");
               entity.Property(e => e.CreateDatetime).HasComment("建立時間");
               entity.Property(e => e.CreateUser).IsUnicode().HasComment("建立者");
               entity.Property(e => e.DESC).HasComment("內容");
               entity.Property(e => e.Status).HasComment("是否作廢");
               entity.Property(e => e.UpdateDatetime).HasComment("更新時間");
               entity.Property(e => e.UpdateUser).IsUnicode().HasComment("新增者");
            });

            modelBuilder.Entity<Sys_RoleGroup>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.System });
                entity.ToTable(e => e.HasComment("系統權限檔"));
                entity.Property(e => e.Id).IsUnicode().HasComment("權限Unikey");
                entity.Property(e => e.System).HasComment("系統名稱");
                entity.Property(e => e.Name).HasComment("權限名稱");
                entity.Property(e => e.CreateDateTime).HasComment("建立時間");
                entity.Property(e => e.CreateUserId).IsUnicode().HasComment("建立者");
                entity.Property(e => e.UpdateDateTime).HasComment("更新時間");
                entity.Property(e => e.UpdateUserId).IsUnicode().HasComment("更新者");
            });

            modelBuilder.Entity<Sys_RoleGroupPermission>(entity =>
            {
                entity.ToTable(e => e.HasComment("權限群組控制檔"));
                entity.Property(e => e.Id).ValueGeneratedOnAdd().HasComment("權限群組控制流水號");
                entity.Property(e => e.MFFuncId).IsUnicode(false).HasComment("系統項目");
                entity.Property(e => e.MFSystem).IsUnicode(false).HasComment("系統名稱");
                entity.Property(e => e.RGId).IsUnicode().HasComment("權限群組");
                entity.HasOne(d => d.Sys_RoleGroup)
                    .WithMany(p => p.Sys_RoleGroupPermission)
                    .HasForeignKey(d => new { d.RGId, d.MFSystem });
            });

            modelBuilder.Entity<Sys_RoleGroupUser>(entity =>
            {
                entity.HasKey(e => new { e.RGId, e.UserId });
                entity.ToTable(e => e.HasComment("使用者權限群組檔"));
                entity.Property(e => e.RGId).IsUnicode().HasComment("權限群組");
                entity.Property(e => e.UserId).IsUnicode().HasComment("權限群組人員");
                entity.HasOne(d => d.RG_)
                    .WithMany(p => p.Sys_RoleGroupUser)
                    .HasForeignKey(d => new { d.RGId });
            });

            modelBuilder.Entity<Sys_SystemSetting>(entity =>
            {
                entity.HasKey(e => new { e.Key, e.Type });
                entity.ToTable(e => e.HasComment("系統設定檔"));
                entity.Property(e => e.Key).HasComment("實體/虛擬");
                entity.Property(e => e.Type).HasComment("類型");
                entity.Property(e => e.ModifiedDateTime).HasComment("更新時間");
                entity.Property(e => e.ModifiedUser).IsUnicode().HasComment("更新者");
                entity.Property(e => e.Name).HasComment("詳細名稱");
                entity.Property(e => e.Order).HasComment("序列");
                entity.Property(e => e.Value).HasComment("值");
            });

            modelBuilder.Entity<Sys_Type>(entity =>
            {
                entity.ToTable(e => e.HasComment("系統代碼類型檔"));
                entity.Property(e => e.TYPE).IsUnicode(false).HasComment("類型");
                entity.Property(e => e.DESC).HasComment("描述");
                entity.Property(e => e.EDITOR).HasDefaultValueSql("((1))").HasComment("是否可編輯");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.ToTable(e => e.HasComment("使用者總表"));
                entity.Property(e => e.ID).IsUnicode().HasComment("使用者ID");
                entity.Property(e => e.Check).HasComment("是否啟用");
                entity.Property(e => e.Email).IsUnicode(false);
                entity.Property(e => e.Idno).IsUnicode(false).HasComment("身分證字號");
                entity.Property(e => e.LastLoginIP).IsUnicode(false);
                entity.Property(e => e.Name).HasComment("姓名");
                entity.Property(e => e.Psw).IsUnicode(false).HasComment("密碼");
                entity.Property(e => e.CheckDate).HasComment("啟用時間");
                entity.Property(e => e.CreateDate).HasComment("建立時間");
                entity.Property(e => e.UpdateDataTime).HasComment("更新時間");
                entity.Property(e => e.UpdateUser).IsUnicode().HasComment("更新者");
            });

            OnModelCreatingGeneratedProcedures(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
