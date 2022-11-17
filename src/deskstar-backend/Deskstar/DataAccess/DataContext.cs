using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Deskstar.Entities;

namespace Deskstar.DataAccess
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<Building> Buildings { get; set; } = null!;
        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<Desk> Desks { get; set; } = null!;
        public virtual DbSet<DeskType> DeskTypes { get; set; } = null!;
        public virtual DbSet<Floor> Floors { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Room> Rooms { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase(databaseName: "TestDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Booking");

                entity.Property(e => e.BookingId)
                    .HasColumnName("BookingID")
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.DeskId).HasColumnName("DeskID");

                entity.Property(e => e.EndTime).HasColumnType("timestamp without time zone");

                entity.Property(e => e.StartTime).HasColumnType("timestamp without time zone");

                entity.Property(e => e.Timestamp)
                    .HasColumnType("timestamp without time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(2)");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Desk)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.DeskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Booking_Desk_null_fk");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Booking_User_null_fk");
            });

            modelBuilder.Entity<Building>(entity =>
            {
                entity.ToTable("Building");

                entity.Property(e => e.BuildingId)
                    .HasColumnName("BuildingID")
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.BuildingName).HasColumnType("character varying");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.Location).HasColumnType("character varying");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Buildings)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("foreign_key_name");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("CompanyID")
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.CompanyName).HasColumnType("character varying");
            });

            modelBuilder.Entity<Desk>(entity =>
            {
                entity.ToTable("Desk");

                entity.Property(e => e.DeskId)
                    .HasColumnName("DeskID")
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.DeskName).HasColumnType("character varying");

                entity.Property(e => e.DeskTypeId).HasColumnName("DeskTypeID");

                entity.Property(e => e.RoomId).HasColumnName("RoomID");

                entity.HasOne(d => d.DeskType)
                    .WithMany(p => p.Desks)
                    .HasForeignKey(d => d.DeskTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Desk_DeskType_null_fk");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Desks)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Desk_Room_null_fk");
            });

            modelBuilder.Entity<DeskType>(entity =>
            {
                entity.ToTable("DeskType");

                entity.Property(e => e.DeskTypeId)
                    .HasColumnName("DeskTypeID")
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.DeskTypeName).HasColumnType("character varying");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.DeskTypes)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DeskType_Company_null_fk");
            });

            modelBuilder.Entity<Floor>(entity =>
            {
                entity.ToTable("Floor");

                entity.Property(e => e.FloorId)
                    .HasColumnName("FloorID")
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.BuildingId).HasColumnName("BuildingID");

                entity.Property(e => e.FloorName).HasColumnType("character varying");

                entity.HasOne(d => d.Building)
                    .WithMany(p => p.Floors)
                    .HasForeignKey(d => d.BuildingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Floor_Building_null_fk");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId)
                    .HasColumnName("RoleID")
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.RoleName).HasColumnType("character varying");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Roles)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Role_Company_null_fk");

                entity.HasMany(d => d.Users)
                    .WithMany(p => p.Roles)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserRole",
                        l => l.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("UserRole_User_null_fk"),
                        r => r.HasOne<Role>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("UserRole_Role_null_fk"),
                        j =>
                        {
                            j.HasKey("RoleId", "UserId").HasName("UserRole_pk");

                            j.ToTable("UserRole");

                            j.IndexerProperty<Guid>("RoleId").HasColumnName("RoleID");

                            j.IndexerProperty<Guid>("UserId").HasColumnName("UserID");
                        });
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");

                entity.Property(e => e.RoomId)
                    .HasColumnName("RoomID")
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.FloorId).HasColumnName("FloorID");

                entity.Property(e => e.RoomName).HasColumnType("character varying");

                entity.HasOne(d => d.Floor)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.FloorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Room_Floor_null_fk");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.MailAddress, "User_Mail")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.FirstName).HasColumnType("character varying");

                entity.Property(e => e.IsApproved).HasDefaultValueSql("false");

                entity.Property(e => e.LastName).HasColumnType("character varying");

                entity.Property(e => e.MailAddress).HasColumnType("character varying");

                entity.Property(e => e.Password).HasColumnType("character varying");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CompanyID_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
