using Microsoft.Datasync.Client;

namespace OfflineSyncPwaDemoApp.LocalMicroService.Models
{
    public class TodoItem2 : DatasyncClientData, IEquatable<TodoItem2>
    {
        public string Title { get; set; }

        public bool IsComplete { get; set; }

        public bool Equals(TodoItem2 other)
            => other != null && other.Id == Id && other.Title == Title && other.IsComplete == IsComplete;
    }
}
