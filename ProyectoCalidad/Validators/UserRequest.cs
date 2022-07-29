using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoCalidad.Validators
{
    public class UserObject
    {
        [RegularExpression(@"^[A-Za-z0-9]+(?:[_-][A-Za-z0-9]+)*$")]
        [MaxLength(20)]
        public string UserName { get; set; }
        
        public string UserPassword { get; set; }
    }

    public class UserEventObject
    {
        [RegularExpression(@"^[A-Za-z0-9]+(?:[_-][A-Za-z0-9]+)*$")]
        [MaxLength(20)]
        public string UserName { get; set; }
    }

    public class LockUserObject
    {
        [RegularExpression(@"^[A-Za-z0-9]+(?:[_-][A-Za-z0-9]+)*$")]
        [MaxLength(20)]
        public string UserName { get; set; }

        [RegularExpression(@"^[0-9]+$")]
        [MaxLength(1)]
        public string LockUser { get; set; }

        [MaxLength(70)]
        public string UnlockDate { get; set; }
    }
}