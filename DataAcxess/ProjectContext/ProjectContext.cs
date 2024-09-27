using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataAcxess.ProjectContext
{
    public partial class ProjectContext(DbContextOptions<ProjectContext> options) : DbContext(options)
    {
        public virtual DbSet<Sys_CODE> Sys_CODE { get; set; }
        public virtual DbSet<Sys_RoleGroup> Sys_RoleGroup { get; set; }
        public virtual DbSet<Sys_RoleGroupPermission> Sys_RoleGroupPermission { get; set; }
        public virtual DbSet<Sys_RoleGroupUser> Sys_RoleGroupUser { get; set; }
        public virtual DbSet<Sys_SystemSetting> Sys_SystemSetting { get; set; }
        public virtual DbSet<Sys_TYPE> Sts_TYPE { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sys_CODE>(entity =>
           {
               entity.HasKey(e => new { e.TYPE, e.CODE, e.LEVEL, e.PARENT_PATH });
               entity.ToTable(e => e.HasComment("系統代碼檔"));
               entity.Property(e => e.TYPE).IsUnicode(false).HasComment("類型");
               entity.Property(e => e.CODE).IsUnicode(false).HasComment("代碼");
               entity.Property(e => e.LEVEL).HasComment("代碼層級");
               entity.Property(e => e.PARENT_PATH).IsUnicode(false).HasComment("資料路徑");
               entity.Property(e => e.CreateDatetime).HasComment("建立時間");
               entity.Property(e => e.CreateUser).IsUnicode(false).HasComment("建立者");
               entity.Property(e => e.DESC).HasComment("內容");
               entity.Property(e => e.Status).HasComment("是否作廢");
               entity.Property(e => e.UpdateDatetime).HasComment("更新時間");
               entity.Property(e => e.UpdateUser).IsUnicode(false).HasComment("新增者");
           });

            modelBuilder.Entity<Sys_RoleGroup>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.System });
                entity.ToTable(e => e.HasComment("系統權限檔"));
                entity.Property(e => e.Id).IsUnicode(false).HasComment("權限Unikey");
                entity.Property(e => e.System).IsUnicode(false).HasComment("系統名稱");
                entity.Property(e => e.Name).HasComment("權限名稱");
                entity.Property(e => e.CreateDateTime).HasComment("建立時間");
                entity.Property(e => e.CreateUserId).IsUnicode(false).HasComment("建立者");
                entity.Property(e => e.UpdateDateTime).HasComment("更新時間");
                entity.Property(e => e.UpdateUserId).IsUnicode(false).HasComment("更新者");
            });

            modelBuilder.Entity<Sys_RoleGroupPermission>(entity =>
            {
                entity.Property(e => e.Id).HasComment("權限群組控制流水號");
                entity.Property(e => e.MFFuncId).IsUnicode(false).HasComment("系統項目");
                entity.Property(e => e.MFSystem).IsUnicode(false).HasComment("系統名稱");
                entity.Property(e => e.RGId).IsUnicode(false).HasComment("權限群組");
                entity.HasOne(d => d.Sys_RoleGroup)
                    .WithMany(p => p.Sys_RoleGroupPermission)
                    .HasForeignKey(d => new { d.RGId, d.MFSystem })
                    .HasConstraintName("FK_Sys_RolePermission_Sys_RoleGroup");
            });

            modelBuilder.Entity<Sys_RoleGroupUser>(entity =>
            {
                entity.HasKey(e => new { e.RGId, e.RGSystem, e.UserId });
                entity.Property(e => e.RGId).IsUnicode(false).HasComment("權限群組");
                entity.Property(e => e.RGSystem).IsUnicode(false).HasComment("系統名稱");
                entity.Property(e => e.UserId).IsUnicode(false).HasComment("權限群組人員");
                entity.HasOne(d => d.RG_)
                    .WithMany(p => p.Sys_RoleGroupUser)
                    .HasForeignKey(d => new { d.RGId, d.RGSystem })
                    .HasConstraintName("FK_Sys_RoleGroupUser_Sys_RoleGroup");
            });

            modelBuilder.Entity<Sys_SystemSetting>(entity =>
            {
                entity.HasKey(e => new { e.Key, e.Type });
                entity.Property(e => e.Key).HasComment("實體/虛擬");
                entity.Property(e => e.Type).HasComment("類型");
                entity.Property(e => e.ModifiedDateTime).HasComment("更新時間");
                entity.Property(e => e.ModifiedUser).HasComment("更新者");
                entity.Property(e => e.Name).HasComment("詳細名稱");
                entity.Property(e => e.Order).HasComment("序列");
                entity.Property(e => e.Value).HasComment("值");
            });

            modelBuilder.Entity<Sys_TYPE>(entity =>
            {
                entity.Property(e => e.TYPE).IsUnicode(false).HasComment("類型");
                entity.Property(e => e.DESC).HasComment("描述");
                entity.Property(e => e.EDITOR).HasDefaultValueSql("((1))").HasComment("是否可編輯");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasKey(e => e.ID).HasName("PK__UserInfo__5A2040DB816A9B64");
                entity.Property(e => e.ID).IsUnicode(false).HasComment("人員ID");
                entity.Property(e => e.Check).HasComment("是否啟用");
                entity.Property(e => e.Email).IsUnicode(false);
                entity.Property(e => e.Idno).IsUnicode(false).HasComment("身分證字號");
                entity.Property(e => e.LastLoginIP).IsUnicode(false);
                entity.Property(e => e.Name).HasComment("人員姓名");
                entity.Property(e => e.Psw).IsUnicode(false).HasComment("密碼");
                entity.Property(e => e.CheckDate).HasComment("啟用時間");
                entity.Property(e => e.CreateDate).HasComment("建立時間");
                entity.Property(e => e.UpdateDataTime).HasComment("更新時間");
                entity.Property(e => e.UpdateUser).IsUnicode(false).HasComment("更新者");
            });

            OnModelCreatingGeneratedProcedures(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
