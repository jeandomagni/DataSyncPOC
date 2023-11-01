using Hangfire;
using Microsoft.Datasync.Client;
using Microsoft.Datasync.Client.SQLiteStore;
using OfflineSyncPwaDemoApp.Local.Patients.Api.Models;

namespace OfflineSyncPwaDemoApp.Local.Patients.Api.Services
{
    public interface IPatientsService
    {
        event EventHandler<PatientsServiceEventArgs> PatientsUpdated;

        Task<IEnumerable<Patient>> Get();

        [Hangfire.AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        Task Refresh();

        Task Remove(Patient item);

        Task<Patient> Save(Patient item);
    }
    public class PatientsService : IPatientsService
    {
        private DatasyncClient _client = null;

        private IOfflineTable<Patient> _table = null;

        public string OfflineDb { get; set; }

        private bool _initialized = false;

        private readonly SemaphoreSlim _asyncLock = new(1, 1);

        public event EventHandler<PatientsServiceEventArgs> PatientsUpdated;

        public Func<Task<AuthenticationToken>> TokenRequestor;

        public Uri DataSyncServiceUri;
        public Uri ApiServiceUri;

        public PatientsService(IConfiguration configuration)
        {
            DataSyncServiceUri = new Uri(configuration["DataSyncServiceUri"]);
            ApiServiceUri = new Uri(configuration["ApiServiceUri"]);
            OfflineDb = configuration["OfflineDbPath"];
            TokenRequestor = null; // no authentication
        }


        private async Task InitializeAsync()
        {
            if (_initialized)
            {
                return;
            }

            try
            {
                await _asyncLock.WaitAsync();
                if (_initialized)
                {
                    return;
                }

                var connectionString = new UriBuilder
                {
                    Scheme = "file",
                    Path = OfflineDb,
                    Query = "?mode=rwc"
                }.Uri.ToString();
                var store = new OfflineSQLiteStore(connectionString);
                store.DefineTable<Patient>();
                var options = new DatasyncClientOptions
                {
                    OfflineStore = store
                };

                _client = TokenRequestor == null
                    ? new DatasyncClient(DataSyncServiceUri, options)
                    : new DatasyncClient(DataSyncServiceUri, new GenericAuthenticationProvider(TokenRequestor), options);
                await _client.InitializeOfflineStoreAsync();

                _table = _client.GetOfflineTable<Patient>();

                _initialized = true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _asyncLock.Release();
            }
        }

        public async Task<IEnumerable<Patient>> Get()
        {
            await InitializeAsync();
            return await _table.ToListAsync();
        }

        public async Task Refresh()
        {
            try
            {
                await InitializeAsync();
                await _client.PushTablesAsync();
                await _table.PullItemsAsync();
                return;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task Remove(Patient item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (string.IsNullOrWhiteSpace(item.Id))
            {
                // Short circuit for when the item has not been saved yet.
                return;
            }
            await InitializeAsync();
            await _table.DeleteItemAsync(item);
            PatientsUpdated?.Invoke(this, new PatientsServiceEventArgs(PatientsServiceEventArgs.ListAction.Delete, item));
        }

        public async Task<Patient> Save(Patient item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            await InitializeAsync();

            PatientsServiceEventArgs.ListAction action = (string.IsNullOrWhiteSpace(item.Id)) ? PatientsServiceEventArgs.ListAction.Add : PatientsServiceEventArgs.ListAction.Update;
            if (string.IsNullOrWhiteSpace(item.Id))
            {
                await _table.InsertItemAsync(item);
            }
            else
            {
                await _table.ReplaceItemAsync(item);
            }
            PatientsUpdated?.Invoke(this, new PatientsServiceEventArgs(action, item));

            return item;
        }
    }
}
