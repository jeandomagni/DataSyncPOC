using Microsoft.Datasync.Client;
using Microsoft.Datasync.Client.SQLiteStore;
using OfflineSyncPwaDemoApp.Local.Medications.Api.Models;

namespace OfflineSyncPwaDemoApp.Local.Medications.Api.Services
{
    public interface IMedicationsService
    {
        event EventHandler<MedicationsServiceEventArgs> MedicationsUpdated;

        Task<IEnumerable<Medication>> Get();

        Task Refresh();

        Task Remove(Medication item);

        Task<Medication> Save(Medication item);
    }
    public class MedicationsService : IMedicationsService
    {
        private DatasyncClient _client = null;

        private IOfflineTable<Medication> _table = null;

        public string OfflineDb { get; set; }

        private bool _initialized = false;

        private readonly SemaphoreSlim _asyncLock = new(1, 1);

        public event EventHandler<MedicationsServiceEventArgs> MedicationsUpdated;

        public Func<Task<AuthenticationToken>> TokenRequestor;

        public Uri DataSyncServiceUri;
        public Uri ApiServiceUri;

        public MedicationsService(IConfiguration configuration)
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
                store.DefineTable<Medication>();
                var options = new DatasyncClientOptions
                {
                    OfflineStore = store
                };

                _client = TokenRequestor == null
                    ? new DatasyncClient(DataSyncServiceUri, options)
                    : new DatasyncClient(DataSyncServiceUri, new GenericAuthenticationProvider(TokenRequestor), options);
                await _client.InitializeOfflineStoreAsync();

                _table = _client.GetOfflineTable<Medication>();

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

        public async Task<IEnumerable<Medication>> Get()
        {
            await InitializeAsync();
            return await _table.ToListAsync();
        }

        public async Task Refresh()
        {
            await InitializeAsync();
            try
            {
                await _table.PushItemsAsync();
                await _table.PullItemsAsync();
                return;

            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task Remove(Medication item)
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
            MedicationsUpdated?.Invoke(this, new MedicationsServiceEventArgs(MedicationsServiceEventArgs.ListAction.Delete, item));
        }

        public async Task<Medication> Save(Medication item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            await InitializeAsync();

            MedicationsServiceEventArgs.ListAction action = (string.IsNullOrWhiteSpace(item.Id)) ? MedicationsServiceEventArgs.ListAction.Add : MedicationsServiceEventArgs.ListAction.Update;
            if (item.Id == null)
            {
                await _table.InsertItemAsync(item);
            }
            else
            {
                await _table.ReplaceItemAsync(item);
            }
            MedicationsUpdated?.Invoke(this, new MedicationsServiceEventArgs(action, item));

            return item;
        }
    }
}
