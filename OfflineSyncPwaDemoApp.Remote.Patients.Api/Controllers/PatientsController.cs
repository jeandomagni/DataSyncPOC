using Microsoft.AspNetCore.Mvc;
using OfflineSyncPwaDemoApp.Remote.Patients.Api.Models;
using OfflineSyncPwaDemoApp.Remote.Patients.Api.Services;

namespace OfflineSyncPwaDemoApp.Remote.Patients.Api.Controllers
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
        public Task<Patient> Post([FromBody] Patient medication)
        {
            return _patientsService.Save(medication);
        }

        [HttpDelete]
        public async Task Delete([FromBody] Patient medication)
        {
            await _patientsService.Remove(medication);
        }
    }
}