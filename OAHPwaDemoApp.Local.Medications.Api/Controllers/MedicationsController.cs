using Microsoft.AspNetCore.Mvc;
using OAHPwaDemoApp.Local.Medications.Api.Models;
using OAHPwaDemoApp.Local.Medications.Api.Services;

namespace OAHPwaDemoApp.Local.Medications.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MedicationsController : ControllerBase
    {

        private readonly ILogger<MedicationsController> _logger;
        private readonly IMedicationsService _medicationsService;

        public MedicationsController(ILogger<MedicationsController> logger, IMedicationsService medicationsService)
        {
            _logger = logger;
            this._medicationsService = medicationsService;
        }

        [HttpGet]
        public async Task<IEnumerable<Medication>> Get()
        {
            var items = await _medicationsService.Get();
            return items;
        }

        [HttpPost]
        public Task<Medication> Post([FromBody] Medication medication)
        {
            return _medicationsService.Save(medication);
        }

        [HttpDelete]
        public async Task Delete([FromBody] Medication medication)
        {
            await _medicationsService.Remove(medication);
        }
    }
}