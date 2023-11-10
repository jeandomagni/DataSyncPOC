using Microsoft.Datasync.Client;
using Microsoft.Datasync.Client.SQLiteStore;
using Microsoft.VisualBasic;
using OfflineSyncPwaDemoApp.LocalMicroService.Models;

namespace OfflineSyncPwaDemoApp.LocalMicroService.Services
{
    public interface ITodoService
    {
        /// <summary>
        /// An event handler that is triggered when the list of items changes.
        /// </summary>
        event EventHandler<TodoServiceEventArgs> TodoItemsUpdated;

        /// <summary>
        /// Get all the items in the list.
        /// </summary>
        /// <returns>The list of items (asynchronously)</returns>
        Task<IEnumerable<TodoItem>> GetItemsAsync();

        /// <summary>
        /// Refreshes the TodoItems list manually.
        /// </summary>
        /// <returns>A task that completes when the refresh is done.</returns>
        Task RefreshItemsAsync();

        /// <summary>
        /// Removes an item in the list, if it exists.
        /// </summary>
        /// <param name="item">The item to be removed</param>
        /// <returns>A task that completes when the item is removed.</returns>
        Task RemoveItemAsync(TodoItem item);

        /// <summary>
        /// Saves an item to the list.  If the item does not have an Id, then the item
        /// is considered new and will be added to the end of the list.  Otherwise, the
        /// item is considered existing and is replaced.
        /// </summary>
        /// <param name="item">The new item</param>
        /// <returns>A task that completes when the item is saved.</returns>
        Task SaveItemAsync(TodoItem item);
    }

    internal class RemoteTodoService : ITodoService
    {
        private DatasyncClient _client = null;

        private IOfflineTable<TodoItem> _table = null;

        public string OfflineDb { get; set; } = "C:\\work\\test\\offline.db";

        private bool _initialized = false;

        private readonly SemaphoreSlim _asyncLock = new(1, 1);

        public event EventHandler<TodoServiceEventArgs> TodoItemsUpdated;

        public Func<Task<AuthenticationToken>> TokenRequestor;

        public Uri ServiceUri;

        public RemoteTodoService(IConfiguration configuration)
        {
            ServiceUri = new Uri(configuration["ServiceUri"]);
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
                store.DefineTable<TodoItem>();
                var options = new DatasyncClientOptions
                {
                    OfflineStore = store
                };

                _client = TokenRequestor == null
                    ? new DatasyncClient(ServiceUri, options)
                    : new DatasyncClient(ServiceUri, new GenericAuthenticationProvider(TokenRequestor), options);
                await _client.InitializeOfflineStoreAsync();

                _table = _client.GetOfflineTable<TodoItem>();

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

        public async Task<IEnumerable<TodoItem>> GetItemsAsync()
        {
            await InitializeAsync();
            return await _table.ToListAsync();
        }

        public async Task RefreshItemsAsync()
        {
            await InitializeAsync();
            await _table.PushItemsAsync();
            await _table.PullItemsAsync();
            return;
        }

        public async Task RemoveItemAsync(TodoItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (item.Id == null)
            {
                // Short circuit for when the item has not been saved yet.
                return;
            }
            await InitializeAsync();
            await _table.DeleteItemAsync(item);
            TodoItemsUpdated?.Invoke(this, new TodoServiceEventArgs(TodoServiceEventArgs.ListAction.Delete, item));
        }

        public async Task SaveItemAsync(TodoItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            await InitializeAsync();

            TodoServiceEventArgs.ListAction action = (item.Id == null) ? TodoServiceEventArgs.ListAction.Add : TodoServiceEventArgs.ListAction.Update;
            if (item.Id == null)
            {
                await _table.InsertItemAsync(item);
            }
            else
            {
                await _table.ReplaceItemAsync(item);
            }
            TodoItemsUpdated?.Invoke(this, new TodoServiceEventArgs(action, item));
        }
    }
}
