using System.Collections;
using System.Collections.Generic;

namespace test.Models
{
    public class BookingPageViewModel
    {
        public ScheduleViewModel Schedule;
        public IEnumerable<BookingViewModel> BookingsInSchedule;
    }
}