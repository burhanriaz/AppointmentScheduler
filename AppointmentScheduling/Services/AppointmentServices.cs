using AppointmentScheduling.Models;
using AppointmentScheduling.Models.AppDbContext;
using AppointmentScheduling.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduling.Services
{
    public class AppointmentServices : IAppointmentServices
    {
        private readonly AppDbContext _appDbcontext;
        public AppointmentServices(AppDbContext appDbContext)
        {
            _appDbcontext = appDbContext;
        }

        public async Task<int> AddUpdate(AppointmentVM model)
        {
            var startDate = DateTime.Parse(model.StartDate);
            var endDate = DateTime.Parse(model.StartDate).AddMinutes(Convert.ToDouble(model.Duration));

            if (model != null || model.Id > 0)
            {
                //update
                return 1;
            }
            else
            {
                //create
                Appointment appointment = new Appointment()
                {
                    Title = model.Title,
                    Discription = model.Discription,
                    StartDate = startDate,
                    EndDate = endDate,
                    Duration = model.Duration,
                    IsDoctorApproved = false,
                    DoctorId = model.DoctorId,
                    PatientId = model.PatientId,
                    AdminId = model.AdminId,

                };
                _appDbcontext.Appointments.Add(appointment);
                await _appDbcontext.SaveChangesAsync();
                return 2;
            }

        }
        public List<AppointmentVM> DoctorEventById(string doctorId)
        {
            var x = _appDbcontext.Appointments.Where(x => x.DoctorId == doctorId).ToList();
            List<AppointmentVM> d = new List<AppointmentVM>();

            foreach (var c in x)
            {
                var sa = _appDbcontext.Appointments.Select(x => new AppointmentVM() { 
                    Id = x.Id,
                    Title = x.Title,
                    Discription = c.Discription,
                    StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Duration = c.Duration,
                    IsDoctorApproved = c.IsDoctorApproved

                }).ToList();
                d = sa;

            }
           return d;
        }


    


    public List<DoctorVM> GetDoctorList()
    {
        var Doctor = (from user in _appDbcontext.Users
                      join userRole in _appDbcontext.UserRoles on user.Id equals userRole.UserId
                      join role in _appDbcontext.Roles.Where(x => x.Name == Helper.Helper.Doctor) on userRole.RoleId equals role.Id
                      select new DoctorVM
                      {
                          Id = user.Id,
                          Name = user.Name
                      }).ToList();
        return Doctor;

    }


    public List<PatientVM> GetPatientList()
    {
        var patients = (from user in _appDbcontext.Users
                        join userRoles in _appDbcontext.UserRoles on user.Id equals userRoles.UserId
                        join roles in _appDbcontext.Roles.Where(x => x.Name == Helper.Helper.Patient) on userRoles.RoleId equals roles.Id
                        select new PatientVM
                        {
                            Id = user.Id,
                            Name = user.Name
                        }
                       ).ToList();

        return patients;
    }

    public List<AppointmentVM> PatientEventById(string patientId)
    {
        var PatientEventById = _appDbcontext.Appointments.Where(x => x.PatientId == patientId).ToList().Select(c => new AppointmentVM()
        {
            Id = c.Id,
            Title = c.Title,
            Discription = c.Discription,
            StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
            EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
            Duration = c.Duration,
            IsDoctorApproved = c.IsDoctorApproved

        }).ToList();
        return PatientEventById;

    }
}
}
