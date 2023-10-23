using Microsoft.AspNetCore.Mvc;
using Microsoft.Datasync.Client;
using OfflineSyncPwaDemoApp.LocalMicroService.Models;
using OfflineSyncPwaDemoApp.LocalMicroService.Services;

namespace OfflineSyncPwaDemoApp.LocalMicroService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {

        private readonly ILogger<TodoController> _logger;
        private readonly ITodoService _todoService;

        public TodoController(ILogger<TodoController> logger, ITodoService todoService)
        {
            _logger = logger;
            _todoService = todoService;
        }

        /// <summary>
        /// The list of items.
        /// </summary>
        public ConcurrentObservableCollection<TodoItem> Items { get; } = new();

        private bool _isRefreshing;
        /// <summary>
        /// True if the service is refreshing the data.
        /// </summary>
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => _isRefreshing = value;
        }

        /// <summary>
        /// Command for refreshing the items.  This will synchronize the backend.
        /// </summary>
        /// <returns>A task that completes when the sync is done.</returns>
        public virtual async Task RefreshItemsAsync()
        {
            // Note that there is a short race condition here inbetween accessing the IsRefreshing
            // property and setting it asynchronously.  In an ideal world, we would use a surrounding
            // async lock to ensure that the critical section is only accessed serially, thus
            // avoiding this problem.
            if (IsRefreshing)
            {
                return;
            }
            IsRefreshing = true;
            // End of critical section

            try
            {
                // Do any database service refreshing needed
                await _todoService.RefreshItemsAsync();

            }
            catch (Exception? ex)
            {

            }
            finally
            {
                IsRefreshing = false;
            }
        }

        [HttpGet]
        public async Task<IEnumerable<TodoItem>> Get()
        {
            await RefreshItemsAsync();
            var items = await _todoService.GetItemsAsync();
            return items;
        }
    }
}