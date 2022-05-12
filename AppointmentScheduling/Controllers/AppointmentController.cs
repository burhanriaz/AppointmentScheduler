using AppointmentScheduling.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduling.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentServices _appointmentService;

        public AppointmentController(IAppointmentServices appointmentServices)
        {
            _appointmentService =   appointmentServices;
        }
        public IActionResult Index()
        {
            ViewBag.Duration = Helper.Helper.GetTimeDropDown();
            ViewBag.DoctorList = _appointmentService.GetDoctorList();
            ViewBag.PatientList = _appointmentService.GetPatientList();

            return View();
        }
    }
}
