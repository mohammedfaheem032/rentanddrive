namespace RentNDrive.Models.UIModel
{
    public class VehTypeView
    {
        public VehicleType vehicleTypeNew { get; set; }
        public IEnumerable<VehicleType> vehicleTypeList { get; set; }
    }
    public class VehManufacturerView
    {
        public VehicleManufacturer vehicleManNew { get; set; }
        public IEnumerable<VehicleManufacturer> vehicleManList { get; set; }
    }

    public class AdminView
    {
        public IEnumerable<UserInfo> UserList { get; set; }
        public UserInfo GetuserInfo { get; set; }
    }
}
