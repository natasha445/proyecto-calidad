using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoCalidad.Validators
{
    public class EventObject
    {
        [RegularExpression(@"^[0-9]+$")]
        [MaxLength(3)]
        public string Id { get; set; }

        [RegularExpression(@"^[A-Za-z0-9]+(?:[_-][A-Za-z0-9]+)*$")]
        [MaxLength(20)]
        public string UserName { get; set; }

        [RegularExpression(@"^[0-9]+$")]
        [MaxLength(2)]
        public string StartMinutes { get; set; }

        [RegularExpression(@"^[0-9]+$")]
        [MaxLength(2)]
        public string StartHour { get; set; }

        [RegularExpression(@"^[0-9]+$")]
        [MaxLength(2)]
        public string StartDay { get; set; }

        [RegularExpression(@"^[0-9]+$")]
        [MaxLength(2)]
        public string StartMonth { get; set; }

        [RegularExpression(@"^[0-9]+$")]
        [MaxLength(4)]
        public string StartYear { get; set; }

        [RegularExpression(@"^[0-9]+$")]
        [MaxLength(2)]
        public string EndMinutes { get; set; }

        [RegularExpression(@"^[0-9]+$")]
        [MaxLength(2)]
        public string EndHour { get; set; }

        [RegularExpression(@"^[0-9]+$")]
        [MaxLength(2)]
        public string EndDay { get; set; }

        [RegularExpression(@"^[0-9]+$")]
        [MaxLength(2)]
        public string EndMonth { get; set; }

        [RegularExpression(@"^[0-9]+$")]
        [MaxLength(4)]
        public string EndYear { get; set; }

        [RegularExpression(@"^[A-Za-z]+(?:[ ][A-Za-z]+)*$")]
        [MaxLength(50)]
        public string EventName { get; set; }

        [RegularExpression(@"^[A-Za-z]+(?:[-][0-9]+)*$")]
        [MaxLength(10)]
        public string EventColor { get; set; }
    }
}