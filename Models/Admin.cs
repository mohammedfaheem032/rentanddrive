using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentNDrive.Models
{
    public class VehicleManufacturer
    {
        public int Id { get; set; }
        [Required]
        public string VehicleManufacturerName { get; set; }
        public string? Remarks { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
    public class VehicleType
    {
        public int Id { get; set; }
        [Required]
        public string VehicleTypeName { get; set; }
        public string? Remarks { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }

    }
