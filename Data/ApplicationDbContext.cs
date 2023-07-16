using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentNDrive.Models;

namespace RentNDrive.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserInfo>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<VehicleManufacturer> vehicleManufacturer { get; set; }
        public DbSet<VehicleType> vehicleType { get; set; }
        public DbSet<VehicleInfo> VehicleInfo { get; set; }
        public DbSet<BookInfo> BookInfo { get; set; }
        public DbSet<Comment> CommentsInfo { get; set; }
        public DbSet<Payment> PaymentInfo { get; set; }
        public DbSet<Survey> SurveyInfo { get; set; }
        public DbSet<Chat> ChatInfo { get; set; }
    }
}