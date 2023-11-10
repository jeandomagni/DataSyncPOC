using Microsoft.AspNetCore.Datasync;
using Microsoft.AspNetCore.Datasync.EFCore;
using Microsoft.EntityFrameworkCore;
using OAHPwaDemoApp.Remote.Medications.Api.Db;
using OAHPwaDemoApp.Remote.Medications.Api.Models;

namespace OAHPwaDemoApp.Remote.Medications.Api.Services
{
    public interface IMedicationsService
    {
        Task<List<Medication>> Get();
        Task<Medication> Save(Medication m);
        Task Remove(Medication medication);

    }
    public class MedicationsService : IMedicationsService
    {
        private readonly AppDbContext _appDbContext;
        private readonly EntityTableRepository<Medication> _repository;

        public MedicationsService(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
            _repository = new EntityTableRepository<Medication>(_appDbContext);
        }

        public Task<List<Medication>> Get()
        {
            return _repository.AsQueryable().ToListAsync();
        }

        public async Task<Medication> Save(Medication m)
        {
            await _repository.CreateAsync(m);

            return await GetById(m.Id);
        }

        public async Task Remove(Medication medication)
        {
            await _repository.DeleteAsync(medication.Id);
        }

        private Task<Medication> GetById(string medId)
        {
            return _repository.ReadAsync(medId);
        }
    }
}
