namespace test.Models
{
    public class BookingViewModel
    {
        public int Id;
        
        public string BookingCode;
        public UserViewModel User;
        public ScheduleViewModel Schedule;
        public int Seat;
    }
}