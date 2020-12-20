using System.Collections.Generic;

namespace cinema.Models
{
    public class HallViewModel
    {
        public int RowCount; //количество рядов
        public int HallNumber; // номер зала
        public int[] SeatsRowCount; //количество мест в каждом ряду
    }
}