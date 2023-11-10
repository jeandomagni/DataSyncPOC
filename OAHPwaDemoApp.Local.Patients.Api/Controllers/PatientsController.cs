using Microsoft.AspNetCore.Mvc;
using OAHPwaDemoApp.Local.Patients.Api.Models;
using OAHPwaDemoApp.Local.Patients.Api.Services;

namespace OAHPwaDemoApp.Local.Patients.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly ILogger<PatientsController> _logger;
        private readonly IPatientsService _patientsService;

        public PatientsController(ILogger<PatientsController> logger, IPatientsService patientsService)
        {
            _logger = logger;
            this._patientsService = patientsService;
        }

        [HttpGet]
        public async Task<IEnumerable<Patient>> Get()
        {
            var items = await _patientsService.Get();
            return items;
        }

        [HttpPost]
        public async Task<Patient> Post([FromBody] Patient patient)
        {
            return await _patientsService.Save(patient);
        }

        [HttpDelete]
        public async Task Delete([FromBody] Patient patient)
        {
            await _patientsService.Remove(patient);
        }
    }
}