using AppointmentScheduling.Models;
using AppointmentScheduling.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentScheduling.Services
{
    public interface IAppointmentServices
    {
        public List<DoctorVM> GetDoctorList();

        public List<PatientVM> GetPatientList();

        public Task<int> AddUpdate(AppointmentVM model);

        public List<AppointmentVM> DoctorEventById( string doctorId);
        public List<AppointmentVM> PatientEventById(string PatientId);



    }
}
