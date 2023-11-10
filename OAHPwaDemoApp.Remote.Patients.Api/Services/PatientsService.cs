using Microsoft.AspNetCore.Datasync.EFCore;
using Microsoft.EntityFrameworkCore;
using OAHPwaDemoApp.Remote.Patients.Api.Db;
using OAHPwaDemoApp.Remote.Patients.Api.Models;

namespace OAHPwaDemoApp.Remote.Patients.Api.Services
{
    public interface IPatientsService
    {
        Task<List<Patient>> Get();
        Task<Patient> Save(Patient p);
        Task Remove(Patient medication);
    }
    public class PatientsService : IPatientsService
    {
        private readonly AppDbContext _appDbContext;
        private readonly EntityTableRepository<Patient> _repository;

        public PatientsService(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
            _repository = new EntityTableRepository<Patient>(_appDbContext);
        }

        public Task<List<Patient>> Get()
        {
            return _repository.AsQueryable().ToListAsync();
        }

        public async Task<Patient> Save(Patient p)
        {
            await _repository.CreateAsync(p);

            return await GetById(p.Id);
        }

        public async Task Remove(Patient patient)
        {
            await _repository.DeleteAsync(patient.Id);
        }

        private Task<Patient> GetById(string patId)
        {
            return _repository.ReadAsync(patId);
        }
    }
}
