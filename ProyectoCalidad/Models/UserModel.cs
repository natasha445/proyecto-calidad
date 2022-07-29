using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoCalidad.Models
{
    public class UserModel
    {
        public string password { get; set; }
        public string isLocked { get; set; }
        public string unlockDate { get; set; }
    }
}
