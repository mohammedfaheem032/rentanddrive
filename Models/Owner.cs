using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentNDrive.Models
{
    public class VehicleInfo
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("IdentityUser")]
        public string OwnerId { get; set; }
        [Required,Display(Name = "Vehicle Name")]
        public string VehicleName { get; set; }
        [Required, Display(Name = "Vehicle Number")]
        public string VehicleNo { get; set; }
        [Required, Display(Name = "Registration Date")]
        public DateTime RegistrationDate { get; set; }
        [Display(Name = "Vehicle Type")]
        [ForeignKey("VehicleType")]
        public virtual int VehicleType { get; set; }
        [NotMapped]
        public string NMVehicleType { get; set; }
        [ForeignKey("VehicleManufacturer")]
        public virtual int VehicleManufacturer { get; set; }
        [NotMapped]
        public string NMVehicleManufacturer { get; set; }
        [Required, Display(Name = "Vehicle Price $")]
        public decimal VehiclePrice { get; set; }
        [Required, Display(Name = "Price Type (eg. Hour, Week)")]
        public string PriceType { get; set; }
        [Display(Name = "Doors")]
        public int Doors { get; set; }
        [Display(Name = "Colour")]
        public string Colour { get; set; }
        [Display(Name = "Passengers")]
        public int Passengers { get; set; }
        [Display(Name = "Luggage")]
        public string Luggage { get; set; }
        [Display(Name = "Transmission")]
        public string Transmission { get; set; }
        [Display(Name = "Air conditioning")]
        public string AirConditioning { get; set; }
        [Display(Name = "Terms")]
        public string Terms { get; set; }
        [Display(Name = "Vehicle Available Status")]
        public bool VehicleAvailableStatus { get; set; }
        [Display(Name = "FeatureVehicle")]
        public bool FeatureVehicle { get; set; }
        [Display(Name = "Vehicle Images")]
        public string VehicleImage { get; set; }
        [Display(Name = "Vehicle Show / Hide")]
        public bool VehicleActiveStatus { get; set; }
        [Required, Display(Name = "Location")]
        public string Location { get; set; }
        [Required, Display(Name = "Latitude")]
        public string VLatitude { get; set; }
        [Required, Display(Name = "Longitude")]
        public string VLongitude { get; set; }
        [NotMapped]
        [Required, Display(Name = "Vehicle Images")]
        public IFormFile NMVehicleImage { get; set; }
        [NotMapped]        
        public string NMOwnerName { get; set; }
    }
    public class BookInfo
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("IdentityUser")]
        public string RenterId { get; set; }
        [ForeignKey("VehicleInfo")]
        [Display(Name = "Vehicle Name")]
        public int VehicleId { get; set; }
        [Required, Display(Name = "Pick Up Location")]
        public string PickUpLocation { get; set; }
        [Required, Display(Name = "Pick Up Date")]
        public DateTime PickUpDate { get; set; }
        [Required, Display(Name = "Drop Off Location")]
        public string DropOffLocation { get; set; }
        [Required, Display(Name = "Drop Off Date")]
        public DateTime DropOffDate { get; set; }
        public string PaymentStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime PaidDate { get; set; }
        public String PaymentMethod { get; set; }
        [NotMapped]
        public string NMOwnerName { get; set; }
        [NotMapped]
        public string NMRenterName { get; set; }
        [NotMapped]
        public string NMVehicleName { get; set; }
        [NotMapped]
        public string NMVehiclePriceType { get; set; }
        [NotMapped]
        public string NMVehiclePrice { get; set; }

    }
    public class Comment
    {
        public int Id { get; set; }
        [Required, Display(Name = "Comments")]
        public string Comments { get; set; }
        [Required, Display(Name = "Ratings")]
        public int Ratings { get; set; }
        public string OwnerId { get; set; }
        [NotMapped]
        public string RenterEmail { get; set; }
        [NotMapped]
        public string RenterName { get; set; }
        public string RenterId { get; set; }
        public int VehicleId { get; set; }
        public DateTime CommentsDate { get; set; } = DateTime.Now;

    }
    public class Payment
    {
        public int Id { get; set; }
        public string RenterId { get; set; }
        public int BookingId { get; set; }
        [Required, Display(Name = "Card Name")]
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string CardExpiryDate { get; set; }
        public string CVV { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public DateTime ExpiryDate { get; set; }
        [NotMapped]
        public string NMVehicleTitle { get; set; }
        [NotMapped]
        public decimal NMPaymentPrice { get; set; }
        [NotMapped]
        public string NMOwnerName { get; set; }
        [NotMapped]
        public string NMRenterName { get; set; }
        [NotMapped]
        public string NMPickUpLocation { get; set; }
        [NotMapped]
        public string NMDropLocation { get; set; }
        [NotMapped]
        public DateTime NMPickUpDate { get; set; }
        [NotMapped]
        public DateTime NMDropOffDate { get; set; }
    }
    public class Survey
    {
        public int Id { get; set; }
        //public string Question1 { get; set; }        
        public SurveyResult Q1Survey { get; set; }
        //public string Question2 { get; set; }
        public SurveyResult Q2Survey { get; set; }
        
        public SurveyResult Q3Survey { get; set; }        
        public SurveyResult Q4Survey { get; set; }        
        public string Q5Survey { get; set; }

        public string OwnerId { get; set; }
        [NotMapped]
        public string RenterEmail { get; set; }
        [NotMapped]
        public string NMRenterName { get; set; }
        [NotMapped]
        public string NMOwnerName { get; set; }
        [NotMapped]
        public string NMVehicleTitle { get; set; }
        public string RenterId { get; set; }
        public int VehicleId { get; set; }
        public DateTime SurveyDate { get; set; }

    }
    public enum SurveyResult
    {
        Excellent,
        Satisfactory,
        Neutral,
        NotGoodEnough,
        ExtremelyUnsatisfactory
    }
    public class Chat
    {
        public int Id { get; set; }
        [ForeignKey("IdentityUser")]
        public string OwnerId { get; set; }
        [ForeignKey("IdentityUser")]
        public string RenterId { get; set; }
        public int? VehicleId { get; set; }
        public string SenderInfo { get; set; }
        [Required, Display(Name = "Message")]
        public string Message { get; set; }
        public DateTime SendDate { get; set; } = DateTime.Now;       
        [NotMapped]
        public string OwnerNameNotMapped { get; set; }
        [NotMapped]
        public string RenterNameNotMapped { get; set; }
        [NotMapped]
        public string VehicleNameNotMapped { get; set; }

    }

}
