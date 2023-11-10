using Microsoft.Datasync.Client;
using Microsoft.Datasync.Client.SQLiteStore;
using OfflineSyncPwaDemoApp.LocalMicroService.Models;

namespace OfflineSyncPwaDemoApp.LocalMicroService.Services
{
    public interface ITodo2Service
    {
        /// <summary>
        /// An event handler that is triggered when the list of items changes.
        /// </summary>
        event EventHandler<TodoService2EventArgs> TodoItemsUpdated;

        /// <summary>
        /// Get all the items in the list.
        /// </summary>
        /// <returns>The list of items (asynchronously)</returns>
        Task<IEnumerable<TodoItem2>> GetItemsAsync();

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
        Task RemoveItemAsync(TodoItem2 item);

        /// <summary>
        /// Saves an item to the list.  If the item does not have an Id, then the item
        /// is considered new and will be added to the end of the list.  Otherwise, the
        /// item is considered existing and is replaced.
        /// </summary>
        /// <param name="item">The new item</param>
        /// <returns>A task that completes when the item is saved.</returns>
        Task SaveItemAsync(TodoItem2 item);
    }

    internal class RemoteTodo2Service : ITodo2Service
    {
        private DatasyncClient _client = null;

        private IOfflineTable<TodoItem2> _table = null;

        public string OfflineDb { get; set; } = "C:\\work\\test\\offline2.db";

        private bool _initialized = false;

        private readonly SemaphoreSlim _asyncLock = new(1, 1);

        public event EventHandler<TodoService2EventArgs> TodoItemsUpdated;

        public Func<Task<AuthenticationToken>> TokenRequestor;

        public Uri ServiceUri;

        public RemoteTodo2Service(IConfiguration configuration)
        {
            ServiceUri = new Uri(configuration["ServiceUri2"]);
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
                store.DefineTable<TodoItem2>();
                var options = new DatasyncClientOptions
                {
                    OfflineStore = store
                };

                _client = TokenRequestor == null
                    ? new DatasyncClient(ServiceUri, options)
                    : new DatasyncClient(ServiceUri, new GenericAuthenticationProvider(TokenRequestor), options);
                await _client.InitializeOfflineStoreAsync();

                _table = _client.GetOfflineTable<TodoItem2>();

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

        public async Task<IEnumerable<TodoItem2>> GetItemsAsync()
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

        public async Task RemoveItemAsync(TodoItem2 item)
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
            TodoItemsUpdated?.Invoke(this, new TodoService2EventArgs(TodoService2EventArgs.ListAction.Delete, item));
        }

        public async Task SaveItemAsync(TodoItem2 item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            await InitializeAsync();

            TodoService2EventArgs.ListAction action = (item.Id == null) ? TodoService2EventArgs.ListAction.Add : TodoService2EventArgs.ListAction.Update;
            if (item.Id == null)
            {
                await _table.InsertItemAsync(item);
            }
            else
            {
                await _table.ReplaceItemAsync(item);
            }
            TodoItemsUpdated?.Invoke(this, new TodoService2EventArgs(action, item));
        }
    }
}
