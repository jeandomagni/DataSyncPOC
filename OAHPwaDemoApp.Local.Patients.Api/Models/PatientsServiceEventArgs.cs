namespace OAHPwaDemoApp.Local.Patients.Api.Models
{
    public class PatientsServiceEventArgs
    {
        public PatientsServiceEventArgs(ListAction action, Patient item)
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
        public Patient Item { get; }

        /// <summary>
        /// The list of possible actions.
        /// </summary>
        public enum ListAction { Add, Delete, Update };
    }
}
