using Microsoft.EntityFrameworkCore;

namespace WebApiDeloitte.Model
{
    public class Context : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Bulletin> Bulletins { get; set; }
        public DbSet<BulletinGrade> BulletinGrades { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Bulletin>().ToTable("Bulletins");
            modelBuilder.Entity<BulletinGrade>().ToTable("BulletinGrade");
            modelBuilder.Entity<Discipline>().ToTable("Discipline");

            modelBuilder.Entity<Student>().HasKey(s => s.Id).HasName("PK_Students");
            modelBuilder.Entity<Bulletin>().HasKey(b => b.Id).HasName("PK_Bulletins");
            modelBuilder.Entity<BulletinGrade>().HasKey(bg => bg.Id).HasName("PK_BulletionGrades");
            modelBuilder.Entity<Discipline>().HasKey(d => d.Id).HasName("PK_Disciplines");

            modelBuilder.Entity<Student>().Property(d => d.Id).HasColumnType("Int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<Student>().Property(d => d.Name).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Student>().Property(d => d.Email).HasColumnType("nvarchar(100)").IsRequired(false);
            modelBuilder.Entity<Student>().Property(d => d.BirthDate).HasColumnType("dateTime").IsRequired();

            modelBuilder.Entity<Bulletin>().Property(d => d.Id).HasColumnType("Int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<Bulletin>().Property(d => d.IdStudenty).HasColumnType("int").IsRequired();
            modelBuilder.Entity<Bulletin>().Property(d => d.DeliveryDate).HasColumnType("dateTime").IsRequired();

            modelBuilder.Entity<BulletinGrade>().Property(d => d.Id).HasColumnType("Int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<BulletinGrade>().Property(d => d.IdDiscipline).HasColumnType("int").IsRequired();
            modelBuilder.Entity<BulletinGrade>().Property(d => d.IdBulletin).HasColumnType("int").IsRequired();
            modelBuilder.Entity<BulletinGrade>().Property(d => d.Grade).HasColumnType("int").IsRequired();

            modelBuilder.Entity<Discipline>().Property(d => d.Id).HasColumnType("Int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<Discipline>().Property(d => d.Name).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Discipline>().Property(d => d.Workload).HasColumnType("int").IsRequired();

            //modelBuilder.Entity<User>().HasOne<UserGroup>().WithMany().HasPrincipalKey(ug => ug.Id).HasForeignKey(u => u.UserGroupId).OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_Users_UserGroups");
            //modelBuilder.Entity<Bulletin>().HasOne<BulletinGrade>().WithMany().HasPrincipalKey(bg => bg.Id).HasForeignKey(d => d.id)
        }
    }
}
