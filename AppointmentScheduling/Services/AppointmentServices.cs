using AppointmentScheduling.Models;
using AppointmentScheduling.Models.AppDbContext;
using AppointmentScheduling.Models.ViewModel;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduling.Services
{
    public class AppointmentServices : IAppointmentServices
    {
        private readonly AppDbContext _appDbcontext;
        private readonly IEmailSender _emailSender;

        public AppointmentServices(AppDbContext appDbContext, IEmailSender emailSender)
        {
            _appDbcontext = appDbContext;
            _emailSender = emailSender;
        }

        public async Task<int> AddUpdate(AppointmentVM model)
        {
            var startDate = DateTime.Parse(model.StartDate);
            var endDate = DateTime.Parse(model.StartDate).AddMinutes(Convert.ToDouble(model.Duration));
            var patient = _appDbcontext.Users.FirstOrDefault(x => x.Id == model.PatientId);
            var doctor = _appDbcontext.Users.FirstOrDefault(x => x.Id == model.DoctorId);


            if (model != null && model.Id > 0)
            {
                //update
                var appointment = _appDbcontext.Appointments.Where(x => x.Id == model.Id).FirstOrDefault();
                appointment.Title = model.Title;
                appointment.Description = model.Description;
                appointment.StartDate = startDate;
                appointment.EndDate = endDate;
                appointment.Duration = model.Duration;
                appointment.IsDoctorApproved = false;
                appointment.DoctorId = model.DoctorId;
                appointment.PatientId = model.PatientId;
                appointment.AdminId = model.AdminId;
               await _appDbcontext.SaveChangesAsync();

                return 1;
            }
            else
            {
                //create
                Appointment appointment = new Appointment()
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartDate = startDate,
                    EndDate = endDate,
                    Duration = model.Duration,
                    IsDoctorApproved = false,
                    DoctorId = model.DoctorId,
                    PatientId = model.PatientId,
                    AdminId = model.AdminId,

                };
                await _emailSender.SendEmailAsync(doctor.Email, "Appointment Created",
                    $"Your Appointment With {patient.Name} is Created and in Pending Status");

                await _emailSender.SendEmailAsync(patient.Email, "Appointment Created",
                  $"Your Appointment With {doctor.Name} is Created and in Pending Status");


                _appDbcontext.Appointments.Add(appointment);
                await _appDbcontext.SaveChangesAsync();
                return 2;
            }

        }

        public async Task<int> DeleteAppoinment(int id)
        {
            var appointment = _appDbcontext.Appointments.Where(x => x.Id == id).FirstOrDefault();
            if (appointment != null)
            {
                _appDbcontext.Appointments.Remove(appointment);
                return await _appDbcontext.SaveChangesAsync();

            }
            return 0;
        }

        public List<AppointmentVM> DoctorEventById(string doctorId)
        {

            var PatientEventById = _appDbcontext.Appointments.Where(x => x.DoctorId == doctorId).ToList().Select(c => new AppointmentVM()
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Duration = c.Duration,
                IsDoctorApproved = c.IsDoctorApproved

            }).ToList();
            return PatientEventById;
        }
        public AppointmentVM GetAppointmentById(int id)
        {
            var GetAppointmentById = _appDbcontext.Appointments.Where(x => x.Id == id).ToList().Select(c => new AppointmentVM()
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Duration = c.Duration,
                IsDoctorApproved = c.IsDoctorApproved,
                DoctorId = c.DoctorId,
                PatientId = c.PatientId,
                PatientName = _appDbcontext.Users.Where(x => x.Id == c.PatientId).Select(x => x.Name).FirstOrDefault(),
                DoctorName = _appDbcontext.Users.Where(x => x.Id == c.DoctorId).Select(x => x.Name).FirstOrDefault(),
            }).SingleOrDefault();
            return GetAppointmentById;

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
                Description = c.Description,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Duration = c.Duration,
                IsDoctorApproved = c.IsDoctorApproved

            }).ToList();
            return PatientEventById;

        }

        public async Task<int> ConfirmEvent(int id)
        {
            var appointment = _appDbcontext.Appointments.Where(x => x.Id == id).FirstOrDefault();
            if (appointment != null)
            {
                appointment.IsDoctorApproved = true;
                return await _appDbcontext.SaveChangesAsync();

            }
            return 0;
        }
    }
}
