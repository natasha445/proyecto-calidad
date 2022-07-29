using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoCalidad.Models
{
    public class EventModel
    {
        public string id { get; set; }
        public string userName { get; set; }
        public string startMinutes { get; set; }
        public string startHour { get; set; }
        public string startDay { get; set; }
        public string startMonth { get; set; }
        public string startYear { get; set; }
        public string endMinutes { get; set; }
        public string endHour { get; set; }
        public string endDay { get; set; }
        public string endMonth { get; set; }
        public string endYear { get; set; }
        public string eventName { get; set; }
        public string eventColor { get; set; }
    }
}
