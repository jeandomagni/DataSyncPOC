namespace OfflineSyncPwaDemoApp.Local.Medications.Api.Models
{
    public class MedicationsServiceEventArgs
    {
        public MedicationsServiceEventArgs(ListAction action, Medication item)
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
        public Medication Item { get; }

        /// <summary>
        /// The list of possible actions.
        /// </summary>
        public enum ListAction { Add, Delete, Update };
    }
}
