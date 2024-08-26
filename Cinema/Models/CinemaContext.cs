using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Models;

public partial class CinemaContext : DbContext
{
    public CinemaContext()
    {
    }

    public CinemaContext(DbContextOptions<CinemaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountRole> AccountRoles { get; set; }

    public virtual DbSet<ActorAndDirector> ActorAndDirectors { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Calendar> Calendars { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Food> Foods { get; set; }

    public virtual DbSet<FoodCategory> FoodCategories { get; set; }

    public virtual DbSet<Membership> Memberships { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<MovieCategory> MovieCategories { get; set; }

    public virtual DbSet<MovieCategoryName> MovieCategoryNames { get; set; }

    public virtual DbSet<MovieRating> MovieRatings { get; set; }

    public virtual DbSet<MovieShow> MovieShows { get; set; }

    public virtual DbSet<OrderFood> OrderFoods { get; set; }

    public virtual DbSet<OrderTicket> OrderTickets { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<PersonRole> PersonRoles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<ServiceFeedBack> ServiceFeedBacks { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);uid=sa;pwd=sa;database=Cinema;TrustServerCertificate=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccId).HasName("PK__Account__91CBC398E611E552");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Phone, "UQ__Account__5C7E359E9104485B").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Account__A9D10534080EB380").IsUnique();

            entity.Property(e => e.AccId).HasColumnName("AccID");
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(4);
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Account__RoleID__4222D4EF");
        });

        modelBuilder.Entity<AccountRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__AccountR__8AFACE3A5380DCEC");

            entity.ToTable("AccountRole");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.AccountTypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<ActorAndDirector>(entity =>
        {
            entity.HasKey(e => new { e.MovieId, e.PersonId, e.RoleId }).HasName("ActorAndDirectorID");

            entity.ToTable("ActorAndDirector");

            entity.Property(e => e.MovieId).HasColumnName("MovieID");
            entity.Property(e => e.PersonId).HasColumnName("PersonID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Movie).WithMany(p => p.ActorAndDirectors)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ActorAndDirector_Movie");

            entity.HasOne(d => d.Person).WithMany(p => p.ActorAndDirectors)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ActorAndDirector_Person");

            entity.HasOne(d => d.Role).WithMany(p => p.ActorAndDirectors)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ActorAndDirector_RoleID");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Booking__73951ACD5A3BD53C");

            entity.ToTable("Booking");

            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.PuchaseDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Booking__Custome__5E8A0973");
        });

        modelBuilder.Entity<Calendar>(entity =>
        {
            entity.HasKey(e => e.CalendarId).HasName("PK__Calendar__53CFDBADFC800493");

            entity.ToTable("Calendar");

            entity.Property(e => e.CalendarId).HasColumnName("CalendarID");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.EmloyeeId).HasColumnName("EmloyeeID");
            entity.Property(e => e.Title).HasMaxLength(25);

            entity.HasOne(d => d.Emloyee).WithMany(p => p.Calendars)
                .HasForeignKey(d => d.EmloyeeId)
                .HasConstraintName("FK__Calendar__Emloye__76619304");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B87B0C3D03");

            entity.ToTable("Customer");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.AccId).HasColumnName("AccID");
            entity.Property(e => e.MembershipId).HasColumnName("MembershipID");

            entity.HasOne(d => d.Acc).WithMany(p => p.Customers)
                .HasForeignKey(d => d.AccId)
                .HasConstraintName("FK__Customer__AccID__45F365D3");

            entity.HasOne(d => d.Membership).WithMany(p => p.Customers)
                .HasForeignKey(d => d.MembershipId)
                .HasConstraintName("FK__Customer__Member__44FF419A");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF14524BE65");

            entity.ToTable("Employee");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.AccId).HasColumnName("AccID");
            entity.Property(e => e.Img).HasColumnName("img");
            entity.Property(e => e.Position).HasMaxLength(30);

            entity.HasOne(d => d.Acc).WithMany(p => p.Employees)
                .HasForeignKey(d => d.AccId)
                .HasConstraintName("FK__Employee__AccID__48CFD27E");
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.FoodId).HasName("PK__Food__856DB3CB56821923");

            entity.ToTable("Food");

            entity.Property(e => e.FoodId).HasColumnName("FoodID");
            entity.Property(e => e.FoodCategoryId).HasColumnName("FoodCategoryID");
            entity.Property(e => e.Image).HasColumnType("ntext");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Size).HasMaxLength(7);

            entity.HasOne(d => d.FoodCategory).WithMany(p => p.Foods)
                .HasForeignKey(d => d.FoodCategoryId)
                .HasConstraintName("FK__Food__FoodCatego__59FA5E80");
        });

        modelBuilder.Entity<FoodCategory>(entity =>
        {
            entity.HasKey(e => e.FoodCategoryId).HasName("PK__FoodCate__5451B7CB5CA51760");

            entity.ToTable("FoodCategory");

            entity.Property(e => e.FoodCategoryId).HasColumnName("FoodCategoryID");
            entity.Property(e => e.FoodCategoryName).HasMaxLength(20);
        });

        modelBuilder.Entity<Membership>(entity =>
        {
            entity.HasKey(e => e.MembershipId).HasName("PK__Membersh__92A785990CBBB643");

            entity.ToTable("Membership");

            entity.Property(e => e.MembershipId).HasColumnName("MembershipID");
            entity.Property(e => e.MembershiplevelName).HasMaxLength(10);
            entity.Property(e => e.RewardPoint).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.MovieId).HasName("PK__Movie__4BD2943AB906060A");

            entity.ToTable("Movie");

            entity.Property(e => e.MovieId).HasColumnName("MovieID");
            entity.Property(e => e.Background).HasColumnType("ntext");
            entity.Property(e => e.Country).HasMaxLength(20);
            entity.Property(e => e.FinishDate).HasColumnType("date");
            entity.Property(e => e.Language).HasMaxLength(20);
            entity.Property(e => e.License).IsUnicode(false);
            entity.Property(e => e.Poster).HasColumnType("ntext");
            entity.Property(e => e.ReleaseDate).HasColumnType("date");
            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.TrailerUrl).IsUnicode(false);
        });

        modelBuilder.Entity<MovieCategory>(entity =>
        {
            entity.HasKey(e => e.MovieCategoryId).HasName("PK__MovieCat__49ADB5C93CD7B2DF");

            entity.ToTable("MovieCategory");

            entity.Property(e => e.MovieCategoryId).HasColumnName("MovieCategoryID");
            entity.Property(e => e.MovieCategoryName).HasMaxLength(20);
        });

        modelBuilder.Entity<MovieCategoryName>(entity =>
        {
            entity.HasKey(e => new { e.MovieId, e.MovieCategoryId }).HasName("MovieCategoryNameID");

            entity.ToTable("MovieCategoryName");

            entity.Property(e => e.MovieId).HasColumnName("MovieID");
            entity.Property(e => e.MovieCategoryId).HasColumnName("MovieCategoryID");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("((1))")
                .HasColumnName("status");

            entity.HasOne(d => d.MovieCategory).WithMany(p => p.MovieCategoryNames)
                .HasForeignKey(d => d.MovieCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MovieCate__Movie__10566F31");

            entity.HasOne(d => d.Movie).WithMany(p => p.MovieCategoryNames)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MovieCate__Movie__0F624AF8");
        });

        modelBuilder.Entity<MovieRating>(entity =>
        {
            entity.HasKey(e => e.MovieRatingId).HasName("PK__MovieRat__AB2CC8532ACCAFDB");

            entity.ToTable("MovieRating");

            entity.Property(e => e.MovieRatingId).HasColumnName("MovieRatingID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.MovieId).HasColumnName("MovieID");

            entity.HasOne(d => d.Customer).WithMany(p => p.MovieRatings)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__MovieRati__Custo__6D0D32F4");

            entity.HasOne(d => d.Movie).WithMany(p => p.MovieRatings)
                .HasForeignKey(d => d.MovieId)
                .HasConstraintName("FK__MovieRati__Movie__6C190EBB");
        });

        modelBuilder.Entity<MovieShow>(entity =>
        {
            entity.HasKey(e => e.MovieShowId).HasName("PK__MovieSho__56C94AA6F3F6AB72");

            entity.ToTable("MovieShow");

            entity.Property(e => e.MovieShowId).HasColumnName("MovieShowID");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.MovieId).HasColumnName("MovieID");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Movie).WithMany(p => p.MovieShows)
                .HasForeignKey(d => d.MovieId)
                .HasConstraintName("FK__MovieShow__Movie__2180FB33");

            entity.HasOne(d => d.Room).WithMany(p => p.MovieShows)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__MovieShow__RoomI__22751F6C");
        });

        modelBuilder.Entity<OrderFood>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("OrderFood");

            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.FoodId).HasColumnName("FoodID");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Booking).WithMany()
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK__OrderFood__Booki__6166761E");

            entity.HasOne(d => d.Food).WithMany()
                .HasForeignKey(d => d.FoodId)
                .HasConstraintName("FK__OrderFood__FoodI__625A9A57");
        });

        modelBuilder.Entity<OrderTicket>(entity =>
        {
            entity.HasKey(e => new { e.BookingId, e.TicketId }).HasName("OrderTicketID");

            entity.ToTable("OrderTicket");

            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.TicketId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("TicketID");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Booking).WithMany(p => p.OrderTickets)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderTick__Booki__681373AD");

            entity.HasOne(d => d.Ticket).WithMany(p => p.OrderTickets)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderTick__Ticke__690797E6");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("PK__Person__AA2FFB8550A4A702");

            entity.ToTable("Person");

            entity.Property(e => e.PersonId).HasColumnName("PersonID");
            entity.Property(e => e.BirthDate).HasColumnType("date");
            entity.Property(e => e.Image).HasColumnType("ntext");
            entity.Property(e => e.PersonName).HasMaxLength(50);
            entity.Property(e => e.WikipediaUrl).IsUnicode(false);
        });

        modelBuilder.Entity<PersonRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__PersonRo__8AFACE3ACDE84400");

            entity.ToTable("PersonRole");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(20);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__Room__32863919DC823DDA");

            entity.ToTable("Room");

            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.RoomName).HasMaxLength(10);
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.SeatId).HasName("PK__Seat__311713D3D6D7C183");

            entity.ToTable("Seat");

            entity.Property(e => e.SeatId).HasColumnName("SeatID");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.SeatCategory).HasMaxLength(10);
            entity.Property(e => e.SeatName)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Room).WithMany(p => p.Seats)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__Seat__RoomID__2DE6D218");
        });

        modelBuilder.Entity<ServiceFeedBack>(entity =>
        {
            entity.HasKey(e => e.SeviceFeedBackId).HasName("PK__ServiceF__B7990B2B073AC56A");

            entity.ToTable("ServiceFeedBack");

            entity.Property(e => e.SeviceFeedBackId).HasColumnName("SeviceFeedBackID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.FeedBackDate).HasColumnType("date");

            entity.HasOne(d => d.Customer).WithMany(p => p.ServiceFeedBacks)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__ServiceFe__Custo__5070F446");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Ticket__712CC627CDB44B34");

            entity.ToTable("Ticket");

            entity.Property(e => e.TicketId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("TicketID");
            entity.Property(e => e.MovieShowId).HasColumnName("MovieShowID");
            entity.Property(e => e.SeatId).HasColumnName("SeatID");
            entity.Property(e => e.Status).HasDefaultValueSql("((0))");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.MovieShow).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.MovieShowId)
                .HasConstraintName("FK__Ticket__MovieSho__42E1EEFE");

            entity.HasOne(d => d.Seat).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.SeatId)
                .HasConstraintName("FK__Ticket__SeatID__43D61337");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
