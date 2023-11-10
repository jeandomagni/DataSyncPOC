using Microsoft.AspNetCore.Mvc;
using Microsoft.Datasync.Client;
using OfflineSyncPwaDemoApp.LocalMicroService.Controllers;
using OfflineSyncPwaDemoApp.LocalMicroService.Services;
using OfflineSyncPwaDemoApp.LocalMicroService.Models;
using OfflineSyncPwaDemoApp.LocalMicroService.Services;

namespace OfflineSyncPwaDemoApp.LocalMicroService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Todo2Controller : ControllerBase
    {
        private readonly ILogger<TodoController> _logger;
        private readonly ITodo2Service _todo2Service;

        public Todo2Controller(ILogger<TodoController> logger, ITodo2Service todoService)
        {
            _logger = logger;
            _todo2Service = todoService;
        }

        /// <summary>
        /// The list of items.
        /// </summary>
        public ConcurrentObservableCollection<TodoItem2> Items { get; } = new();

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
                await _todo2Service.RefreshItemsAsync();

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
        public async Task<IEnumerable<TodoItem2>> Get()
        {
            await RefreshItemsAsync();
            var items = await _todo2Service.GetItemsAsync();
            return items;
        }
    }
}
