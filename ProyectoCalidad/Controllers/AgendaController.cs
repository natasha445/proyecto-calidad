using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ProyectoCalidad.Services;
using ProyectoCalidad.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProyectoCalidad.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AgendaController : ControllerBase
    {
        private AgendaService agendaService;

        public AgendaController()
        {
            agendaService = new AgendaService();
        }

        [HttpPost]
        [Route("UserEvents")]
        [ValidateParam(typeof(UserEventObject))]
        public IActionResult UserEvents(UserEventObject requestBody)
        {
            return agendaService.getUserEvents(requestBody);
        }

        [HttpPost]
        [Route("RegisterEvent")]
        [ValidateParam(typeof(EventObject))]
        public IActionResult RegisterEvent(EventObject eventObject)
        {
            return agendaService.RegisterEvent(eventObject);
        }

        [HttpPost]
        [Route("UpdateEvent")]
        [ValidateParam(typeof(EventObject))]
        public IActionResult UpdateEvent(EventObject eventObject)
        {
            return agendaService.UpdateEvent(eventObject);
        }

        [HttpPost]
        [Route("RemoveEvent")]
        [ValidateParam(typeof(EventObject))]
        public IActionResult RemoveEvent(EventObject eventObject)
        {
            return agendaService.RemoveEvent(eventObject);
        }
    }
}
