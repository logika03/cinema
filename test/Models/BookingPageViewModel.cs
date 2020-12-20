using System.Collections.Generic;

namespace cinema.Models
{
    public class BookingPageViewModel
    {
        public ScheduleViewModel Schedule;
        public IEnumerable<BookingViewModel> BookingsInSchedule;
    }
}