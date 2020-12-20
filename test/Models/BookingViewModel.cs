﻿namespace cinema.Models
{
    public class BookingViewModel
    {
        public int Id;
        
        public string BookingCode;
        public string UserNickName;
        public ScheduleViewModel Schedule;
        public int Row;
        public int Seat;
        public int ScheduleId { get; set; }
    }
}