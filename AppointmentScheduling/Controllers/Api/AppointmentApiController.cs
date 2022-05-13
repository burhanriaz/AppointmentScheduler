using AppointmentScheduling.Models;
using AppointmentScheduling.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AppointmentScheduling.Controllers.Api
{
    [Route("api/Appointment")]
    [ApiController]
    public class AppointmentApiController : Controller
    {
        private readonly IAppointmentServices _appointmentServices;
        private readonly IHttpContextAccessor _httpContextAccosser;
        private readonly string LoginId;
        private readonly string role;


        public AppointmentApiController(IAppointmentServices appointmentServices, IHttpContextAccessor httpContextAccosser)
        {
            _appointmentServices = appointmentServices;
            _httpContextAccosser = httpContextAccosser;
            LoginId = _httpContextAccosser.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            role = _httpContextAccosser.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }

        [HttpPost]
        [Route("SaveCalendarData")]
        public IActionResult SaveCalendarData(AppointmentVM model)
        {
            CommanResponse<int> commanResponse = new CommanResponse<int>();
            try
            {
                commanResponse.status = _appointmentServices.AddUpdate(model).Result;
                if (commanResponse.status == 1)
                {
                    commanResponse.message = Helper.Helper.appointmentUpdated;
                }
                if (commanResponse.status == 2)
                {
                    commanResponse.message = Helper.Helper.appointmentAdded;
                }

            }
            catch (Exception e)
            {
                commanResponse.message = e.Message;
                commanResponse.status = Helper.Helper.failure_code;
            }
            return Ok(commanResponse);
        }

        [HttpGet]
        [Route("GetCalendarData")]
        public IActionResult GetCalendarData(string doctorId)
        {
            CommanResponse<List<AppointmentVM>> commanResponse = new CommanResponse<List<AppointmentVM>>();
            try
            {
                if (role == Helper.Helper.Patient)
                {
                    commanResponse.dataenum = _appointmentServices.PatientEventById(LoginId);
                    commanResponse.status = Helper.Helper.success_code;
                }
                else if (role == Helper.Helper.Doctor)
                {
                    commanResponse.dataenum = _appointmentServices.DoctorEventById(LoginId);
                    commanResponse.status = Helper.Helper.success_code;
                }
                else
                {
                    commanResponse.dataenum = _appointmentServices.DoctorEventById(doctorId);
                    commanResponse.status = Helper.Helper.success_code;
                }
            }
            catch (Exception e)
            {
                commanResponse.message = e.Message;
                commanResponse.status = Helper.Helper.failure_code;
            }
            return Ok(commanResponse);
        }

        [HttpGet]
        [Route("GetCalendarDataById/{id}")]
        public IActionResult GetCalendarDataById(int id)
        {
            CommanResponse<AppointmentVM> commanResponse = new CommanResponse<AppointmentVM>();
            try
            {

                commanResponse.dataenum = _appointmentServices.GetAppointmentById(id);
                commanResponse.status = Helper.Helper.success_code;

            }
            catch (Exception e)
            {
                commanResponse.message = e.Message;
                commanResponse.status = Helper.Helper.failure_code;
            }
            return Ok(commanResponse);
        }
        [HttpGet]
        [Route("ConfirmEvent/{id}")]
        public IActionResult ConfirmEvent(int id)
        {
            CommanResponse<int> commanResponse = new CommanResponse<int>();
            try
            {

                var result = _appointmentServices.ConfirmEvent(id).Result;
                if(result > 0)
                {
                    commanResponse.status = Helper.Helper.success_code;
                    commanResponse.message = Helper.Helper.meetingConfirm;
                }
                else
                {
                    commanResponse.status = Helper.Helper.failure_code;
                    commanResponse.message = Helper.Helper.meetingConfirmError;

                }
            }
            catch (Exception e)
            {
                commanResponse.message = e.Message;
                commanResponse.status = Helper.Helper.failure_code;
            }
            return Ok(commanResponse);
        }
        [HttpGet]
        [Route("DeleteAppoinment/{id}")]
        public async Task<IActionResult> DeleteAppoinment(int id)
        {
            CommanResponse<int> commanResponse = new CommanResponse<int>();
            try
            {

                commanResponse.status = await _appointmentServices.DeleteAppoinment(id);
                commanResponse.message = commanResponse.status == 1 ? Helper.Helper.appointmentDeleted : Helper.Helper.somethingWentWrong;

            }
            catch (Exception e)
            {
                commanResponse.message = e.Message;
                commanResponse.status = Helper.Helper.failure_code;
            }
            return Ok(commanResponse);
        }
    }
}
