using RentNDrive.Models;

namespace RentNDrive.ImplementationLogics.IBusinessLogic
{
    public interface IVehType 
    {
        string CreateandUpdateVType(int Id, VehicleType vehicletypename);
        VehicleType GetListById(int Id);
        List<VehicleType> GetList();
    }

    public interface IVehManufacturer
    {
        string CreateandUpdateVManufacturer(int Id, VehicleManufacturer vehiclemanufacturer);
        VehicleManufacturer GetListById(int Id);
        List<VehicleManufacturer> GetList();
    }
}
