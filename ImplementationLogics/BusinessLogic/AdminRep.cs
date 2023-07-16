using RentNDrive.Data;
using RentNDrive.ImplementationLogics.IBusinessLogic;
using RentNDrive.Models;

namespace RentNDrive.ImplementationLogics.BusinessLogic
{
    public class AdminRep : IVehType, IVehManufacturer
    {
        private readonly ApplicationDbContext _con;
        public AdminRep(ApplicationDbContext con)
        {
            _con = con;
        }
        public string CreateandUpdateVType(int Id, VehicleType vehicletype)
        {
            if (Id == 0) //insert
            {
                vehicletype.CreatedDate = DateTime.Now;
                _con.vehicleType.Add(vehicletype);
            }
            else //Update
            {   
                vehicletype.ModifiedDate = DateTime.Now;
                _con.vehicleType.Update(vehicletype);
            }
           var result= _con.SaveChanges();
            if (result>0)
                return "Success";
            return "Unable to to Process";        }

        public VehicleType GetListById(int Id)
        {
            return _con.vehicleType.FirstOrDefault(e=>e.Id == Id);
        }

        public List<VehicleType> GetList()
        {
            return _con.vehicleType.ToList();
        }

        public string CreateandUpdateVManufacturer(int Id, VehicleManufacturer vehiclemanufacturer)
        {
            if (Id == 0) //insert
            {
                vehiclemanufacturer.CreatedDate = DateTime.Now;
                _con.vehicleManufacturer.Add(vehiclemanufacturer);
            }
            else //Update
            {
                vehiclemanufacturer.ModifiedDate = DateTime.Now;
                _con.vehicleManufacturer.Update(vehiclemanufacturer);
            }
            var result = _con.SaveChanges();
            if (result > 0)
                return "Success";
            return "Unable to to Process";
        }

        VehicleManufacturer IVehManufacturer.GetListById(int Id)
        {
            return _con.vehicleManufacturer.FirstOrDefault(e => e.Id == Id);
        }

        List<VehicleManufacturer> IVehManufacturer.GetList()
        {
            return _con.vehicleManufacturer.ToList();
        }
    }
}
