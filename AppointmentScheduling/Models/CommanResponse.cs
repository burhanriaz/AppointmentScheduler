namespace AppointmentScheduling.Models
{
    public class CommanResponse<T>
    {
       
        public int status { get; set; }
        public string message { get; set; }
        public T dataenum { get; set; }
    }
}
