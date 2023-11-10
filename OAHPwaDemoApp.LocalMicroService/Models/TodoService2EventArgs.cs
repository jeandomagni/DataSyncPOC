namespace OfflineSyncPwaDemoApp.LocalMicroService.Models
{
    public class TodoService2EventArgs : EventArgs
    {
        public TodoService2EventArgs(ListAction action, TodoItem2 item)
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
        public TodoItem2 Item { get; }

        /// <summary>
        /// The list of possible actions.
        /// </summary>
        public enum ListAction { Add, Delete, Update };
    }
}
