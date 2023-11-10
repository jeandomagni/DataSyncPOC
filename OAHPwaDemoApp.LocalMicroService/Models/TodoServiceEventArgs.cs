namespace OfflineSyncPwaDemoApp.LocalMicroService.Models
{
    public class TodoServiceEventArgs : EventArgs
    {
        public TodoServiceEventArgs(ListAction action, TodoItem item)
        {
            Action = action;
            Item = item;
        }

        /// <summary>
        /// The action that was performed
        /// </summary>
        public ListAction Action { get; }

        /// <summary>
        /// The item that was used.
        /// </summary>
        public TodoItem Item { get; }

        /// <summary>
        /// The list of possible actions.
        /// </summary>
        public enum ListAction { Add, Delete, Update };
    }
}
